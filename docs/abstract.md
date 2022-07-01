# Abstract Class

[.NET Source](https://dotnetcademy.net/Learn/2045/Pages/12)

## The abstract keyword

There is a special modifier abstract which allows us to enforce certain semantics on the structure and use of our base classes. This is applied in two ways, either to the class declaration itself, or to type members, such as methods or properties.

## Abstract classes

When the abstract modifier is applied to a class, it enforces that the class must be inherited, and cannot be instantiated by itself:

```cs
public abstract class Animal { }

public class Dog : Animal { }

public class Program
{
    public static void Main()
    {
        var animal = new Animal(); // Not valid, as the class is abstract
        var dog = new Dog(); // This is fine.   
    }   
}
```

Through the use of the abstract keyword when applied to a class, it means that we can provide some base functionality that can be shared with derived types.

### Abstract type members

When the abstract member is applied to a type member, such as a method, or property, it enforces that the derived type must implement that member.

```cs
public abstract string Name { get; set; }

public abstract void MakeNoise();
```

Abstract members can be called like normal methods, but it allows the derived type to implement the desired functionality. An more complete example could be:

```cs
public abstract class Message
{
    public abstract void Send();
}

public class Email : Message
{
    public override void Send()
    {
        // Send the message via email.
    }   
}
```

In the above example, we've created a derived type called Email which implements the abstract method Send(). When implementing an abstract method, you must use the override keyword which tells the compiler that the method is overriding the corresponding method from the base class. The method that is marked as abstract must not have a method body, and must be terminated with a semicolon:

```cs
public abstract void Send();
```
