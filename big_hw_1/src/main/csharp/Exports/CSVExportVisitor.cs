using System;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using BankApplication.Model;

namespace BankApplication.Exports
{
    public class CSVExportVisitor : ExportVisitor
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
            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, config))
            {
                // Write accounts
                csv.WriteField("=== Accounts ===");
                csv.NextRecord();
                csv.WriteHeader<BA>();
                csv.NextRecord();
                foreach (var account in facade.GetAccounts())
                {
                    csv.WriteRecord(account);
                    csv.NextRecord();
                }

                // Write categories
                csv.WriteField("=== Categories ===");
                csv.NextRecord();
                csv.WriteHeader<Category>();
                csv.NextRecord();
                foreach (var category in facade.GetCategories())
                {
                    csv.WriteRecord(category);
                    csv.NextRecord();
                }

                // Write operations
                csv.WriteField("=== Operations ===");
                csv.NextRecord();
                csv.WriteHeader<Operation>();
                csv.NextRecord();
                foreach (var operation in facade.GetOperations())
                {
                    csv.WriteRecord(operation);
                    csv.NextRecord();
                }
            }
        }
    }
} 