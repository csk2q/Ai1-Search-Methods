using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Numerics;
using Ai1_Search_Methods.SearchMethods;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods;

internal class Program
{
    

    static void Main(string[] args) => new Program().RunMain();
    void RunMain()
    {
        Console.WriteLine("Hello, search methods!\n");
        LoadDataFiles();


        // var dfs = new BreathFirstSearch();
        // foreach (var start in coordinates.Keys)
        //     foreach (var end in coordinates.Keys)
        //     {
        //         Console.WriteLine(string.Join("->", dfs.RunSearch(start, end)));
        //     }


        // Console.WriteLine("DFS");
        // var dfs = new BreathFirstSearch();
        //dfs.PrintRunSearch("Anthony", "Attica");
        //dfs.PrintRunSearch("Leon", "Manhattan");

        //Console.WriteLine("\nBFS");
        //var bfs = new BreathFirstSearch();
        //bfs.PrintRunSearch("Anthony", "Attica");
        //bfs.PrintRunSearch("Leon", "Manhattan");

        //Console.WriteLine("\nBestFS");
        //var bestFS = new BestFirstSearch();
        //bestFS.PrintRunSearch("Leon", "Manhattan");

        // Console.WriteLine("\nID-DFS");
        // var idDFS = new IDDFS();
        // idDFS.PrintRunSearch("Leon", "Manhattan");
        
        Console.WriteLine("\nA*");
        var aStar = new AStarSearch();
        aStar.PrintRunSearch("Leon", "Manhattan");



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
}

internal class GlobalData
{
    public static FrozenDictionary<string, List<string>> adjacencies;
    public static FrozenDictionary<string, Vector2> coordinates;
}