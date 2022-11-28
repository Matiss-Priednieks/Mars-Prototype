using Godot;
using System;

public class Planet : Spatial
{

    public override void _Ready()
    {
        foreach (PlanetMeshFace child in GetChildren())
        {
            PlanetMeshFace face = child;
            face.RegenerateMesh();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
