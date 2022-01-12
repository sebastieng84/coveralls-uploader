using System.Collections.Generic;

namespace coveralls_uploader.Models.Coveralls
{
    public class Git
    {
        public Head Head { get; set; }
        public string Branch { get; set; }
        public ICollection<Remote> Remotes { get; set; } = new List<Remote>();

        public void Merge(Git git)
        {
            Branch ??= git.Branch;

            if (Head is null)
            {
                Head = git.Head;
            }
            else
            {
                Head.Merge(git.Head);   
            }

            if (Remotes.Count == 0)
            {
                Remotes = git.Remotes;
            }
        }
    }
}