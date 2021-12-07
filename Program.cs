// See https://aka.ms/new-console-template for more information

using System;
using coveralls_uploader.Parsers;

var parser = new LcovParser();
var test = parser.Parse(@"C:\Users\sebas\Downloads\lcov.info");

Console.WriteLine(test.Count);