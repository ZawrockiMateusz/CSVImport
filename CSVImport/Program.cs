using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVImport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Data Source:");
            var dataSource = Console.ReadLine();
            Console.WriteLine("CSV File Path:");
            var filePath = Console.ReadLine();
            Console.WriteLine("Database Name:");
            var dbName = Console.ReadLine();
            Console.WriteLine("Table name:");
            var tableName = Console.ReadLine();

            var lineNumber = 0;
            using (SqlConnection conn = new SqlConnection(@"data source=" + dataSource + ";initial catalog=" + dbName + ";trusted_connection=true")) //establishing connection string 
            {
                conn.Open(); //opening connection with db
                using (StreamReader reader = new StreamReader(@filePath)) //path for .csv file
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if(lineNumber != 0)
                        {
                            var values = line.Split(';');

                            var sql = "INSERT INTO " + dbName + ".dbo." + tableName + " VALUES ('" + values[0] + "','" + values[1] + "'," + values[2] + ")"; //preparing sql query

                            //inserting .csv data into database
                            var cmd = new SqlCommand(); 
                            cmd.CommandText = sql;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                        lineNumber += 1;
                    }
                }
                conn.Close(); //closing connection with db
            }
            Console.WriteLine("Import Zakończony");
            Console.ReadKey();
        }
    }
}
