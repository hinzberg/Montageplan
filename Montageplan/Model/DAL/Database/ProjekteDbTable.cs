using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Montageplan.Model.DAL.Database
{
    public class ProjekteDbTable : SqlCeReadableDatabase<Projekt>
    {
        public ProjekteDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<Projekt> ReadAll()
        {
            return base.Read("SELECT * FROM Projekte");
        }

        public void InsertOrUpdate(Projekt projekt)
        {
            if (projekt.Id == 0)
                this.Insert(projekt);
            else
                this.Update(projekt);
        }

        public void Insert(Projekt projekt)
        {
            StringBuilder query = new StringBuilder("INSERT INTO Projekte ");
            query.Append("(Name, Startdatum, Endedatum, HexColor, IsCompleted, IsTemplate) VALUES ");
            query.Append("(@Name, @Startdatum, @Endedatum, @HexColor, @IsCompleted, @IsTemplate)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Name", projekt.Name));
            paramList.Add(new SqlCeParameter("@Startdatum", base.ConvertValueForDbParam(projekt.Startdatum)));
            paramList.Add(new SqlCeParameter("@Endedatum", base.ConvertValueForDbParam(projekt.Endedatum)));
            paramList.Add(new SqlCeParameter("@HexColor", projekt.ProjectColorBrush.ToString()));
            paramList.Add(new SqlCeParameter("@IsCompleted", projekt.IsCompleted));
            paramList.Add(new SqlCeParameter("@IsTemplate", projekt.IsTemplate));

            projekt.Id = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
        }

        public void Update(Projekt projekt)
        {
            StringBuilder query = new StringBuilder("UPDATE Projekte SET ");
            query.Append("Name=@Name, Startdatum=@Startdatum, Endedatum=@Endedatum, HexColor=@HexColor, IsCompleted=@IsCompleted, IsTemplate=@IsTemplate ");
            query.Append("WHERE Id=@Id");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", projekt.Id));
            paramList.Add(new SqlCeParameter("@Name", projekt.Name));
            paramList.Add(new SqlCeParameter("@Startdatum", base.ConvertValueForDbParam(projekt.Startdatum)));
            paramList.Add(new SqlCeParameter("@Endedatum", base.ConvertValueForDbParam(projekt.Endedatum)));
            paramList.Add(new SqlCeParameter("@HexColor", projekt.ProjectColorBrush.ToString()));
            paramList.Add(new SqlCeParameter("@IsCompleted", projekt.IsCompleted));
            paramList.Add(new SqlCeParameter("@IsTemplate", projekt.IsTemplate));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(Projekt projekt)
        {
            string query = "DELETE FROM Projekte WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", projekt.Id));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<Projekt>
        protected override Projekt ReadEntity(System.Data.IDataRecord data)
        {
            Projekt projekt = new Projekt();
            projekt.Id = (int)data["Id"];
            projekt.Name = (string)data["Name"];
            projekt.IsCompleted = (bool)data["IsCompleted"];
            projekt.IsTemplate = (bool)data["IsTemplate"];
            if (DBNull.Value != data["Startdatum"]) projekt.Startdatum = (DateTime)data["Startdatum"];
            if (DBNull.Value != data["Endedatum"]) projekt.Endedatum = (DateTime)data["Endedatum"];
            string brushString = (string)data["HexColor"];
            projekt.ProjectColorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(brushString);

            return projekt;
        }

    }
}
