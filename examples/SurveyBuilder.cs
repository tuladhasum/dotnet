/*
Source: https://dotnetfiddle.net/SdmL89#
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SurveyBuilder
{
	public class Program
	{
		static public void Main()
		{
			var survey = new Survey("How much are you Programer?").AddQuestion(new SinglePicklistQuestion{Label = "Do you like programming?", Options = new List<PicklistOption>{new PicklistOption{Text = "Yes", Points = 20}, new PicklistOption{Text = "No", Points = 10}, }}).AddQuestion(new MultiPicklistQuestion{Label = "What programming languages do u know?", Options = new List<PicklistOption>{new PicklistOption{Text = "C#", Points = 10}, new PicklistOption{Text = "Ada", Points = 10}, new PicklistOption{Text = "Pascal", Points = 10}}}).AddQuestion(new SinglePicklistQuestion{Label = "How much Open Source project, where did you take part?", Options = new List<PicklistOption>{new PicklistOption{Text = "0", Points = 10}, new PicklistOption{Text = "1 - 2", Points = 20}, new PicklistOption{Text = "3 - 5", Points = 30}, new PicklistOption{Text = "more than 5", Points = 40}, }});
			Console.WriteLine("Hello! Let's determine how much you are programer.");
			var session = new SurveySession{Survey = survey, Answers = new List<Answer>()};
			foreach (var question in survey.Questions)
			{
				var answer = question.Ask();
				session.Answers.Add(answer);
			}

			session.Score = survey.GetScore(session.Answers);
			Console.WriteLine("Thank you, for participating in our survey!");
			Console.WriteLine("Your score is {0}.", session.Score);
		}
	}

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

	public abstract class Answer
	{
		public Question Question { get; set; }

		public int Points { get; set; }
	}

	public class TextQuestion : Question
	{
		private const int MaxUserInputTextLength = 128;
		protected override bool ValidateInput(string input, out string errorMessage)
		{
			if (input.Length > MaxUserInputTextLength)
			{
				errorMessage = string.Format("Input text is too long. Expected {0} or less characters.", MaxUserInputTextLength);
				return false;
			}

			errorMessage = null;
			return true;
		}

		protected override Answer CreateAnswer(string validInput)
		{
			return new TextAnswer{Text = validInput, Question = this};
		}
	}

	public class TextAnswer : Answer
	{
		public string Text { get; set; }
	}

	public class NumberQuestion : Question
	{
		private const int IntegerMaxTextLength = 10;
		protected override bool ValidateInput(string input, out string errorMessage)
		{
			if (input.Length > IntegerMaxTextLength)
			{
				errorMessage = string.Format("Input text is too long. Expected {0} or less characters.", IntegerMaxTextLength);
				return false;
			}

			int value;
			if (!int.TryParse(input, out value))
			{
				errorMessage = "Incorrect format for number.";
				return false;
			}

			errorMessage = null;
			return true;
		}

		protected override Answer CreateAnswer(string validInput)
		{
			return new NumberAnswer{Number = int.Parse(validInput), Question = this};
		}
	}

	public class NumberAnswer : Answer
	{
		public int Number { get; set; }
	}

	public class DateQuestion : Question
	{
		const string DateFormat = "dd/mm/yyyy";
		private const string ParseFormat = "dd/MM/yyyy";
		protected override void PrintQuestion()
		{
			Console.WriteLine(Label);
			Console.WriteLine("Please, input data in following format: \"{0}\"", DateFormat);
		}

		protected override bool ValidateInput(string input, out string errorMessage)
		{
			if (input.Length > DateFormat.Length)
			{
				errorMessage = string.Format("Input text is too long. Expected input in format \"{0}\".", DateFormat);
				return false;
			}

			DateTime date;
			if (!DateTime.TryParseExact(input, ParseFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				errorMessage = "Incorrect format for date.";
				return false;
			}

			errorMessage = null;
			return true;
		}

		protected override Answer CreateAnswer(string validInput)
		{
			return new DateAnswer{Date = DateTime.ParseExact(validInput, ParseFormat, CultureInfo.InvariantCulture), Question = this};
		}
	}

	public class DateAnswer : Answer
	{
		public DateTime Date { get; set; }
	}

	public abstract class PickListQuestion : Question
	{
		public List<PicklistOption> Options { get; set; }

		protected override void PrintQuestion()
		{
			Console.WriteLine(Label);
			int optionNumber = 1;
			foreach (var choice in Options)
			{
				Console.WriteLine("[{0}] {1}", optionNumber, choice.Text);
				optionNumber++;
			}
		}
	}

	public class MultiPicklistQuestion : PickListQuestion
	{
		protected override void PrintQuestion()
		{
			base.PrintQuestion();
			Console.Write("Please, select option numbers.");
			Console.WriteLine("Input numbers in format \"<num1>,<num2>,<num3>,...\"");
			Console.WriteLine("For example: 1, 3, 4");
		}

		protected override bool ValidateInput(string input, out string errorMessage)
		{
			var optionNumbers = input.Split(',');
			if (optionNumbers.Length > Options.Count)
			{
				errorMessage = "You select more options, than available.";
				return false;
			}

			var options = new List<int>();
			foreach (var textOption in optionNumbers)
			{
				int optionNumber;
				if (!int.TryParse(textOption, out optionNumber))
				{
					errorMessage = "Incorrect format for option number.";
					return false;
				}

				options.Add(optionNumber);
			}

			errorMessage = null;
			return true;
		}

		protected override Answer CreateAnswer(string validInput)
		{
			var optionNumbers = validInput.Split(',').Select(num => int.Parse(num.Trim())).ToList();
			var selectedOptions = optionNumbers.Select(choiceNumber => Options[choiceNumber - 1]).ToList();
			return new MultiPicklistAnswer{Options = selectedOptions, Question = this, Points = selectedOptions.Sum(op => op.Points)};
		}
	}

	public class PicklistOption
	{
		public string Text { get; set; }

		public int Points { get; set; }
	}

	public class MultiPicklistAnswer : Answer
	{
		public List<PicklistOption> Options { get; set; }
	}

	public class SinglePicklistQuestion : PickListQuestion
	{
		protected override void PrintQuestion()
		{
			base.PrintQuestion();
			Console.WriteLine("Please, select one option number.");
		}

		protected override bool ValidateInput(string input, out string errorMessage)
		{
			int number;
			if (!int.TryParse(input, out number))
			{
				errorMessage = "Incorrect format for option number.";
				return false;
			}

			if (number <= 0)
			{
				errorMessage = "Option number should be 1 or greater.";
				return false;
			}

			if (number > Options.Count)
			{
				errorMessage = "Option number should be {0} or less.";
				return false;
			}

			errorMessage = null;
			return true;
		}

		protected override Answer CreateAnswer(string validInput)
		{
			var selectedOption = Options[int.Parse(validInput) - 1];
			return new SinglePicklistAnswer{PicklistOption = selectedOption, Question = this, Points = selectedOption.Points};
		}
	}

	public class SinglePicklistAnswer : Answer
	{
		public PicklistOption PicklistOption { get; set; }
	}

	public class Survey
	{
		public Survey(string name)
		{
			Name = name;
			Questions = new List<Question>();
		}

		public string Name { get; set; }

		public List<Question> Questions { get; set; }

		public Survey AddQuestion(Question question)
		{
			Questions.Add(question);
			return this;
		}

		public int GetScore(List<Answer> answers)
		{
			return answers.Sum(a => a.Points);
		}
	}

	public class SurveySession
	{
		public Survey Survey { get; set; }

		public List<Answer> Answers { get; set; }

		public int Score { get; set; }
	}
}
