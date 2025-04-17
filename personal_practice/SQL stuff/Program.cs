using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using static System.Console;

namespace SQL_stuff
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=data;Trusted_Connection=True;TrustServerCertificate=true;";
            string query = "select name from sys.databases";

            List<string> dataTableList = new List<string>();
            List<string> tables = new List<string>();
            string input;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command3 = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command3.ExecuteReader())
                    {
                        int index = 0;
                        WriteLine("\t\tDatabase");
                        Write(new string('=', 50));
                        WriteLine();
                        while (reader.Read())
                        {
                            WriteLine($"{index} = {reader["name"]}");
                            dataTableList.Add(reader["name"].ToString());
                            index++;
                        }
                    }
                    Write("Choose database: ");
                    input = ReadLine();
                    int.TryParse(input, out int Input);
                    WriteLine();
                    WriteLine($"\t     Tables in {dataTableList[Input]}");
                    Write(new string('=', 50));
                    WriteLine();
                    tables = GetAllTables(dataTableList[Input], connectionString);
                }
                //ReadKey();
                connection.Close();
            }
            WriteLine("What would you like to do");
            WriteLine("0 - get specific table");
            WriteLine("1 - get all users");
            WriteLine("2 - get a user");
            int answer;
            string temp;
            input = ReadLine();
            int user = int.Parse(input);
            switch (user)
            {
                case 0:
                    Write("Enter table: ");
                    temp = ReadLine();
                    answer = int.Parse(temp);
                    GetTable(answer, connectionString);
                    break;
                case 1:
                    Write("Enter table to get all users: ");
                    temp = ReadLine();
                    answer = int.Parse(temp);
                    GetAllUsers(tables[answer], connectionString);
                    break;
                case 2:
                    Write("Enter user id: ");
                    temp = ReadLine();
                    answer = int.Parse(temp);
                    GetUser(answer, connectionString);
                    break;
            }

            ReadKey();
        }

        static void GetAllUsers(string table, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<string> tables = new List<string>();
                connection.Open();

                using (SqlCommand command = new SqlCommand("select table_name from information_schema.tables", connection)) using (SqlDataReader reader = command.ExecuteReader()) while (reader.Read()) tables.Add(reader["table_name"].ToString());
                string query = $"use data; select * from {table}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            if (i == (dataReader.FieldCount - 1)) Write($"{dataReader.GetName(i)}");
                            else Write($"{dataReader.GetName(i)} | ");
                        }
                        while (dataReader.Read())
                        {
                            WriteLine();
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                if (i == (dataReader.FieldCount - 1)) Write($"{dataReader[i]}");
                                else Write($"{dataReader[i]} | ");
                            }
                            WriteLine();
                        }
                    }
                }
                connection.Close();
            }
        }

        static void GetUser(int id, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"use data; select userfirstname, usermiddlename, userlastname from users where userid = {id}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            if (i == (dataReader.FieldCount - 1)) Write($"{dataReader.GetName(i)}");
                            else Write($"{dataReader.GetName(i)}, ");
                        }
                        while (dataReader.Read())
                        {
                            WriteLine();
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                if (i == (dataReader.FieldCount - 1)) Write($"{dataReader[i]}");
                                else Write($"{dataReader[i]}, ");
                            }
                            WriteLine();
                        }
                    }
                }
                connection.Close();
            }
        }

        static List<string> GetAllTables(string table, string connectionString)
        {
            List<string> tables = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"use {table}; select table_name from information_schema.tables", connection)) using (SqlDataReader reader = command.ExecuteReader()) while (reader.Read()) tables.Add(reader["table_name"].ToString());

                for (int i = 0; i < tables.Count; i++) WriteLine(i + " - " + tables[i]);
                connection.Close();
                return tables;
            }
        }

        static void GetTable(int index, string connectionString)
        {
            List<string> tables = new List<string>();
            List<string> data = new List<string>();
            List<string> numberOfData = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select table_name from information_schema.tables", connection)) using (SqlDataReader reader = command.ExecuteReader()) while (reader.Read()) tables.Add(reader["table_name"].ToString());

                using (SqlCommand command1 = new SqlCommand($"select column_name from information_schema.columns where table_name = '{tables[index]}'", connection)) using (SqlDataReader reader1 = command1.ExecuteReader()) while (reader1.Read()) data.Add(reader1["column_name"].ToString());

                foreach (var item in data)
                {
                    using (SqlCommand command2 = new SqlCommand($"select count('{item}') from information_schema.columns where table_name = '{tables[index]}'", connection)) using (SqlDataReader reader2 = command2.ExecuteReader()) while (reader2.Read()) numberOfData.Add(reader2[0].ToString());
                }

                WriteLine("\t\t" + tables[index]);
                Write(new string('=', 50));
                WriteLine();
                WriteLine("# of entries: " + numberOfData[0]);
                for (int i = 0; i < data.Count; i++) WriteLine(data[i]);
                ReadKey();
                connection.Close();
            }
        }
    }
}
