using System;

namespace coveralls_uploader.Models.Coveralls
{
    public class Remote
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public Remote(string name, string url)
        {
            Name = name;
            Url = url;
        }

        private bool Equals(Remote other)
        {
            return Name == other.Name && Url == other.Url;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Remote remote)
            {
                return false;
            }
            
            return ReferenceEquals(this, remote) || Equals(remote);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Url);
        }
    }
}