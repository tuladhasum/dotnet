# Virtual

The virtual keyword is a special access modifier which can be applied to type members of classes that are intended to be inherited.

```cs
using System;

public class Lightsaber
{
    public virtual string Color { get { return "Green"; } }
}

public class SithLightsaber : Lightsaber
{
    public override string Color { get { return "Red"; } }
}

public class Program
{
    public static void Main()
    {
        var lightsaber = new Lightsaber();
        Console.WriteLine(lightsaber.Color);

        var sithLightsaber = new SithLightsaber();
        Console.WriteLine(sithLightsaber.Color);
    }
}
```

The purpose of the virtual access modifier is to allow you to provide a default implementation of your property on your base class that can optionally be overriden on your derived class. It is similar to the abstract keywords, but carries optional semantics instead of required semantics.

The virtual keyword can be applied to all instance type members, but not constructors. Here is an example of a virtual method:

```cs
public virtual void Send()
{
    // Send via the console.
    Console.WriteLine(Content); 
}
```

When implemented on a derived type, the override keyword must be used to tell the compier that it is overriding the corresponding type member on the base class.

The virtual keyword cannot be applied to the class declaration, it is only for instance type members (non-static);