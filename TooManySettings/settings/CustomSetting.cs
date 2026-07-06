namespace TooManySettings.settings;

public class CustomSetting
{
    public string Name => Setting.name;
    public object Default { get; set; }
    public RunSetting Setting { get; set; }
}