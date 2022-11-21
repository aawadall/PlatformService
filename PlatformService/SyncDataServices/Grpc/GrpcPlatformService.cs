using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcPlatformService> _logger;

        public GrpcPlatformService(IPlatformRepo repository, IMapper mapper, ILogger<GrpcPlatformService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;

            _logger.LogInformation("GrpcPlatformService initialized");
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Getting all platforms...");

            var response = new PlatformResponse();
            var platforms = _repository.GetAllPlatforms();
            foreach (var platform in platforms)
            {
                response.Platforms.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}
