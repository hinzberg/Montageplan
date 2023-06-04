using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class DatabaseUpdater
    {
        private class EngagementCalendarItemU27
        {
            public EngagementCalendarItemU27()
            {
                this.EngagementId = 0;
                this.KolonneId = 0;
                this.ProjektId = 0;
                this.Date = DateTime.MinValue;
            }

            public int EngagementId { get; set; }
            public int KolonneId { get; set; }
            public int ProjektId { get; set; }
            public DateTime Date { get; set; }
        }
        private class EngagementCalendarDbTableU27 : SqlCeReadableDatabase<EngagementCalendarItemU27>
        {
            public EngagementCalendarDbTableU27(string connString)
                : base(connString)
            {
            }

            public List<EngagementCalendarItemU27> Read()
            {
                List<EngagementCalendarItemU27> engagements = new List<EngagementCalendarItemU27>();
                string query = @"SELECT en.Id, en.KolonneId, en.ProjektId, ca.Date
                                FROM Engagements AS en INNER JOIN
                                CalendarDates AS ca ON en.CalendarDateId = ca.Id";
                engagements.AddRange(base.Read(query));
                return engagements;
            }

            public void UpdateDate(int engagementId, DateTime date)
            {
                string query = "UPDATE Engagements SET Date=@Date WHERE Id=@Id";

                List<SqlCeParameter> paramList = new List<SqlCeParameter>();
                paramList.Add(new SqlCeParameter("@Date", date));
                paramList.Add(new SqlCeParameter("@Id", engagementId));

                base.ExecuteQuery(query, paramList.ToArray());
            }

            // SqlCeReadableDatabase<EngagementCalendarItemU27>
            protected override EngagementCalendarItemU27 ReadEntity(System.Data.IDataRecord data)
            {
                EngagementCalendarItemU27 item = new EngagementCalendarItemU27();
                item.EngagementId = (int)data["Id"];
                item.KolonneId = (int)data["KolonneId"];
                item.ProjektId = (int)data["ProjektId"];
                item.Date = (DateTime)data["Date"];
                return item;
            }
        }


        private bool isSucceeded;

        public DatabaseUpdater()
        {
            this.isSucceeded = false;
        }

        public bool IsSucceeded
        {
            get { return this.isSucceeded; }
        }

        public void Update()
        {
            List<Action> updates = new List<Action>(this.GetUpdateActions());
            int version = 1;
            try
            {
                AppConfig.MetaData.ReadMetaData();
                foreach (var update in updates)
                {
                    if (version > AppConfig.MetaData.DatabaseVersion && version <= AppConfig.REQUIRED_DB_VERSION)
                    {
                        update();
                        AppConfig.MetaData.UpdateDatabaseVersion(version);
                    }
                    version++;
                }
                this.isSucceeded = true;
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("Datenbankupdate Version: " + version, exp.Message, true);
                MessageBoxHelper.ShowError("Fehler beim Aktualisieren der Datenbank!\r\n" + exp.Message);
            }
        }

        private List<Action> GetUpdateActions()
        {
            List<Action> updates = new List<Action>();
            updates.Add(this.UpdateVersion1);
            updates.Add(this.UpdateVersion2);
            updates.Add(this.UpdateVersion3);
            updates.Add(this.UpdateVersion4);
            updates.Add(this.UpdateVersion5);
            updates.Add(this.UpdateVersion6);
            updates.Add(this.UpdateVersion7);
            updates.Add(this.UpdateVersion8);
            updates.Add(this.UpdateVersion9);
            updates.Add(this.UpdateVersion10);
            updates.Add(this.UpdateVersion11);
            updates.Add(this.UpdateVersion12);
            updates.Add(this.UpdateVersion13);
            updates.Add(this.UpdateVersion14);
            updates.Add(this.UpdateVersion15);
            updates.Add(this.UpdateVersion16);
            updates.Add(this.UpdateVersion17);
            updates.Add(this.UpdateVersion18);
            updates.Add(this.UpdateVersion19);
            updates.Add(this.UpdateVersion20);
            updates.Add(this.UpdateVersion21);
            updates.Add(this.UpdateVersion22);
            updates.Add(this.UpdateVersion23);
            updates.Add(this.UpdateVersion24);
            updates.Add(this.UpdateVersion25);
            updates.Add(this.UpdateVersion26);
            updates.Add(this.UpdateVersion27);
            updates.Add(this.UpdateVersion28);
            updates.Add(this.UpdateVersion29);
            updates.Add(this.UpdateVersion30);
            updates.Add(this.UpdateVersion31);
            updates.Add(this.UpdateVersion32);
            updates.Add(this.UpdateVersion33);
            updates.Add(this.UpdateVersion34);
            return updates;
        }

        /// <summary>
        /// Erstellt die Projekttabelle.
        /// </summary>
        private void UpdateVersion1()
        {
            string query = "CREATE TABLE Projekte (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "Name nvarchar(50) NOT NULL, " +
                "Startdatum datetime, " +
                "Endedatum datetime, " +
                "HexColor nchar(9) NOT NULL, " +
                "IsCompleted bit NOT NULL, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Mitarbeitertabelle.
        /// </summary>
        private void UpdateVersion2()
        {
            string query = "CREATE TABLE Mitarbeiter (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "IsActive bit NOT NULL, " +
                "Name nvarchar(50) NOT NULL, " +
                "Vorname nvarchar(50) NOT NULL, " +
                "KannFuehrerSein bit NOT NULL, " +
                "FunktionId int, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Funktionentabelle.
        /// </summary>
        private void UpdateVersion3()
        {
            string query = "CREATE TABLE Funktionen (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "Name nvarchar(30) NOT NULL, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Fehlzeitentabelle.
        /// </summary>
        private void UpdateVersion4()
        {
            string query = "CREATE TABLE Fehlzeiten (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "FehlzeitartId int NOT NULL, " +
                "MitarbeiterId int NOT NULL, " +
                "Startdatum datetime NOT NULL, " +
                "Endedatum datetime NOT NULL, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den MitarbeiterId-Fremdschlüssel für die Fehlzeitentabelle.
        /// </summary>
        private void UpdateVersion5()
        {
            string query = "ALTER TABLE Fehlzeiten " +
                "ADD CONSTRAINT FK_MitarbeiterId " +
                "FOREIGN KEY (MitarbeiterId) REFERENCES Mitarbeiter(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den MitarbeiterId-Index für die Fehlzeitentabelle.
        /// </summary>
        private void UpdateVersion6()
        {
            string query = "CREATE INDEX IX_MitarbeiterId ON Fehlzeiten(MitarbeiterId)";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Fehlzeitartentabelle.
        /// </summary>
        private void UpdateVersion7()
        {
            string query = "CREATE TABLE Fehlzeitarten (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "Bezeichnung nvarchar(30) NOT NULL, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt Standardeinträge für die Fehlzeitartentabelle.
        /// </summary>
        private void UpdateVersion8()
        {
            string query = "INSERT INTO Fehlzeitarten (Bezeichnung) VALUES ('Urlaub')";
            this.ExecuteQuery(query);

            query = "INSERT INTO Fehlzeitarten (Bezeichnung) VALUES ('Krankheit')";
            this.ExecuteQuery(query);

            query = "INSERT INTO Fehlzeitarten (Bezeichnung) VALUES ('Sonstiges')";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Kolonnentabelle.
        /// </summary>
        private void UpdateVersion9()
        {
            string query = "CREATE TABLE Kolonnen (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "Nummer nvarchar(30) NOT NULL, " +
                "Bezeichnung nvarchar(50), " +
                "IsActive bit NOT NULL, " +
                "FuehrerId int NOT NULL, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Benutzertabelle.
        /// </summary>
        private void UpdateVersion10()
        {
            string query = "CREATE TABLE Users (" +
                "Username nvarchar(30) NOT NULL, " +
                "Password nvarchar(30) NOT NULL, " +
                "IsAdministrator bit NOT NULL, " +
                "PRIMARY KEY (Username))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Auftragstabelle. Ein Auftrag ist eine Kolonne mit mind. einem Kolonnenführer als Mitarbeiter,
        /// die mit einem Projekt an einem bestimmten Tag verknüpft ist.
        /// </summary>
        private void UpdateVersion11()
        {
            string query = "CREATE TABLE Engagements (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "CalendarDateId int NOT NULL, " +
                "KolonneId int NOT NULL, " +
                "ProjektId int NOT NULL, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den Engagements-Unique für die Fehlzeitentabelle.
        /// </summary>
        private void UpdateVersion12()
        {
            string query = "CREATE INDEX UQ_Engagements1 ON Engagements(CalendarDateId, KolonneId, ProjektId)";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Kalendertagetabelle.
        /// </summary>
        private void UpdateVersion13()
        {
            string query = "CREATE TABLE CalendarDates (" +
                "Id int IDENTITY(1,1) NOT NULL, " +
                "Date datetime NOT NULL, " +
                "TimeFrom datetime, " +
                "TimeTo datetime, " +
                "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den CalendarDateId-Fremdschlüssel für die Engagements-Tabelle.
        /// </summary>
        private void UpdateVersion14()
        {
            string query = "ALTER TABLE Engagements " +
                "ADD CONSTRAINT FK_CalendarDateId " +
                "FOREIGN KEY (CalendarDateId) REFERENCES CalendarDates(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Aufträge-Mitarbeiter-Verknüpfungstabelle.
        /// </summary>
        private void UpdateVersion15()
        {
            string query = "CREATE TABLE EngagementsMitarbeiter (" +
                "EngagementsId int NOT NULL, " +
                "MitarbeiterId int NOT NULL, " +
                "IstFuehrer bit NOT NULL, " +
                "PRIMARY KEY (EngagementsId, MitarbeiterId))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den MitarbeiterId-Fremdschlüssel für die Engagements-Mitarbeitertabelle.
        /// </summary>
        private void UpdateVersion16()
        {
            string query = "ALTER TABLE EngagementsMitarbeiter " +
                "ADD CONSTRAINT FK_MitarbeiterId " +
                "FOREIGN KEY (MitarbeiterId) REFERENCES Mitarbeiter(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den EngagementsId-Fremdschlüssel für die Engagements-Mitarbeitertabelle.
        /// </summary>
        private void UpdateVersion17()
        {
            string query = "ALTER TABLE EngagementsMitarbeiter " +
                "ADD CONSTRAINT FK_EngagementsId " +
                "FOREIGN KEY (EngagementsId) REFERENCES Engagements(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Kolonnen-Mitarbeitertabelle.
        /// </summary>
        private void UpdateVersion18()
        {
            string query = "CREATE TABLE KolonnenMitarbeiter (" +
                "MitarbeiterId int NOT NULL, " +
                "KolonneId int NOT NULL, " +
                "IstFuehrer bit NOT NULL, " +
                "PRIMARY KEY (MitarbeiterId, KolonneId))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den MitarbeiterId-Fremdschlüssel für die Kolonnen-Mitarbeitertabelle.
        /// </summary>
        private void UpdateVersion19()
        {
            string query = "ALTER TABLE KolonnenMitarbeiter " +
                "ADD CONSTRAINT FK_MitarbeiterId " +
                "FOREIGN KEY (MitarbeiterId) REFERENCES Mitarbeiter(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den KolonneId-Fremdschlüssel für die Kolonnen-Mitarbeitertabelle.
        /// </summary>
        private void UpdateVersion20()
        {
            string query = "ALTER TABLE KolonnenMitarbeiter " +
                "ADD CONSTRAINT FK_KolonneId " +
                "FOREIGN KEY (KolonneId) REFERENCES Kolonnen(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Entfernt die Spalte "FuehrerId" von der Kolonnentabelle. Diese Spalte wird nicht mehr benötigt,
        /// nachdem das Update "18" ausgeführt wurde.
        /// </summary>
        private void UpdateVersion21()
        {
            string query = "ALTER TABLE Kolonnen DROP COLUMN FuehrerId";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Spalte "Description" wird der Mitarbeitertabelle neu hinzugefügt.
        /// </summary>
        private void UpdateVersion22()
        {
            string query = "ALTER TABLE Mitarbeiter ADD COLUMN Description nvarchar(50)";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Spalte "HexColor" wird der Kolonnentabelle neu hinzugefügt.
        /// </summary>
        private void UpdateVersion23()
        {
            string query = "ALTER TABLE Kolonnen ADD COLUMN HexColor nchar(9) default '#000000' NOT NULL";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt die Tabelle "EngagementsProjekte".
        /// </summary>
        private void UpdateVersion24()
        {
            string query = "CREATE TABLE EngagementsProjekte (" +
                 "Id int IDENTITY(1,1) NOT NULL, " +
                 "EngagementId int NOT NULL, " +
                 "ProjektId int NOT NULL, " +
                 "StartTime datetime, " +
                 "EndTime datetime, " +
                 "PRIMARY KEY (Id))";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Erstellt den EngagementsId-Fremdschlüssel für die Engagements-Projekttabelle.
        /// </summary>
        private void UpdateVersion25()
        {
            string query = "ALTER TABLE EngagementsProjekte " +
                "ADD CONSTRAINT FK_EngagementId " +
                "FOREIGN KEY (EngagementId) REFERENCES Engagements(Id) ON DELETE CASCADE";

            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Spalte "Date" wird der Engagements-Tabelle neu hinzugefügt.
        /// </summary>
        private void UpdateVersion26()
        {
            string query = "ALTER TABLE Engagements ADD COLUMN Date datetime";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Konvertiert die alte Engagement-Struktur der Datenbank auf die neue.
        /// </summary>
        private void UpdateVersion27()
        {
            EngagementCalendarDbTableU27 engagementsTableU27 = new EngagementCalendarDbTableU27(AppConfig.DatabaseConnectionString);
            List<EngagementCalendarItemU27> engagementItems = engagementsTableU27.Read();
            if (engagementItems.Count > 0)
            {
                EngagementsProjekteDbTable engProTable = new EngagementsProjekteDbTable();
                foreach (var engItem in engagementItems)
                {
                    engagementsTableU27.UpdateDate(engItem.EngagementId, engItem.Date);

                    DateTime date = engItem.Date;
                    EngagementProjektEntity entity = new EngagementProjektEntity();
                    entity.ProjektId = engItem.ProjektId;
                    entity.EngagementId = engItem.EngagementId;
                    // Bei der vorherigen Version konnte man noch keine Projekte-Zeitangaben einstellen. Desegen wird immer 'null' gesetzt
                    entity.StartTime = null;
                    entity.EndTime = null;
                    engProTable.Insert(entity);
                }
            }
        }

        /// <summary>
        /// Die Spalte "Date" der Engagements-Tabelle nimmt keine Null-Werte mehr an.
        /// </summary>
        private void UpdateVersion28()
        {
            string query = "ALTER TABLE Engagements ALTER COLUMN Date datetime NOT NULL";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Entfernt den Unique "UQ_Engagements1" der Engagements-Tabelle.
        /// </summary>
        private void UpdateVersion29()
        {
            string query = "DROP INDEX Engagements.UQ_Engagements1";
            this.ExecuteQuery(query);
        }
     
        /// <summary>
        /// Entfernt den Fremdschlüssel "FK_CalendarDateId" der Engagements-Tabelle.
        /// </summary>
        private void UpdateVersion30()
        {
            string query = "ALTER TABLE Engagements DROP CONSTRAINT FK_CalendarDateId";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Spalte "CalendarDateId" der Engagements-Tabelle wird entfernt.
        /// </summary>
        private void UpdateVersion31()
        {
            string query = "ALTER TABLE Engagements DROP COLUMN CalendarDateId";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Spalte "ProjektId" der Engagements-Tabelle wird entfernt.
        /// </summary>
        private void UpdateVersion32()
        {
            string query = "ALTER TABLE Engagements DROP COLUMN ProjektId";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Tabelle "CalendarDates" wird entfernt.
        /// </summary>
        private void UpdateVersion33()
        {
            string query = "DROP TABLE CalendarDates";
            this.ExecuteQuery(query);
        }

        /// <summary>
        /// Die Spalte "IsTemplate" wird der Projektetabelle neu hinzugefügt.
        /// </summary>
        private void UpdateVersion34()
        {
            string query = "ALTER TABLE Projekte ADD COLUMN IsTemplate bit DEFAULT 1 NOT NULL";
            this.ExecuteQuery(query);
        }




        private void ExecuteQuery(string query)
        {
            SqlCeDatabase db = new SqlCeDatabase(AppConfig.DatabaseConnectionString);
            db.ExecuteQuery(query);
            db.CloseConnection();
        }

    }
}
