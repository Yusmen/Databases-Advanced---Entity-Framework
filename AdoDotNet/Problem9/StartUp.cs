using AdoDotNet;
using System;
using System.Data.SqlClient;

namespace Problem9
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string execProc = "exec usp_GetOlder @Id";
                using (SqlCommand command = new SqlCommand(execProc, connection))
                {

                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        string name = (string)reader[0];
                        int age = (int)reader[1];

                        Console.WriteLine($"{name} - {age} years old");
                    }

                }
            }

        }
    }
}
