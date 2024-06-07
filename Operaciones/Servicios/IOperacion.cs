using Comunes.Respuesta;
using Operaciones.Modelos;

namespace Operaciones.Servicios;

public interface IOperacion
{
    Task<RespuestaBD> CrearOperacion(EntradaCrearOperacion entradaOperacion);
}
