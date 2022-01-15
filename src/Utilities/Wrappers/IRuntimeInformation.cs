using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace coveralls_uploader.Utilities.Wrappers
{
    public interface IRuntimeInformation
    {
        bool IsOSPlatform(OSPlatform osPlatform);
    }
}