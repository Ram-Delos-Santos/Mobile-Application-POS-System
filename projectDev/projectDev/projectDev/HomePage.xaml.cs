using projectDev.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace projectDev
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public UserModel User { get; set; }
        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                User = await App.MyDatabase.GetUser();
                OnPropertyChanged(nameof(User));
                myCollectionView.ItemsSource = await App.MyDatabase.ReadProduct();
            }
            catch { }
        }
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FlyoutMenuPage());
        }
        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PopUpPage());
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductDetail());
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            myCollectionView.ItemsSource = await App.MyDatabase.Search(e.NewTextValue);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var product = (ProductModel)frame.BindingContext;
            await PopupNavigation.Instance.PushAsync(new PopItemPage(product));
        }
    }
}
