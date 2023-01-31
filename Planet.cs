using Godot;
using System;
[Tool]
public class Planet : Spatial
{
    [Export] PlanetData PlanetData;
    public override void _Ready()
    {
        foreach (PlanetMeshFace child in GetChildren())
        {
            PlanetMeshFace face = child;
            face.RegenerateMesh(PlanetData);
        }
    }
}
