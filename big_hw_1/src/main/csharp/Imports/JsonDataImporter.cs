using System;
using System.IO;
using System.Text.Json;
using BankApplication.Model;

namespace BankApplication.Imports
{
    public class JsonDataImporter : DataImporter
    {
        private readonly CommonFacade facade;

        public JsonDataImporter(CommonFacade facade)
        {
            this.facade = facade;
        }

        public void ImportFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} does not exist.");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<ImportData>(jsonString);

                // Import accounts
                foreach (var account in data.Accounts)
                {
                    facade.CreateBA(account.Name);
                }

                // Import categories
                foreach (var category in data.Categories)
                {
                    facade.CreateCategory(category.Name, category.IsExpenditure);
                }

                // Import operations
                foreach (var operation in data.Operations)
                {
                    facade.CreateOperation(
                        operation.IsExpenditure,
                        operation.BAId,
                        operation.Sum,
                        operation.Time,
                        operation.CategoryId,
                        operation.Description
                    );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error importing JSON file: {e.Message}");
            }
        }

        private class ImportData
        {
            public BA[] Accounts { get; set; }
            public Category[] Categories { get; set; }
            public Operation[] Operations { get; set; }
        }
    }
} 