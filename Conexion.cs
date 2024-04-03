using Microsoft.Data.SqlClient;
using System.Text;

namespace ATMStarBank;

public class Conexion
{
    // CAMBIA EL NOMBRE DE TU SERVIDOR PLSSS
    private string Stringconexion="Server=KEVDOMINICK;Database=ATMStarBank;Integrated Security=True;TrustServerCertificate=True;";
    public SqlConnection conexion;
    public Conexion()
    {
        conexion =new SqlConnection(Stringconexion);
    }
    public SqlConnection? AbrirConexion()
    {
        try
        {
            conexion.Open();
            return conexion;    
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se realizó la conexión: Error {ex.Message}");
            Console.ReadKey();
            return null;
        }
        
    }
    public SqlConnection CerrarConexion()
    {
        conexion.Close();
        return conexion;
    }

    
}