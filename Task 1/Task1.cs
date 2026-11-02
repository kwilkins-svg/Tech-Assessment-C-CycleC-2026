// Instructions: Write a function (use template below to get started) that takes a list of integers and returns a list of only the positive even numbers, doubled. Add at least one error-handling condition

// For submission, commit and push the edited file to Github

// Start the function (Start of Template)

public class Task1
{
    static void Main()
    {
        List<object> myNumbers = new List<object> { -1, 2, "failed", -4, 6, null, 18 };
        List<int> result = GetDoubledPositiveEvens(myNumbers);
        Console.WriteLine(string.Join(", ", result)); 
    }

    public static List<int> GetDoubledPositiveEvens(List<object> numbers)
    {
        if (numbers == null)
        {
            throw new ArgumentNullException(nameof(numbers), "Input list cannot be null.");
        }

        List<int> doubledEvens = new List<int>();

        foreach (var item in numbers)
        {
            if (item is int number && number > 0 && number % 2 == 0)
            {
                doubledEvens.Add(number * 2);
            }
        }

        return doubledEvens;
    }   

}