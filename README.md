DjToKey
=======

Dostępne obiekty i ich funkcje:

Document.Alert(s) - wyświetlenie komunikatu na ekranie

Keyboard.KeyDown(k)
Keyboard.KeyUp(k)
Keyboard.KeyPress(k)
Keyboard.TextEntry(s)
Keyboard.Sleep(x)

Mouse.HorizontalScroll(x)
Mouse.VerticalScroll(x)
Mouse.LeftButtonClick()
Mouse.LeftButtonDoubleClick()
Mouse.LeftButtonDown()
Mouse.LeftButtonUp()
Mouse.MoveMouseTo(x, y)
Mouse.MoveMouseBy(x, y)
Mouse.RightButtonClick()
Mouse.RightButtonDoubleClick()
Mouse.RightButtonDown()
Mouse.RightButtonUp()
Mouse.XButtonClick()
Mouse.XButtonDoubleClick()
Mouse.XButtonDown()
Mouse.XButtonUp()

Control.ControlId - ID naciśniętego
Control.ControlName - nazwa klawisza/kontrolki
Control.ControlType - typ kontrolki (Analog, Digital, Button)

Value.Raw - wartość przekazana przez intefejs
Value.Transformed - wartość transponowana z [0;127] lub [1;127] na [-1;1]