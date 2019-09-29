using System;
using System.Collections;
using Npgsql;

namespace ConsoleAppSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=postgres;Password=Aa3sdf&&;Database=postgres";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var databaseTable = "numtable";
                
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + databaseTable +
                        " (id SERIAL PRIMARY KEY, number INTEGER NOT NULL UNIQUE)";
                    cmd.ExecuteNonQuery();
                }
                
                var arrayNumbers = new ArrayList { 17, 27, 37, 43, 10, 88, 16, 23, 88};
                foreach (var number in arrayNumbers)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO " + databaseTable +
                                          " (number) VALUES (" + number + ") ON CONFLICT (number) DO NOTHING";
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var cmd = new NpgsqlCommand("SELECT * FROM " + databaseTable, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("|---------------|---------------|");
                        Console.WriteLine("|       id      |     number    |");
                        Console.WriteLine("|---------------|---------------|");
                        while (reader.Read())
                        {
                            Console.Write("|\t{0}\t|\t{1}\t|\n", reader[0], reader[1]);
                            Console.WriteLine("|---------------|---------------|");
                        }
                    }   
                }
                
                connection.Close();
            }
        }
    }
}