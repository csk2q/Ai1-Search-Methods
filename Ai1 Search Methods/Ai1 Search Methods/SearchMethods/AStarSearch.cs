using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class AStarSearch() : SearchMethod()
{
    string goal = "not yet set";

    // Based off of the pseudocode from: https://brilliant.org/wiki/a-star-search/
    public override string[] RunSearch(string start, string goal)
    {
        this.goal = goal;

        if (start == goal)
            return [start];

        // Keep and open list of frontier nodes and always pick the closest option

        Node root = new(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        SortedList<float, Node> leafNodes = [];
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


    // h(n)
    private float estCostToGoal(string nodeName)
    {
        return distanceBetween(nodeName, goal);
    }
}
