using Godot;
using System;

public class MaterialCacher : Spatial
{
    Material[] Materials = { (Material)ResourceLoader.Load("res://RocketParticle.tres") };

    public override void _Ready()
    {
        foreach (Material material in Materials)
        {
            var test = new Particles();
            GD.Print(material);
            test.ProcessMaterial = material;
            test.OneShot = true;
            test.Emitting = true;
            AddChild(test);
        }
    }

}
