Contributing to DjToKey
=======================

First of all, thanks for your interest in contributing to DjToKey! :tada:

This project was started as a totally crazy prototype, but I felt it may be
kinda useful, and is actually pretty cool.

At the moment, I am working on polishing after a huge feature update, version
0.4 and the project is very close to consider as "finished", aka version 1.0.

But, you are welcome to start contributing!

## Submitting issues
* You can create an issue here, but please include as many details as possible 
with your report, especially if you found a bug,
* There will be a DTKDevicePackageCreate application released in the coming weeks,
where you would be able to prepare your own definition files for any MIDI
hardware found, so please do not fill issues on additional hardware support,
* You should try to search if a similar issue has already been submitted.

## Creating pull requests
* If you would like to introduce a new feature, some patch, small fix or 
anything else - you are welcome! Fork the repository, push your changes,
create a pull request. If I think the changes are good, I will merge your
PR. If not - you will receive feedback what is wrong and we can work
on resolution,
* Try to maintain coding style already implemented, do not change it,
* All public APIs should be documented and this is checked by StyleCop.

## Branching strategy
* The main development branch (and the default branch for GitHub) is "devel",
against which are most commits and pull requests,
* DjToKey uses a variant of Git Flow, with feature branches, release branches,
while the master is always considered "stable",
* Every release is merged to master and tagged.
