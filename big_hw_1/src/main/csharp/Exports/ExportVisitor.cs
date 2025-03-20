using BankApplication.Model;

namespace BankApplication.Exports
{
    public interface ExportVisitor
    {
        void Visit(CommonFacade facade);
        void ExportToFile(string filePath);
    }
} 