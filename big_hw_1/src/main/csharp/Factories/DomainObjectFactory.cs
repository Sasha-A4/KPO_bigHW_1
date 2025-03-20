using System;
using BankApplication.Model;

namespace BankApplication.Factories
{
    public class DomainObjectFactory
    {
        private int nextAccountId = 1;
        private int nextCategoryId = 1;
        private int nextOperationId = 1;

        public static BA CreateBA(string name, double balance)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя счета не может быть пустым");
            }
            return new BA(balance, name);
        }

        public static Category CreateCategory(string name, bool isExpenditure)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя категории не может быть пустыми");
            }
            return new Category(name, isExpenditure);
        }

        public static Operation CreateOperation(bool isExpenditure, int baId, double sum, DateTime time, int categoryId, string description)
        {
            if (sum < 0)
            {
                throw new ArgumentException("Сумма операции не может быть отрицательной!");
            }
            if (sum == 0)
            {
                throw new ArgumentException("Сумма операции не может быть нулевой!");
            }
            if (time == default)
            {
                throw new ArgumentException("Время проведения операции не может быть null!");
            }
            if (time > DateTime.Now)
            {
                throw new ArgumentException("Время операции не может быть в будущем!");
            }
            if (baId <= 0)
            {
                throw new ArgumentException("Идентификатор счета должен быть положительным!");
            }
            if (categoryId <= 0)
            {
                throw new ArgumentException("Идентификатор категории должен быть положительным!");
            }
            return new Operation(null, isExpenditure, baId, sum, time, categoryId, description);
        }

        public void ResetIds()
        {
            nextAccountId = 1;
            nextCategoryId = 1;
            nextOperationId = 1;
        }
    }
} 