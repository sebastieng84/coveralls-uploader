# coveralls-uploader ⭐
![GitHub](https://img.shields.io/github/license/sebastieng84/coveralls-uploader) [![Coverage Status](https://coveralls.io/repos/github/sebastieng84/coveralls-uploader/badge.svg?branch=master)](https://coveralls.io/github/sebastieng84/coveralls-uploader?branch=master)

coveralls-uploader is a dotnet tool that allows you to parse a lcov code coverage report and upload to [Coveralls.io](https://coveralls.io) 🚀

## Installation
### Global tool
```
dotnet tool install --global coveralls-uploader --version 0.4.1
```
### Local tool
```
dotnet new tool-manifest # if you are setting up this repo
dotnet tool install --local coveralls-uploader --version 0.4.1
```

## Usage

Use `coveralls-uploader [options]` to parse and upload the report.

Options:
```
  -i, --input <input> (REQUIRED)  File path to the code coverage report 
  -t, --token <token>             The repository token 
  -s, --source                    Include source file content in the request 
  -v, --verbose                   Show verbose output 
  --version                       Show version information
  -?, -h, --help                  Show help and usage information 
```
