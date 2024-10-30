using Model;
using Npgsql.Replication.PgOutput.Messages;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer
{
    
    /// <summary>
    /// Интерфейс для реализации паттерна репозитория, предоставляющий CRUD операции для Stydent
    /// </summary>
    public interface IRepository
        {
        /// <summary>
        ///Получение всех студентов из базы данных
        /// </summary>
        /// <returns>Перечисление объектов Student</returns>
         IEnumerable<Student> GetAll();
        /// <summary>
        /// Добавление студента в базу данных
        /// </summary>
        /// <param name="student">Объект Student</param>
        /// <returns>ID добавленного студента в базу данных</returns>
    
         int Add(Student student);
        /// <summary>
        /// Удаление студента из бахы данных
        /// </summary>
        /// <param name="id">ID студента</param>
         void Delete(int id);
        /// <summary>
        /// Получение студента из базы данных по его ID
        /// </summary>
        /// <param name="id">ID студента</param>
        /// <returns>Объект класса Student</returns>
         Student GetByID(int id);
        /// <summary>
        /// Изменяет студента в базе данных 
        /// </summary>
        /// <param name="student">Объект класса Student</param>
         void Update(Student student);
    }

}
