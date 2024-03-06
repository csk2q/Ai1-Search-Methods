using System.Numerics;

namespace Ai1_Search_Methods.SearchMethods;

public class AStarSearch(Dictionary<string, List<string>> adjacencies, Dictionary<string, Vector2> coordinates) : SearchMethod(adjacencies, coordinates)
{
    public override string[] RunSearch(string start, string goal)
    {
        throw new NotImplementedException();
    }
}