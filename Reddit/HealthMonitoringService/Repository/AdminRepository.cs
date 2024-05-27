using HealthMonitoringService.Model;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthMonitoringService.Models;
using System.IO;

namespace HealthMonitoringService.Repository
{
    public class AdminRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        private string filePath="adminList.txt";
        public AdminRepository()
        {
          
        }


        public void AddAdmin(string email)
        {

           
                using (StreamWriter writer = new StreamWriter(filePath, true)) 
                {
                    writer.WriteLine(email);
                }
               
           
        }

        public List<string> ReadAdmins()
        {
            List<string> admins = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        admins.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Došlo je do greške prilikom čitanja iz fajla: {ex.Message}");
            }

            return admins;
        }

        public void DeleteAdmin(string admin)
        {

            List<string>lista=ReadAdmins();

            
            if (lista.Remove(admin))
            {
                using (StreamWriter writer = new StreamWriter(filePath)) // pišemo ponovo ceo fajl bez obrisanog admina
                {
                    foreach (var email in lista)
                    {
                        writer.WriteLine(email);
                    }
                }
               
            }
          
        }



    }
}
