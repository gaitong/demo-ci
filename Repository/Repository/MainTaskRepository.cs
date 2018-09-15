using Dapper;
using Data.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data.Repository
{
    public interface IMainTaskRepository
    {
        List<MainTask> GetAll();
        MainTask FindBy(int id);
        int Add(MainTask entity);
        int Update(MainTask entity);
        int Delete(MainTask entity);
    }

    public class MainTaskRepository : IMainTaskRepository
    {
        private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<MainTask> GetAll()
        {
            return this._db.Query<MainTask>("SELECT * FROM [MainTask]").ToList();
        }

        public MainTask FindBy(int id)
        {
            var sqlCommand = string.Format(@"SELECT * FROM[MainTask] WHERE[Id] = @Id");
            return this._db.Query<MainTask>(sqlCommand, new
            {
                id
            }).FirstOrDefault();
        }

        public int Add(MainTask entity)
        {
            var sqlCommand = string.Format(@"INSERT INTO [MainTask] ([Name]) VALUES (@Name)");
            return this._db.Execute(sqlCommand, new
            {
                entity.Name
            });
        }

        public int Update(MainTask entity)
        {
            var sqlCommand = string.Format(@"UPDATE [MainTask] SET [Name] = @Name WHERE [Id] = @Id");
            return this._db.Execute(sqlCommand, new
            {
                entity.Name,
                entity.Id
            });
        }

        public int Delete(MainTask entity)
        {
            var sqlCommand = string.Format(@"DELETE FROM [MainTask] WHERE [Id] = @Id");
            return this._db.Execute(sqlCommand, new
            {
                entity.Id
            });
        }
    }
}