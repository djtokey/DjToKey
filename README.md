DjToKey
=======

[![Build status](https://ci.appveyor.com/api/projects/status/0sb4u0damy7p1x79/branch/master?svg=true)](https://ci.appveyor.com/project/ktos/djtokey/branch/master)

DjToKey allows you to prepare custom scripts fired when some events on your MIDI
device happens. Originally built for Hercules DJControl MP3 LE device, it allows
mouse simulation, keyboard simulation and other actions, bound to your DJ console.

For example, you can bind your deck wheel from DJ console to a mouse wheel. 
All scripts are written in JavaScript and run on V8 engine.

Copyright (C) Marcin Badurowicz 2015

* Icon used from: <https://icons8.com/>
* Script engine: [Microsoft ClearScript](https://clearscript.codeplex.com/)
* JSON (de)serialization: Newtonsoft.Json


## Hardware
This software was made for Hercules DJControl MP3 LE MIDI device and has mappings
only for this device. If you would like to use it for another device, there is a
need to prepare a JSON file describing all possible control ids, names and types
on a device, named as it is visible in the operating system, and packaging it
into `.dtkpkg` file.

Creating custom device files will be easier when simple authoring software will
be released, later this year.

## Scripting
A guide to all objects and possible functions is available in 
[Scripting](docs/scripting.md) document.

## Plugins
Since version 0.3, DjToKey is a lot more modular. A quick
guide to possible plugins is avaliable in [Plugins](docs/plugins.md) document.

## Contributing


Have a lot of fun.

--ktos
