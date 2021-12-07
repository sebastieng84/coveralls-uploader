using System.Collections.Generic;


namespace coveralls_uploader.Parsers
{
    public interface IParser
    {
        IList<SourceFileCoverage> Parse(string filePath);
    }
}