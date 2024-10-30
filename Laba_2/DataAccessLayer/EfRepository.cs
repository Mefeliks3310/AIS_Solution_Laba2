using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{

    public class EfRepository : IRepository
    {
        private readonly DbContext _context;

        /// <summary>
        /// Конструктор, инициализирующий объект репозитория с заданным контекстом базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public int Add(Student student)
        {
            _context.Set<Student>().Add(student);
            _context.SaveChanges();
            return student.Id;
        }

        public void Delete(int id)
        {
            var student = _context.Set<Student>().Find(id);
            if (student != null)
            {
                _context.Set<Student>().Remove(student);
                _context.SaveChanges();
            }
        }

        public Student GetByID(int id)
        {
            return _context.Set<Student>().Find(id);
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Set<Student>().ToList();
        }

        public void Update(Student student)
        {
            _context.Set<Student>().Update(student);
            _context.SaveChanges();
        }
    }
}
