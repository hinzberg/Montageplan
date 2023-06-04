using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class MitarbeiterFunktion
    {
        public const int MAX_LENGTH_NAME = 30;


        public MitarbeiterFunktion()
            : this("")
        {
        }
        public MitarbeiterFunktion(string name)
        {
            this.Id = 0;
            this.Name = name;
        }

        /// <summary>
        /// Datenbank Id (PK)
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
