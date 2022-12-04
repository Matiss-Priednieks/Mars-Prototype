using Godot;
using System;

public class Mars : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 MouseDelta;
    Vector2 NextMousePos, PrevMousePos;
    bool MouseEntered = false;
    bool Rotating = false;
    public float rotationSpeed = 0.5f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translation = new Vector3(0, 0, 0);
        if (Input.IsActionJustPressed("mousepress") && MouseEntered)
        {
            PrevMousePos = GetViewport().GetMousePosition();
            Rotating = true;
        }
        if (Input.IsActionJustReleased("mousepress"))
        {
            Rotating = false;
        }

        if (Rotating)
        {
            Vector2 Xrotation;
            Xrotation.x = (NextMousePos.x - PrevMousePos.x);
            Xrotation.y = -(NextMousePos.y - PrevMousePos.y);
            Xrotation = Xrotation.Normalized();
            NextMousePos = GetViewport().GetMousePosition();
            RotateY((NextMousePos.x - PrevMousePos.x) * rotationSpeed * delta);
            RotateZ(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
            RotateX(Xrotation.x * Xrotation.y * rotationSpeed * delta);
            //do one axis rotation at a time by selecting it first. Make it so it's limited to a certain amount of degrees for the Y axis.
            PrevMousePos = NextMousePos;
        }
        // RotationDegrees += new Vector3(0, rotationSpeed, 0); // demo rotation

    }

    private void _on_Area_mouse_entered()
    {
        MouseEntered = true;
    }
    private void _on_Area_mouse_exited()
    {
        MouseEntered = false;
    }




}





