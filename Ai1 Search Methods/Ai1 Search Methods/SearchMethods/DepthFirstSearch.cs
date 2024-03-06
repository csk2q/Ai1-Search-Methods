using System.Numerics;

namespace Ai1_Search_Methods.SearchMethods;

public class DepthFirstSearch(Dictionary<string, List<string>> adjacencies, Dictionary<string, Vector2> coordinates)
    : SearchMethod(adjacencies, coordinates)
{
    public override string[] RunSearch(string start, string goal)
    {
        if (start == goal)
            return [start];
        
        //Use a stack to keep track of of past paths.
        HashSet<string> failedNodes = [];
        Stack<string> path = [];
        path.Push(start);

        string curNode = start;
        while (path.Count > 0 && path.Peek() != goal)
        {
            var adjNodes = adjacencies[curNode];

            List<string> newAdjNodes = [];

            foreach (var adjNode in adjNodes)
            {
                //If we had not been here add to search.
                if (!failedNodes.Contains(adjNode) && !path.Contains(adjNode))
                    newAdjNodes.Add(adjNode);
            }

            if (newAdjNodes.Count > 0)
            {
                curNode = newAdjNodes[0];
                path.Push(curNode);
            }
            else
            {
                failedNodes.Add(curNode);
                path.Pop();
                curNode = path.Peek();
            }

        }

        return path.Reverse().ToArray();
    }
}