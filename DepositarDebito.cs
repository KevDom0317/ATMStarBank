using Microsoft.Data.SqlClient;
using System;

namespace ATMStarBank
{
    public class DepositarDebito
    {
        private readonly Conexion _conexion;

        public DepositarDebito()
        {
            _conexion = new Conexion();
        }

        public void DepositarSaldoDebito(string numeroTarjeta, decimal cantidadADepositar)
        {
            try
            {
                using (var connection = _conexion.AbrirConexion())
                {
                    if (connection != null)
                    {
                        string query = "SELECT Saldo_Deb FROM Usuarios WHERE Tarjeta_Deb = @NumeroTarjeta";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    decimal saldoDebito = reader.GetDecimal(0);
                                    decimal nuevoSaldo = saldoDebito + cantidadADepositar;

                                    if (nuevoSaldo <= 25000)
                                    {
                                        // Actualizar el saldo en la base de datos
                                        string updateQuery = "UPDATE Usuarios SET Saldo_Deb = @NuevoSaldo WHERE Tarjeta_Deb = @NumeroTarjeta";

                                        using (var updateCommand = new SqlCommand(updateQuery, connection))
                                        {
                                            updateCommand.Parameters.AddWithValue("@NuevoSaldo", nuevoSaldo);
                                            updateCommand.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);

                                            int filasAfectadas = updateCommand.ExecuteNonQuery();

                                            if (filasAfectadas > 0)
                                            {
                                                Console.WriteLine($"Cantidad depositada exitosamente. Nuevo saldo: {nuevoSaldo}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("No se pudo actualizar el saldo en la base de datos.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("La cantidad a depositar excede el límite máximo permitido.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No se encontró la tarjeta de débito en la base de datos.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al depositar saldo en la cuenta de débito: {ex.Message}");
            }
        }
    }
}
