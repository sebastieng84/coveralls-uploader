using System.Collections.Generic;
using System.IO;
using coveralls_uploader.Models;

namespace coveralls_uploader.Parsers
{
    public interface IParser
    {
        IList<FileCoverage> Parse(FileInfo fileInfo);
    }
}