using System;

[Serializable]
public class Edge {
    public int id;

    public int start;
    public int end;

    public string direction;

    public Edge(int id, int start, int end, string direction) {
        this.id = id;
        this.start = start;
        this.end = end;
        this.direction = direction;
    }

    public override bool Equals(object obj)
    {
        var edge = obj as Edge;
        if (edge == null)
            return false;

        return id == edge.id 
                && start == edge.start
                && end == edge.end
                && direction.Equals(edge.direction);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode() 
            + start.GetHashCode() 
            + end.GetHashCode()
            + direction.GetHashCode();
    }

	public override string ToString()
	{
		return string.Format("Edge: {0} => {1}", start, end);
	}
}