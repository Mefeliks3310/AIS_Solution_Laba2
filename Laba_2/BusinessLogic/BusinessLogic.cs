using Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;

namespace BusinessLogic
{
    /// <summary>
    /// Класс Logic реализует бизнес-логику для работы с данными студентов, включая операции добавления, удаления, обновления и получения данных.
    /// </summary>
    public class Logic
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Конструктор класса Logic, инициализирующий подключение к базе данных и выбор репозитория.
        /// </summary>
        public Logic()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            var dbContext = CreateDbContext(connectionString);
            _repository = new DapperRepository(configuration);
        }

        /// <summary>
        /// Создает и настраивает объект StudentDbContext с использованием строки подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        /// <returns>Экземпляр StudentDbContext.</returns>
        private StudentDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new StudentDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Добавляет нового студента в базу данных после проверки данных на корректность и уникальность.
        /// </summary>
        /// <param name="name">Имя студента.</param>
        /// <param name="group">Группа студента.</param>
        /// <param name="speciality">Специальность студента.</param>
        /// <returns>Результат операции в виде строки, содержащей сообщение об успехе или ошибке.</returns>
        public string AddStudent(string name, string group, string speciality)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(group) || string.IsNullOrWhiteSpace(speciality))
            {
                return "Ошибка. Некоторые поля остались пустыми.";
            }
            if (!name.All(char.IsLetter) || !speciality.All(char.IsLetter))
            {
                return "Ошибка. Неверный тип данных для имени или специализации.";
            }

            var existingStudents = _repository.GetAll();
            if (existingStudents.Any(s => s.Name == name && s.Group == group && s.Speciality == speciality))
            {
                return "Студент с такими же параметрами уже существует.";
            }

            return $"Студент номер {_repository.Add(new Student { Name = name, Group = group, Speciality = speciality })} успешно добавлен!";
        }

        /// <summary>
        /// Удаляет студента из базы данных по ID.
        /// </summary>
        /// <param name="id">ID студента для удаления.</param>
        /// <returns>Сообщение об успешном удалении или ошибке, если студент не найден.</returns>
        public string DeleteObject(int id)
        {
            foreach (Student student in _repository.GetAll())
            {
                if (id == student.Id)
                {
                    _repository.Delete(id);
                    return "Студент удален.";
                }
            }
            return "Студент с таким ID не был найден";
        }

        /// <summary>
        /// Обновляет информацию о студенте в базе данных.
        /// </summary>
        /// <param name="name">Новое имя студента.</param>
        /// <param name="group">Новая группа студента.</param>
        /// <param name="speciality">Новая специальность студента.</param>
        /// <param name="id">ID студента для обновления.</param>
        /// <returns>Результат операции в виде строки с сообщением об успешном обновлении или ошибке.</returns>
        public string UpdateObject(string name, string group, string speciality, int id)
        {
            if (int.TryParse(name, out int result_name) || int.TryParse(speciality, out int result_spec))
            {
                return "Ошибка. Неверный тип данных для имени или специализации.";
            }
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(group) || string.IsNullOrWhiteSpace(speciality))
            {
                return "Ошибка. Некоторые поля остались пустыми.";
            }

            bool idExists = false;
            foreach (Student student in _repository.GetAll())
            {
                if (student.Name == name && student.Group == group && student.Speciality == speciality)
                {
                    return "Студент с такими же параметрами уже есть в базе.";
                }
                if (student.Id == id)
                {
                    idExists = true;
                }
            }

            if (idExists)
            {
                var updateStudent = new Student { Id = id, Name = name, Group = group, Speciality = speciality };
                _repository.Update(updateStudent);
                return "Данные о студенте изменены.";
            }
            return "Такого ID не существует среди всех студентов.";
        }

        /// <summary>
        /// Возвращает гистограмму количества студентов по каждой специальности.
        /// </summary>
        /// <returns>Словарь, где ключ — название специальности, значение — количество студентов на этой специальности.</returns>
        public Dictionary<string, int> Gistogramma()
        {
            Dictionary<string, int> dataSpeciality = new Dictionary<string, int>();
            List<string> listSpeciality = new List<string>();

            foreach (Student student in _repository.GetAll())
            {
                listSpeciality.Add(student.Speciality);
            }

            var groupedSpec = listSpeciality.GroupBy(n => n).Select(group => new { Speciality = group.Key, Count = group.Count() });
            foreach (var group in groupedSpec)
            {
                dataSpeciality[group.Speciality] = group.Count;
            }

            return dataSpeciality;
        }

        /// <summary>
        /// Получает информацию о студенте по его ID.
        /// </summary>
        /// <param name="id">ID студента.</param>
        /// <returns>Перечисление объектов, представляющих поля студента (ID, имя, группа, специальность).</returns>
        public IEnumerable<object> GetByID(int id)
        {
            var error = "Ошибка при получении студента: Введите существующий ID";
            Student student;

            try
            {
                student = _repository.GetByID(id);
            }
            catch (Exception)
            {
                student = null;
            }

            if (student != null)
            {
                yield return student.Id + ".";
                yield return student.Name;
                yield return student.Group;
                yield return student.Speciality;
            }
            else
            {
                yield return error;
            }
        }

        /// <summary>
        /// Возвращает список всех студентов.
        /// </summary>
        /// <returns>Перечисление объектов, представляющих студентов (ID, имя, группа, специальность).</returns>
        public IEnumerable<IEnumerable<object>> GetSutednts()
        {
            foreach (Student student in _repository.GetAll())
            {
                yield return new List<object>
                {
                    student.Id,
                    student.Name,
                    student.Group,
                    student.Speciality
                };
            }
        }
    }
}
