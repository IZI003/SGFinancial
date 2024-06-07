namespace Operaciones.Modelos;

public class EntradaCrearOperacion
{
   public int idCuenta { get; set; }
    public int idTipoOperacion { get; set; }
    public int idConcepto { get; set; }
    public decimal monto { get; set; }
}