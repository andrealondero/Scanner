using Plugin.Media;
using Plugin.Media.Abstractions;

using ZXing.Net.Mobile.Forms;

using Xamarin.Forms;
using System;

namespace Scanner
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            photoButton.Clicked += async (sender, args) =>
            {
                //await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("ERRORE", "Impossibile accedere alla fotocamera", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Front
                    //Name = DateTime.Now + scannedCode.Text + ".jpg"
                });

                if (file == null)
                    return;
                await DisplayAlert("File Location", file.Path, "OK");

                imageTake1.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };

            scanButton.Clicked += async (sender, args) => 
            {
                var scanner = new ZXingScannerPage();
                await Navigation.PushAsync(scanner);
                scanner.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopAsync();
                        scannedCode.Text = result.Text;
                    });
                };
            };
        }

        private async void AddPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Images",
                Name = DateTime.Now + scannedCode.Text + ".jpg"
            });

            if (photo == null)
                return;
            await DisplayAlert("File Location", photo.Path, "OK");

            if (imageTake2 == null)
            {
                imageTake2.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });
            }
            else
            {
                imageTake3.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });
            }
        }

        

        private void Firma_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}
