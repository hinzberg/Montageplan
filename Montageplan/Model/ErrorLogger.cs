using Montageplan.Model.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    /// <summary>
    /// Verwaltet das Loggen von Fehlern und speichert die Informationen in einer Textdatei ab.
    /// Jeden Tag wird eine neue Logdatei angelegt und im Logsorder gespeichert.
    /// Jeder neue Logeintrag am selben Tag, wird ganz unten in der Textdatei hinzugefügt.
    /// </summary>
    public class ErrorLogger
    {
        /*
        Vollständiger Logeintrag mit StackTrace - Beispiel:

        [30.03.2013 11.02.21.598] [0.01.02]
        'Fehler beim Speichern der Configs'
        Illegales Zeichen im Pfad.
        StackTrace:
        -> Save:ConfigsXml
        -> Save:AppConfig
        -> window_Closed:MainPresenter
        -> OnClosed:Window
        -> WmDestroy:Window
        -> WindowFilterMessage:Window
        -> PublicHooksFilterMessage:HwndSource-> WndProc:HwndWrapper
        */

        private const string LOG_DIR_NAME = "logs";

        private readonly string version;
        private readonly string logDirectory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version">Programmversion</param>
        /// <param name="directory">Verzeichnis, in dem die Logs gespeichert werden</param>
        public ErrorLogger(string version, string directory)
            : this(version, directory, 7)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version">Programmversion</param>
        /// <param name="directory">Verzeichnis, in dem die Logs gespeichert werden</param>
        /// <param name="maxStackTraceFrames">Maximale Anzahl der Frames (Aufrufhierarchie von Methoden), 
        /// die ausgegeben werden, wenn der StackTrace angezeigt werden soll</param>
        public ErrorLogger(string version, string directory, int maxStackTraceFrames)
        {
            this.version = version;
            this.logDirectory = directory + @"\" + LOG_DIR_NAME;
            this.MaxStackTraceFrames = maxStackTraceFrames;
        }

        /// <summary>
        /// Maximale Anzahl der Frames (Aufrufhierarchie von Methoden), 
        /// die ausgegeben werden, wenn der StackTrace angezeigt werden soll. Der Standardwert ist 7.
        /// </summary>
        public int MaxStackTraceFrames { get; set; }

        /// <summary>
        /// Schreibt einen Logeintrag.
        /// </summary>
        /// <param name="message">Meldung</param>
        public void Write(string message)
        {
            this.Write("", message, false);
        }
        /// <summary>
        /// Schreibt einen Logeintrag mit dem aktuellen StackTrace.
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="withStackTrace">Soll der StackTrace mitgeloggt werden ?</param>
        public void Write(string message, bool withStackTrace)
        {
            this.Write("", message, withStackTrace);
        }
        /// <summary>
        /// Schreibt einen Logeintrag mit einem Titel (gekennzeichnet mit '').
        /// </summary>
        /// <param name="title">Titel</param>
        /// <param name="message">Meldung</param>
        public void Write(string title, string message)
        {
            this.Write(title, message, false);
        }
        /// <summary>
        /// Schreibt einen Logeintrag mit einem Titel (gekennzeichnet mit '') und dem aktuellen StackTrace.
        /// </summary>
        /// <param name="title">Titel</param>
        /// <param name="message">Meldung</param>
        /// <param name="withStackTrace">Soll der StackTrace mitgeloggt werden ?</param>
        public void Write(string title, string message, bool withStackTrace)
        {
            string header = this.BuildHeader();
            string log = "";

            log += header + "\r\n";

            if (title != "")
                log += string.Format("'{0}'\r\n", title);

            log += message;

            if (withStackTrace)
            {
                log += "\r\n" + this.BuildStackTrace(this.MaxStackTraceFrames);
            }
            log += "\r\n\r\n"; // Freie Zeile zum nächsten Log

            string file = this.logDirectory + @"\" + this.BuildFileName();
            this.WriteLog(log, file);
        }

        /// <summary>
        /// Erstellt die erste Zeile eines Logeintrags und gibt sie zurück.
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildHeader()
        {
            string header;
            string date = string.Format("{0:dd.MM.yyyy HH.mm.ss.fff}", DateTime.Now);
            header = string.Format("[{0}] [{1}]", date, this.version);
            return header;
        }

        /// <summary>
        /// Erstellt den StackTrace und gibt ihn zurück.
        /// </summary>
        /// <param name="maxStackTraceFrames">Maximale Anzahl der Frames (Aufrufhierarchie von Methoden)</param>
        /// <returns></returns>
        private string BuildStackTrace(int maxStackTraceFrames)
        {
            string text = "StackTrace:\r\n";
            StackTrace stackTrace = new StackTrace();
            StackFrame[] frames = stackTrace.GetFrames();
            int startIndex = 2; // Die wersten 2 Methoden vom Trace kommen aus dieser Klasse - Unwichtig
            for (int n = startIndex; n <= maxStackTraceFrames + startIndex; n++)
            {
                if (n < frames.Length)
                {
                    StackFrame frame = frames[n];
                    text += string.Format("-> {0}:{1}", frame.GetMethod().Name, frame.GetMethod().ReflectedType.Name);
                    if (n <= maxStackTraceFrames)
                        text += "\r\n";
                }
                else
                    break;
            }
            return text;
        }

        /// <summary>
        /// Erstellt den Dateinamen der Textdatei und gibt ihn zurück.
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildFileName()
        {
            string file = string.Format("logs {0:yyyy-MM-dd}.txt", DateTime.Today);
            return file;
        }

        /// <summary>
        /// Schreibt den Logeintrag als Textdatei auf die Festplatte.
        /// </summary>
        /// <param name="log">Logeintrag</param>
        /// <param name="file">Logdatei</param>
        protected virtual void WriteLog(string log, string file)
        {
            try
            {
                DirectoryCreator.CreateIfNotExists(this.logDirectory);
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    writer.Write(log);
                }
            }
            catch (Exception)
            {
            }
        }


    }
}
