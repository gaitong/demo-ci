using Dapper;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data.Repository
{
    public interface ISubTaskRepository
    {
        List<SubTask> GetAll();
        SubTask FindBy(int id);
        IEnumerable<SubTask> FindByMainProjectId(int mainTaskId);
        int Add(SubTask entity);
        int Update(SubTask entity);
        int Delete(SubTask entity);
        int DeleteByMainTaskId(int mainTaskId);
        int Active(int id, bool active);
    }

    public class SubTaskRepository : ISubTaskRepository
    {
        private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<SubTask> GetAll()
        {
            return this._db.Query<SubTask>("SELECT * FROM [SubTask]").ToList();
        }

        public SubTask FindBy(int id)
        {
            var sqlCommand = string.Format(@"SELECT * FROM[SubTask] WHERE Id = @Id");
            return this._db.Query<SubTask>(sqlCommand, new
            {
                id
            }).FirstOrDefault();
        }

        public IEnumerable<SubTask> FindByMainProjectId(int mainTaskId)
        {
            var sqlCommand = string.Format(@"SELECT * FROM[SubTask] WHERE [MainTaskId] = @MainTaskId");
            return this._db.Query<SubTask>(sqlCommand, new
            {
                mainTaskId
            }).ToList();
        }

        public int Add(SubTask entity)
        {
            var sqlCommand = string.Format(@"INSERT INTO [SubTask] ([MainTaskId],[Detail],[Active]) VALUES (@MainTaskId,@Detail,@Active)");
            return this._db.Execute(sqlCommand, new
            {
                entity.MainTaskId,
                entity.Detail,
                entity.Active
            });
        }

        public int Update(SubTask entity)
        {
            var sqlCommand = string.Format(@"UPDATE [SubTask] SET [MainTaskId] = @MainTaskId, [Detail] = @Detail, [Active] = @Active WHERE [Id] = @Id");
            return this._db.Execute(sqlCommand, new
            {
                entity.MainTaskId,
                entity.Detail,
                entity.Active,
                entity.Id
            });
        }

        public int Delete(SubTask entity)
        {
            var sqlCommand = string.Format(@"DELETE FROM [SubTask] WHERE [Id] = @Id");
            return this._db.Execute(sqlCommand, new
            {
                entity.Id
            });
        }

        public int DeleteByMainTaskId(int mainTaskId)
        {
            var sqlCommand = string.Format(@"DELETE FROM [SubTask] WHERE [MainTaskId] = @MainTaskId");
            return this._db.Execute(sqlCommand, new
            {
                mainTaskId
            });
        }

        public int Active(int id, bool active)
        {
            var sqlCommand = string.Format(@"UPDATE [SubTask] SET [Active] = @Active WHERE [Id] = @Id");
            return this._db.Execute(sqlCommand, new
            {
                active,
                id
            });
        }
    }
}