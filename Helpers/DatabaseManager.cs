using System.Collections.Generic;
using System.Data.SqlClient;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Entities;

namespace MedicalAppointments.Helpers
{
    public static class DatabaseManager
    {
        private static readonly string connectionString = "Server=localhost;Database=AppEscritorio;Trusted_Connection=True;";

        public static List<Rol> ObtenerRoles()
        {
            var lista = new List<Rol>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("SELECT Id, NombreRol FROM Roles", connection); // Renombramos para encajar con el DTO
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Rol
                    {
                        Id = reader.GetInt32(0),
                        NombreRol = reader.GetString(1)
                    });
                }
            }

            return lista;
        }

        public static List<Especialidad> ObtenerEspecialidades()
        {
            var lista = new List<Especialidad>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("SELECT Id, NombreEspecialidad  FROM Especialidades", connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Especialidad
                    {
                        Id = reader.GetInt32(0),
                        NombreEspecialidad = reader.GetString(1)
                    });
                }
            }

            return lista;
        }
    }
}
