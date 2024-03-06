using System.Numerics;

namespace Ai1_Search_Methods.SearchMethods;

public class BreathFirstSearch(Dictionary<string, List<string>> adjacencies, Dictionary<string, Vector2> coordinates) : SearchMethod(adjacencies, coordinates)
{
    public override string[] RunSearch(string start, string goal)
    {
        if (start == goal)
            return [start];
        
        Node root = new Node(start);
        Node? goalNode = null;
        HashSet<string> seenNodes = [start];
        List<Node> leafNodes = [root];

        while (goalNode is null)
        {
            List<Node> newLeafs = [];
            foreach (Node leafNode in leafNodes)
            {
                var adjNodes = adjacencies[leafNode.Name];
                bool addedNodes = false;
                foreach (var adjNode in adjNodes)
                {
                    if (!seenNodes.Contains(adjNode))
                    {
                        addedNodes = true;
                        newLeafs.Add(leafNode.AddChild(adjNode));
                    }

                    if (adjNode == goal)
                    {
                        goalNode = newLeafs.Last();
                        break;
                    }
                }

                if (!addedNodes)
                    Console.WriteLine("BFS found a dead end.");
                
                if(goalNode is not null)
                    break;
            }
            leafNodes = newLeafs;
        }

        LinkedList<string> path = [];

        for (Node? curNode = goalNode; curNode is not null; curNode = curNode.Parent)
        {
            path.AddFirst(curNode.Name);

        }

        // Console.WriteLine(goalNode.depth);
        return path.ToArray();
    }
}