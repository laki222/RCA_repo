using System;
using System.Threading.Tasks;
using HealthMonitoringService.Repository;
using HealthMonitoringService.Models;

namespace AdminTools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string predefinedUsername = "admin";
            string predefinedPassword = "admin";

            AdminRepository adminRepository = new AdminRepository();

            Console.WriteLine("Welcome to the Console Login Application!");

            // Prompt user for username
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            // Prompt user for password
            Console.Write("Enter password: ");
            string password = ReadPassword();

            // Check if entered credentials match the predefined ones
            if (username == predefinedUsername && password == predefinedPassword)
            {
                Console.WriteLine("\nLogin successful! Welcome, " + username + "!");
                ShowMenuAsync(username, password, adminRepository);
            }
            else
            {
                Console.WriteLine("\nLogin failed! Invalid username or password.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Method to read password without displaying it in the console
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*"); // Print asterisk instead of actual character
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        public static void ShowMenuAsync(string username, string password, AdminRepository adminRepository)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add Admin");
                Console.WriteLine("2. Remove Admin");
                Console.WriteLine("3. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAdminToTableAsync(adminRepository);
                        break;
                    case "2":
                        RemoveAdminFromTableAsync(adminRepository);
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static void AddAdminToTableAsync( AdminRepository adminRepository)
        {
            Console.Write("Enter email: ");
            string username = Console.ReadLine();

            adminRepository.AddAdmin(username);
            Console.WriteLine("Admin added successfully.");
        }

        public static void RemoveAdminFromTableAsync(AdminRepository adminRepository)
        {

            Console.Write("Enter email: ");
            string username = Console.ReadLine();

            adminRepository.DeleteAdmin(username);
            Console.WriteLine("Admin removed successfully.");
        }
    }
}
