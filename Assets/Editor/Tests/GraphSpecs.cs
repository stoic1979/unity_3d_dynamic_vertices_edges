using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class GraphSpecs {
    [Test]
    public void VertexEquals()
    {
        var v0 = new Vertex (10, 0, 1, 2, "v", "vertex");
        var v1 = new Vertex (10, 0, 1, 2, "v", "vertex");
        var v2 = new Vertex (0, 0, 1, 2, "v", "vertex");
        var v3 = new Vertex (10, 1, 1, 2, "v", "vertex");
        var v4 = new Vertex (10, 0, 2, 2, "v", "vertex");
        var v5 = new Vertex (10, 0, 1, 3, "v", "vertex");
        var v6 = new Vertex (10, 0, 1, 2, "u", "vertex");
        var v7 = new Vertex (10, 0, 1, 2, "v", "node");

        Assert.AreEqual (v0, v0);
        Assert.AreEqual (v0, v1);
        Assert.AreNotEqual (v0, v2);
        Assert.AreNotEqual (v0, v3);
        Assert.AreNotEqual (v0, v4);
        Assert.AreNotEqual (v0, v5);
        Assert.AreNotEqual (v0, v6);
        Assert.AreNotEqual (v0, v7);

        var l0 = new List<Vertex>{ v0, v1 };
        var l1 = new List<Vertex>{ v0, v1 };
        var l2 = new List<Vertex>{ v2, v2 };

        Assert.AreEqual (l0, l0);
        Assert.AreEqual (l0, l1);
        Assert.AreNotEqual (l0, l2);
    }

    [Test]
    public void EdgeEquals()
    {
        var e0 = new Edge (10, 0, 1, "north");
        var e1 = new Edge (10, 0, 1, "north");
        var e2 = new Edge (0, 0, 1, "north");
        var e3 = new Edge (10, 2, 1, "north");
        var e4 = new Edge (10, 0, 2, "north");
        var e5 = new Edge (10, 0, 1, "south");

        Assert.AreEqual (e0, e0);
        Assert.AreEqual (e0, e1);
        Assert.AreNotEqual (e0, e2);
        Assert.AreNotEqual (e0, e3);
        Assert.AreNotEqual (e0, e4);
        Assert.AreNotEqual (e0, e5);

        var l0 = new List<Edge>{ e0, e1 };
        var l1 = new List<Edge>{ e0, e1 };
        var l2 = new List<Edge>{ e0, e2 };

        Assert.AreEqual (l0, l0);
        Assert.AreEqual (l0, l1);
        Assert.AreNotEqual (l0, l2);
    }

    [Test]
    public void AddVertex()
    {
        var map = new Graph ();

        map.AddVertex (0, 0, 0, "center", "Center of the world");
        map.AddVertex (1, 0, 0, "left", "Left of center");
        map.AddVertex (-1, 0, 0, "right", "Right of center");
        map.AddVertex (0, 1, 0, "top", "Above center");
        map.AddVertex (0, -1, 0, "bottom", "Below center");
        map.AddVertex (0, 0, 1, "elsewhere", "Somewhere else");

        var expected = new List<Vertex> {
            new Vertex (0, 0, 0, 0, "center", "Center of the world"),
            new Vertex (1, 1, 0, 0, "left", "Left of center"),
            new Vertex (2, -1, 0, 0, "right", "Right of center"),
            new Vertex (3, 0, 1, 0, "top", "Above center"),
            new Vertex (4, 0, -1, 0, "bottom", "Below center"),
            new Vertex (5, 0, 0, 1, "elsewhere", "Somewhere else")
        };

        Assert.AreEqual (expected, map.vertices);
    }

    [Test]
    public void AddEdge()
    {
        var map = new Graph ();

        map.AddVertex (0, 0, 0, "center", "Center of the world");
        map.AddVertex (1, 0, 0, "left", "Left of center");
        map.AddVertex (-1, 0, 0, "right", "Right of center");
        map.AddVertex (0, 1, 0, "top", "Above center");
        map.AddVertex (0, -1, 0, "bottom", "Below center");
        map.AddVertex (0, 0, 1, "elsewhere", "Somewhere else");

        map.AddEdge (0, 1, "left");
        map.AddEdge (0, 2, "right");
        map.AddEdge (0, 3, "up");
        map.AddEdge (0, 4, "down");

        var expected = new List<Edge> {
            new Edge (0, 0, 1, "left"),
            new Edge (1, 0, 2, "right"),
            new Edge (2, 0, 3, "up"),
            new Edge (3, 0, 4, "down")
        };

        Assert.AreEqual (expected, map.edges);
    }

    [Test]
    public void RemoveVertex()
    {
        var map = new Graph ();

        map.AddVertex (0, 0, 0, "center", "Center of the world");
        map.AddVertex (1, 0, 0, "left", "Left of center");
        map.AddVertex (-1, 0, 0, "right", "Right of center");
        map.AddVertex (0, 1, 0, "top", "Above center");
        map.AddVertex (0, -1, 0, "bottom", "Below center");
        map.AddVertex (0, 0, 1, "elsewhere", "Somewhere else");

        map.AddEdge (0, 1, "left");
        map.AddEdge (0, 2, "right");
        map.AddEdge (0, 3, "up");
        map.AddEdge (0, 4, "down");

        map.RemoveVertex (2);

        var expectedVertices = new List<Vertex> {
            new Vertex (0, 0, 0, 0, "center", "Center of the world"),
            new Vertex (1, 1, 0, 0, "left", "Left of center"),
            new Vertex (3, 0, 1, 0, "top", "Above center"),
            new Vertex (4, 0, -1, 0, "bottom", "Below center"),
            new Vertex (5, 0, 0, 1, "elsewhere", "Somewhere else")
        };

        var expectedEdges = new List<Edge> {
            new Edge (0, 0, 1, "left"),
            new Edge (2, 0, 3, "up"),
            new Edge (3, 0, 4, "down")
        };

        Assert.AreEqual (expectedVertices, map.vertices);
        Assert.AreEqual (expectedEdges, map.edges);
    }

    [Test]
    public void RemoveEdge()
    {
        var map = new Graph ();

        map.AddVertex (0, 0, 0, "center", "Center of the world");
        map.AddVertex (1, 0, 0, "left", "Left of center");
        map.AddVertex (-1, 0, 0, "right", "Right of center");
        map.AddVertex (0, 1, 0, "top", "Above center");
        map.AddVertex (0, -1, 0, "bottom", "Below center");
        map.AddVertex (0, 0, 1, "elsewhere", "Somewhere else");

        map.AddEdge (0, 1, "left");
        map.AddEdge (0, 2, "right");
        map.AddEdge (0, 3, "up");
        map.AddEdge (0, 4, "down");

        map.RemoveEdge (2);

		map.AddEdge (0, 5, "diagonal");
		map.RemoveEdge (4);


        var expectedVertices = new List<Vertex> {
            new Vertex (0, 0, 0, 0, "center", "Center of the world"),
            new Vertex (1, 1, 0, 0, "left", "Left of center"),
            new Vertex (2, -1, 0, 0, "right", "Right of center"),
            new Vertex (3, 0, 1, 0, "top", "Above center"),
            new Vertex (4, 0, -1, 0, "bottom", "Below center"),
            new Vertex (5, 0, 0, 1, "elsewhere", "Somewhere else")
        };

        var expectedEdges = new List<Edge> {
            new Edge (0, 0, 1, "left"),
            new Edge (1, 0, 2, "right"),
            new Edge (3, 0, 4, "down")
        };

        Assert.AreEqual (expectedVertices, map.vertices);
        Assert.AreEqual (expectedEdges, map.edges);
    }
}