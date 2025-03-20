using System;
using System.Collections.Generic;
using BankApplication.Model;

namespace BankApplication.Service
{
    public interface IOperationFacade
    {
        void CreateOperation(bool isExpenditure, int baId, double sum, DateTime time, int categoryId, string description);
        void CreateOperation(bool isExpenditure, int baId, double sum, DateTime time, int categoryId);
        void DeleteOperation(int id);
        void DeleteOperation(int baId, DateTime time);
        void GetOperationInfo(int id);
        Operation GetOperation(int id);
        void ChangeDescription(int id, string newDescription);
        void ChangeDescription(int baId, DateTime time, string newDescription);
        List<Operation> GetOperations();
        double CalculateNetProfit(DateTime start, DateTime end);
        double CalculateNetIncome(DateTime start, DateTime end);
        double CalculateNetExpenditures(DateTime start, DateTime end);
        Dictionary<int, double> GroupOperationsByCategory(DateTime start, DateTime end);
        List<Operation> GetOperationsForPeriod(DateTime start, DateTime end);
        List<Operation> GetOperationsForAccount(int accountId);
        List<Operation> GetOperationsForCategory(int categoryId);
    }
} 