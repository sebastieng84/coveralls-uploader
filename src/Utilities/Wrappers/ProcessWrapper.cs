using System.Diagnostics;

namespace coveralls_uploader.Utilities.Wrappers
{
    public class ProcessWrapper : IProcess
    {
        private readonly Process _process;

        public ProcessStartInfo StartInfo
        {
            get => _process.StartInfo;
            set => _process.StartInfo = value;
        }

        public int ExitCode => _process.ExitCode;

        public ProcessWrapper(Process process)
        {
            _process = process;
        }

        public event DataReceivedEventHandler? OutputDataReceived
        {
            add => _process.OutputDataReceived += value;
            remove => _process.OutputDataReceived -= value;
        }

        public bool Start()
        {
            return _process.Start();
        }

        public void BeginOutputReadLine()
        {
            _process.BeginOutputReadLine();
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }
    }
}