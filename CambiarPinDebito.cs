using Microsoft.Data.SqlClient;
using System;

namespace ATMStarBank
{
    public class CambiarPINDebito
    {
        private readonly Conexion _conexion;

        public CambiarPINDebito()
        {
            _conexion = new Conexion();
        }

        public void CambiarPINDebito(string numeroTarjeta, string nuevoPIN)
        {
            try
            {
                // Verificar que el nuevo PIN no sea igual al PIN actual
                if (ValidarNuevoPIN(numeroTarjeta, nuevoPIN))
                {
                    // El nuevo PIN es diferente al actual, proceder con la actualización
                    using (var connection = _conexion.AbrirConexion())
                    {
                        if (connection != null)
                        {
                            // Actualizar el PIN en la base de datos
                            string updateQuery = "UPDATE Usuarios SET PIN_Deb = @NuevoPIN WHERE Tarjeta_Deb = @NumeroTarjeta";

                            using (var command = new SqlCommand(updateQuery, connection))
                            {
                                command.Parameters.AddWithValue("@NuevoPIN", nuevoPIN);
                                command.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);

                                int filasAfectadas = command.ExecuteNonQuery();

                                if (filasAfectadas > 0)
                                {
                                    Console.WriteLine("PIN de la cuenta de débito actualizado exitosamente.");
                                }
                                else
                                {
                                    Console.WriteLine("No se pudo actualizar el PIN en la base de datos.");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("El nuevo PIN debe ser diferente al PIN actual.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar el PIN de la cuenta de débito: {ex.Message}");
            }
        }

        private bool ValidarNuevoPIN(string numeroTarjeta, string nuevoPIN)
        {
            // Obtener el PIN actual de la base de datos
            string pinActual = ObtenerPINActual(numeroTarjeta);

            // Comparar el nuevo PIN con el PIN actual
            return !string.Equals(pinActual, nuevoPIN);
        }

        private string ObtenerPINActual(string numeroTarjeta)
        {
            string pinActual = null;

            try
            {
                using (var connection = _conexion.AbrirConexion())
                {
                    if (connection != null)
                    {
                        // Consultar el PIN actual en la base de datos
                        string selectQuery = "SELECT PIN_Deb FROM Usuarios WHERE Tarjeta_Deb = @NumeroTarjeta";

                        using (var command = new SqlCommand(selectQuery, connection))
                        {
                            command.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);

                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    pinActual = reader["PIN_Deb"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el PIN actual de la cuenta de débito: {ex.Message}");
            }

            return pinActual;
        }
    }
}
