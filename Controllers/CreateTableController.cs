
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class CreateTableController : Controller
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString = "Server=localhost;Database=Proyecto;Trusted_Connection=True;TrustServerCertificate=True";

        // Acción para mostrar el formulario (GET)
        [HttpGet]
        public ActionResult CrearTabla()
        {
            return View();
        }

        // Acción para procesar el formulario (POST)
        [HttpPost]
        public ActionResult CrearTabla(
            string tableName,
            List<string> columnNames,
            List<string> columnTypes,
            List<int?> columnSizes,
            List<bool> allowNulls,
            List<bool> isPrimaryKeys,
            List<bool> isUniques,
            List<string> defaultValues)
        {
            // Validación de entrada
            if (string.IsNullOrEmpty(tableName) || columnNames == null || columnNames.Count == 0 || columnTypes == null || columnTypes.Count == 0)
            {
                ViewData["ErrorMessage"] = "Debe proporcionar un nombre de tabla y al menos una columna.";
                return View();
            }

            try
            {
                // Generar la consulta SQL para crear la tabla
                string query = $"CREATE TABLE {tableName} (";

                for (int i = 0; i < columnNames.Count; i++)
                {
                    // Construir definición de la columna
                    string columnDefinition = $"{columnNames[i]} {columnTypes[i]}";

                    // Agregar tamaño si aplica (por ejemplo, VARCHAR(255))
                    if (columnTypes[i].ToUpper() == "VARCHAR" && columnSizes[i].HasValue)
                    {
                        columnDefinition += $"({columnSizes[i]})";
                    }

                    // Agregar restricciones
                    if (isPrimaryKeys[i])
                    {
                        columnDefinition += " PRIMARY KEY";
                    }
                    else
                    {
                        if (isUniques[i]) columnDefinition += " UNIQUE";
                        if (!allowNulls[i]) columnDefinition += " NOT NULL";
                    }

                    // Agregar valor predeterminado si se especifica
                    if (!string.IsNullOrEmpty(defaultValues[i]))
                    {
                        columnDefinition += $" DEFAULT '{defaultValues[i]}'";
                    }

                    // Agregar coma si no es la última columna
                    query += columnDefinition;
                    if (i < columnNames.Count - 1) query += ", ";
                }

                query += ");";

                // Ejecutar la consulta SQL
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                // Notificar éxito
                ViewData["SuccessMessage"] = "Tabla creada exitosamente.";
                return View();
            }
            catch (Exception ex)
            {
                // Notificar error
                ViewData["ErrorMessage"] = $"Error al crear la tabla: {ex.Message}";
                return View();
            }
        }
    }
}