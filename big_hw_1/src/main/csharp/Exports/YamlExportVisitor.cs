using System;
using System.IO;
using YamlDotNet.Serialization;
using BankApplication.Model;

namespace BankApplication.Exports
{
    public class YamlExportVisitor : ExportVisitor
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

            var serializer = new SerializerBuilder()
                .Build();

            string yamlString = serializer.Serialize(data);
            File.WriteAllText(filePath, yamlString);
        }
    }
} 