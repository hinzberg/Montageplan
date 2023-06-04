using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Montageplan.Model.DAL.Database
{
    public class KolonnenDbTable : SqlCeReadableDatabase<Kolonne>
    {
        public const int BEZEICHNUNG_MAX_LENGTH = 50;

        public KolonnenDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<Kolonne> ReadAll()
        {
            return base.Read("SELECT * FROM Kolonnen");
        }

        public void InsertOrUpdate(Kolonne projekt)
        {
            if (projekt.Id == 0)
                this.Insert(projekt);
            else
                this.Update(projekt);
        }

        public void Insert(Kolonne kolonne)
        {
            StringBuilder query = new StringBuilder("INSERT INTO Kolonnen ");
            query.Append("(Nummer, Bezeichnung, IsActive, HexColor) VALUES ");
            query.Append("(@Nummer, @Bezeichnung, @IsActive, @HexColor)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Nummer", kolonne.Nummer));
            paramList.Add(new SqlCeParameter("@Bezeichnung", kolonne.Bezeichnung));
            paramList.Add(new SqlCeParameter("@IsActive", kolonne.IsActive));
            paramList.Add(new SqlCeParameter("@HexColor", kolonne.KolonneColorBrush.ToString()));

            kolonne.Id = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
        }

        public void Update(Kolonne kolonne)
        {
            StringBuilder query = new StringBuilder("UPDATE Kolonnen SET ");
            query.Append("Nummer=@Nummer, Bezeichnung=@Bezeichnung, IsActive=@IsActive, HexColor=@HexColor ");
            query.Append("WHERE Id=@Id");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", kolonne.Id));
            paramList.Add(new SqlCeParameter("@Nummer", kolonne.Nummer));
            paramList.Add(new SqlCeParameter("@Bezeichnung", kolonne.Bezeichnung));
            paramList.Add(new SqlCeParameter("@IsActive", kolonne.IsActive));
            paramList.Add(new SqlCeParameter("@HexColor", kolonne.KolonneColorBrush.ToString()));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(Kolonne kolonne)
        {
            string query = "DELETE FROM Kolonnen WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", kolonne.Id));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<Kolonne>
        protected override Kolonne ReadEntity(System.Data.IDataRecord data)
        {
            Kolonne kolonne = new Kolonne();
            kolonne.Id = (int)data["Id"];
            kolonne.Nummer = (string)data["Nummer"];
            kolonne.Bezeichnung = (string)data["Bezeichnung"];
            kolonne.IsActive = (bool)data["IsActive"];
            string brushString = (string)data["HexColor"];
            kolonne.KolonneColorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(brushString);

            kolonne.Fuehrer = new Mitarbeiter();
            return kolonne;
        }

    }
}
