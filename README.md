DjToKey
=======

This is a very basic MIDI-controller to script mapper. It allows you to prepare
custom scripts for moving mouse, pressing keys and similar things, fired every 
time some action on your MIDI device occurs. For example, you can bind your 
deck wheel from DJ console to a mouse wheel.

A scripts are written in JavaScript, run on V8 engine.

Copyright (C) Marcin Badurowicz 2015

* Icon used from: <https://icons8.com/>
* Script engine: [Microsoft ClearScript](https://clearscript.codeplex.com/)
* JSON (de)serialization: Newtonsoft.Json

## Script objects available

There is a set of few objects available for manipulating mouse and keyboard
from your script.

### Document object
* `Document.Alert(string)` - shows message on a screen

### Keyboard object
* `Keyboard.KeyDown(key)` - executes key down on a virtual key. Keycodes are available in KeyCode object,
* `Keyboard.KeyUp(key)` - executes key up
* `Keyboard.KeyPress(key)` - presses a virtual key
* `Keyboard.TextEntry(string)` - sends text entry to a active window
* `Keyboard.Sleep(number)` - sleeps for a number of miliseconds

### Mouse object
* `Mouse.HorizontalScroll(number)` - scrolls for a number of lines - negative goes down, positive - up
* `Mouse.VerticalScroll(number)` - scroll horizontally for a number of lines
* `Mouse.LeftButtonClick()` - simulates left button click
* `Mouse.LeftButtonDoubleClick()` - left button double click
* `Mouse.LeftButtonDown()` - left button down
* `Mouse.LeftButtonUp()` - left button up
* `Mouse.MoveMouseTo(number, number)` - moves cursor to a position X, Y on the screen, respectively 
* `Mouse.MoveMouseBy(number, number)` - moves mouse cursor by a given delta
* `Mouse.RightButtonClick()` - simulates right mouse button
* `Mouse.RightButtonDoubleClick()`
* `Mouse.RightButtonDown()`
* `Mouse.RightButtonUp()`
* `Mouse.XButtonClick()` - simulates third mouse button
* `Mouse.XButtonDoubleClick()`
* `Mouse.XButtonDown()`
* `Mouse.XButtonUp()`

### Control object
When handling an event, there is Control object available which has some information about
what control has been pressed and what is it's id, name and type. Value of a signal is in 
Value object.

* `Control.ControlId` - ID of control activated
* `Control.ControlName` - name of control or button
* `Control.ControlType` - type of control - Analog (0-127), Digital (-1, 1) or Button

### Value object
Value is an object with value send from MIDI device

* `Value.Raw` - raw value send from the interface
* `Value.Transformed` - if the value is 127, this returns 1, else -1. It is useful in situations
where there is some control returning only Digital values (127 and 0 or 127 and 1), you don't
have to work on value to get meaning, it is done automatically.

## Hardware
This software was made for Hercules DJControl MP3 LE MIDI device and has mappings only for
this device. If you would like to use it for another device, there is a need to prepare
a JSON file describing all possible control ids, names and types on a device, named as it is
visible in the application.

## Examples
```JavaScript
Mouse.VerticalScroll(Value.Transformed)
```

This script is taking value send from the device and translates it into either 1 or -1. And then
runs VerticalScroll function which simulates vertical scrolling. Positive value is scrolling up,
while negative - down.

```JavaScript
if (Value.Raw == 0) Keyboard.TextEntry("Magic!");
```

When button is down it sends a value of 127 and a event is fired. When it is up - it sends
a value of 0. To achieve only one action, it is fired only when button is going up. Then,
it uses TextEntry to put some verbatim string into active window.

Have a lot of fun.

--ktos