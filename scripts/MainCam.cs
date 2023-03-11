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
    bool RotatingLock, RotatingY, RotatingZ;
    Tween Snapback;

    public override void _Ready()
    {
        RotatingLock = false;
        RotatingY = false;
        RotatingZ = false;
        Snapback = GetNode<Tween>("MainCam/Snapback");
    }

    public override void _Process(float delta)
    {


        if (Input.IsActionJustPressed("XRot"))
        {
            RotatingLock = !RotatingLock;
        }


        if (Input.IsActionJustPressed("rotate") && MouseEntered)
        {
            PrevMousePos = GetViewport().GetMousePosition();
            Rotating = true;
        }
        if (Input.IsActionJustReleased("rotate"))
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

            if (RotatingLock)
            {
                RotateX(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
                RotateY(-(NextMousePos.x - PrevMousePos.x) * rotationSpeed * delta);
                RotateZ(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
            }


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





