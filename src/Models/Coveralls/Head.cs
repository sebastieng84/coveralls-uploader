using System;
using Newtonsoft.Json;

namespace coveralls_uploader.Models.Coveralls
{
    public class Head
    {
        public string Id { get; set; }
        [JsonProperty("author_name")] public string AuthorName { get; set; }
        [JsonProperty("author_email")] public string AuthorEmail { get; set; }
        [JsonProperty("committer_name")] public string CommitterName { get; set; }
        [JsonProperty("committer_email")] public string CommitterEmail { get; set; }
        public string Message { get; set; }

        public void Merge(Head head)
        {
            if (head is null)
            {
                return;
            }
            
            Id ??= head.Id;
            AuthorName ??= head.AuthorName;
            AuthorEmail ??= head.AuthorEmail;
            CommitterName ??= head.CommitterName;
            CommitterEmail ??= head.CommitterEmail;
            Message ??= head.Message;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Head other)
            {
                return false;
            }
            
            return Id == other.Id && 
                   AuthorName == other.AuthorName && 
                   AuthorEmail == other.AuthorEmail &&
                   CommitterName == other.CommitterName &&
                   CommitterEmail == other.CommitterEmail &&
                   Message == other.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, AuthorName, AuthorEmail, CommitterName, CommitterEmail, Message);
        }
    }
}