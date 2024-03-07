using System.Diagnostics;
using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class AStarSearch() : SearchMethod()
{
    string goal = "not yet set";

    // Based off of the pseudocode from: https://brilliant.org/wiki/a-star-search/
    // https://www.youtube.com/watch?v=ySN5Wnu88nE
    public override string[] RunSearch(string start, string goal)
    {
        this.goal = goal;

        if (start == goal)
            return [start];

        // Keep and open list of frontier nodes and always pick the closest option

        StarNode root = new(start);
        StarNode? goalNode = null;
        HashSet<string> seenNodes = [start];
        SortedList<float, StarNode> leafNodes = [];
        leafNodes.Add(distanceBetween(start, goal), root);

        while (goalNode is null)
        {

            StarNode closestToGoalNode = leafNodes.GetValueAtIndex(0);
            leafNodes.RemoveAt(0);

            bool didAddNode = false;

            foreach (var adjNodeName in adjacencies[closestToGoalNode.Name])
            {
                //If goal
                if (adjNodeName == goal)
                {
                    // Add goal to tree and return
                    goalNode = closestToGoalNode.AddChild(adjNodeName);
                    break;
                }

                // If have seen node before
                if (seenNodes.Contains(adjNodeName))
                {
                    // Check if we found a shorter path


                }
                else // If have not seen node
                {
                    didAddNode = true;
                    var newNode = closestToGoalNode.AddChild(adjNodeName);
                    leafNodes.Add(Heuristic(newNode), newNode);
                    seenNodes.Add(adjNodeName);
                }
            }

            if (!didAddNode)
                Console.WriteLine("AStar found a dead end.");
            //Debug.Assert(didAddNode, "BestFS found a dead end.");


        }

        LinkedList<string> path = [];

        for (StarNode? curNode = goalNode; curNode is not null; curNode = curNode.Parent)
        {
            path.AddFirst(curNode.Name);

        }

        return path.ToArray();
    }


    // h(node) = estimated cost *through* node to goal
    private float Heuristic(StarNode starNode)
    {
        return starNode.costToThisNode + estCostToGoal(starNode.Name);
    }

    private float estCostToGoal(string nodeName)
    {
        return distanceBetween(nodeName, goal);
    }
}

class StarNode()
{
    // g(n)
    public float costToThisNode = 0;
    public int depth = 0;

    public string Name = "Name not set";
    public StarNode? Parent = null;
    public List<StarNode> Children = [];

    public StarNode(string name) : this()
    {
        Name = name;
    }

    public StarNode AddChild(string name)
    {
        var node = new StarNode
        {
            Name = name,
            Parent = this,
            depth = depth + 1,
        };
        Children.Add(node);
        return node;
    }

    public override string ToString()
    {
        return Name;
    }
}
