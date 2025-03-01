﻿using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;

namespace BestBuyCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            var departments = GetAllDepartments();

            foreach (var department in departments)
            {
                Console.WriteLine(department);

                LoggerMessage($"Returning {department}");

            }

            try
            {
                Console.WriteLine(departments[8]);
            }
            catch (Exception e)
            {
                LoggerError(e);
            }

            Console.WriteLine("Here are your current departments that you manage. You have to add a new department for the new sports section.");
            Console.WriteLine("Do you want to create a new sports section? Type 'yes' or 'no'.");

            if (Console.ReadLine().Contains("yes"))
            {
                Console.WriteLine("What would you like to name the department?");
                InsertNewDepartment(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("You should probably create a new department if you want to do this correctly.");
                Console.WriteLine("So do you want to add a new department or what? Type 'yes' or 'no'.");

                if (Console.ReadLine().Contains("yes"))
                {
                    Console.WriteLine("What would you like to name the department?");
                    InsertNewDepartment(Console.ReadLine());
                }
                else
                {
                    Console.WriteLine("Okay then.");
                }
            }
            Console.WriteLine("Now go away!");

        }

        static List<string> GetAllDepartments()
        {
            MySqlConnection conn = new MySqlConnection();

            try
            {
                LoggerMessage("Accessing Connection File");
                conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");
            }
            catch (Exception e)
            {
                LoggerError(e);
            }
            

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT name FROM departments;";

            using (conn)
            {
                conn.Open();
                List<string> allDepartments = new List<string>();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    var currentDepartment = reader.GetString("Name");
                    allDepartments.Add(currentDepartment);
                }

                return allDepartments;
            }
        }

        static void InsertNewDepartment(string newDepartmentName)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO departments (name) VALUES (@deptname);";
            cmd.Parameters.AddWithValue("deptName", newDepartmentName);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        static void DeleteDepartment(string departmentName)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM departments WHERE Name = @departmentName;";
            cmd.Parameters.AddWithValue("departmentName", departmentName);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        static void UpdateDepartment(string currentDepartmentName, string newDepartmentName)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE departments SET name = @newName WHERE name = @currentName;";
            cmd.Parameters.AddWithValue("newName", newDepartmentName);
            cmd.Parameters.AddWithValue("currentName", currentDepartmentName);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Testing using int instead of strings
        static void DeleteDepartment2(string departmentName, int departmentID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("connectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM departments WHERE name = @departmentName, id = @departmentID";
            cmd.Parameters.AddWithValue("departmentName", departmentName);
            cmd.Parameters.AddWithValue("departmentID", departmentID);
        }

        // Logging Information -- Logs found in log.txt
        static void LoggerMessage(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Environment.NewLine} ------------------------------- {Environment.NewLine}");
            sb.Append($"{message} {DateTime.Now} -- accessed by user named '{Environment.UserName}'");
            sb.Append($"{Environment.NewLine} ------------------------------- {Environment.NewLine}");
            var filePath = "logs/";
            File.AppendAllText(filePath + "log.txt", sb.ToString());
        }

        static void LoggerError(Exception error)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Environment.NewLine} --------------ERROR ERROR--------------ERROR ERROR-------------- {Environment.NewLine}");
            sb.Append($"{error.Message} {DateTime.Now} -- accessed by user named '{Environment.UserName}'");
            sb.Append($"{Environment.NewLine} --------------ERROR ERROR--------------ERROR ERROR-------------- {Environment.NewLine}");
            var filePath = "logs/";

            File.AppendAllText(filePath + "log.txt", sb.ToString());
        }

    }
}
