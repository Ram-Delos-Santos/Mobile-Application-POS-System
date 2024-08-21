using projectDev.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projectDev
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        Model.UserModel _user;
        public SettingPage()
        {
            InitializeComponent();
        }
        public SettingPage(UserModel user) : this()
        {
            Title = "Edit User";
            _user = user;
            nameprofEntry.Text = user.ProfName;
            emailEntry.Text = user.Email;
            contactEntry.Text = user.Phone;
            imageprofEntry.Text = user.ProfImage;
            nameprofEntry.Focus();
        }

        async void imageprofEntry_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Please select an image file"
                });
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var filePath = await SaveFileToLocalStorage(result.FileName, stream);
                    imageprofEntry.Text = filePath;
                    profImage.Source = ImageSource.FromFile(filePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to import image: {ex.Message}", "OK");
            }
        }

        private async Task<string> SaveFileToLocalStorage(string fileName, Stream fileStream)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using (var file = File.Create(filePath))
            {
                await fileStream.CopyToAsync(file);
            }
            return filePath;
        }
        private async Task<string> SaveImageToLocalStorage(string imageUrl)
        {
            if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                var webClient = new System.Net.WebClient();
                var imageBytes = await webClient.DownloadDataTaskAsync(new Uri(imageUrl));
                var filePath = Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(imageUrl));
                File.WriteAllBytes(filePath, imageBytes);
                return filePath;
            }
            return imageUrl;
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameprofEntry.Text) || string.IsNullOrWhiteSpace(emailEntry.Text) || string.IsNullOrWhiteSpace(contactEntry.Text))
            {
                await DisplayAlert("Invalid", "Blank is Invalid!", "OK");
                return;
            }
            var newUser = new UserModel
            {
                ProfName = nameprofEntry.Text,
                Email = emailEntry.Text,
                Phone = contactEntry.Text,
                ProfImage = imageprofEntry.Text
            };
            await App.MyDatabase.DeleteAllUsers();
            await App.MyDatabase.CreateUser(newUser);
            await Navigation.PopAsync();
        }
    }
}