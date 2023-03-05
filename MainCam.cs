using Godot;
using System;

public class MainCam : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 MouseDelta;
    Vector2 NextMousePos, PrevMousePos;
    bool MouseEntered = false;
    bool Rotating = false;
    public float rotationSpeed = 0.5f;
    bool RotatingX, RotatingY, RotatingZ;
    Tween Snapback;

    public override void _Ready()
    {
        RotatingX = false;
        RotatingY = false;
        RotatingZ = false;
        Snapback = GetNode<Tween>("MainCam/Snapback");
    }

    public override void _Process(float delta)
    {


        if (Input.IsActionJustPressed("XRot"))
        {
            RotatingX = true;
            RotatingY = false;
            RotatingZ = false;
        }
        if (Input.IsActionJustPressed("YRot"))
        {
            RotatingY = true;
            RotatingX = false;
            RotatingZ = false;
        }
        if (Input.IsActionJustPressed("ZRot"))
        {
            RotatingZ = true;
            RotatingX = false;
            RotatingY = false;
        }



        if (Input.IsActionJustPressed("mousepress") && MouseEntered)
        {
            PrevMousePos = GetViewport().GetMousePosition();
            Rotating = true;
        }
        if (Input.IsActionJustReleased("mousepress"))
        {
            Rotating = false;
        }
        if (Input.IsActionJustPressed("ui_escape"))
        {
            Snapback.InterpolateProperty(this, "rotation_degrees", RotationDegrees, new Vector3(0, 0, 0), 0.5f, Tween.TransitionType.Elastic, Tween.EaseType.InOut);
            Snapback.Start();
        }

        if (Rotating)
        {
            NextMousePos = GetViewport().GetMousePosition();

            if (RotatingX) RotateX((NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
            if (RotatingY) RotateY((NextMousePos.x - PrevMousePos.x) * rotationSpeed * delta);
            if (RotatingZ) RotateZ(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);

            PrevMousePos = NextMousePos;
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





