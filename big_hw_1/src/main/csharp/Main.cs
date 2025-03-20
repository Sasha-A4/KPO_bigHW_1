using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using YamlDotNet.Serialization;
using CsvHelper;
using CsvHelper.Configuration;

namespace BankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Приложение банка запущено ===");

            var di = new DI();
            var facade = new CommonFacade();
            di.RegisterSingleton(typeof(CommonFacade), facade);

            bool validChoice = false;
            string choice = "";
            while (!validChoice)
            {
                Console.WriteLine("Выберите формат файла для импорта данных:");
                Console.WriteLine("1 - JSON");
                Console.WriteLine("2 - CSV");
                Console.WriteLine("3 - YAML");
                Console.Write("Введите номер формата: ");
                choice = Console.ReadLine()?.Trim();
                if (choice == "1" || choice == "2" || choice == "3")
                {
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте ещё раз.");
                }
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Импорт данных из JSON...");
                    var jsonImporter = new JsonDataImporter(facade);
                    jsonImporter.ImportFile("data.json");
                    break;
                case "2":
                    Console.WriteLine("Импорт данных из CSV...");
                    var csvImporter = new CSVDataImporter(facade);
                    csvImporter.ImportFile("data.csv");
                    break;
                case "3":
                    Console.WriteLine("Импорт данных из YAML...");
                    var yamlImporter = new YamlDataImporter(facade);
                    yamlImporter.ImportFile("data.yaml");
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Импорт не выполнен.");
                    break;
            }

            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                string option = Console.ReadLine()?.Trim().ToLower();
                try
                {
                    switch (option)
                    {
                        case "1":
                            ListAccounts(facade);
                            break;
                        case "2":
                            CreateAccount(facade);
                            break;
                        case "3":
                            UpdateAccountName(facade);
                            break;
                        case "4":
                            DeleteAccount(facade);
                            break;
                        case "5":
                            ListCategories(facade);
                            break;
                        case "6":
                            CreateCategory(facade);
                            break;
                        case "7":
                            UpdateCategoryName(facade);
                            break;
                        case "8":
                            DeleteCategory(facade);
                            break;
                        case "9":
                            ListOperations(facade);
                            break;
                        case "10":
                            CreateOperation(facade);
                            break;
                        case "11":
                            UpdateOperationDescription(facade);
                            break;
                        case "12":
                            DeleteOperation(facade);
                            break;
                        case "13":
                            CalculateNetProfit(facade);
                            break;
                        case "14":
                            CalculateTotalIncome(facade);
                            break;
                        case "15":
                            CalculateTotalExpenditure(facade);
                            break;
                        case "16":
                            CalculateAccountProfit(facade);
                            break;
                        case "17":
                            GroupOperationsByCategory(facade);
                            break;
                        case "18":
                            GroupOpsByCategoryForAccount(facade);
                            break;
                        case "exit":
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверная команда. Повторите попытку.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                }
                Console.WriteLine(); // пустая строка для разделения вывода
            }

            // Экспорт данных
            Console.WriteLine("Экспорт данных в JSON, CSV и YAML...");

            var jsonExportVisitor = new JsonExportVisitor();
            facade.ExportData(jsonExportVisitor);
            jsonExportVisitor.ExportToFile("data.json");

            var csvExportVisitor = new CSVExportVisitor();
            facade.ExportData(csvExportVisitor);
            csvExportVisitor.ExportToFile("data.csv");

            var yamlExportVisitor = new YamlExportVisitor();
            facade.ExportData(yamlExportVisitor);
            yamlExportVisitor.ExportToFile("data.yaml");

            Console.WriteLine("Выход из приложения.");
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите целое число.");
            }
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите число (с плавающей точкой, если нужно).");
            }
        }

        private static DateTime ReadDateTime(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите дату и время в формате yyyy-MM-dd HH:mm.");
            }
        }

        private static bool ReadBoolean(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "true" || input == "false")
                {
                    return bool.Parse(input);
                }
                Console.WriteLine("Ошибка: введите true или false.");
            }
        }

        private static void PrintMenu()
        {
            foreach (var menuItem in facade.GetMenuItems())
            {
                if (menuItem == "Выберите опцию: ")
                {
                    Console.Write(menuItem);
                }
                else
                {
                    Console.WriteLine(menuItem);
                }
            }
        }

        // CRUD для счетов
        private static void ListAccounts(CommonFacade facade)
        {
            Console.WriteLine("==== Счета ====");
            foreach (var account in facade.GetAccounts())
            {
                Console.WriteLine($"ID: {account.Id}, Имя: {account.Name}, Баланс: {account.Balance}");
            }
        }

        private static void CreateAccount(CommonFacade facade)
        {
            Console.Write("Введите имя нового счета: ");
            string name = Console.ReadLine();
            facade.CreateBA(name);
            Console.WriteLine("Счет создан.");
        }

        private static void UpdateAccountName(CommonFacade facade)
        {
            int id = ReadInt("Введите ID счета для изменения: ");
            Console.Write("Введите новое имя счета: ");
            string newName = Console.ReadLine();
            facade.ChangeBAName(id, newName);
        }

        private static void DeleteAccount(CommonFacade facade)
        {
            int id = ReadInt("Введите ID счета для удаления: ");
            facade.DeleteBA(id);
        }

        // CRUD для категорий
        private static void ListCategories(CommonFacade facade)
        {
            Console.WriteLine("==== Категории ====");
            foreach (var cat in facade.GetCategories())
            {
                Console.WriteLine($"ID: {cat.Id}, Имя: {cat.Name}, Тип: {(cat.IsExpenditure ? "Расход" : "Доход")}");
            }
        }

        private static void CreateCategory(CommonFacade facade)
        {
            Console.Write("Введите имя новой категории: ");
            string name = Console.ReadLine();
            bool isExpense = ReadBoolean("Введите тип (true для расхода, false для дохода): ");
            facade.CreateCategory(name, isExpense);
            Console.WriteLine("Категория создана.");
        }

        private static void UpdateCategoryName(CommonFacade facade)
        {
            int id = ReadInt("Введите ID категории для изменения: ");
            Console.Write("Введите новое имя категории: ");
            string newName = Console.ReadLine();
            facade.ChangeCategoryName(id, newName);
        }

        private static void DeleteCategory(CommonFacade facade)
        {
            int id = ReadInt("Введите ID категории для удаления: ");
            facade.DeleteCategory(id);
        }

        // CRUD для операций
        private static void ListOperations(CommonFacade facade)
        {
            Console.WriteLine("==== Операции ====");
            foreach (var op in facade.GetOperations())
            {
                Console.WriteLine($"ID: {op.Id}, Тип: {(op.IsExpenditure ? "Расход" : "Доход")}, " +
                                $"Счет ID: {op.BAId}, Сумма: {op.Sum}, Дата: {op.Time}, " +
                                $"Категория ID: {op.CategoryId}, Описание: {op.Description}");
            }
        }

        private static void CreateOperation(CommonFacade facade)
        {
            bool isExpense = ReadBoolean("Введите тип операции (true для расхода, false для дохода): ");
            int accountId = ReadInt("Введите ID счета: ");
            double sum = ReadDouble("Введите сумму: ");
            int catId = ReadInt("Введите ID категории: ");
            Console.Write("Введите описание операции: ");
            string descr = Console.ReadLine();
            facade.CreateOperation(isExpense, accountId, sum, DateTime.Now, catId, descr);
        }

        private static void UpdateOperationDescription(CommonFacade facade)
        {
            int id = ReadInt("Введите ID операции для изменения: ");
            Console.Write("Введите новое описание: ");
            string desc = Console.ReadLine();
            facade.ChangeDescription(id, desc);
        }

        private static void DeleteOperation(CommonFacade facade)
        {
            int id = ReadInt("Введите ID операции для удаления: ");
            facade.DeleteOperation(id);
        }

        // Аналитика
        private static void CalculateNetProfit(CommonFacade facade)
        {
            DateTime start = ReadDateTime("Введите начальную дату (yyyy-MM-dd HH:mm): ");
            DateTime end = ReadDateTime("Введите конечную дату (yyyy-MM-dd HH:mm): ");
            double profit = facade.CalculateNetProfit(start, end);
            Console.WriteLine($"Чистая прибыль за период: {profit}");
        }

        private static void CalculateTotalIncome(CommonFacade facade)
        {
            DateTime start = ReadDateTime("Введите начальную дату (yyyy-MM-dd HH:mm): ");
            DateTime end = ReadDateTime("Введите конечную дату (yyyy-MM-dd HH:mm): ");
            double income = facade.CalculateNetIncome(start, end);
            Console.WriteLine($"Общий доход за период: {income}");
        }

        private static void CalculateTotalExpenditure(CommonFacade facade)
        {
            DateTime start = ReadDateTime("Введите начальную дату (yyyy-MM-dd HH:mm): ");
            DateTime end = ReadDateTime("Введите конечную дату (yyyy-MM-dd HH:mm): ");
            double expenditure = facade.CalculateNetExpenditures(start, end);
            Console.WriteLine($"Общие расходы за период: {expenditure}");
        }

        private static void CalculateAccountProfit(CommonFacade facade)
        {
            int id = ReadInt("Введите ID счета: ");
            DateTime start = ReadDateTime("Введите начальную дату (yyyy-MM-dd HH:mm): ");
            DateTime end = ReadDateTime("Введите конечную дату (yyyy-MM-dd HH:mm): ");
            double profit = facade.CalculateAccountProfit(id, start, end);
            Console.WriteLine($"Прибыль для счета ID {id} за указанный период: {profit}");
        }

        private static void GroupOperationsByCategory(CommonFacade facade)
        {
            DateTime start = ReadDateTime("Введите начальную дату (yyyy-MM-dd HH:mm): ");
            DateTime end = ReadDateTime("Введите конечную дату (yyyy-MM-dd HH:mm): ");
            var groups = facade.GroupOperationsByCategory(start, end);
            Console.WriteLine("Суммы по категориям:");
            foreach (var group in groups)
            {
                Console.WriteLine($"Категория ID {group.Key} => {group.Value}");
            }
        }

        private static void GroupOpsByCategoryForAccount(CommonFacade facade)
        {
            int id = ReadInt("Введите ID счета: ");
            DateTime start = ReadDateTime("Введите начальную дату (yyyy-MM-dd HH:mm): ");
            DateTime end = ReadDateTime("Введите конечную дату (yyyy-MM-dd HH:mm): ");
            var groups = facade.GroupOpsByCatForAccount(id, start, end);
            Console.WriteLine($"Для счета ID {id}, суммы по категориям:");
            foreach (var group in groups)
            {
                Console.WriteLine($"Категория ID {group.Key} => {group.Value}");
            }
        }
    }
} 