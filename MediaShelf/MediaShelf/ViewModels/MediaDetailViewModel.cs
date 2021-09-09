using DM.MovieApi;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MediaShelf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MediaShelf.ViewModels
{
    public class MediaDetailViewModel
    {
        public DisplayMovie TheDisplayMovie { get; private set; }
        public ObservableCollection<MovieCastMember> Cast { get; private set; } = new ObservableCollection<MovieCastMember>();
        public ObservableCollection<MovieCrewMember> Crew { get; private set; } = new ObservableCollection<MovieCrewMember>();

        private IApiMovieRequest movieAPI;
        public ICommand LoadDataAsyncCommand { get; set; }
        public MediaDetailViewModel(Movie movie)
        {
            movieAPI = MovieDbFactory.Create<IApiMovieRequest>().Value;

            TheDisplayMovie = new DisplayMovie(movie);

            LoadDataAsyncCommand = new AsyncCommand(async () =>
            {
                ApiQueryResponse<MovieCredit> response = await movieAPI.GetCreditsAsync(TheDisplayMovie.TheMovie.Id);

                MovieCredit credit = response.Item;

                foreach (var cast in credit.CastMembers)
                {
                    Cast.Add(cast);
                }

                foreach (var crew in credit.CrewMembers)
                {
                    Crew.Add(crew);
                }
            });
        }
    }
}
