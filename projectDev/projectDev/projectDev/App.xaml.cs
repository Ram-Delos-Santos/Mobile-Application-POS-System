using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace projectDev
{
    public partial class App : Application
    {
        private static SQLiteHelper db;
        public static SQLiteHelper MyDatabase
        {
            get
            {
                if(db == null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "MyStore.db3"));
                }
                return db;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new HomePage());
            var navigationPage = new NavigationPage(new HomePage());
            navigationPage.BarBackgroundColor = Color.FromHex("#61892f");
            MainPage = navigationPage;
        }
        protected override void OnStart()
        { }
        protected override void OnSleep()
        { }
        protected override void OnResume()
        { }
    }
}
