# AstroGrep.Core

This is a fork of the original [AstroGrep repo](https://github.com/joshball/astrogrep). 

AstroGrep is a nice Windows grep utility, good speed and customizability. The app includes one assembly that is responsible for the actual searching. I wanted to be able to use just that functionality, so I pulled out that code and refactored a bit to remove references to other parts of the solution. I also renamed the assembly from `libAstroGrep.dll` to `AstroGrep.Core.dll`.

Here's a sample usage:
```csharp
var searchSpec = new SearchSpec()
{
    StartDirectories = new List<string>() { @"c:\some\dir" },    
    SearchText = "fizzbin",
};

var filterSpec = new FileFilterSpec()
{
    FileFilter = "*.txt"
};

var grep = new Grep(searchSpec, filterSpec);
grep.Execute();
var matchResults = grep.MatchResults;
```

