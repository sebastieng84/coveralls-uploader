using System.CommandLine;
using System.IO;

namespace coveralls_uploader
{
    public class UploadCommand : RootCommand
    {
        public UploadCommand()
        {
            var addArgument = new Argument<FileInfo>(
                "input",
                "File path to the code coverage report.");

            AddArgument(addArgument);
            //addArgument.AddValidator();

        }
    }
}