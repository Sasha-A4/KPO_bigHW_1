using System;
using System.Collections.Generic;
using System.Linq;

namespace BankApplication.Model
{
    public class CommonFacade
    {
        private readonly List<BA> accounts;
        private readonly List<Category> categories;
        private readonly List<Operation> operations;
        private int nextAccountId;
        private int nextCategoryId;
        private int nextOperationId;

        private static readonly string[] MenuItems = new[]
         {
             "==== Меню банка ====",
             "1. Список счетов",
             "2. Создать счет",
             "3. Изменить имя счета",
             "4. Удалить счет",
             "5. Список категорий",
             "6. Создать категорию",
             "7. Изменить имя категории",
             "8. Удалить категорию",
             "9. Список операций",
             "10. Создать операцию",
             "11. Изменить описание операции",
             "12. Удалить операцию",
             "13. Аналитика: Чистая прибыль за период",
             "14. Аналитика: Общий доход за период",
             "15. Аналитика: Общие расходы за период",
             "16. Аналитика: Прибыль по счёту",
             "17. Аналитика: Группировка всех операций по категориям",
             "18. Аналитика: Группировка операций по категориям для счёта",
             "0 или 'exit' - Выход",
             "Выберите опцию: "
         };

        public string[] GetMenuItems() => MenuItems;


        public CommonFacade()
        {
            accounts = new List<BA>();
            categories = new List<Category>();
            operations = new List<Operation>();
            nextAccountId = 1;
            nextCategoryId = 1;
            nextOperationId = 1;
        }

        // Account operations
        public List<BA> GetAccounts() => accounts;

        public BA CreateBA(string name)
        {
            var account = new BA
            {
                Id = nextAccountId++,
                Name = name,
                Balance = 0
            };
            accounts.Add(account);
            return account;
        }

        public void ChangeBAName(int id, string newName)
        {
            var account = accounts.FirstOrDefault(a => a.Id == id);
            if (account != null)
            {
                account.Name = newName;
            }
        }

        public void DeleteBA(int id)
        {
            var account = accounts.FirstOrDefault(a => a.Id == id);
            if (account != null)
            {
                // Remove all operations associated with this account
                var accountOperations = operations.Where(o => o.BAId == id).ToList();
                foreach (var operation in accountOperations)
                {
                    DeleteOperation(operation.Id);
                }
                accounts.Remove(account);
            }
        }

        // Category operations
        public List<Category> GetCategories() => categories;

        public Category CreateCategory(string name, bool isExpenditure)
        {
            var category = new Category
            {
                Id = nextCategoryId++,
                Name = name,
                IsExpenditure = isExpenditure
            };
            categories.Add(category);
            return category;
        }

        public void ChangeCategoryName(int id, string newName)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                category.Name = newName;
            }
        }

        public void DeleteCategory(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                // Remove all operations associated with this category
                var categoryOperations = operations.Where(o => o.CategoryId == id).ToList();
                foreach (var operation in categoryOperations)
                {
                    DeleteOperation(operation.Id);
                }
                categories.Remove(category);
            }
        }

        // Operation operations
        public List<Operation> GetOperations() => operations;

        public Operation CreateOperation(bool isExpenditure, int baId, double sum, DateTime time, int categoryId, string description)
        {
            var operation = new Operation
            {
                Id = nextOperationId++,
                IsExpenditure = isExpenditure,
                BAId = baId,
                Sum = sum,
                Time = time,
                CategoryId = categoryId,
                Description = description
            };

            operations.Add(operation);

            // Update account and category
            var account = accounts.FirstOrDefault(a => a.Id == baId);
            if (account != null)
            {
                account.AddOperation(operation);
            }

            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                category.AddOperation(operation);
            }

            return operation;
        }

        public void ChangeDescription(int id, string newDescription)
        {
            var operation = operations.FirstOrDefault(o => o.Id == id);
            if (operation != null)
            {
                operation.Description = newDescription;
            }
        }

        public void DeleteOperation(int id)
        {
            var operation = operations.FirstOrDefault(o => o.Id == id);
            if (operation != null)
            {
                // Update account and category
                var account = accounts.FirstOrDefault(a => a.Id == operation.BAId);
                if (account != null)
                {
                    account.RemoveOperation(operation);
                }

                var category = categories.FirstOrDefault(c => c.Id == operation.CategoryId);
                if (category != null)
                {
                    category.RemoveOperation(operation);
                }

                operations.Remove(operation);
            }
        }

        // Analytics operations
        public double CalculateNetProfit(DateTime start, DateTime end)
        {
            return CalculateNetIncome(start, end) - CalculateNetExpenditures(start, end);
        }

        public double CalculateNetIncome(DateTime start, DateTime end)
        {
            return operations
                .Where(o => !o.IsExpenditure && o.Time >= start && o.Time <= end)
                .Sum(o => o.Sum);
        }

        public double CalculateNetExpenditures(DateTime start, DateTime end)
        {
            return operations
                .Where(o => o.IsExpenditure && o.Time >= start && o.Time <= end)
                .Sum(o => o.Sum);
        }

        public double CalculateAccountProfit(int accountId, DateTime start, DateTime end)
        {
            var account = accounts.FirstOrDefault(a => a.Id == accountId);
            return account?.GetBalanceForPeriod(start, end) ?? 0;
        }

        public Dictionary<int, double> GroupOperationsByCategory(DateTime start, DateTime end)
        {
            var result = new Dictionary<int, double>();
            foreach (var category in categories)
            {
                result[category.Id] = category.GetTotalForPeriod(start, end);
            }
            return result;
        }

        public Dictionary<int, double> GroupOpsByCatForAccount(int accountId, DateTime start, DateTime end)
        {
            var account = accounts.FirstOrDefault(a => a.Id == accountId);
            return account?.GetOperationsByCategoryForPeriod(start, end) ?? new Dictionary<int, double>();
        }

        // Export operations
        public void ExportData(ExportVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
} 