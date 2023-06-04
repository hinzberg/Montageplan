using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class FehlzeitartenDbTable : SqlCeReadableDatabase<Fehlzeitart>
    {
        public FehlzeitartenDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<Fehlzeitart> RealAll()
        {
            return base.Read("SELECT * FROM Fehlzeitarten");
        }

        // SqlCeReadableDatabase<Fehlzeitart>
        protected override Fehlzeitart ReadEntity(System.Data.IDataRecord data)
        {
            Fehlzeitart fehlzeit = new Fehlzeitart();
            fehlzeit.Id = (int)data["Id"];
            fehlzeit.Bezeichnung = (string)data["Bezeichnung"];

            return fehlzeit;
        }

    }
}
