using Microsoft.Data.SqlClient;
using System;

namespace ATMStarBank
{
    public class RetirarDebito
    {
        private readonly Conexion _conexion;

        public RetirarDebito()
        {
            _conexion = new Conexion();
        }

        public decimal RetirarSaldoDebito(string numeroTarjeta, decimal cantidadARetirar)
        {
            decimal saldoRestante = 0;

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
                                    if (saldoDebito >= cantidadARetirar)
                                    {
                                        saldoRestante = saldoDebito - cantidadARetirar;
                                        Console.WriteLine($"Saldo restante: {saldoRestante}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cantidad solicitada mayor al saldo disponible.");
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
                Console.WriteLine($"Error al retirar saldo de la cuenta de débito: {ex.Message}");
            }

            return saldoRestante;
        }
    }
}
