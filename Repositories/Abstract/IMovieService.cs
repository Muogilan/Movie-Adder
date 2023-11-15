using MovieStore.Models.Domain;

namespace MovieStore.Repositories.Abstract
{

        public interface IMovieService
        {
            bool Add(Movie model);
            bool Update(Movie model);
            Movie GetById(int id);
            bool Delete(int id);
            IQueryable<Movie> List();

        }
}

