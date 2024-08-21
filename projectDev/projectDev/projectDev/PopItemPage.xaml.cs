using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using System;
using Xamarin.Forms;
using projectDev.Model;
using System.Linq;

namespace projectDev
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopItemPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ProductModel Product { get; set; }

        public PopItemPage(ProductModel product)
        {
            InitializeComponent();
            Product = product;
            BindingContext = Product;
        }
    }
}
