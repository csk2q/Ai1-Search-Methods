using System.Diagnostics;
using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class BestFirstSearch() : SearchMethod()
{
    public override string[] RunSearch(string start, string goal)
    {
        if (start == goal)
            return [start];

        // Keep and open list of frontier nodes and always pick the closest option

        Node root = new(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        SortedList<float, Node> leafNodes = []; // TODO convert to PriorityQueue for speed
        leafNodes.Add(distanceBetween(start, goal), root);

        while (goalNode is null)
        {

            Node closestToGoalNode = leafNodes.GetValueAtIndex(0);
            leafNodes.RemoveAt(0);

            //bool didAddNode = false;

            foreach (var adjNodeName in adjacencies[closestToGoalNode.Name])
            {

                if (adjNodeName == goal)
                {
                    goalNode = closestToGoalNode.AddChild(adjNodeName);
                    break;
                }
                else if (!seenNodes.Contains(adjNodeName))
                {
                    //didAddNode = true;
                    leafNodes.Add(distanceBetween(adjNodeName, goal), closestToGoalNode.AddChild(adjNodeName));
                    seenNodes.Add(adjNodeName);
                }
            }

            //if (!didAddNode)
                //Console.WriteLine("BestFS found a dead end.");


        }

        LinkedList<string> path = [];

        for (Node? curNode = goalNode; curNode is not null; curNode = curNode.Parent)
        {
            path.AddFirst(curNode.Name);

        }

        return path.ToArray();
    }

/*    private string closestAdj(string name)
    {
        var adj = adjacencies[name];
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
    }

    private static float distance(Vector2 a, Vector2 b) => (a - b).Length();
    private float distanceBetween(string a, string b) => distance(coordinates[a], coordinates[b]);*/

}