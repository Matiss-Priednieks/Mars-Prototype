using Godot;
using System;
using System.Text;
using Godot.Collections;
class GUI : CanvasLayer
{
    HSlider SliderData;
    [Signal] public delegate void UpdateTimeScale(int SliderValue);
    [Signal] public delegate void ResearchFuel();
    [Signal] public delegate void AddFuel();

    PackedScene MainMenu;
    KinematicBody Player;
    Rover PlayerClass;

    Label H2O, Scrap, Research, Recovery, WeatherData, ResourceError, CompletedMsg;
    int H2Ocount, Scrapcount, Researchcount, Recoverycount;
    Panel ResearchAndDev, PauseMenu, HelpMenu, LockedMessage;
    Panel Settings, Menu;
    AudioStreamPlayer Success, Error, ButtonClick;

    Timer ErrorTimer;
    bool FuelResearched = false;

    bool isMenu, isSettings, isHelp;

    Button ResearchButton, RecoveryButton, FuelButton;
    public override void _Ready()
    {
        ButtonClick = GetNode<AudioStreamPlayer>("LockedMsg/Click");
        Success = GetNode<AudioStreamPlayer>("LockedMsg/Success");
        Error = GetNode<AudioStreamPlayer>("LockedMsg/Error");
        LockedMessage = GetNode<Panel>("LockedMsg");
        ResourceError = GetNode<Label>("LockedMsg/Panel/ResourceError");
        ErrorTimer = GetNode<Timer>("LockedMsg/Timer");
        CompletedMsg = GetNode<Label>("LockedMsg/Panel/Completed");

        Player = GetNode<KinematicBody>("../%Player");
        PlayerClass = GetNode<Rover>("../%Player");
        MainMenu = (PackedScene)ResourceLoader.Load("res://scenes/MainMenu.tscn");

        GetNode("VBoxContainer/HTTPRequest").Connect("request_completed", this, "OnRequestCompleted");
        GetNode("VBoxContainer/Button").Connect("pressed", this, "OnButtonPressed");

        this.Connect("UpdateTimeScale", Player, "SetTimeScale");
        this.Connect("ResearchFuel", Player, "SetResearchComplete");
        this.Connect("AddFuel", Player, "AddFuel");

        ResearchAndDev = GetNode<Panel>("ResearchAndDev");
        HelpMenu = GetNode<Panel>("Help");

        Menu = GetNode<Panel>("PauseMenu/Menu");
        Settings = GetNode<Panel>("PauseMenu/Settings");


        SliderData = GetNode<HSlider>("TimeScale/LeftUI/HSplitContainer/HSlider");
        WeatherData = GetNode<Label>("VBoxContainer/WeatherData");
        H2O = GetNode<Label>("ResourceUI/VBoxContainer/ResourceGrid/H2O");
        Scrap = GetNode<Label>("ResourceUI/VBoxContainer/ResourceGrid/SCRAP");
        Research = GetNode<Label>("ResourceUI/VBoxContainer/ResourceGrid/RESEARCH");
        Recovery = GetNode<Label>("ResourceUI/VBoxContainer/ResourceGrid/RECOVERY");

        ResearchButton = ResearchAndDev.GetNode<Button>("RnDButtons/Research");
        RecoveryButton = ResearchAndDev.GetNode<Button>("RnDButtons/Recovery");

        PauseMenu = GetNode<Panel>("PauseMenu");

        FuelButton = ResearchAndDev.GetNode<Button>("RnDButtons/CraftFuel");

    }

    public void OnButtonPressed()
    {
        HTTPRequest httpRequest = GetNode<HTTPRequest>("VBoxContainer/HTTPRequest");
        httpRequest.Request("https://api.nasa.gov/insight_weather/?api_key=OcEGj33d2t0Wu7OGgEQCUbPzoX5n0oohJgKwq7ud&feedtype=json&ver=1.0");
    }

    public void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        JSONParseResult json = JSON.Parse(Encoding.UTF8.GetString(body));
        var weather = json.Result as Dictionary;

        WeatherData.Text = weather["validity_checks"].ToString();

        var TestData = weather as Dictionary;

        GD.Print(TestData);

