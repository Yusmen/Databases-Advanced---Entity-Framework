using AdoDotNet;
using System;
using System.Data.SqlClient;

namespace Problem6
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());


            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {

                connection.Open();

                string villainQuery = "SELECT Name FROM Villains WHERE Id = @villainId";
                string villainName;
                using (SqlCommand command = new SqlCommand(villainQuery, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);
                    villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }
                }

                int affectedRows = DeleteMinionVillainById(connection, villainId);

                DeleteVillainsById(connection, villainId);
                Console.WriteLine($"{villainName} was deleted");
                Console.WriteLine($"{affectedRows} were released");

            }
        }

        private static void DeleteVillainsById(SqlConnection connection, int villainId)
        {
            string deleteVillain = @"DELETE FROM Villains
      WHERE Id = @villainId";
            using (SqlCommand command = new SqlCommand(deleteVillain, connection))
            {

                command.Parameters.AddWithValue("@villainId", villainId);

                command.ExecuteNonQuery();

            }
        }

        private static int DeleteMinionVillainById(SqlConnection connection, int villainId)
        {

            string deleteVillain = @"DELETE FROM MinionsVillains 
      WHERE VillainId = @villainId";
            using (SqlCommand command = new SqlCommand(deleteVillain, connection))
            {

                command.Parameters.AddWithValue("@villainId", villainId);

                return (int)command.ExecuteNonQuery();

            }
        }
    }
}
