using System;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using BankApplication.Model;

namespace BankApplication.Imports
{
    public class CSVDataImporter : DataImporter
    {
        private readonly CommonFacade facade;

        public CSVDataImporter(CommonFacade facade)
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
                var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ","
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    // Skip until we find the Accounts section
                    while (csv.Read() && csv.GetField(0) != "=== Accounts ===") { }
                    csv.Read(); // Skip header

                    // Read accounts
                    while (csv.Read() && csv.GetField(0) != "=== Categories ===")
                    {
                        var account = csv.GetRecord<BA>();
                        if (account != null)
                        {
                            facade.CreateBA(account.Name);
                        }
                    }

                    // Skip header
                    csv.Read();

                    // Read categories
                    while (csv.Read() && csv.GetField(0) != "=== Operations ===")
                    {
                        var category = csv.GetRecord<Category>();
                        if (category != null)
                        {
                            facade.CreateCategory(category.Name, category.IsExpenditure);
                        }
                    }

                    // Skip header
                    csv.Read();

                    // Read operations
                    while (csv.Read())
                    {
                        var operation = csv.GetRecord<Operation>();
                        if (operation != null)
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error importing CSV file: {e.Message}");
            }
        }
    }
} 