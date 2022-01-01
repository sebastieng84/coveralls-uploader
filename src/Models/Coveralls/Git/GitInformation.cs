namespace coveralls_uploader.Models.Coveralls.Git
{
    public class GitInformation
    {
        public Head Head { get; set; }
        public string Branch { get; set; }
        public IEnumerable<Remote> Remotes { get; set; } = new List<Remote>();
    }
}