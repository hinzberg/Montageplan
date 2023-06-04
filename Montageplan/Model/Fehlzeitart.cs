using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class Fehlzeitart
    {
        public Fehlzeitart()
        {
            this.Id = 0;
            this.Bezeichnung = "";
        }

        public int Id { get; set; }
        public string Bezeichnung { get; set; }

    }
}
