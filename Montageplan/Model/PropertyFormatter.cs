using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    /// <summary>
    /// Formatierungen verschiedener Datentypen.
    /// </summary>
    public static class PropertyFormatter
    {
        /// <summary>
        /// Formatiert das Datum und die Uhrzeit: dd.mm.yyyy HH:mm und gibt die Formatierung zurück. Wenn kein gültiges Datum vorliegt,
        /// wird als Platzhalter '-' angezeigt.
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime? date)
        {
            string fDate = "-";
            if (date.HasValue)
                fDate = string.Format("{0:dd.MM.yyyy HH:mm}", date);
            return fDate;
        }
        /// <summary>
        /// Formatiert das Datum: dd.mm.yyyy und gibt die Formatierung zurück. Wenn kein gültiges Datum vorliegt,
        /// wird als Platzhalter '-' angezeigt.
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static string FormatDate(DateTime? date)
        {
            string fDate = "-";
            if (date.HasValue)
                fDate = string.Format("{0:dd.MM.yyyy}", date);
            return fDate;
        }
        /// <summary>
        /// Formatiert die Zeit des Datums: HH:mm und gibt die Formatierung zurück.
        /// </summary>
        /// <param name="date">Datum mit Zeitangabe</param>
        /// <returns></returns>
        public static string FormatTime(DateTime date)
        {
            string fDate = string.Format("{0:HH:mm}", date);
            return fDate;
        }
        /// <summary>
        /// Formatiert das Datum: dd.mm.yyyy und gibt die Formatierung zurück.
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static string FormatDateWithoutPlaceholder(DateTime? date)
        {
            string fDate = "";
            if (date.HasValue)
                fDate = string.Format("{0:dd.MM.yyyy}", date);
            return fDate;
        }

        /// <summary>
        /// Formatiert den boolschen Wert zu 'Ja' oder 'Nein' und gibt den Wert zurück.
        /// </summary>
        /// <param name="value">Bool</param>
        /// <returns></returns>
        public static string FormatBool(bool value)
        {
            string fBool = "Nein";
            if (value)
                fBool = "Ja";
            return fBool;
        }

        /// <summary>
        /// Formatiert den String. Wenn der String leer oder null ist wird '-' zurückgegeben.
        /// </summary>
        /// <param name="value">String</param>
        /// <returns></returns>
        public static string FormatString(string value)
        {
            string fString = "-";
            if (!string.IsNullOrEmpty(value))
                fString = value;
            return fString;
        }

    }
}
