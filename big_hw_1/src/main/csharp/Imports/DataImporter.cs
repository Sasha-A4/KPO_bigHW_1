using BankApplication.Model;

namespace BankApplication.Imports
{
    public interface DataImporter
    {
        void ImportFile(string filePath);
    }
} 