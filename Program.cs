using System;
using System.Reflection;
using CodilityRunner;

class Program
{
    static void Main(string[] args)
    {
        // Create a test input like Codility would provide
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-input.txt");
        string[] lines = File.ReadAllLines(filePath);

        int[] H = lines[0].Split().Select(int.Parse).ToArray(); //First line is the array H
        int X = int.Parse(lines[1]); // Second line is the integer X
        int Y = int.Parse(lines[2]); // Third line is the integer Y

        // Dynamically find and invoke the solution method
        Type solutionType = typeof(Solution);
        var method = solutionType.GetMethod("solution");
        var instance = Activator.CreateInstance(solutionType);

        // Invoke the method and print result
        object result = method.Invoke(instance, new object[] { H, X, Y });
        Console.WriteLine("Result: " + result);
    }
}
