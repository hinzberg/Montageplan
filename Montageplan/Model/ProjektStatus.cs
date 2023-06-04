using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public enum ProjektStatus
    {
        /// <summary>
        /// Das Projekt ist noch nicht gestartet und wurde noch mit keinem Termin verknüpft.
        /// Projekt hat aber Start- und Enddatum.
        /// </summary>
        Idle,
        /// <summary>
        /// Das Projekt ist noch nicht gestartet, aber mit einem Termin verknüpft
        /// </summary>
        Linked,
        /// <summary>
        /// Das Projekt ist gestartet und wurde mit einem Termin verknüpft
        /// </summary>
        Running,
        /// <summary>
        /// Das Projekt ist gestartet, aber noch nicht mit einem Termin verknüpft
        /// </summary>
        Warning,
        /// <summary>
        /// Das Projekt hat kein Start und Endedatum und kann deshalb auch nicht verspätet sein.
        /// </summary>
        NoTime
    }
}
