using System;

[Serializable]
public class Vertex {
    public int id;

    public int x;
    public int y;
    public int z;

    public string name;
    public string description;

    public Vertex(int id, int x, int y, int z, string name, string description) {
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.name = name;
        this.description = description;
    }

    public override bool Equals(object obj)
    {
        var vertex = obj as Vertex;
        if (vertex == null)
            return false;
        
        return id == vertex.id
                && x == vertex.x
                && y == vertex.y
                && z == vertex.z
                && name.Equals(vertex.name)
                && description.Equals(vertex.description);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode() 
            + x.GetHashCode() 
            + y.GetHashCode()
            + z.GetHashCode()
            + name.GetHashCode()
            + description.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("{0}, ({1}, {2}, {3}), {4}, {5}", id, x, y, z, name, description);
    }
}