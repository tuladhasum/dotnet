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
