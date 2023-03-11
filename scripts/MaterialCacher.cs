using Godot;
using System;

public class MaterialCacher : Spatial
{
    Material[] Materials = { (Material)ResourceLoader.Load("res://assets/RocketParticle.tres") };

    private int _frames = 0;
    private bool _loaded = false;

    public override void _Ready()
    {
        foreach (Material material in Materials)
        {
            Particles particlesInstance = new Particles();
            particlesInstance.ProcessMaterial = material;
            particlesInstance.OneShot = true;
            particlesInstance.Emitting = true;
            AddChild(particlesInstance);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_frames >= 3)
        {
            SetPhysicsProcess(false);
            _loaded = true;
        }
        _frames += 1;
    }


}
