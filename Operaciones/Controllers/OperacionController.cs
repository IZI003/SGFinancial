using Comunes.Config;
using Comunes.Respuesta;
using Operaciones.Modelos;
using Operaciones.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Operaciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionController : Controller
    {
        private readonly IOperacion operacionService;

        public OperacionController(IOperacion operacionService)
        {
            this.operacionService = operacionService;
        }

        // POST: OperacionController/Create
        [HttpPost]
        public async Task<IActionResult> CrearOperacion([FromBody] EntradaCrearOperacion entradaOperacion)
        {
            var response = new RespuestaApi<RespuestaBD>();
            var salida = await operacionService.CrearOperacion(entradaOperacion);

            if (!salida.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.O_Men_200, 400, mensaje: salida.Mensaje, codigoInterno: GestionErrores.O_Cod_200);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida;

            return Ok(response);
        }
    }
}
