Plugins
=======

Thanks to `System.ComponentModel.Composition` from first MEF, DjToKey is now
a bit more modular application.

Plugin architecture is based on a class library called PluginContracts, and
your plugins needs to reference it to have proper interfaces to implement.

PluginContracts is versioned differently than DjToKey itself, and with
SemVer in mind, so this means that after releasing first public version,
public API will not change without major version change.

The current version of PluginContracts API is 0.9.0.

## General plugins overview
Plugins are class libraries (*.dll) built referencing `System.ComponentModel.Composition`
and `Ktos.DjToKey.Plugins.Contracts` (in a specific version). The Composition
is needed to mark exported objects and types. DjToKey will try only to load
files with DLL extension from \plugins directory (relative to the application),
and from %LOCALAPPDATA%\DjToKey\plugins directory.

## Possible plugins
There are a few category of plugins you can create:
* Script objects,
* Script types,
* Device handlers.

### Script objects
You can add new objects which will be available to scripts run in DjToKey.
To create a script object plugin, you need to prepare a class implementing
`IScriptObject` interface. 

The interface has 2 fields to be in a plugin class:
* Name (string) - name of an object added to script engine
* Object (object) - an instance of a class to be added to script engine as a object.

The important part is that Object must be an instance, so you have to create
an object for yourself.

Example script object plugin may look like this:

```csharp
/// <summary>
/// A very basic sample of a plugin - when in plugins directory for a DjToKey,
/// it automatically registers object called "TestPlugin" with a "DoWork" method
/// returning "Hello, world!" message.
/// </summary>
[Export(typeof(IScriptObject))]
public class TestPlugin : IScriptObject
{
    public string Name
    {
        get
        {
            return "TestPlugin";
        }
    }

    public object Object
    {
        get
        {
            return tpi;
        }
    }

    private TestPluginImpl tpi;

    public TestPlugin()
    {
        tpi = new TestPluginImpl();
    }
}

public class TestPluginImpl
{
    public string DoWork()
    {
        return "Hello, world!";
    }
}
```

It is a common convention for a DjToKey plugins prepared to have a class which
implements interface and a class which is a really class which instance is
registered as a script object. In this example, there is TestPlugin object
registered (because of Name property) with a `DoWork()` method.

The `[Export]` attribute is a sign for MEF to import every found class with
such attribute into a proper list in a DjToKey app to put into script engine
later.

In DjToKey script it will be run like this, for example:

```javascript
var hello = TestPlugin.DoWork()
Document.Alert(hello);
```

### Script types
There is sometimes a need not only to add some object with methods, but also
a class, so the user can make objects for himself. Or, maybe there is a need
to pass enum to script, to have meaningful names instead of magic numbers.

Let's talk about an example enum type:

```csharp
public enum Button
{
	Magic = 46,
	Vinyl = 45,
	Up = 41,
	Down = 42,
	Load = 44,
	Files = 43
}
```

This enum to be registered in script and to work properly must be presented
in a form of IScriptType implementation. For example:

```csharp
[Export(typeof(IScriptType))]
public class DjButton : IScriptType
{
	private const string objName = "DjButton";

	public string Name
	{
		get
		{
			return objName;
		}
	}

	public Type Type
	{
		get
		{
			return typeof(Button);
		}
	}
}
```

Again there is `[Export]` attribute, but now interface makes us to implement
Name, the name of a type in a script engine, and a Type itself. With enum, in
script engine, there is a possibility to achieve something like this:

```javascript
var x = DjButton.Magic - DjButton.Vinyl;
Document.Alert(x + "!"); // x is now 1
```

### Device handlers
The device handler plugin must implement `IDeviceHandler` interface. A device
handler will provide to the DjToKey the `IEnumerable<string>` of devices
available to handle. Not all of this device may be actually handled, as this
list is tested against Device Packages in `\devices` directory. If there is
no suitable Device Package, device will not be presented to the user.

The most important method for a device handler is the `Load(string deviceName)`,
executed when user wants to use the device of the specified name. This should
populate the `IEnumerable<Control> Controls` giving application all possible
controls for application. The DjToKey will populate `IList<ControlBinding> Bindings`,
matching scripts with the Controls.

Device handler will be given also the IScriptEngine instance, with the Script Engine,
and can handle the `EventHandler<ScriptErrorEventArgs> ScriptErrorOccured` event.
It also have to return currently active device as `ActiveDevice` property.

Method `void Unload()` is executed by the application if the user is closing the
application or is switching to another device.