using MediaShelf.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaShelf
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainMediaPage();
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
