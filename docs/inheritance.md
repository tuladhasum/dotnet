# Inheritance

Example - https://dotnetcademy.net/Learn/2045/Pages/16

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
