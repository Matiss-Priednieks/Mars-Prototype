using Godot;
using System;

public class MainMenu : Spatial
{
    AnimationPlayer LaunchPt2;
    AnimationPlayer MenuSwapper;

    public override void _Ready()
    {
        LaunchPt2 = GetNode<AnimationPlayer>("rocket/LaunchPt2");
        MenuSwapper = GetNode<AnimationPlayer>("CanvasLayer/MenuSwapper");
    }
    public void _on_Button_pressed()
    {
        LaunchPt2.Play("LaunchOffScreen");

    }

    public void _on_LaunchPt2_animation_finished(string anim)
    {
        GetTree().ChangeScene("scenes/Planet.tscn");
    }

    public void _on_Settings_pressed()
    {
        MenuSwapper.Play("MenuSwap");
    }

    public void _on_BackButton_pressed()
    {
        MenuSwapper.Play("MenuSwapBack");
    }

    public void _on_ExitButton_pressed()
    {
        GetTree().Quit();
    }


}
