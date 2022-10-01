using MetroBoulotDodo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFREI_MetroBoulotDodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetroController : Controller
    {
        private metroService _metroService;

        public MetroController(metroService metroService)
        {
            this._metroService = metroService;
        }

        [HttpGet("Crearbre")]
        public IEnumerable<string> Crearbre()
        {

            return new List<string>() { "c#", "sql" };
        }

        [HttpGet("Retourarbre")]
        public string RetourStation()
        {
            
            return _metroService.retourarbre(); ;
        }

        [HttpGet("isConnexe")]
        public string isConnexe()
        {

            return _metroService.isConnexe(); ;
        }


    }
}
