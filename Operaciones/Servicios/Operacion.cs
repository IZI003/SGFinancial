using AccesoDatos;
using Comunes.Respuesta;
using Dapper;
using Operaciones.Modelos;

namespace Operaciones.Servicios;

public class Operacion: IOperacion
{
    private BD bd;
    public Operacion(BD bd)
    {
        this.bd = bd;
    }

    public async Task<RespuestaBD> CrearOperacion(EntradaCrearOperacion entradaOperacion)
    {
        var salida = new RespuestaBD();
        try
        {
            //controlamos el tipo de operacion y la cuenta existan
            var queryValidador = @"SELECT cta.saldo,tOpe.es_credito, cta.id_cuenta
                        FROM cuenta cta 
                        LEFT JOIN tipo_operacion tOpe ON (tOpe.id_tipo_operacion = @idTipoOperacion)
                        WHERE cta.id_cuenta = @idCuenta";

            using var con = bd.ObtenerConexion();
            var validador = await con.QueryFirstOrDefaultAsync<validadorCuenta>(queryValidador, new { entradaOperacion.idCuenta, entradaOperacion.idTipoOperacion });

            if (validador?.id_cuenta == null || !validador.es_credito.HasValue || entradaOperacion.monto <= 0)
            {
                salida = new RespuestaBD($"ERROR|Error al Crear la Operacion, parametros incorrectos");

                return salida;
            }

            //sumamos o restamos dependiendo de si es credito
            var saldo = validador?.es_credito == true ? (validador?.saldo + entradaOperacion.monto) : (validador?.saldo - entradaOperacion.monto);

            if (saldo < 0)
            {
                salida = new RespuestaBD($"ERROR|Error al Crear la Operacion, Saldo Insuficiente");

                return salida;
            }

            var queryInsertOperacion = @"Insert into operacion (id_tipo_operacion,monto,saldo_actual,id_cuenta,fecha ) 
                        values ( @idTipoOperacion,
                                @monto,
                                @saldo,
                                @idCuenta,GETDATE())";

             await con.QueryAsync(queryInsertOperacion, new {
                 entradaOperacion.idTipoOperacion,
                 entradaOperacion.monto,
                 saldo,
                 entradaOperacion.idCuenta
             });

            var queryUpdateCuenta = @$"UPDATE cuenta
                                        SET saldo = @saldo 
                                        WHERE id_cuenta = @idCuenta;";

            await con.QueryAsync(queryUpdateCuenta, new { saldo , entradaOperacion.idCuenta });


            salida = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida = new RespuestaBD($"ERROR|Error al Crear la cuenta. {ex.Message}");
        }

        return salida;
    }
}
