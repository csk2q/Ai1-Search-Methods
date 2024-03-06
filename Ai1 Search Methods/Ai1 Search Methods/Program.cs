﻿using System.Numerics;
using Ai1_Search_Methods.SearchMethods;

namespace Ai1_Search_Methods;

internal class Program
{
    Dictionary<string, List<string>> adjacencies = [];
    Dictionary<string, Vector2> coordinates = [];

    static void Main(string[] args) => new Program().RunMain();
    void RunMain()
    {
        Console.WriteLine("Hello, search methods!\n");
        LoadDataFiles();


        // var dfs = new BreathFirstSearch(adjacencies, coordinates);
        // foreach (var start in coordinates.Keys)
        //     foreach (var end in coordinates.Keys)
        //     {
        //         Console.WriteLine(string.Join("->", dfs.RunSearch(start, end)));
        //     }
        
        
        // Console.WriteLine("DFS");
        // Console.WriteLine(string.Join("->", dfs.RunSearch("Anthony", "Attica")));
        // Console.WriteLine(string.Join("->", dfs.RunSearch("Leon", "Manhattan")));
        
        // var bfs = new BreathFirstSearch(adjacencies, coordinates);
        // Console.WriteLine("\nBFS");
        // Console.WriteLine(string.Join("->", bfs.RunSearch("Anthony", "Attica")));
        // Console.WriteLine(string.Join("->", bfs.RunSearch("Leon", "Manhattan")));

        var bestFS = new BestFirstSearch(adjacencies, coordinates);
        Console.WriteLine(string.Join("->", bestFS.RunSearch("Leon", "Manhattan")));
        




    }

    void LoadDataFiles()
    {
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
        
        
    }
}