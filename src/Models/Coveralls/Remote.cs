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
    }
}