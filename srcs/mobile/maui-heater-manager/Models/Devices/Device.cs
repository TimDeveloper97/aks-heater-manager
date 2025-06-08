namespace maui_heater_manager.Models.Devices;

public class Device
{
    public string Title { get; set; }
    public string Status { get; set; }
    public string Icon { get; set; }
    public Color Color { get; set; } = Colors.White; 
}
