using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.Models.Domain;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genreService;
        public MovieController(IMovieService MovieService, IFileService fileService, IGenreService genreService)
        {
            _movieService = MovieService;
            _fileService = fileService;
            _genreService = genreService;
        }
        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var data = _movieService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(Movie model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = _movieService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(MovieList));
            }
            else
            {
                TempData["msg"] = "Error on server side";   
                return View(model);
            }
        }

        public IActionResult MovieList()
        {
            var data = this._movieService.List();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var result = _movieService.Delete(id);
            return RedirectToAction(nameof(MovieList));
        }



    }
}
