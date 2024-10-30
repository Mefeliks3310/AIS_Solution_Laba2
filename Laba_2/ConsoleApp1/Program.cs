using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        /// <summary>
        /// Главная точка входа программы. Отображает меню и обрабатывает пользовательский ввод для управления данными студентов.
        /// </summary>
        static void Main(string[] args)
        {
            Logic logic = new Logic();
            string input = "";
            string menu = $"{new string(' ', Console.WindowWidth / 2)} Система <<DecanatPRO>>\n1 - Добавить студента | 2 - Удалить студента | 3 - Изменить студента | 4 - Вывести весь список студентов | 5 - Вывести гистограмму | 6 - Вывести по ID | 7 - Выход";
            Console.WriteLine(menu);
            input = Console.ReadLine();
            while (input != "7")
            {
                UI(input, logic);
                input = Console.ReadLine();
                Console.Clear();
                Console.WriteLine(menu);
            }
        }

        /// <summary>
        /// Обрабатывает ввод пользователя и выполняет соответствующие действия (добавление, удаление, обновление, вывод данных).
        /// </summary>
        /// <param name="s">Выбор действия пользователя из меню.</param>
        /// <param name="logic">Экземпляр класса Logic для выполнения операций над данными.</param>
        public static void UI(string s, Logic logic)
        {
            List<string> items;
            switch (s)
            {
                case "1":
                    items = GetItems();
                    Console.WriteLine(logic.AddStudent(items[0], items[1], items[2]));
                    break;
                case "2":
                    DataOutput(logic.GetSutednts());
                    Console.WriteLine(logic.DeleteObject(TakeId()));
                    break;
                case "3":
                    DataOutput(logic.GetSutednts());
                    int id = TakeId();
                    List<string> items_list = GetItems();
                    Console.WriteLine(logic.UpdateObject(items_list[0], items_list[1], items_list[2], id));
                    break;
                case "4":
                    DataOutput(logic.GetSutednts());
                    break;
                case "5":
                    foreach (var pair in logic.Gistogramma())
                    {
                        string count = new string('■', pair.Value);
                        Console.WriteLine($"{count} - {pair.Key}({pair.Value})");
                    }
                    break;
                case "6":
                    IEnumerable<object> student = logic.GetByID(TakeId());
                    foreach (var student_item in student)
                    {
                        Console.Write(student_item + " ");
                    }
                    Console.WriteLine();
                    break;
                case "7":
                    Console.WriteLine("Выход.");
                    break;
                default:
                    Console.WriteLine($"{s} - некорректный тип данных");
                    break;
            }
        }

        /// <summary>
        /// Выводит данные о студентах на консоль.
        /// </summary>
        /// <param name="objects">Список студентов, полученный из базы данных.</param>
        public static void DataOutput(IEnumerable<IEnumerable<object>> objects)
        {
            foreach (var student in objects)
            {
                var studentList = student.ToList();
                Console.WriteLine($"{student.ElementAt(0)}. {student.ElementAt(1)} {student.ElementAt(2)} {student.ElementAt(3)}");
            }
        }

        /// <summary>
        /// Получает от пользователя данные для создания нового объекта студента.
        /// </summary>
        /// <returns>Список строк с данными (ФИО, группа, специальность).</returns>
        public static List<string> GetItems()
        {
            Console.WriteLine("Введите ФИО студента:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите группу студента:");
            string group = Console.ReadLine();
            Console.WriteLine("Введите специальность студента:");
            string speciality = Console.ReadLine();
            List<string> items = new List<string> { name, group, speciality };
            return items;
        }

        /// <summary>
        /// Проверяет введенный пользователем ID студента и возвращает его после успешной проверки.
        /// </summary>
        /// <returns>ID студента.</returns>
        public static int TakeId()
        {
            int id = -1;
            Console.WriteLine("Введите ID студента.");
            while (id < 1)
            {
                try
                {
                    id = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка. ID должно содержать положительное число.");
                }
            }
            return id;
        }
    }
}