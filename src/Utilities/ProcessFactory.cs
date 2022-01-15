using System.Diagnostics;
using coveralls_uploader.Utilities.Wrappers;

namespace coveralls_uploader.Utilities
{
    public class ProcessFactory
    {
        public virtual IProcess Create(ProcessStartInfo processStartInfo)
        {
            var process = new Process();
            process.StartInfo = processStartInfo;

            return new ProcessWrapper(process);
        }
    }
}