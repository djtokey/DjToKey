DjToKey
=======

[![Build status](https://ci.appveyor.com/api/projects/status/0sb4u0damy7p1x79/branch/master?svg=true)](https://ci.appveyor.com/project/ktos/djtokey/branch/master)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/ktos/DjToKey/devel/LICENSE)
[![GitHub release](https://img.shields.io/github/release/ktos/djtokey.svg)]()

An open source mapper from different controllers (currently MIDI) to custom JavaScript scripts for mouse/keyboard simulation.

## Short description

DjToKey allows you to prepare custom scripts fired when some events on your MIDI
device happens. Originally built for Hercules DJControl MP3 LE device, it allows
mouse and keyboard simulation and other actions, bound to your DJ console.

For example, you can bind your deck wheel from DJ console to a mouse wheel. 
All scripts are written in JavaScript and run on V8 engine.

## Releases and changelog

The current release is [DjToKey version 0.4.0](https://github.com/ktos/DjToKey/releases/download/v0.4/DjToKey-0.4.0.master.2d5bee0.zip).

For the more information (changelog, source code downloads), head over to [releases](https://github.com/ktos/DjToKey/releases) section.
All releases are only available as ZIP files, there is no installer at the moment.
All releases are build for the x86 (32-bit) platform.

## Requirements
DjToKey requires [Visual C++ Redistruable 2013](https://www.microsoft.com/en-us/download/details.aspx?id=40784)
for x86 to be installed on the computer, and some kind of supported hardware.

## Hardware
This software was made for Hercules DJControl MP3 LE MIDI device and has mappings
only for this device at the moment. If you would like to use it for another device,
there is a need to prepare a JSON file describing all possible control ids, names
and types on a device, named as it is visible in the operating system, and packaging it
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
You are welcome to contribute to DjToKey! Create issues, Pull Requests, support
new hardware, add new core features. More informaction is available in the 
[Contributing](CONTRIBUTING.md) document.

## Additional information

Copyright &copy; Marcin Badurowicz 2015-2016. Licensed under [MIT license](/LICENSE).

* Icon used from: <https://icons8.com/>
* Script engine: [Microsoft ClearScript](https://clearscript.codeplex.com/)
* JSON (de)serialization: Newtonsoft.Json

Have a lot of fun.

--ktos
