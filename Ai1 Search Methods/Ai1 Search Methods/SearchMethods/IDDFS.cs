using System.Diagnostics;
using System.Numerics;

namespace Ai1_Search_Methods.SearchMethods;

public class IDDFS(Dictionary<string, List<string>> adjacencies, Dictionary<string, Vector2> coordinates) : SearchMethod(adjacencies, coordinates)
{
    const int DepthStepSize = 3; //Note: depth is counted up from zero.


    public override string[] RunSearch(string start, string goal)
    {
        //Search exhaustively to a depth, then increase depth and restart search

        int curMaxDepth = DepthStepSize;

        Node root = new Node(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        LinkedList<Node> leafNodes = [];
        leafNodes.AddLast(root);


        var curNode = leafNodes.Last;


        while (goalNode is null && leafNodes.Count > 0)
        {
            // If hit max depth check another leaf
            while (curNode is not null && curNode.Value.depth >= curMaxDepth)
                curNode = curNode.Previous;

            //If out of nodes to check 
            if (curNode is null)
            {
                //Increase depth and restart/continue search.
                // TODO Ask professor if saving leaves from previous depths is OK
                curNode = leafNodes.Last;
                curMaxDepth += DepthStepSize;
                continue;
            }



            // Add adjacencies
            var adjNodes = adjacencies[curNode!.Value.Name];
            bool addedNodes = false;
            foreach (var adjNodeName in adjNodes)
            {

                if (!seenNodes.Contains(adjNodeName))
                {
                    //If this is the first child add remove the parent from leafNodes
                    if (!addedNodes)
                        leafNodes.RemoveLast();
                    addedNodes = true;

                    leafNodes.AddLast(curNode.Value.AddChild(adjNodeName));
                    seenNodes.Add(adjNodeName);
                }

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

        }

        // Build and return path to goal
        LinkedList<string> path = [];
        for (Node? node = goalNode; node is not null; node = node.Parent)
        {
            path.AddFirst(node.Name);

        }
        return path.ToArray();
    }
}