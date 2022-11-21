using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

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
            string? grpcServiceAddress = _configuration["GrpcPlatform"];
            _logger.LogInformation($"Calling gRPC Service {grpcServiceAddress}");

            var channel = GrpcChannel.ForAddress(grpcServiceAddress);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platforms);                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not call gRPC Server {ex.Message}");
                return null;
            }
        }
    }
}