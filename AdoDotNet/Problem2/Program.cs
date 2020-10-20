using AdoDotNet;
using System;
using System.Data.SqlClient;

namespace Problem2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                string sqlQuerry = @" SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
    FROM Villains AS v
    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
GROUP BY v.Id, v.Name
  HAVING COUNT(mv.VillainId) > 3
ORDER BY COUNT(mv.VillainId)";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuerry, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader["Name"];
                            int count = (int)reader[1];

                            Console.WriteLine($"{name} - {count}");

                        }
                    }
                }
            }
        }
    }
}
