using System.Collections.Generic;

namespace coveralls_uploader.Models.Coveralls.Git
{
    public class GitInformation
    {
        public Head Head { get; set; }
        public string Branch { get; set; }
        public IEnumerable<Remote> Remotes { get; set; } = new List<Remote>();
        
        public GitInformation(Head head, string branch)
        {
            Head = head;
            Branch = branch;
        }
    }
}