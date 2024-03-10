using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Numerics;
using Ai1_Search_Methods.SearchMethods;
using Spectre.Console;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods;

internal class Program
{
    Dictionary<string, SearchMethod> searchMethods = new()
    {
        { "breadth-first search", new BreathFirstSearch() },
        { "depth-first search", new DepthFirstSearch() },
        { "ID-DFS search", new IDDFS() },
        { "best-first search", new BestFirstSearch() },
        { "A* search", new AStarSearch() },
    };

    static void Main(string[] args) => new Program().RunMain();

    void RunMain()
    {
        Console.Clear();
        Console.Write("Hello, search methods!\nPress [enter] to begin.");
        Console.ReadLine();

        Console.Write("Loading datafiles...");
        LoadDataFiles();
        Console.WriteLine("Done");


        var cityNames = coordinates.Keys.Sort();
        var selectCityPrompt = new SelectionPrompt<string>().AddChoices(cityNames);

        //Pre-warm the jit so the code runs faster.
#if !DEBUG
        WarmJit();
#endif
        // RunEveryOption(); // DEBUG make sure every search can finish
        
        do
        {
            
            Console.WriteLine("----------------------------------------");
            string startCity = AnsiConsole.Prompt(selectCityPrompt.Title("Choose starting city:"));
            string goalCity = AnsiConsole.Prompt(selectCityPrompt.Title("Choose goal city:"));

            var searchMethodPrompt = new SelectionPrompt<string>()
                .AddChoices(searchMethods.Keys).AddChoices("All methods")
                .Title("Choose search Method:");
            var searchMethodName = AnsiConsole.Prompt(searchMethodPrompt);


            if(searchMethodName != "All methods")
                TestSearchMethod(searchMethodName, startCity, goalCity);
            else
            {
                var colors = new Stack<ConsoleColor>(Enum.GetValues<ConsoleColor>().Reverse().ToArray());
                colors.Pop(); // Remove dark blue
                foreach (var searchName in searchMethods.Keys)
                {
                    Console.ForegroundColor = colors.Pop();
                    TestSearchMethod(searchName, startCity, goalCity);
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }

        } while ("yes" == AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("\nStart another search?")
                         .AddChoices(["yes", "no"])));


        Console.Clear();
        Console.WriteLine("Process exited...");
        Console.ReadLine();
    }

    void TestSearchMethod(string searchMethodName, string startCity, string goalCity)
    {
        var searchMethod = searchMethods[searchMethodName];

        var stopwatch = Stopwatch.StartNew();
        var resultPath = searchMethod.RunSearch(startCity, goalCity);
        stopwatch.Stop();

        Console.WriteLine($"Summary of {searchMethodName} from {startCity} to {goalCity}:");
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed.TotalMilliseconds} ms");
        Console.WriteLine("Path found: " + string.Join("->", resultPath));
        if (isPathValid(resultPath, out List<(string, string)> invalidConnections))
            Console.WriteLine($"Total path length: {calculatePathLength(resultPath)}\u00b0");
        else
            Console.WriteLine($"Path has invalid connections: {string.Join("; ", invalidConnections)}");
    }

    float calculatePathLength(string[] cities)
    {
        float length = 0;
        for (int i = 1; i < cities.Length; i++)
            length += SearchMethod.distanceBetween(cities[i - 1], cities[i]);
        return length;
    }

    bool isPathValid(string[] cities, out List<(string, string)> invalidConnections)
    {
        invalidConnections = [];
        bool pathValid = true;

        for (int i = 1; i < cities.Length; i++)
        {
            if (!adjacencies[cities[i - 1]].Contains(cities[i]))
                pathValid = false;
            invalidConnections.Add((cities[i - 1], cities[i]));
        }

        return pathValid;
    }


    void LoadDataFiles()
    {
        Dictionary<string, List<string>> adjacencies = [];
        Dictionary<string, Vector2> coordinates = [];

        //Load adjacencies
        var fileAdjLines = File.ReadAllLines("Adjacencies.txt");
        foreach (var line in fileAdjLines)
        {
            var names = line.Split();

            // Add relations in both directions

            if (adjacencies.TryGetValue(names[0], out var adjCities))
                adjCities.Add(names[1]);
            else
            {
                var list = new List<string> { names[1] };
                adjacencies.Add(names[0], list);
            }

            if (adjacencies.TryGetValue(names[1], out adjCities))
                adjCities.Add(names[0]);
            else
            {
                var list = new List<string> { names[0] };
                adjacencies.Add(names[1], list);
            }
        }

        //Load coordinates
        var fileCordLines = File.ReadAllLines("coordinates.csv");
        foreach (var line in fileCordLines)
        {
            var data = line.Split(',');
            coordinates.Add(data[0], new Vector2(float.Parse(data[1]), float.Parse(data[2])));
        }

        GlobalData.adjacencies = adjacencies.ToFrozenDictionary();
        GlobalData.coordinates = coordinates.ToFrozenDictionary();
    }

    void WarmJit()
    {
        Console.Write("Warming jit...");
        // For each search method
        foreach (var method in searchMethods)
        {
            //Run an arbitrary search and discard the result
            _ = method.Value.RunSearch("Leon", "Manhattan");
        }

        Console.WriteLine("Done");
    }
    
    void RunEveryOption()
    {
        Console.Write("Running ever option...");
        // For each search method
        foreach (var method in searchMethods)
        {
            Console.WriteLine($"Begin {method.Key}");
            foreach (var start in coordinates.Keys)
                foreach (var goal in coordinates.Keys)
                    _ = method.Value.RunSearch(start, goal);
        }

        Console.WriteLine("Test Complete");
    }
}

internal static class GlobalData
{
    public static FrozenDictionary<string, List<string>> adjacencies;
    public static FrozenDictionary<string, Vector2> coordinates;
}