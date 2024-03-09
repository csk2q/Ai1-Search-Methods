using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class BreathFirstSearch() : SearchMethod()
{
    public override string[] RunSearch(string start, string goal)
    {
        if (start == goal)
            return [start];
        
        Node root = new(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        List<Node> leafNodes = [root];

        while (goalNode is null)
        {
            List<Node> newLeaves = [];
            foreach (Node leafNode in leafNodes)
            {
                var adjNodes = adjacencies[leafNode.Name];
                foreach (var adjNodeName in adjNodes)
                {
                    // Add new leaf nodes
                    if (!seenNodes.Contains(adjNodeName))
                    {
                        newLeaves.Add(leafNode.AddChild(adjNodeName));
                        seenNodes.Add(adjNodeName);
                    }

                    // Break out if we found our goal
                    if (adjNodeName == goal)
                    {
                        goalNode = newLeaves.Last();
                        break;
                    }
                }
                
                // Break out if we found our goal
                if(goalNode is not null)
                    break;
            }
            
            // Update new leaf nodes
            leafNodes = newLeaves;
        }

        //Travers node chain to start
        LinkedList<string> path = [];
        for (Node? curNode = goalNode; curNode is not null; curNode = curNode.Parent)
            path.AddFirst(curNode.Name);
        //Return path
        return path.ToArray();
    }
}