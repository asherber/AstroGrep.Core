# AstroGrep.Core

*This started out as a fork of [joshball's copy](https://github.com/joshball/astrogrep) of the AstroGrep repo. On 2023-10-13, I rebased things on the actual [SVN repo](https://sourceforge.net/p/astrogrep/code/HEAD/tree/trunk/AstroGrep/), so I can keep it more up to date.*

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

### SVN config

Wiring up the SVN repo has to be done locally.

1. Define the remote branch in `.git/config`
   ```
   [svn-remote "upstream"]
   	url = https://svn.code.sf.net/p/astrogrep/code/trunk
   	fetch = :refs/remotes/git-svn-upstream
   ```

2. Import the SVN branch
   ```
   git svn fetch upstream
   ```

3. Create local branch
   ```
   git checkout -b upstream refs/remotes/git-svn-upstream
   ```

4. Update
   ```
   git svn rebase
   ```

   
