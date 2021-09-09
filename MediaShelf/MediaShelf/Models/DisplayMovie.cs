using DM.MovieApi.MovieDb.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaShelf.Models
{
    public class DisplayMovie
    {
        public Movie TheMovie { get; private set; }
        public string PosterPath
        {
            get
            {
                return $"{Constants.ImageURL}{TheMovie.PosterPath}";
            }
        }

        public DisplayMovie(Movie movie)
        {
            TheMovie = movie;
        }
    }
}
