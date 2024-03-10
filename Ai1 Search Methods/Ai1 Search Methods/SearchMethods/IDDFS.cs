using System.Numerics;
using static Ai1_Search_Methods.GlobalData;

namespace Ai1_Search_Methods.SearchMethods;

public class IDDFS() : SearchMethod()
{
    const int DepthStepSize = 3; //Note: depth is counted up from zero.


    //Search exhaustively with dfs to a depth, then increase depth and restart search
    public override string[] RunSearch(string start, string goal)
    {
        if (start == goal)
            return [start];

        int curMaxDepth = 3;

        Node root = new(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        LinkedList<Node> leafNodes = [];
        leafNodes.AddLast(root);

        
        var curNode = leafNodes.Last;
        // Continue until goal or out of nodes
        while (goalNode is null && leafNodes.Count > 0)
        {
            // If hit max depth move to another leaf
            while (curNode is not null && curNode.Value.depth >= curMaxDepth)
                curNode = curNode.Previous;

            // If out of nodes to check 
            if (curNode is null)
            {
                //Increase depth and restart/continue search.
                curNode = leafNodes.Last;
                curMaxDepth += DepthStepSize;
                continue;
            }

            // Add adjacent nodes
            var adjNodes = adjacencies[curNode!.Value.Name];
            bool addedNodes = false;
            foreach (var adjNodeName in adjNodes)
            {
                if (!seenNodes.Contains(adjNodeName))
                {
                    //If this is the first child
                    if (!addedNodes)
                        //Remove the parent from leafNodes
                        leafNodes.Remove(curNode);
                    
                    //Add node
                    addedNodes = true;
                    leafNodes.AddLast(curNode.Value.AddChild(adjNodeName));
                    seenNodes.Add(adjNodeName);
                }

                // Break out if goal found
                if (adjNodeName == goal)
                {
                    goalNode = leafNodes.Last!.Value;
                    break;
                }
            }

            if (addedNodes)
                //If node added jump to newest node
                curNode = leafNodes.Last;
            else
                //If no node was added step back
                curNode = curNode.Previous;
            
            // Console.WriteLine($"Name: {curNode?.Value.Name} Depth: {curNode?.Value.depth}");
        }

        // Build and return path to goal
        LinkedList<string> path = [];
        for (Node? node = goalNode; node is not null; node = node.Parent)
            path.AddFirst(node.Name);
        return path.ToArray();
    }
}