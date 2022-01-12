using System.IO;

namespace coveralls_uploader.Models
{
    public class CommandOptions
    {
        public FileInfo Input { get; set; }
        public bool Source { get; set; }
        public string Token { get; set; }

        public CommandOptions()
        {
        }

        public CommandOptions(FileInfo input, bool source, string token)
        {
            Input = input;
            Source = source;
            Token = token;
        }
    }
}