        foreach (var keys in TestData.Keys)
        {
            var keyName = keys as Dictionary;
            var sol = TestData["1219"] as Dictionary;
            var solAT = sol["AT"] as Dictionary;

            GD.Print(solAT["av"]);
            GD.Print(weather);
        }
    }

    public void _on_HSlider_value_changed(float value)
    {
        EmitSignal("UpdateTimeScale", SliderData.Value);
    }

    public void UpdateResources(string resourceType, int amount)
    {
        if (resourceType.Equals("H2O"))
        {
            H2O.Text = amount.ToString();
            H2Ocount = amount;
        }
        else if (resourceType.Equals("Scrap"))
        {
            Scrap.Text = amount.ToString();
            Scrapcount = amount;
        }
        else if (resourceType.Equals("Research"))
        {
            Research.Text = amount.ToString();
            Researchcount = amount;
        }
        else if (resourceType.Equals("Recovery"))
        {
            Recovery.Text = amount.ToString();
            Recoverycount = amount;
        }
    }

    public void CloseRDMenu()
    {
        //hide r&d menu
        ButtonClick.Play();
        ResearchAndDev.Hide();
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("R&D"))
        {
            ButtonClick.Play();
            ResearchAndDev.Visible = !ResearchAndDev.Visible;
        }
        if (Input.IsActionJustReleased("HideUI"))
        {
            ButtonClick.Play();

            GD.Print(Visible);
            Visible = !Visible;
        }

        if (Input.IsActionJustReleased("pause_menu"))
        {
            ButtonClick.Play();
            // if any of these are visible when escape is pressed, hide them.
            if (HelpMenu.Visible || Settings.Visible || ResearchAndDev.Visible)
            {
                ResearchAndDev.Hide();
                HelpMenu.Hide();
                Settings.Hide();
                Menu.Show();
            }
            else
            {
                PauseMenu.Visible = !PauseMenu.Visible;
            }
        }

        if (Researchcount == 4)
        {
            ResearchButton.Disabled = false;
        }
        else
        {
            ResearchButton.Disabled = true;
        }
        if (Recoverycount == 2 && Scrapcount == 3)
        {
            RecoveryButton.Disabled = false;
        }
        else
        {
            RecoveryButton.Disabled = true;
        }
        if (H2Ocount > 0 && FuelResearched)
        {
            FuelButton.Disabled = false;
        }
        else
        {
            FuelButton.Disabled = true;
        }
    }

    public void _on_Research_pressed()
    {
        EmitSignal("ResearchFuel");
        ButtonClick.Play();
        Researchcount = 0;
        FuelResearched = true;

        ErrorTimer.Start();
        LockedMessage.Show();
        CompletedMsg.Show();
        Success.Play();
    }

    public void _on_Recovery_pressed()
    {
        ButtonClick.Play();
        Recoverycount = 0;
        Scrapcount = 0;
        PlayerClass.RecoveryMission();
        Success.Play();
    }

    public void _on_CraftFuel_pressed()
    {
        ButtonClick.Play();
        EmitSignal("AddFuel");
        H2Ocount--;
        Success.Play();
    }

    public void _on_Resume_pressed()
    {
        ButtonClick.Play();
        PauseMenu.Hide();
    }

    public void _on_Settings_pressed()
    {
        ButtonClick.Play();
        isHelp = false;
        isSettings = true;
        Menu.Hide();
        Settings.Show();
    }

    public void _on_BackButton_pressed()
    {
        ButtonClick.Play();
        Settings.Hide();
        Menu.Show();
    }

    public void _on_Help_pressed()
    {
        ButtonClick.Play();
        isHelp = true;
        isSettings = false;
        HelpMenu.Show();
        PauseMenu.Hide();
    }

    public void _on_Exit_pressed()
    {
        ButtonClick.Play();
        GetTree().ChangeSceneTo(MainMenu);
    }
    public void _on_Research_gui_input(InputEvent @event)
    {
        if (@event.IsActionReleased("mousepress") && ResearchButton.Disabled)
        {
            ButtonClick.Play();
            LockedMessage.Show();
            ResourceError.Show();
            ErrorTimer.Start();
            Error.Play();
        }
    }
    public void _on_CraftFuel_gui_input(InputEvent @event)
    {
        if (@event.IsActionReleased("mousepress") && FuelButton.Disabled)
        {
            ButtonClick.Play();
            LockedMessage.Show();
            ResourceError.Show();
            ErrorTimer.Start();
            Error.Play();
        }
    }
    public void _on_Recovery_gui_input(InputEvent @event)
    {
        if (@event.IsActionReleased("mousepress") && RecoveryButton.Disabled)
        {
            ButtonClick.Play();
            LockedMessage.Show();
            ResourceError.Show();
            ErrorTimer.Start();
            Error.Play();
        }
    }
}