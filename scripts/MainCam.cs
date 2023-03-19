using Godot;
using System;

public class MainCam : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 MouseDelta;
    Vector3 NextMousePos, PrevMousePos;
    bool MouseEntered = false;
    bool Rotating = false;
    public float rotationSpeed = 0.3f;
    bool RotatingY, RotatingZ;
    Tween Snapback;

    public override void _Ready()
    {
        RotatingY = false;
        RotatingZ = false;
        Snapback = GetNode<Tween>("MainCam/Snapback");
    }

    public override void _Process(float delta)
    {

        if (Input.IsActionJustPressed("rotate") && MouseEntered)
        {
            PrevMousePos = new Vector3(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y, 0);
            Rotating = true;
        }
        if (Input.IsActionJustReleased("rotate"))
        {
            Rotating = false;
            PrevMousePos = Vector3.Zero;
        }
        if (Input.IsActionJustPressed("ui_escape"))
        {
            Snapback.InterpolateProperty(this, "rotation_degrees", RotationDegrees, new Vector3(0, 0, 0), 0.5f, Tween.TransitionType.Elastic, Tween.EaseType.InOut);
            Snapback.Start();
        }

        if (Rotating)
        {
            NextMousePos = new Vector3(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y, 0);
            // RotateX(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
            RotateY((NextMousePos.x - PrevMousePos.x) * rotationSpeed * delta);
            RotateZ(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
            PrevMousePos = NextMousePos;
        }
        else
        {
            NextMousePos = Vector3.Zero;
        }

    }

    private void _on_StaticBody_mouse_entered()
    {
        MouseEntered = true;
    }
    private void _on_StaticBody_mouse_exited()
    {
        MouseEntered = false;
    }



}





