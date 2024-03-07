using Microsoft.AspNetCore.Mvc;
using ProyectoChart.Models;
using ProyectoChart.Models.ViewModels;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ProyectoChart.Controllers
{
    public class HomeController : Controller
    {
        private readonly dbContext _dbContext;
        public HomeController(dbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult resumenVenta()
        {
            DateTime FechaInicio = DateTime.Now;
            FechaInicio = FechaInicio.AddDays(-5);

            List<VMVenta> lista = (from tdVenta in _dbContext.Venta
                                   where tdVenta.FechaRegistro.Value.Date >= FechaInicio.Date
                                   group tdVenta by tdVenta.FechaRegistro.Value.Date
                                   into grupo
                                   select new VMVenta
                                   {
                                       fecha = grupo.Key.ToString("dd/MM/yyyy"),
                                       cantidad = grupo.Count()
                                   }
                                   ) .ToList();

            return StatusCode(StatusCodes.Status200OK, lista); 
        }

        public IActionResult resumenProducto()
        {
            List<VMProducto> lista = (from tdDetalleVenta in _dbContext.DetalleVenta
                                   group tdDetalleVenta by tdDetalleVenta.NombreProducto
                                   into grupo
                                   orderby grupo.Count() descending
                                   select new VMProducto
                                   {
                                       producto = grupo.Key,
                                       cantidad = grupo.Count()
                                   }).Take(4).ToList();

            return StatusCode(StatusCodes.Status200OK, lista);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}