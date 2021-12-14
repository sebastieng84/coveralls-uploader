using System.Collections.Generic;
using coveralls_uploader.Models;


namespace coveralls_uploader.Parsers
{
    public interface IParser
    {
        IList<FileCoverage> Parse(string filePath);
    }
}