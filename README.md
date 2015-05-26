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

#### KeyCode object
Keycode object is used to get numeric codes for all keys on your keyboard. Key names are directly imported
from Windows API, so one may find them confusing.

* `KeyCode.CANCEL` - Control-break processing
* `KeyCode.BACK` - BACKSPACE key
* `KeyCode.TAB` - TAB key
* `KeyCode.CLEAR` - CLEAR key
* `KeyCode.RETURN` - ENTER key
* `KeyCode.SHIFT` - SHIFT key
* `KeyCode.CONTROL` - CTRL key
* `KeyCode.MENU` - ALT key
* `KeyCode.PAUSE` - PAUSE key
* `KeyCode.CAPITAL` - CAPS LOCK key* `KeyCode.KANA` - Input Method Editor (IME) Kana mode
* `KeyCode.HANGEUL` - IME Hanguel mode (maintained for compatibility; use HANGUL)
* `KeyCode.HANGUL` - IME Hangul mode
* `KeyCode.JUNJA` - IME Junja mode
* `KeyCode.FINAL` - IME final mode
* `KeyCode.HANJA` - IME Hanja mode
* `KeyCode.KANJI` - IME Kanji mode
* `KeyCode.ESCAPE` - ESC key
* `KeyCode.CONVERT` - IME convert
* `KeyCode.NONCONVERT` - IME nonconvert
* `KeyCode.ACCEPT` - IME accept
* `KeyCode.MODECHANGE` - IME mode change request
* `KeyCode.SPACE` - SPACEBAR
* `KeyCode.PRIOR` - PAGE UP key
* `KeyCode.NEXT` - PAGE DOWN key
* `KeyCode.END` - END key
* `KeyCode.HOME` - HOME key
* `KeyCode.LEFT` - LEFT ARROW key
* `KeyCode.UP` - UP ARROW key
* `KeyCode.RIGHT` - RIGHT ARROW key
* `KeyCode.DOWN` - DOWN ARROW key
* `KeyCode.SELECT` - SELECT key
* `KeyCode.PRINT` - PRINT key
* `KeyCode.EXECUTE` - EXECUTE key
* `KeyCode.SNAPSHOT` - PRINT SCREEN key
* `KeyCode.INSERT` - INS key
* `KeyCode.DELETE` - DEL key
* `KeyCode.HELP` - HELP key
* `KeyCode.VK_0` - 0 key
* `KeyCode.VK_1` - 1 key
* `KeyCode.VK_2` - 2 key
* `KeyCode.VK_3` - 3 key
* `KeyCode.VK_4` - 4 key
* `KeyCode.VK_5` - 5 key
* `KeyCode.VK_6` - 6 key
* `KeyCode.VK_7` - 7 key
* `KeyCode.VK_8` - 8 key
* `KeyCode.VK_9` - 9 key
* `KeyCode.VK_A` - A key
* `KeyCode.VK_B` - B key
* `KeyCode.VK_C` - C key
* `KeyCode.VK_D` - D key
* `KeyCode.VK_E` - E key
* `KeyCode.VK_F` - F key
* `KeyCode.VK_G` - G key
* `KeyCode.VK_H` - H key
* `KeyCode.VK_I` - I key
* `KeyCode.VK_J` - J key
* `KeyCode.VK_K` - K key
* `KeyCode.VK_L` - L key
* `KeyCode.VK_M` - M key
* `KeyCode.VK_N` - N key
* `KeyCode.VK_O` - O key
* `KeyCode.VK_P` - P key
* `KeyCode.VK_Q` - Q key
* `KeyCode.VK_R` - R key
* `KeyCode.VK_S` - S key
* `KeyCode.VK_T` - T key
* `KeyCode.VK_U` - U key
* `KeyCode.VK_V` - V key
* `KeyCode.VK_W` - W key
* `KeyCode.VK_X` - X key
* `KeyCode.VK_Y` - Y key
* `KeyCode.VK_Z` - Z key
* `KeyCode.LWIN` - Left Windows key (Microsoft Natural keyboard)
* `KeyCode.RWIN` - Right Windows key (Natural keyboard)
* `KeyCode.APPS` - Applications key (Natural keyboard)
* `KeyCode.SLEEP` - Computer Sleep key
* `KeyCode.NUMPAD0` - Numeric keypad 0 key
* `KeyCode.NUMPAD1` - Numeric keypad 1 key
* `KeyCode.NUMPAD2` - Numeric keypad 2 key
* `KeyCode.NUMPAD3` - Numeric keypad 3 key
* `KeyCode.NUMPAD4` - Numeric keypad 4 key
* `KeyCode.NUMPAD5` - Numeric keypad 5 key
* `KeyCode.NUMPAD6` - Numeric keypad 6 key
* `KeyCode.NUMPAD7` - Numeric keypad 7 key
* `KeyCode.NUMPAD8` - Numeric keypad 8 key
* `KeyCode.NUMPAD9` - Numeric keypad 9 key
* `KeyCode.MULTIPLY` - Multiply key
* `KeyCode.ADD` - Add key
* `KeyCode.SEPARATOR` - Separator key
* `KeyCode.SUBTRACT` - Subtract key
* `KeyCode.DECIMAL` - Decimal key
* `KeyCode.DIVIDE` - Divide key
* `KeyCode.F1` - F1 key
* `KeyCode.F2` - F2 key
* `KeyCode.F3` - F3 key
* `KeyCode.F4` - F4 key
* `KeyCode.F5` - F5 key
* `KeyCode.F6` - F6 key
* `KeyCode.F7` - F7 key
* `KeyCode.F8` - F8 key
* `KeyCode.F9` - F9 key
* `KeyCode.F10` - F10 key
* `KeyCode.F11` - F11 key
* `KeyCode.F12` - F12 key
* `KeyCode.F13` - F13 key
* `KeyCode.F14` - F14 key
* `KeyCode.F15` - F15 key
* `KeyCode.F16` - F16 key
* `KeyCode.F17` - F17 key
* `KeyCode.F18` - F18 key
* `KeyCode.F19` - F19 key
* `KeyCode.F20` - F20 key
* `KeyCode.F21` - F21 key
* `KeyCode.F22` - F22 key
* `KeyCode.F23` - F23 key
* `KeyCode.F24` - F24 key
* `KeyCode.NUMLOCK` - NUM LOCK key
* `KeyCode.SCROLL` - SCROLL LOCK key
* `KeyCode.LSHIFT` - Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
* `KeyCode.RSHIFT` - Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
* `KeyCode.LCONTROL` - Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
* `KeyCode.RCONTROL` - Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
* `KeyCode.LMENU` - Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
* `KeyCode.RMENU` - Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
* `KeyCode.BROWSER_BACK` - Windows 2000/XP: Browser Back key
* `KeyCode.BROWSER_FORWARD` - Windows 2000/XP: Browser Forward key
* `KeyCode.BROWSER_REFRESH` - Windows 2000/XP: Browser Refresh key
* `KeyCode.BROWSER_STOP` - Windows 2000/XP: Browser Stop key
* `KeyCode.BROWSER_SEARCH` - Windows 2000/XP: Browser Search key
* `KeyCode.BROWSER_FAVORITES` - Windows 2000/XP: Browser Favorites key
* `KeyCode.BROWSER_HOME` - Windows 2000/XP: Browser Start and Home key
* `KeyCode.VOLUME_MUTE` - Windows 2000/XP: Volume Mute key
* `KeyCode.VOLUME_DOWN` - Windows 2000/XP: Volume Down key
* `KeyCode.VOLUME_UP` - Windows 2000/XP: Volume Up key
* `KeyCode.MEDIA_NEXT_TRACK` - Windows 2000/XP: Next Track key
* `KeyCode.MEDIA_PREV_TRACK` - Windows 2000/XP: Previous Track key
* `KeyCode.MEDIA_STOP` - Windows 2000/XP: Stop Media key
* `KeyCode.MEDIA_PLAY_PAUSE` - Windows 2000/XP: Play/Pause Media key
* `KeyCode.LAUNCH_MAIL` - Windows 2000/XP: Start Mail key
* `KeyCode.LAUNCH_MEDIA_SELECT` - Windows 2000/XP: Select Media key
* `KeyCode.LAUNCH_APP1` - Windows 2000/XP: Start Application 1 key
* `KeyCode.LAUNCH_APP2` - Windows 2000/XP: Start Application 2 key
* `KeyCode.OEM_1` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the ';:' key 
* `KeyCode.OEM_PLUS` - Windows 2000/XP: For any country/region, the '+' key
* `KeyCode.OEM_COMMA` - Windows 2000/XP: For any country/region, the ',' key
* `KeyCode.OEM_MINUS` - Windows 2000/XP: For any country/region, the '-' key
* `KeyCode.OEM_PERIOD` - Windows 2000/XP: For any country/region, the '.' key
* `KeyCode.OEM_2` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '/?' key 
* `KeyCode.OEM_3` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '`~' key 
* `KeyCode.OEM_4` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '[{' key
* `KeyCode.OEM_5` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '\|' key
* `KeyCode.OEM_6` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the ']}' key
* `KeyCode.OEM_7` - Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote' key
* `KeyCode.OEM_8` - Used for miscellaneous characters; it can vary by keyboard.
* `KeyCode.OEM_102` - Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
* `KeyCode.PROCESSKEY` - Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
* `KeyCode.PACKET` - Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
* `KeyCode.ATTN` - Attn key
* `KeyCode.CRSEL` - CrSel key
* `KeyCode.EXSEL` - ExSel key
* `KeyCode.EREOF` - Erase EOF key
* `KeyCode.PLAY` - Play key
* `KeyCode.ZOOM` - Zoom key
* `KeyCode.NONAME` - Reserved
* `KeyCode.PA1` - PA1 key
* `KeyCode.OEM_CLEAR` - Clear key

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

### Global object
`Global` is an object with two functions: 
* `Set(key, value)` - sets (or adds) a key with a given value,
* `Get(key)` - retrieves a value for a given key.

It is common between all scripts in application, so you can set some value and use it in other
scripts. It is preserved during whole application life, emptied at startup.

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