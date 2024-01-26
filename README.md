# Auburn Atelier Unity Core

Code shared between Auburn Atelier projects.

There are three main methods you could use to install this project:
1. Just download it and copy-paste all the files into your projects assets.
2. Download it, and install the package locally from disk.
3. Directly add the git repostiory as a reference

If you're familiar with git (or willing to become familiar), I recommend option #3. Instructions for 
installing git can be found at the bottom of this readme. Options #1 and #2 might seem easier, but
option #3 has the advantage that if you want to download your code onto another machine, you won't
need to manually re-download and install this package.

## Downloading the Code

If you're either (1) dowloading and copy-pasting the code directly, or (2) downloading it, but 
installing it as a package, you'll have to start by... downloading the code! Just click the green 
"< > Code" button near the top-right of the git repository main page, and hit "Download ZIP".

If you're just copy-pasting, you can take every thing in the main folder and drag it into your
project's asset folder - either through the Unity Editor, or in your file explorer.

## Installing as a local package

If you've download the code, but you'd like to use it as a proper unity package, then after you've
unzipped the contents of this repository, you can 
[follow the official unity instructions](https://docs.unity3d.com/Manual/upm-ui-local.html) for 
installing a local package.

I recommend being mindful of where you put the directory, since when you do this, Unity won't copy
the project files over to your project. I'd extract the ZIP to a folder sitting next to your Unity
project.

## Installing via Git

If git is installed on your system (and set up in your PATH), Unity will be able to retrieve this
package directly from git on your behalf. This has the advantage that if you ever download your 
project source onto another machine, Unity will handle retrieving the package for you, eliminating
any manual steps.

If you don't have git set up on your machine, you can follow the instructions on the bottom of
this readme.

With git installed on your machine, you can 
[follow the official unity instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html) for
installing a package using git.

The Git URL you'll want can be found by clicking the green "< > Code" button near the top-right of
main git repository web page. It should show up in a small grey box underneath the options "HTTPS",
"SSH", and "GitHub CLI". Simply click the double-box icon top copy the URL to your clipboard.

If you haven't set up your local git installation to work with SSH, you can just use the HTTPS url.

## Setting up Git

If you don't have git set up on your system, you can
[install it through the official website](https://git-scm.com/download/win). If you're comfortable
using the command line, this should be plenty. 

If you'd prefer to use a desktop application, 
[GitHub made a solid one](https://desktop.github.com/).

When you're installing git, make sure that it modifies the PATH variable to contain the git
executable. The official installer should do this, I'm less sure about the desktop application. If
you're having issues getting Unity to find git when you've installed the desktop application, just
download the official installation as well.

If you don't intend to use git yourself, but want it to be available for Unity, I'd just go with
the official installer. In fact, even if you'd like to use the desktop app, you could still keep
the official install around for Unity's sake. No one's going to force you to use the command line
interface if you don't want to.
