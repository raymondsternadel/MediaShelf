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
    public partial class MediaDetailPage : ContentPage
    {
        public MediaDetailPage(Movie movie)
        {
            InitializeComponent();

            BindingContext = new MediaDetailViewModel(movie);
        }
    }
}