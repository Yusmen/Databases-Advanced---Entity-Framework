using AdoDotNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Problem5
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

            string countryName = Console.ReadLine();
            List<string> list = new List<string>();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                string updateTownNames = @"UPDATE Towns
   SET Name = UPPER(Name)
 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                connection.Open();

                using (SqlCommand command = new SqlCommand(updateTownNames, connection))
                {

                    command.Parameters.AddWithValue("@countryName", countryName);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} town names were affected.");


                }

                string townNames = @" SELECT t.Name 
   FROM Towns as t
   JOIN Countries AS c ON c.Id = t.CountryCode
  WHERE c.Name = @countryName";

     
                using (SqlCommand command = new SqlCommand(townNames, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add((string)reader[0]);

                        }

                    }


                }


            }
        
            Console.WriteLine(string.Join(" ",list));



        }
    }
}
