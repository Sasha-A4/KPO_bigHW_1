using System;
using System.Collections.Generic;

namespace BankApplication.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsExpenditure { get; set; }
        public List<Operation> Operations { get; set; }

        public Category()
        {
            Operations = new List<Operation>();
        }

        public void AddOperation(Operation operation)
        {
            Operations.Add(operation);
        }

        public void RemoveOperation(Operation operation)
        {
            Operations.Remove(operation);
        }

        public double GetTotalForPeriod(DateTime start, DateTime end)
        {
            double total = 0;
            foreach (var operation in Operations)
            {
                if (operation.Time >= start && operation.Time <= end)
                {
                    total += operation.IsExpenditure ? -operation.Sum : operation.Sum;
                }
            }
            return total;
        }

        public Dictionary<int, double> GetOperationsByAccountForPeriod(DateTime start, DateTime end)
        {
            var result = new Dictionary<int, double>();
            foreach (var operation in Operations)
            {
                if (operation.Time >= start && operation.Time <= end)
                {
                    if (!result.ContainsKey(operation.BAId))
                    {
                        result[operation.BAId] = 0;
                    }
                    result[operation.BAId] += operation.IsExpenditure ? -operation.Sum : operation.Sum;
                }
            }
            return result;
        }
    }
} 