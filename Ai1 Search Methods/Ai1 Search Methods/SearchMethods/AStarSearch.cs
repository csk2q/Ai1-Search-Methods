// #define DEBUG_PRINT

using System.Diagnostics;
using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class AStarSearch() : SearchMethod()
{
    string goalNodeName = "not yet set";
    // private const bool debugPrints = false;

    // Based off of the pseudocode description from: https://www.youtube.com/watch?v=ySN5Wnu88nE
    public override string[] RunSearch(string start, string goal)
    {
        goalNodeName = goal;

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


#if DEBUG_PRINT
                Console.WriteLine("Starting with " + shortestNode.Name);
#endif

            bool didAddNode = false;

            foreach (var adjNodeName in adjacencies[shortestNode.Name])
            {
#if DEBUG_PRINT
                    Console.WriteLine("\tChecking adj " + adjNodeName);
#endif

                //If goal
                if (adjNodeName == goal)
                {
#if DEBUG_PRINT
                        Console.WriteLine("\t\tFound goal!");
#endif

                    // Add goal to tree and return
                    goalNode = shortestNode.AddChild(adjNodeName);
                    break;
                }

                // If have seen node before
                if (seenNodes.TryGetValue(adjNodeName, out StarNode? adjNode))
                {
                    // # No use to backtracking, it is never shorter. (At least in this dataset)
                    /*// Check if we found a shorter path
#if DEBUG_PRINT
                                           Console.Write("\t\tSeen node before...");
#endif

                    if (shortestNode.costToThisNode < adjNode.costToThisNode)
                    {
                        Debug.Assert(false, "FOUND A SHORTER PATH");
                        // Update node cost and parent
                        adjNode.costToThisNode =
                            shortestNode.costToThisNode + distanceBetween(shortestNode.Name, adjNodeName);
                        adjNode.Parent = shortestNode;

#if DEBUG_PRINT
                            Console.WriteLine("Shorter path found.");
#endif
                    }
                    else
                    {
#if DEBUG_PRINT
                            Console.WriteLine("Path not shorter.");
#endif
                    }*/
                }
                else // If have not seen node
                {
#if DEBUG_PRINT
                        Console.WriteLine("\t\tThis is a new node.");
#endif

                    didAddNode = true;
                    var newNode = shortestNode.AddChild(adjNodeName);
                    leafNodes.Enqueue(newNode, Heuristic(newNode));
                    seenNodes.Add(adjNodeName, newNode);
                }
            }


            // if (!didAddNode)
            //     Console.WriteLine($"AStar found a dead end. Node: {shortestNode.Name} Adjacencies: {string.Join(", ", adjacencies[shortestNode.Name])}");
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
        //Over estimate distance
        return distanceBetween(nodeName, goalNodeName) * (float)1.2;
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