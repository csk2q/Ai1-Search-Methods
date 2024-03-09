using System.Diagnostics;
using System.Numerics;
using static Ai1_Search_Methods.GlobalData;


namespace Ai1_Search_Methods.SearchMethods;

public abstract class SearchMethod()
{
    
    public abstract string[] RunSearch(string start, string goal);
    public virtual string[] PrintRunSearch(string start, string goal)
    {
        var result = RunSearch(start, goal);
        Console.WriteLine(string.Join("->", result));
        return result;
    }

    /*protected static string closestAdj(string name)
    {
        var location = coordinates[name];

        string closestCity = "none";
        float lowest = float.MaxValue;


        int i = 0;
        foreach (var city in adjacencies[name])
        {
            var diff = distance(location, coordinates[city]);
            if (diff < lowest)
            {
                lowest = diff;
                closestCity = city;
            }

            i++;
        }

        Debug.Assert(closestCity != "none", "No city was found?");

        return closestCity;
    }*/

    public static float distance(Vector2 a, Vector2 b) => (a - b).Length();
    public static float distanceBetween(string a, string b) => distance(coordinates[a], coordinates[b]);
}