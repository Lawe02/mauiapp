using MauiApp2.Services;
using MauiApp3.Services;

namespace MauiApp3;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        UpdateWeatherAsync();
    }

    private async void UpdateWeatherAsync()
    {
        try
        {
            // Call the GetWeatherAsync method
            float temperature = await WeatherForecastService.GetWeatherAsync();
            // Update UI with the temperature value
            // For example, you might have a Label on your MainPage
            // and you can set its Text property with the temperature 
            temperatureLabel.Text = $"Temperature: {temperature} °C   ";
            Date.Text = DateTime.Now.ToString("D");
            Time.Text = DateTime.Now.ToString("HH:mm");

            if (temperature >= 14) weatherIcon.Source = "C:\\Users\\lawza\\source\\repos\\MauiApp2\\Resources\\Images\\solll.jpeg";
            else if (temperature < 14 && temperature > 0) weatherIcon.Source = "C:\\Users\\lawza\\source\\repos\\MauiApp2\\Resources\\Images\\clouuud.png";
            else weatherIcon.Source = "C:\\Users\\lawza\\source\\repos\\MauiApp2\\Resources\\Images\\snowing.png";
        }
        catch (Exception ex)
        {
            // Handle exceptions if needed
            Console.WriteLine($"Error getting weather: {ex.Message}");
        }


    }
    private async void StartStopSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var _iotHubService = new IoTHubService();
        // Assuming you want to invoke a method when the switch is toggled
        if (e.Value)
        {
            await _iotHubService.InvokeDirectMethodAsync("StartSendingData");
        }
        else
        {
            await _iotHubService.InvokeDirectMethodAsync("StopSendingData");
        }
    }
}