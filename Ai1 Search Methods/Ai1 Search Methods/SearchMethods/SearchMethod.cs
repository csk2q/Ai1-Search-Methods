using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ai1_Search_Methods.SearchMethods;

public abstract class SearchMethod(Dictionary<string, List<string>> adjacencies, Dictionary<string, Vector2> coordinates)
{
    protected Dictionary<string, List<string>> adjacencies = adjacencies;
    protected Dictionary<string, Vector2> coordinates = coordinates;
    
    public abstract string[] RunSearch(string start, string goal);
    public string[] PrintRunSearch(string start, string goal)
    {
        var result = RunSearch(start, goal);
        Console.WriteLine(string.Join("->", result));
        return result;
    }
}