# Inheritance

Example - https://dotnetcademy.net/Learn/2045/Pages/16

Good Explanation - https://stackoverflow.com/questions/391483/what-is-the-difference-between-an-abstract-method-and-a-virtual-method

__An abstract function cannot have functionality.__ You're basically saying, any child class MUST give their own version of this method, however it's too general to even try to implement in the parent class.

__A virtual function__, is basically saying look, here's the functionality that may or may not be good enough for the child class. So if it is good enough, use this method, if not, then override me, and provide your own functionality.


```cs
public abstract class Question
{
    public string Label { get; set; }

    public Answer Ask()
    {
        PrintQuestion();

        while (true)
        {
            var input = Console.ReadLine();
            string errorMessage;
            if (!ValidateInput(input, out errorMessage))
            {
                Console.WriteLine(errorMessage);
                Console.WriteLine("Please, correct your input.");
                continue;
            }

            return CreateAnswer(input);
        }
    }

    protected virtual void PrintQuestion()
    {
        Console.WriteLine(Label);   
    }

    protected abstract bool ValidateInput(string input, out string errorMessage);
    protected abstract Answer CreateAnswer(string validInput);
}
```

```cs
public class TextQuestion : Question
{
    private const int MaxUserInputTextLength = 128;

    protected override bool ValidateInput(string input, out string errorMessage)
    {
        if (input.Length > MaxUserInputTextLength)
        {
            errorMessage = string.Format("Input text is too long. Expected {0} or less characters.", MaxUserInputTextLength;
            return false;
        }

        errorMessage = null;
        return true;
    }

    protected override Answer CreateAnswer(string validInput)
    {
        return new TextAnswer { Text = validInput, Question = this };
    }
} 
```
