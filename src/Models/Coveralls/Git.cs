using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace coveralls_uploader.Models.Coveralls
{
    public class Git
    {
        public Head Head { get; set; }
        public string Branch { get; set; }
        public ICollection<Remote> Remotes { get; set; } = new List<Remote>();

        public void Merge(Git other)
        {
            if (other is null)
            {
                return;
            }
            
            Branch ??= other.Branch;

            if (Head is null)
            {
                Head = other.Head;
            }
            else
            {
                Head.Merge(other.Head);
            }

            if (Remotes.Count == 0)
            {
                Remotes = other.Remotes;
            }
        }

        private bool Equals(Git other)
        {
            return Equals(Head, other.Head) &&
                   Branch == other.Branch && 
                   Equals(Remotes, other.Remotes);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Git other)
            {
                return false;
            }

            return ReferenceEquals(this, obj) || 
                   Equals(other);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Head, Branch, Remotes);
        }
    }
}