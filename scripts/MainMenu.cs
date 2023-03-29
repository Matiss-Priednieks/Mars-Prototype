using Godot;
using System;

public class MainMenu : Spatial
{
    AnimationPlayer LaunchPt2;
    AnimationPlayer MenuSwapper;
    VBoxContainer MainMenuBox, Settings;
    AudioStreamPlayer ButtonClick;
    PackedScene MainGame;
    Tween MenuSwap, SettingsSwap;
    int Direction = 1;
    public override void _Ready()
    {
        ButtonClick = GetNode<AudioStreamPlayer>("CanvasLayer/Panel/Click");
        LaunchPt2 = GetNode<AnimationPlayer>("rocket/LaunchPt2");
        MainGame = (PackedScene)ResourceLoader.Load("scenes/Planet.tscn");

        MenuSwap = GetNode<Tween>("CanvasLayer/Panel/MenuSwap");
        SettingsSwap = GetNode<Tween>("CanvasLayer/Panel/SettingsSwap");

        MainMenuBox = GetNode<VBoxContainer>("CanvasLayer/Panel/MainMenu");
        Settings = GetNode<VBoxContainer>("CanvasLayer/Panel/Settings");

    }
    public void _on_Button_pressed()
    {
        ButtonClick.Play();
        LaunchPt2.Play("LaunchOffScreen");
    }

    public void _on_LaunchPt2_animation_finished(string anim)
    {
        GetTree().ChangeSceneTo(MainGame);
    }

    public void _on_Settings_pressed()
    {
        ButtonClick.Play();
        MenuSwap.InterpolateProperty(MainMenuBox, "rect_position", MainMenuBox.RectPosition, new Vector2(MainMenuBox.RectPosition.x + 1000, MainMenuBox.RectPosition.y), 1.5f, Tween.TransitionType.Circ, Tween.EaseType.InOut);
        SettingsSwap.InterpolateProperty(Settings, "rect_position", Settings.RectPosition, new Vector2(Settings.RectPosition.x - 1000, Settings.RectPosition.y), 1.5f, Tween.TransitionType.Circ, Tween.EaseType.InOut);
        MenuSwap.Start();
        SettingsSwap.Start();
    }

    public void _on_BackButton_pressed()
    {
        ButtonClick.Play();
        MenuSwap.InterpolateProperty(MainMenuBox, "rect_position", MainMenuBox.RectPosition, new Vector2(MainMenuBox.RectPosition.x - 1000, MainMenuBox.RectPosition.y), 1.5f, Tween.TransitionType.Circ, Tween.EaseType.InOut);
        SettingsSwap.InterpolateProperty(Settings, "rect_position", Settings.RectPosition, new Vector2(Settings.RectPosition.x + 1000, Settings.RectPosition.y), 1.5f, Tween.TransitionType.Circ, Tween.EaseType.InOut);
        MenuSwap.Start();
        SettingsSwap.Start();

    }

    public void _on_ExitButton_pressed()
    {
        ButtonClick.Play();
        GetTree().Quit();
    }

}
