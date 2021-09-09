using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using MediaShelf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaShelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMediaPage : ContentPage
    {
        public MainMediaPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter as Movie;
            await Navigation.PushModalAsync(new MediaDetailPage(param));
        }
    }
}