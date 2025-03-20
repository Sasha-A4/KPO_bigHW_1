using System;
using System.IO;
using System.Text.Json;
using BankApplication.Model;

namespace BankApplication.Exports
{
    public class JsonExportVisitor : ExportVisitor
    {
        private CommonFacade facade;
        private string filePath;

        public void Visit(CommonFacade facade)
        {
            this.facade = facade;
        }

        public void ExportToFile(string filePath)
        {
            this.filePath = filePath;
            var data = new
            {
                Accounts = facade.GetAccounts(),
                Categories = facade.GetCategories(),
                Operations = facade.GetOperations()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, jsonString);
        }
    }
} 