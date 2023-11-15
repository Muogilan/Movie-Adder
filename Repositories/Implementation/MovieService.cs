using MovieStore.Models.Domain;
using MovieStore.Models.DTO;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext ctx;
        public MovieService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Movie model)
        {
            try
            {

                ctx.Movie.Add(model);
                ctx.SaveChanges();
                foreach (int genreId in model.Genre)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    };
                    ctx.MovieGenre.Add(movieGenre);
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                ctx.Movie.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
            return ctx.Movie.Find(id);
        }

        public MovieListVm List()
        {
            var List = ctx.Movie.AsQueryable();
            var data = new MovieListVm
            {
                MovieList = List
            };
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                ctx.Movie.Update(model);
                //we have to add these genre ids on moviegenre table
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
