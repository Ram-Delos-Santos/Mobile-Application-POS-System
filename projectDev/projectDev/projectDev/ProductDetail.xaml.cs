using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using projectDev.Model;

namespace projectDev
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetail : ContentPage
    {

        Model.ProductModel _product;

        public ProductDetail()
        {
            InitializeComponent();
        }

        public ProductDetail(ProductModel product) : this()
        {
            Title = "Edit Product";
            _product = product;
            nameEntry.Text = product.Name;
            descEntry.Text = product.Description;
            priceEntry.Text = product.Price;
            imageEntry.Text = product.Image;
            nameEntry.Focus();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) || string.IsNullOrWhiteSpace(descEntry.Text) || string.IsNullOrWhiteSpace(priceEntry.Text))
            {
                await DisplayAlert("Invalid", "Blank is Invalid!", "OK");
                return;
            }

            if (_product != null)
            {
                _product.Name = nameEntry.Text;
                _product.Description = descEntry.Text;
                _product.Price = priceEntry.Text;
                _product.Image = imageEntry.Text;
                await App.MyDatabase.UpdateProduct(_product);
            }
            else
            {
                var newProduct = new ProductModel
                {
                    Name = nameEntry.Text,
                    Description = descEntry.Text,
                    Price = priceEntry.Text,
                    Image = imageEntry.Text
                };
                await App.MyDatabase.CreateProduct(newProduct);
            }

            await Navigation.PopAsync();
        }

        async void Button_Clicked_1(object sender, EventArgs e)
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
                    imageEntry.Text = filePath;
                    productImage.Source = ImageSource.FromFile(filePath);
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", $"Failed to import image: {ex.Message}", "OK");
            }
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
        private async Task<string> SaveFileToLocalStorage(string fileName, Stream fileStream)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using (var file = File.Create(filePath))
            {
                await fileStream.CopyToAsync(file);
            }
            return filePath;
        }
    }
}
