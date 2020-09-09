using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace Pradeep_PhoneBook
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "Brush_Experimental" });
            MainPage = new MainPage();
            SQLiteConnection con = DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
