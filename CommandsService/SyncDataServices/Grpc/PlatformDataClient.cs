using AutoMapper;
using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformDataClient> _logger;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper, ILogger<PlatformDataClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

            _logger.LogInformation("Initilizing PlatformDataClient (gRPC)");   
        }
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            throw new NotImplementedException();
        }
    }
}