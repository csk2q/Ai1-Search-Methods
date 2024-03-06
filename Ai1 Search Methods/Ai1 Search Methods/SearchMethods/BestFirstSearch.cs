using System.Diagnostics;
using System.Numerics;

namespace Ai1_Search_Methods.SearchMethods;

public class BestFirstSearch(Dictionary<string, List<string>> adjacencies, Dictionary<string, Vector2> coordinates) : SearchMethod(adjacencies, coordinates)
{
    public override string[] RunSearch(string start, string goal)
    {
        if (start == goal)
            return [start];
        
        // Keep and open list of frontire nodes and allways pick the closest option
        //TODO REFACTOR - Copy pasted from BFS

        Node root = new Node(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        List<Node> leafNodes = [root];

        while (goalNode is null)
        {
        }

        return path.ToArray(); // DEBUG RETURN
    }

    private string closestAdj(string name)
    {
        var adj = adjacencies[name];
        var location = coordinates[name];

        string closestCity = "none";
        float lowest = float.MaxValue;

        
        int i = 0;
        foreach (var city in adjacencies[name])
        {
            var diff = dist(location, coordinates[city]);
            if (diff < lowest)
            {
                lowest = diff;
                closestCity = city;
            }

            i++;
        }
        
        Debug.Assert(closestCity != "none", "No city was found?");

        return closestCity;
    }

    private static float dist(Vector2 a, Vector2 b) => (a - b).Length();
}