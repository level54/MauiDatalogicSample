using Android.App;
using Android.Content.PM;
using Android.OS;
using Com.Datalogic.Decode;
using Com.Datalogic.Device;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiDatalogicSampleApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity, IReadListener
{
    BarcodeManager decoder = null;

    protected override void OnResume()
    {
        base.OnResume();

        // If the decoder instance is null, create it.
        if (decoder == null)
        {
            // Remember an onPause call will set it to null.
            decoder = new BarcodeManager();
        }

        // From here on, we want to be notified with exceptions in case of errors.
        ErrorManager.EnableExceptions(true);

        try
        {
            // add our class as a listener
            decoder.AddReadListener(this);
        }
        catch (DecodeException e)
        {
            Console.WriteLine("Error while trying to bind a listener to BarcodeManager");
        }
    }

    protected override void OnPause()
    {
        base.OnPause();

        // If we have an instance of BarcodeManager.
        if (decoder != null)
        {
            try
            {
                // Unregister our listener from it and free resources
                decoder.RemoveReadListener(this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to remove a listener from BarcodeManager");
            }
        }
    }

    public void OnRead(IDecodeResult decodeResult)
    {
        Console.WriteLine($"Text={decodeResult.Text}");
        Console.WriteLine($"BarcodeID={decodeResult.BarcodeID.ToString()}");

        MainThread.BeginInvokeOnMainThread(() => {
            Console.WriteLine($"WeakReferenceMessenger Send : Format={decodeResult.BarcodeID}, Value={decodeResult.Text}");
            WeakReferenceMessenger.Default.Send(decodeResult);
        });

    }
}
