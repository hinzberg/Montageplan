using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class UsersDbTable : SqlCeReadableDatabase<User>
    {
        public UsersDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<User> RealAll()
        {
            return base.Read("SELECT * FROM Users");
        }

        public void Insert(User user)
        {
            StringBuilder query = new StringBuilder("INSERT INTO Users ");
            query.Append("(Username, Password, IsAdministrator) VALUES ");
            query.Append("(@Username, @Password, @IsAdministrator)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Username", user.Username));
            paramList.Add(new SqlCeParameter("@Password", user.Password));
            paramList.Add(new SqlCeParameter("@IsAdministrator", user.IsAdministrator));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Update(User user)
        {
            StringBuilder query = new StringBuilder("UPDATE Users SET ");
            query.Append("Password=@Password, IsAdministrator=@IsAdministrator ");
            query.Append("WHERE Username=@Username");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Username", user.Username));
            paramList.Add(new SqlCeParameter("@Password", user.Password));
            paramList.Add(new SqlCeParameter("@IsAdministrator", user.IsAdministrator));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(User user)
        {
            string query = "DELETE FROM Users WHERE Username=@Username";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Username", user.Username));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<User>
        protected override User ReadEntity(System.Data.IDataRecord data)
        {
            User user = new User();
            user.Username = (string)data["Username"];
            user.Password = (string)data["Password"];
            user.IsAdministrator = (bool)data["IsAdministrator"];

            return user;
        }

    }
}
