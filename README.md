# Auburn Atelier Unity Core

Code shared between Auburn Atelier projects.

If you have git installed on your machine, you can
[follow the official unity instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
to add this as a package using the following url:
```
https://github.com/devynms/UnityAtelier.git#v0.1.0
```

For more details, follow the [Installation Instructions](#installation-instructions) below.


## Project Status

This is a personal project, initially made public for the 2024 Cleveland Game Developers Game Jam.
I may be able to respond to issues or handle pull requests for major bugs, but I can't promise much.

---

## Table Of Contents
* [Installation Instructions](#installation-instructions)
  - [Downloading the Code](#downloading-the-code)
  - [Installing as a local package](#installing-as-a-local-package)
  - [Installing via Git](#installing-via-git)
  - [Setting up Git](#setting-up-git)
* [Project Overview](#project-overview)
  - [State Machines](#state-machines)
  - [2D Character Controller](#2d-character-controller)
  - [Containers](#containers)
  - [Messages and Events](#messages-and-events)
  - [Misc Code](#misc-code)
  - [Presets](#presets)
  - [Tilemap](#tilemap)

---

## Installation Instructions

There are three main methods you could use to install this project:
1. Just download it and copy-paste all the files into your projects assets.
2. Download it, and install the package locally from disk.
3. Directly add the git repostiory as a reference

If you're familiar with git (or willing to become familiar), I recommend option #3. Instructions for 
installing git can be found at the bottom of this readme. Options #1 and #2 might seem easier, but
option #3 has the advantage that if you want to download your code onto another machine, you won't
need to manually re-download and install this package.

### Downloading the Code

If you're either (1) dowloading and copy-pasting the code directly, or (2) downloading it, but 
installing it as a package, you'll have to start by... downloading the code! Just click the green 
"< > Code" button near the top-right of the git repository main page, and hit "Download ZIP".

If you're just copy-pasting, you can take every thing in the main folder and drag it into your
project's asset folder - either through the Unity Editor, or in your file explorer.

### Installing as a local package

If you've download the code, but you'd like to use it as a proper unity package, then after you've
unzipped the contents of this repository, you can 
[follow the official unity instructions](https://docs.unity3d.com/Manual/upm-ui-local.html) for 
installing a local package.

I recommend being mindful of where you put the directory, since when you do this, Unity won't copy
the project files over to your project. I'd extract the ZIP to a folder sitting next to your Unity
project.

### Installing via Git

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

You can also [target a specific revision](https://docs.unity3d.com/Manual/upm-git.html#revision)
with this system.

This is the current revision, using an https link:
```
https://github.com/devynms/UnityAtelier.git#v0.1.0
```

If you haven't set up your local git installation to work with SSH, you can just use the HTTPS url.

### Setting up Git

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

---

## Project Overview

This project mostly contains a bunch of useful code - components, scriptable objects, helper
classes, some editor extensions. It also contains some
[presets](https://docs.unity3d.com/Manual/Presets.html) I've found to be helpful. Finally, it
contains one bare-bones tileset I've used when setting up projects. Most of the information about
specific code can be found as documentation on that code.

### State Machines

Some helper code that I've found very useful for setting up state machines for game entities. 
These state machines can be used to set up extremely simple/mechanical object states, can be
used in a player-controller MonoBehaviour to maniupulate the player object, or multiple state
machines can even be layered together to create a sort of AI (for example; to separate an AI
behavior state machine from a mechanical object-state state machine).

An example of how to use this has been kludged into a doc-string on the 
[StateMachine source](Runtime/Actors/StateMachine.cs).

### 2D Character Controller

Compared to the 3D `CharacterController` MonoBehaviour, manipulating kinematic `RigidBody2D`s always
felt very clunky. I studied the source code implementing Godot's KinematicBody2D, and used it to
implement my own KinematicBody2D.

See the [KinematicBody2D source](Runtime/Physics2D/KinematicBody2D.cs) for more details.

[KinematicPlatform](Runtime/Physics2D/KinematicPlatform.cs) and 
[KinematicMobile](Runtime/Physics2D/KinematicMobile.cs) can also be used together to implement
platforms. Attach KinematicPlatform to an object with a KinematicBody2D to implement the platform,
and attach KinematicMobile to character using KinematicBody2D to implement message recievers
for platform move events.

### Containers

I implemented a simplistic "container" system, that allows prefabs to reference other objects that
will end up in the same scene as them (without needing a direct object link).

See [GameObjectContainer](Runtime/Containers/GameObjectContainer.cs) and 
[GameObjectRegistrar](Runtime/Containers/GameObjectRegistrar.cs) for more details.

### Messages and Events

Similar in some ways to the container system, I implemented a "message" system, which allows message
types to be encoded as scriptable objects, which then get dispatched to recievers via unity events
which can be code-lessly in the editor.

See the [Messages](Runtime/Messages/) folder.

This system is definitely a bit messier than the containers system. See the 
[MessageType source](Runtime/Messages/MessageType.cs) for a full rant about that.

There are also some helpers designed to help trigger / receive `UnityEvent`s. Poke around the
[Events](Runtime/Events/) folder. See 
[TimedSequence](Runtime/Events/TimedSequence.cs) in particular to get the gist of those. This is all
just getting stuff that you could perfectly well handle via code into components that can be
manipulated in the editor.

### Misc Code

There are some audio related helpers in [Audio](Runtime/Audio/). Check out 
[AudioPanel](Runtime/Audio/AudioPanel.cs) + [PanelPlayer](Runtime/Audio/PanelPlayer.cs).

[GameObjectTracker](Runtime/Cameras/GameObjectTracker.cs) re-implements a basic version of unity's
built-in [constraint behaviours](https://docs.unity3d.com/Manual/Constraints.html) in terms of object
containers.

There are some helpers for dealing with 2D 8-way directions (see 
[EightwayDirection](Runtime/Eightway/EightwayDirection.cs)).

There are some helpers for adding Scenes (as the `string` scene name) as fields with 
`[SerializeField]`. See [SceneAttribute](Runtime/Scenes/SceneAttribute.cs).

### Presets

There are some helpful presets in the [Presets](Presets/) folder. 

The [Sprites](Presets/Sprites/) sub-folder contains some `TextureImporter` presets which I've found useful when
dealing with pixel-perfect pixel art.

The [Sounds](Presets/Sounds/) presets for the `AudioImporter` are more generally useful, and should be self-obvious
as well.

### Tilemap

In the [Tilemaps](Tilemaps/Proto/) folder. Just a dumb bare-bones tilemap. The sprites are in 
[Sprites/Tiles](Sprites/Tiles/).
