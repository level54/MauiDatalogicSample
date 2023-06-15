using Com.Datalogic.Decode;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiDatalogicSampleApp;

public partial class MainPage : ContentPage, IRecipient<IDecodeResult>
{
	public MainPage()
	{
		InitializeComponent();
        WeakReferenceMessenger.Default.Register<IDecodeResult>(this);
    }

    public void Receive(IDecodeResult message)
    {
        Console.WriteLine($"WeakReferenceMessenger Receive : Format={message.BarcodeID}, Value={message.Text}");

        txtBarcodeText.Text = message.Text;
		txtBarcodeID.Text = message.BarcodeID.ToString();
    }
}

