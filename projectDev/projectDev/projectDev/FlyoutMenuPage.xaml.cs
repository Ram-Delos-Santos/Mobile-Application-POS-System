using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projectDev
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutMenuPage : ContentPage
    {
        ObservableCollection<FlyoutItemPage> items;

        public FlyoutMenuPage()
        {
            InitializeComponent();

            items = new ObservableCollection<FlyoutItemPage>
            {
                new FlyoutItemPage {IconSource = "home.png", Title="Home", TargetPage=typeof(HomePage)},
                new FlyoutItemPage {IconSource = "item.png", Title="Item List", TargetPage=typeof(ItemPage)},
                new FlyoutItemPage {IconSource = "setting.png", Title="Account Settings", TargetPage = typeof(SettingPage)},
                new FlyoutItemPage {IconSource = "about.png", Title="About", TargetPage = typeof(AboutPage)}
            };
            myListView.ItemsSource = items;
        }

        async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is FlyoutItemPage selectedItem)
            {
                Page page = (Page)Activator.CreateInstance(selectedItem.TargetPage);
                await Navigation.PushAsync(page);
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}
