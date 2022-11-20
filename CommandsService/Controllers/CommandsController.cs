using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<CommandsController> _logger;

        public CommandsController(ICommandRepo repo, IMapper mapper, ILogger<CommandsController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            _logger.LogInformation($"--> GetCommandsForPlatform: {platformId}");
            if (!_repo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _repo.GetCommandsForPlatform(platformId);

            _logger.LogInformation($"--> GetCommandsForPlatform: {platformId} - {commands.Count()} commands found");
            
            return Ok(_mapper.Map<CommandReadDto>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            _logger.LogInformation($"-> GetCommandForPlatform: {platformId} / {commandId}");
            if (!_repo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _repo.GetCommand(platformId, commandId);
            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            _logger.LogInformation($"Creating command for platform id {platformId}");

            if (!_repo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _repo.CreateCommand(platformId, commandModel);

            if(!_repo.SaveChanges())
            {
                return BadRequest();
            }

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                                  new { platformId = platformId, commandId = commandModel.Id },
                                  _mapper.Map<CommandReadDto>(commandModel));
        }
    }
}