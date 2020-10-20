using AdoDotNet;
using System;
using System.Data.SqlClient;

namespace Problem4
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

            string[] minionInfo = Console.ReadLine().Split();
            string[] villainInfo = Console.ReadLine().Split();

            string minionName = minionInfo[1];
            int age = int.Parse(minionInfo[2]);
            string townName = minionInfo[3];

            string villainName = villainInfo[1];


            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {

                connection.Open();

                int? id = GetTownByName(townName, connection);

                if (id == null)
                {
                    AddTown(connection, townName);
                }

                id = GetTownByName(townName, connection);

                AddMinion(connection, minionName, age, id,villainName);

                int? villainId = GetVillainByName(connection, villainName);

                if (villainId == null)
                {
                    AddVillain(connection, villainName);
                }

                villainId = GetVillainByName(connection, villainName);

                int? minionId = GetMinionByName(connection, minionName);

                AddMinionVillain(connection, villainId, minionId);
            }

        }

        private static void AddMinionVillain(SqlConnection connection, int? villainId, int? minionId)
        {
            string insertMinionVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using (SqlCommand command = new SqlCommand(insertMinionVillain, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", minionId);
                command.ExecuteNonQuery();

            }
        }

        private static int? GetMinionByName(SqlConnection connection, string minionName)
        {
            string minionIdQuerry = "SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(minionIdQuerry, connection))
            {
                command.Parameters.AddWithValue("@Name", minionName);

                return (int?)command.ExecuteScalar();
            }
        }

        private static void AddVillain(SqlConnection connection, string villainName)
        {
            string insertVillain = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
            }
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static int? GetVillainByName(SqlConnection connection, string villainName)
        {
            string townIdQuerry = "SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(townIdQuerry, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);

                return (int?)command.ExecuteScalar();

            }

        }

        private static void AddMinion(SqlConnection connection, string minionName, int age, int? id,string villainName)
        {
            string insertMinionSql = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinionSql, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", id);

                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            

        }

        private static int? GetTownByName(string townName, SqlConnection connection)
        {
            string townIdQuerry = "SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(townIdQuerry, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);

                return (int?)command.ExecuteScalar(); 

            }

        }

        private static void AddTown(SqlConnection connection, string townName)
        {
            string insertTownSql = "INSERT INTO Towns (Name) VALUES (@townName)";
            using (SqlCommand command = new SqlCommand(insertTownSql, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);
                command.ExecuteNonQuery();

            }
            Console.WriteLine($"Town {townName} was added to the database.");
        }
    }
}
