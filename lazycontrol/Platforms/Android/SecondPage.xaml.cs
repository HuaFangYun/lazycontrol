using Android.Bluetooth;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static Android.Graphics.ColorSpace;


namespace lazycontrol.Platforms.Android;

public partial class SecondPage : ContentPage
{
	private BluetoothDevice	 device;
    private IBluetoothConnector connector;
    private ICommuication commuication1;

	public BluetoothDevice CurrentDevice
	{
		get { return device; }
		set { device = value; }
	}

	public SecondPage(IBluetoothConnector connect,ICommuication commuication)
	{
		InitializeComponent();
        this.Loaded += SecondPage_Loaded;
        connector = connect;
        commuication1 = commuication;
        var btn = FindByName("SendData") as Button;
        btn.Clicked += Button_Clicked;
		
	}

    private  void SecondPage_Loaded(object? sender, EventArgs e)
    {
        //connector = new BluetoothConnector();
        connector?.ConnectToDevice(CurrentDevice);
        //Message($"Device Connected {CurrentDevice?.Name}");
        try
        {
            commuication1.Test("from android");
           // connector.DataReceived += Connector_DataReceived;
        }
        catch (Exception ex) {
            Debug.WriteLine($"{ex}: HERE");
        }
    }

    private void Connector_DataReceived(byte[] obj)
    {
        Message("here");
    }

    private async void Message(string message) {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        string text = message;
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(cancellationTokenSource.Token);


    }

    private  void Button_Clicked(object sender, EventArgs e)
    {
        connector.SendData("here in android");
    }
}