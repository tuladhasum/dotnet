# Enum


```cs
using System;
					
public class Program
{
	// https://www.tutorialspoint.com/csharp/csharp_enums.htm
	enum Days { Sun, Mon, Tue, Wed, Thu, Fri, Sat }
	
	public static void Main()
	{
		int WeekdayStart = (int) Days.Mon;
		int WeekdayEnd = (int) Days.Fri;
		
		Console.WriteLine("Monday: {0}", WeekdayStart);
		Console.WriteLine("Friday: {0}", WeekdayEnd);
	}
}
```
