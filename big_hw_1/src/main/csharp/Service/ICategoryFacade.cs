using System;
using System.Collections.Generic;
using BankApplication.Model;

namespace BankApplication.Service
{
    public interface ICategoryFacade
    {
        void CreateCategory(string name, bool isExpenditure);
        void DeleteCategory(int id);
        void DeleteCategory(string name);
        void GetCategoryInfo(int id);
        Category GetCategory(int id);
        void ChangeCategoryName(int id, string newName);
        List<Category> GetCategories();
        double GetTotalForPeriod(int categoryId, DateTime start, DateTime end);
        Dictionary<int, double> GetOperationsByAccountForPeriod(int categoryId, DateTime start, DateTime end);
        bool IsExpenditure(int categoryId);
    }
} 