using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CategoryManagement.Helpers
{
    public static class DatabaseHelper
    {
        private const string ConnectionString = "Server=localhost;Database=CategoryDB;User=root;Password=Kartik@123;";

        public static MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (MySqlException ex)
            {
                connection.Dispose();
                throw new InvalidOperationException(
                    "Could not connect to the database. Please check that MySQL is running and the connection settings are correct.", ex);
            }
        }
    }
}
