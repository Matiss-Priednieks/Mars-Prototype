using Godot;
using System;

public class PlanetData : Resource
{
    [Export] float Radius = 3;
    [Export] int Resolution = 5;
    [Export] Texture UVTex;


    public float GetRadius()
    {
        return Radius;
    }
    public int GetResolution()
    {
        return Resolution;
    }
    public Texture GetUV()
    {
        return UVTex;
    }
    // public Vector3 PointOnPlanet(Vector3 PointOnUnitSphere)
    // {

    // }
}
