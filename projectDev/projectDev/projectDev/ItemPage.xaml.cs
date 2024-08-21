using projectDev.Model;
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
    public partial class ItemPage : ContentPage
    {
        public ItemPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                myCollectionView.ItemsSource = await App.MyDatabase.ReadProduct();
            }
            catch { }
        }
        async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductDetail());
        }
        async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var emp = item.CommandParameter as ProductModel;
            await Navigation.PushAsync(new ProductDetail(emp));
        }
        async void SwipeItem_Invoked_1(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var emp = item.CommandParameter as ProductModel;
            var result = await DisplayAlert("Delete", $"Delete {emp.Name} from the Database?", "Yes", "No");
            if (result)
            {
                await App.MyDatabase.DeleteProduct(emp);
                myCollectionView.ItemsSource = await App.MyDatabase.ReadProduct();
            }
        }
        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            myCollectionView.ItemsSource = await App.MyDatabase.Search(e.NewTextValue);
        }
    }
}
