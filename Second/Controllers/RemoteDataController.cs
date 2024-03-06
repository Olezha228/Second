using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Second.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemoteDataController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RemoteDataController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _httpClient.GetAsync("http://host.docker.internal:32768/Data");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode, "Error fetching data from ProjectA");
        }
    }
}
