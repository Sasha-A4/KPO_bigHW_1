using System;
using System.Collections.Generic;

namespace BankApplication.Model
{
    public class BA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public List<Operation> Operations { get; set; }

        public BA()
        {
            Operations = new List<Operation>();
        }

        public void AddOperation(Operation operation)
        {
            Operations.Add(operation);
            UpdateBalance(operation);
        }

        public void RemoveOperation(Operation operation)
        {
            Operations.Remove(operation);
            // Reverse the balance change
            UpdateBalance(new Operation
            {
                IsExpenditure = operation.IsExpenditure,
                Sum = -operation.Sum,
                Time = operation.Time,
                CategoryId = operation.CategoryId,
                Description = $"Reversal of: {operation.Description}"
            });
        }

        private void UpdateBalance(Operation operation)
        {
            if (operation.IsExpenditure)
            {
                Balance -= operation.Sum;
            }
            else
            {
                Balance += operation.Sum;
            }
        }

        public double GetBalanceForPeriod(DateTime start, DateTime end)
        {
            double periodBalance = 0;
            foreach (var operation in Operations)
            {
                if (operation.Time >= start && operation.Time <= end)
                {
                    if (operation.IsExpenditure)
                    {
                        periodBalance -= operation.Sum;
                    }
                    else
                    {
                        periodBalance += operation.Sum;
                    }
                }
            }
            return periodBalance;
        }

        public Dictionary<int, double> GetOperationsByCategoryForPeriod(DateTime start, DateTime end)
        {
            var result = new Dictionary<int, double>();
            foreach (var operation in Operations)
            {
                if (operation.Time >= start && operation.Time <= end)
                {
                    if (!result.ContainsKey(operation.CategoryId))
                    {
                        result[operation.CategoryId] = 0;
                    }
                    result[operation.CategoryId] += operation.IsExpenditure ? -operation.Sum : operation.Sum;
                }
            }
            return result;
        }
    }
} 