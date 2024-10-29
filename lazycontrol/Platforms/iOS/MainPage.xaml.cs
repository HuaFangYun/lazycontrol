namespace lazycontrol.Platforms.iOS;

public partial class MainPage : ContentPage
{

    private readonly IPlatformService _platformService;

    public MainPage(IPlatformService platformService)
    {
        InitializeComponent();
        _platformService = platformService;
        DisplayPlatformMessage();
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        DisplayAlert("Alert", "iOS button clicked!", "OK");
    }

    private void DisplayPlatformMessage()
    {
        DisplayAlert("Platform Message", _platformService.GetPlatformMessage(), "OK");
    }
}