using System.Collections.Generic;

namespace coveralls_uploader.Models.Coveralls.Git
{
    public class GitInformation
    {
        public Head Head { get; set; }
        public string Branch { get; set; }
        public ICollection<Remote> Remotes { get; set; } = new List<Remote>();
    }
}