using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework7
{
    class Program
    {
        static void Main()
        {
            //создаем файл, если не существует
            string fileName = @".\workers.txt";
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Создаем файл именем по умолчанию: workers.txt");
                var fs = File.Create(fileName);
                fs.Close();
            }
            var rep  = new Repository(fileName);
            
            string msgFirstScreen = "Добро пожаловать в справочник сотрудников. Что вы хотите сделать?\n" +
                "1 -      Добавить запись для нового сотрудника;\n" +
                "2 -      Удалить запись;\n" +
                "3 -      Вывести сотрудников в различных диапазонах;\n" +
                "Enter -  Вывести всех сотрудников и сортировать по разным полям;\n" +
                "Escape - Выйти.";
            string msgWorkersScreen = "Текущая сортировка: {0}. Чтобы отсортировать, нажмите соответствующую клавишу:\n" +
                "1 - Сортировать по ID\n" +
                "2 - Сортировать по дате добавления\n" +
                "3 - Сортировать по ФИО\n" +
                "4 - Сортировать по возрасту\n" +
                "5 - Сортировать по росту\n" +
                "6 - Сортировать по дате рождения\n" +
                "7 - Сортировать по месту рождения\n" +
                "Escape - выход в предыдущее меню";
            string msgFilteredWorkersScreen = "Чтобы отфильтровать, нажмите соответствующую клавишу:\n" +
                "1 - Фильтровать по ID\n" +
                "2 - Фильтровать по дате добавления\n" +
//                "3 - Фильтровать по ФИО\n" +
                "4 - Фильтровать по возрасту\n" +
                "5 - Фильтровать по росту\n" +
                "6 - Фильтровать по дате рождения\n" +
//                "7 - Фильтровать по месту рождения\n" +
                "Escape - выход в предыдущее меню";
            while (true)
            {
                Console.Clear();
                Console.WriteLine(msgFirstScreen);
                //флаг для выхода из цикла
                bool escPushed = false;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        //целое число, соответствующее нажатой клавише в меню, соотетствует тексту из msgWorkersScrenn
                        int sortField = 1;
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(rep.Print(sortField));
                            Console.WriteLine(msgWorkersScreen, sortField);
                            sortField = GetWorkersMenuKey(out escPushed);
                            if (escPushed)
                            {
                                escPushed = false;
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        escPushed = true;
                        break;
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine(rep.Print());
                        Console.WriteLine("\nДобавляем запись...");
                        rep.AddWorker();
                        Console.Clear();
                        Console.WriteLine(rep.Print());
                        Console.WriteLine("\nСотрудник успешно добавлен! Сохранить файл? (Да - Enter(Y), Нет - Escape(N))");
                        if (GetYesOrNoKey()) rep.SaveRepository(true);
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        Console.WriteLine(rep.Print());
                        Console.WriteLine("\nКого удалить? Введите ID сотрудника: ");
                        int id = GetIntegerFromConsole();
                        if(!rep.DeleteWorker(id))
                        {
                            Console.WriteLine($"Сотрудник c ID={id} не найден! Нажмите любую клавишу.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(rep.Print());
                            Console.WriteLine("Сотрудник удален. Сохранить файл? (Да - Enter(Y), Нет - Escape(N))");
                            if (GetYesOrNoKey()) rep.SaveRepository();
                        }
                        break;
                    case ConsoleKey.D3:
                        int? filterField = null;
                        object fldFrom = null, fldTo = null;
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(rep.Print(1, rep.FilterWorkers(filterField, fldFrom, fldTo)));
                            Console.WriteLine(msgFilteredWorkersScreen);
                            filterField = GetWorkersMenuKey(out escPushed);
                            switch (filterField)
                            {
                                case 1:
                                case 4:
                                case 5:
                                    Console.WriteLine("\nВведите начальное значение: ");
                                    fldFrom = GetIntegerFromConsole();
                                    Console.WriteLine("\nВведите конечное значение: ");
                                    fldTo = GetIntegerFromConsole();
                                    break;
                                case 2:
                                case 6:
                                    Console.WriteLine("\nВведите начальное значение: ");
                                    fldFrom = GetDateTimeFromConsole();
                                    Console.WriteLine("\nВведите конечное значение: ");
                                    fldTo = GetDateTimeFromConsole();
                                    break;
                                default:
                                    break;
                            }
                            if (escPushed)
                            {
                                escPushed = false;
                                break;
                            }
                        }
                        break;
                    default:
                        continue;
                }
                if (escPushed)
                {
                    Console.WriteLine("Сохранить файл? (Да - Enter(Y), Нет - Escape(N))");
                    if (GetYesOrNoKey()) rep.SaveRepository();
                    break;
                }
            }
        }
        /// <summary>
        /// Ожидает нажатия клавиши и преобразует коды клавиш 1-7 в тип int
        /// </summary>
        /// <param name="escPushed">Для назначения флага выхода из цикла</param>
        /// <returns>По умолчанию возвращает 1</returns>
        static int GetWorkersMenuKey(out bool escPushed)
        {
            int field = 0;
            escPushed = false;
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                    escPushed = true;
                    break;
                case ConsoleKey.D1:
                    field = 1;
                    break;
                case ConsoleKey.D2:
                    field = 2;
                    break;
                case ConsoleKey.D3:
                    field = 3;
                    break;
                case ConsoleKey.D4:
                    field = 4;
                    break;
                case ConsoleKey.D5:
                    field = 5;
                    break;
                case ConsoleKey.D6:
                    field = 6;
                    break;
                case ConsoleKey.D7:
                    field = 7;
                    break;
                default:
                    field = 1;
                    break;
            }
            return field;
        }
        static bool GetYesOrNoKey()
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                    return false;
                case ConsoleKey.N:
                    return false;
                case ConsoleKey.Enter:
                    return true;
                case ConsoleKey.Y:
                    return true;
                default:
                    return true;
            }
        }
        public static int GetIntegerFromConsole()
        {
            int consoleInt;
            while (!int.TryParse(Console.ReadLine(), out consoleInt) && consoleInt >= 0)
                Console.WriteLine("Некорректный ввод, введите целое неотрицательное число!");
            return consoleInt;
        }
        public static DateTime GetDateTimeFromConsole()
        {
            DateTime consoleDate;
            while (!DateTime.TryParse(Console.ReadLine(), out consoleDate))
                Console.WriteLine("Некорректный ввод даты!");
            return consoleDate;
        }
    }
}
