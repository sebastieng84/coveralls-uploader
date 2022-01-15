using System.Diagnostics;

namespace coveralls_uploader.Utilities.Wrappers
{
    public interface IProcess
    {
        public ProcessStartInfo StartInfo { get; set; }
        public int ExitCode { get; }
        bool Start();
        void BeginOutputReadLine();
        void WaitForExit();
        event DataReceivedEventHandler? OutputDataReceived;
    }
}