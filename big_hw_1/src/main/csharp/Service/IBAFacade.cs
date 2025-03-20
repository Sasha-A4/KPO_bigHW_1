using System;
using System.Collections.Generic;
using BankApplication.Model;

namespace BankApplication.Service
{
    public interface IBAFacade
    {
        void CreateBA(string name);
        void DeleteBA(int id);
        void GetBAInfo(int id);
        BA GetBA(int id);
        void AddToBalance(int id, double diff);
        void ChangeBAName(int id, string newName);
        List<BA> GetAccounts();
        double CalculateAccountProfit(int accountId, DateTime start, DateTime end);
        Dictionary<int, double> GetOperationsByCategoryForPeriod(int accountId, DateTime start, DateTime end);
        double GetBalance(int accountId);
        void UpdateBalance(int accountId, double newBalance);
    }
} 