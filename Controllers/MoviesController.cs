using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDataAccess.DataAccess;
using MongoDataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieSuggestionService _movieSuggestionService;
        public MoviesController(MovieSuggestionService movieSuggestionService)
        {
            _movieSuggestionService = movieSuggestionService;
        }

        // GET: api/<MoviesController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _movieSuggestionService.GetMoviesAsync());
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public async Task<Movie> Get(string id)
        {
            return await _movieSuggestionService.GetMovieAsync(id);
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie movie)
        {
            await _movieSuggestionService.CreateMovie(movie);
            return CreatedAtAction(nameof(Get), new { id = movie.Id}, movie);
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Movie movie)
        {
            await _movieSuggestionService.UpdateMovie(id, movie);
            return NoContent();
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _movieSuggestionService.DeleteMovie(id);
            return NoContent();
        }
    }
}
