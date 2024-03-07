using System.Diagnostics;
using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class AStarSearch() : SearchMethod()
{
    string goalNodeName = "not yet set";
    private const bool debugPrints = false;

    // Based off of the pseudocode description from: https://www.youtube.com/watch?v=ySN5Wnu88nE
    public override string[] RunSearch(string start, string goal)
    {
        this.goalNodeName = goal;

        if (start == goal)
            return [start];

        // Keep and open list of frontier nodes and always pick the closest option

        StarNode root = new(start);
        StarNode? goalNode = null;
        Dictionary<string, StarNode> seenNodes = new() { { start, root } };
        PriorityQueue<StarNode, float> leafNodes = new();
        leafNodes.Enqueue(root, Heuristic(root));

        while (goalNode is null)
        {
            StarNode shortestNode = leafNodes.Dequeue();
            if (debugPrints)
                Console.WriteLine("Starting with " + shortestNode.Name);

            bool didAddNode = false;

            foreach (var adjNodeName in adjacencies[shortestNode.Name])
            {
                if (debugPrints)
                    Console.WriteLine("\tChecking adj " + adjNodeName);

                //If goal
                if (adjNodeName == goal)
                {
                    if (debugPrints)
                        Console.WriteLine("\t\tFound goal!");

                    // Add goal to tree and return
                    goalNode = shortestNode.AddChild(adjNodeName);
                    break;
                }

                // If have seen node before
                if (seenNodes.TryGetValue(adjNodeName, out StarNode? adjNode))
                {
                    if (debugPrints)
                        Console.Write("\t\tSeen node before...");

                    // Check if we found a shorter path
                    if (shortestNode.costToThisNode < adjNode.costToThisNode)
                    {
                        // Update node cost and parent
                        adjNode.costToThisNode =
                            shortestNode.costToThisNode + distanceBetween(shortestNode.Name, adjNodeName);
                        adjNode.Parent = shortestNode;

                        if (debugPrints)
                            Console.WriteLine("Shorter path found.");
                    }
                    else
                    {
                        if (debugPrints)
                            Console.WriteLine("Path not shorter.");
                    }
                }
                else // If have not seen node
                {
                    if (debugPrints)
                        Console.WriteLine("\t\tThis is a new node.");

                    didAddNode = true;
                    var newNode = shortestNode.AddChild(adjNodeName);
                    leafNodes.Enqueue(newNode, Heuristic(newNode));
                    seenNodes.Add(adjNodeName, newNode);
                }
            }


            if (!didAddNode && debugPrints)
                Console.WriteLine(
                    $"AStar found a dead end. Node: {shortestNode.Name} Adjacencies: {string.Join(", ", adjacencies[shortestNode.Name])}");
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
        return starNode.costToThisNode + EstCostToGoal(starNode.Name);
    }

    private float EstCostToGoal(string nodeName)
    {
        return distanceBetween(nodeName, goalNodeName);
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