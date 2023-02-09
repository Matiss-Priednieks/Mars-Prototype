using Godot;
using System;

public class PlanetData : Resource
{
    [Export] float Radius = 3;
    [Export] int Resolution = 5;
    [Export] Texture UVTex;
    [Export] Texture UVTexHeightMap;
    [Export] OpenSimplexNoise NoiseMap = new OpenSimplexNoise();
    [Export(PropertyHint.Range, "0.1f,1,")] float Amplitude = 0;

    public void _Ready()
    {
    }
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

    public void SetNoiseMap(OpenSimplexNoise noise)
    {
        var noiseMap = noise;
    }
    public Vector3 PointOnPlanet(Vector3 PointOnUnitSphere)
    {
        var imgX = (int)PointOnUnitSphere.x;
        var imgY = (int)PointOnUnitSphere.y;
        var heightColour = UVTexHeightMap.GetData().GetPixel(imgX, imgY);
        var height = heightColour.s;
        height = Mathf.Clamp(height, 0, 1);

        return height * Radius * PointOnUnitSphere;
    }
}
