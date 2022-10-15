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
        public string Crearbre()
        {

            return _metroService.Crearbre();
        }

        [HttpGet("Retourarbre")]
        public string RetourStation()
        {

            return _metroService.retourarbre(); ;
        }

        [HttpGet("isConnexe")]
        public bool isConnexe()
        {

            return _metroService.isConnexe();
        }

        [HttpGet("Dijstra/{sta1}/{sta2}")]
        public string getDijkstra(int sta1, int sta2)
        {

            return _metroService.getDijkstra(sta1, sta2);
        }

        [HttpGet("ACPM")]
        public string getACPM()
        {

            return _metroService.getACPM();
        }



    }
}
