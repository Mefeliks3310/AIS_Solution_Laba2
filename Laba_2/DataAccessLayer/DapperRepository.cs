using Dapper;
using Npgsql;
using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Model;
using System.Linq;

namespace DataAccessLayer
{
   
    public class DapperRepository : IRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор, который инициализирует строку подключения на основе конфигурации.
        /// </summary>
        /// <param name="configuration">Конфигурация для получения строки подключения.</param>
        public DapperRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        
        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

       
        public int Add(Student student)
        {
            using (var connection = CreateConnection())
            {
                var sql = "INSERT INTO student (name, \"group\", speciality) VALUES (@Name, @Group, @Speciality) RETURNING ID";
                return connection.QuerySingle<int>(sql, student);
            }
        }

       
        public Student GetByID(int id)
        {
            using (var conn = CreateConnection())
            {
                return conn.Query<Student>("SELECT * FROM student WHERE id = " + id).FirstOrDefault();
            }
        }

        
        public IEnumerable<Student> GetAll()
        {
            using (var conn = CreateConnection())
            {
                return conn.Query<Student>("SELECT * FROM student").ToList();
            }
        }

       
        public void Delete(int id)
        {
            using (var conn = CreateConnection())
            {
                var sql = "DELETE FROM student WHERE id = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }

        
        public void Update(Student student)
        {
            using (var conn = CreateConnection())
            {
                var sqlQuery = "UPDATE student SET name = @Name, \"group\" = @Group, speciality = @Speciality WHERE Id = @Id";
                conn.Execute(sqlQuery, student);
            }
        }
    }
}
