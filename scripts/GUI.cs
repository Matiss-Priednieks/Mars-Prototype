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

    Label H2O, Scrap, Research, Recovery, WeatherData;
    int H2Ocount, Scrapcount, Researchcount, Recoverycount;
    Panel ResearchAndDev, PauseMenu;

    Button ResearchButton, RecoveryButton, FuelButton;
    public override void _Ready()
    {
        GetNode("VBoxContainer/HTTPRequest").Connect("request_completed", this, "OnRequestCompleted");
        GetNode("VBoxContainer/Button").Connect("pressed", this, "OnButtonPressed");

        this.Connect("UpdateTimeScale", GetNode<KinematicBody>("../%Player"), "SetTimeScale");
        this.Connect("ResearchFuel", GetNode<KinematicBody>("../%Player"), "SetResearchComplete");
        this.Connect("AddFuel", GetNode<KinematicBody>("../%Player"), "AddFuel");

        ResearchAndDev = GetNode<Panel>("ResearchAndDev");
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
        ResearchAndDev.Hide();
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("R&D"))
        {
            ResearchAndDev.Visible = !ResearchAndDev.Visible;
        }
        if (Input.IsActionJustReleased("HideUI"))
        {
            GD.Print(Visible);
            Visible = !Visible;
        }

        if (Input.IsActionJustReleased("pause_menu"))
        {
            GD.Print(PauseMenu.Visible);
            PauseMenu.Visible = !PauseMenu.Visible;
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
        if (H2Ocount > 0)
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
        Researchcount = 0;
    }

    public void _on_Recovery_pressed()
    {

    }

    public void _on_CraftFuel_pressed()
    {
        EmitSignal("AddFuel");
        H2Ocount--;
    }

}