using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using redis.Model;
using StackExchange.Redis;

namespace redis.Controllers
{
    [ApiController]
    [Route("Api/Book")]
    public class BookController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;

        public BookController(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis;

        }
        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook(Book model)
        {
            var content = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model));

            await _cache.SetAsync("Book_" + model.Isbn, content, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            });
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var redisKeys = _redis.GetServer("127.0.0.1", 9191).Keys(pattern: "Book_*")
                .AsQueryable().Select(p => p.ToString()).ToList();

            var result = new List<Book>();

            foreach (var redisKey in redisKeys)
            {
                result.Add(JsonSerializer.Deserialize<Book>(await _cache.GetStringAsync(redisKey)));
            }

            return Ok(result);

        }
        [HttpGet("GetBook")]
        public async Task<IActionResult> GetBook(string isbn)
        {
            var bookContent = await _cache.GetStringAsync($"Book_{isbn}");

            if (bookContent == null)
                return NotFound();


            return Ok(JsonSerializer.Deserialize<Book>(bookContent));
        }
        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            await _cache.RemoveAsync($"Book_{isbn}");

            return Ok();
        }
    }
}