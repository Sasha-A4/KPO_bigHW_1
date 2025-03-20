using System;
using BankApplication.Model;

namespace BankApplication.Command
{
    public class CreateOperationCommand : Command
    {
        private readonly CommonFacade facade;
        private readonly bool isExpenditure;
        private readonly int baId;
        private readonly double sum;
        private readonly DateTime time;
        private readonly int categoryId;
        private readonly string description;

        public CreateOperationCommand(
            CommonFacade facade,
            bool isExpenditure,
            int baId,
            double sum,
            DateTime time,
            int categoryId,
            string description)
        {
            this.facade = facade;
            this.isExpenditure = isExpenditure;
            this.baId = baId;
            this.sum = sum;
            this.time = time;
            this.categoryId = categoryId;
            this.description = description;
        }

        public void Execute()
        {
            facade.CreateOperation(isExpenditure, baId, sum, time, categoryId, description);
        }
    }
} 