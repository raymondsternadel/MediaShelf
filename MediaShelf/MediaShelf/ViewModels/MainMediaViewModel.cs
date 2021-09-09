using DM.MovieApi;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MediaShelf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MediaShelf.ViewModels
{
    public class MainMediaViewModel : INotifyPropertyChanged
    {
        private int currentPage = 1;
        private int lastLoadedPage = 1;
        private IApiMovieRequest MovieAPI;

        private bool moviesBusy = false;
        public ObservableCollection<DisplayMovie> Movies { get; private set; } = new ObservableCollection<DisplayMovie>();
        public ICommand LoadNextPageCommand { get; set; }

        public ICommand SearchMoviesCommand { get; set; }
        public ICommand PickerMovieResultsCommand { get; set; }

        private string currentlyAppliedSearchTerm = string.Empty;
        public string SearchFieldValue { get; set; }
        public string PickerValue { get; set; }

        public MainMediaViewModel()
        {
            MovieDbFactory.RegisterSettings(Constants.APIKey);
            MovieAPI = MovieDbFactory.Create<IApiMovieRequest>().Value;

            SearchMoviesCommand = new AsyncCommand(async () =>
            {
                if (SearchFieldValue != string.Empty)
                {
                    //currentFilterField = "Search";
                    PickerValue = string.Empty;
                    OnPropertyChanged(nameof(PickerValue));
                    await LoadSearchResultsAsync();
                }
            });

            PickerMovieResultsCommand = new AsyncCommand(async () =>
            {
                if (PickerValue != string.Empty)
                {
                    //currentFilterField = "Filter";
                    SearchFieldValue = string.Empty;
                    OnPropertyChanged(nameof(SearchFieldValue));
                    await LoadPickerResultsAsync();
                }
            });

            LoadNextPageCommand = new AsyncCommand(async () =>
            {
                if (Movies.Count < 20)
                    return;

                currentPage += 1;

                if (SearchFieldValue != string.Empty)
                    await LoadSearchResultsAsync(true);
                else if (PickerValue != string.Empty)
                    await LoadPickerResultsAsync(true);
            });
        }

        public async Task LoadSearchResultsAsync(bool isLoadingNextPage = false)
        {
            // Check if the movies list is already being altered
            if (moviesBusy)
            {
                while (moviesBusy)
                {
                    await Task.Delay(25);
                }
            }

            if (SearchFieldValue == currentlyAppliedSearchTerm && currentPage == lastLoadedPage)
                return;

            moviesBusy = true;

            if (!isLoadingNextPage)
                Movies.Clear();

            currentlyAppliedSearchTerm = SearchFieldValue;
            var response = await MovieAPI.SearchByTitleAsync(SearchFieldValue, currentPage);

            foreach (var movieInfo in response.Results)
            {
                var movie = await MovieAPI.FindByIdAsync(movieInfo.Id);
                Movies.Add(new DisplayMovie(movie.Item));
                OnPropertyChanged(nameof(Movies));
            }

            moviesBusy = false;
        }

        public async Task LoadPickerResultsAsync(bool isLoadingNextPage = false)
        {
            if (!isLoadingNextPage)
                Movies.Clear();
            
            ApiSearchResponse<MovieInfo> response = null;
            ApiSearchResponse<Movie> response2 = null;

            switch (PickerValue)
            {
                case "Popular":
                    response = await MovieAPI.GetPopularAsync(currentPage);
                    break;
                case "Top Rated":
                    response = await MovieAPI.GetTopRatedAsync(currentPage);
                    break;
                case "Now Playing":
                    response2 = await MovieAPI.GetNowPlayingAsync(currentPage);
                    break;
                case "Upcoming":
                    response2 = await MovieAPI.GetUpcomingAsync(currentPage);
                    break;
            }

            if (response != null)
            {
                foreach (var movieInfo in response.Results)
                {
                    var movie = await MovieAPI.FindByIdAsync(movieInfo.Id);
                    Movies.Add(new DisplayMovie(movie.Item));
                    OnPropertyChanged(nameof(Movies));
                }
            }

            if (response2 != null)
            {
                foreach (var movie in response2.Results)
                {
                    Movies.Add(new DisplayMovie(movie));
                    OnPropertyChanged(nameof(Movies));
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
