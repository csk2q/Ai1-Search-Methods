namespace Ai1_Search_Methods;

public class Node()
{
    public int depth = 0;
    
    public string Name;
    public Node Parent = null;
    public List<Node> Children = [];

    public Node(string name) : this()
    {
        Name = name;
    }

    public Node AddChild(string name)
    {
        var node = new Node
        {
            Name = name,
            Parent = this,
            depth = depth + 1,
        };
        Children.Add(node);
        return node;
    }
}

public class Tree
{
    
}