using System;
using System.IO;
using YamlDotNet.Serialization;
using BankApplication.Model;

namespace BankApplication.Imports
{
    public class YamlDataImporter : DataImporter
    {
        private readonly CommonFacade facade;

        public YamlDataImporter(CommonFacade facade)
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
                string yamlString = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder()
                    .Build();

                var data = deserializer.Deserialize<ImportData>(yamlString);

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
                Console.WriteLine($"Error importing YAML file: {e.Message}");
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