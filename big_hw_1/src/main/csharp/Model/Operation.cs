using System;

namespace BankApplication.Model
{
    public class Operation
    {
        public int Id { get; set; }
        public bool IsExpenditure { get; set; }
        public int BAId { get; set; }
        public double Sum { get; set; }
        public DateTime Time { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }

        public Operation()
        {
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Operation[Id={Id}, Type={(IsExpenditure ? "Expenditure" : "Income")}, " +
                   $"AccountId={BAId}, Sum={Sum}, Time={Time}, CategoryId={CategoryId}, " +
                   $"Description={Description}]";
        }
    }
} 