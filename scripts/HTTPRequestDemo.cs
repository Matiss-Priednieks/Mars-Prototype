using Godot;
using System;
using System.Text;
using Godot.Collections;
class HTTPRequestDemo : CanvasLayer
{
    Label WeatherData;
    HSlider SliderData;
    [Signal] public delegate void UpdateTimeScale(int SliderValue);
    public override void _Ready()
    {
        GetNode("VBoxContainer/HTTPRequest").Connect("request_completed", this, "OnRequestCompleted");
        GetNode("VBoxContainer/Button").Connect("pressed", this, "OnButtonPressed");
        this.Connect("UpdateTimeScale", GetNode<KinematicBody>("../%Player"), "SetTimeScale");
        SliderData = GetNode<HSlider>("LeftUI/HSplitContainer/HSlider");
        WeatherData = GetNode<Label>("VBoxContainer/WeatherData");
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
            // var keyName = keys as Dictionary;
            // var sol = TestData["1219"] as Dictionary;
            // var solAT = sol["AT"] as Dictionary;

            // GD.Print(solAT["av"]);
            // GD.Print(weather);
        }
    }

    public void _on_HSlider_value_changed(float value)
    {
        EmitSignal("UpdateTimeScale", SliderData.Value);
    }
}