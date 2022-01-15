using System.Runtime.InteropServices;

namespace coveralls_uploader.Utilities.Wrappers
{
    public class RuntimeInformationWrapper : IRuntimeInformation
    {
        public bool IsOSPlatform(OSPlatform osPlatform)
        {
            return RuntimeInformation.IsOSPlatform(osPlatform);
        }
    }
}