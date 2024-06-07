namespace Comunes.Parametros;
internal class TipoOperacion
{
    public static IEnumerable<string> TipoOperacionList = new List<string>
    {
        "DEBITO_RETITO",
        "DEBITO_PAGO_SERVICIO",
        "DEBITO_TRANSFERENCIA",
        "CREDITO_TRANSFERENCIA",
        "CREDITO_DEPOSITO",
        "CREDITO_HABERES",
        "CREDITO_INTERESES",
     };

    public static bool EsTipoOperacionValido(string value)
    {
        return TipoOperacionList.Contains(value.ToUpper().Trim());
    }
}
