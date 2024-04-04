using Microsoft.Data.SqlClient;
using System;
using System.Linq.Expressions;

namespace ATMStarBank
{
    public class VerTransacciones
    {
        private readonly Conexion _conexion;

        public VerTransacciones()
        {
            _conexion = new Conexion();
        }

        public void MostrarTransacciones(string numeroTarjeta)
        {
            Console.WriteLine("--- Transacciones ---");

            if (numeroTarjeta.Transacciones.Count > 0)
            {
                Console.WriteLine("Ordenar transacciones:");
                Console.WriteLine("1. Ascendente");
                Console.WriteLine("2. Descendente");

                int opcion;
                while (true)
                {
                    try
                    {
                        opcion = Convert.ToInt32(Console.ReadLine());
                        if (opcion == 1 || opcion == 2)
                            break;
                        else
                            Console.WriteLine("Ingresa una opción válida (1 o 2).");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ingresa un dato válido.");
                    }
                }

                if (opcion == 1)
                {
                    cuenta.Transacciones.Sort();
                }
                else if (opcion == 2)
                {
                    cuenta.Transacciones.Sort((a, b) => -1 * a.CompareTo(b));
                }

                Console.WriteLine("Transacciones ordenadas:");
                foreach (var transaccion in cuenta.Transacciones)
                {
                    Console.WriteLine(transaccion > 0 ? "Depósito: $" + transaccion : "Retiro: $" + (-transaccion));
                }
            }
            else
            {
                Console.WriteLine("No hay transacciones disponibles.");
            }
        }
    }
}
