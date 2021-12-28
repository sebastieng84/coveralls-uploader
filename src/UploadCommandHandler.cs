using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using coveralls_uploader.Services;

namespace coveralls_uploader
{
    public class UploadCommandHandler : ICommandHandler
    {
        private readonly MainService _mainService;
        
        public FileInfo Input { get; set; }

        public UploadCommandHandler(MainService mainService)
        {
            _mainService = mainService;
        }
        
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await _mainService.RunAsync();

            return 0;
        }
    }
}