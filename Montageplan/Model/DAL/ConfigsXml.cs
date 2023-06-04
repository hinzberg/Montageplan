using System;
using System.Windows.Media;
using System.Xml;

namespace Montageplan.Model.DAL
{
    /// <summary>
    /// Benutzereinstellungen werden als xml-Datei im Stammverzeichnis abgelegt.
    /// Die xml-Datei kann geladen und gespeichert werden.
    /// </summary>
    public class ConfigsXml
    {
        private const string XML_NAME = "configs.xml";
        private const string XML_VERSION = "1.0";

        private readonly string file;

        public ConfigsXml(string directory)
        {
            this.file = directory + @"\" + XML_NAME;
        }

        /// <summary>
        /// Speichert die Einstellungen als xml-Datei ab
        /// </summary>
        /// <param name="settings">Einstellungen</param>
        public void Save(UserSettings settings)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlNode root = doc.CreateElement("configs");
            doc.AppendChild(root);

            XmlAttribute verConfigsAttr = doc.CreateAttribute("version");
            verConfigsAttr.Value = XML_VERSION;
            root.Attributes.Append(verConfigsAttr);

            XmlAttribute dateConfigsAttr = doc.CreateAttribute("time");
            dateConfigsAttr.Value = string.Format("{0:dd.MM.yyyy HH:mm}", DateTime.Now);
            root.Attributes.Append(dateConfigsAttr);

            XmlElement daysAlignment = doc.CreateElement("KalenderTageAusrichtung");
            daysAlignment.InnerText = settings.KalenderTageAusrichtung.ToString();
            root.AppendChild(daysAlignment);

            XmlElement startDatePrint = doc.CreateElement("StartDatumDruck");
            startDatePrint.InnerText = settings.StartDatumDruck.ToString();
            root.AppendChild(startDatePrint);

            XmlElement endeDatePrint = doc.CreateElement("EndeDatumDruck");
            endeDatePrint.InnerText = settings.EndeDatumDruck.ToString();
            root.AppendChild(endeDatePrint);

            XmlElement mitarbeiterBezeichnung = doc.CreateElement("MitarbeiterDisplayWithBezeichnung");
            mitarbeiterBezeichnung.InnerText = settings.MitarbeiterDisplayWithBezeichnung.ToString();
            root.AppendChild(mitarbeiterBezeichnung);

            XmlElement mitarbeiterName = doc.CreateElement("MitarbeiterDisplayWithName");
            mitarbeiterName.InnerText = settings.MitarbeiterDisplayWithName.ToString();
            root.AppendChild(mitarbeiterName);

            XmlElement mitarbeiterVorname = doc.CreateElement("MitarbeiterDisplayWithVorname");
            mitarbeiterVorname.InnerText = settings.MitarbeiterDisplayWithVorname.ToString();
            root.AppendChild(mitarbeiterVorname);

            XmlElement showKolonnePrefix = doc.CreateElement("ShowKolonnePrefix");
            showKolonnePrefix.InnerText = settings.ShowKolonnePrefix.ToString();
            root.AppendChild(showKolonnePrefix);

            XmlElement dayBrush = doc.CreateElement("DayBrush");
            dayBrush.InnerText = settings.DayBrush.Color.ToString();
            root.AppendChild(dayBrush);

            XmlElement pageOffsetX = doc.CreateElement("SeitenAbstandX");
            pageOffsetX.InnerText = settings.Print.PageOffsetX.ToString();
            root.AppendChild(pageOffsetX);

            XmlElement pageOffsetY = doc.CreateElement("SeitenAbstandY");
            pageOffsetY.InnerText = settings.Print.PageOffsetY.ToString();
            root.AppendChild(pageOffsetY);

            try
            {
                doc.Save(this.file);
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("Fehler beim Speichern der Configs", exp.Message);
            }
        }

        /// <summary>
        /// Läd die Einstellungen von der xml-Datei und gibt sie zurück.
        /// </summary>
        /// <returns>Einstellungen</returns>
        public UserSettings Load()
        {
            UserSettings loadedSettings = new UserSettings();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.file);

                XmlNode configs = doc.SelectSingleNode("configs");
                if (configs != null)
                {
                    XmlNode e = configs.SelectSingleNode("KalenderTageAusrichtung");
                    if (e != null)
                    {
                        DaysAlignment daysAlignmentSettings =
                            (DaysAlignment) Enum.Parse(typeof (DaysAlignment), e.InnerText);
                        loadedSettings.KalenderTageAusrichtung = daysAlignmentSettings;
                    }

                    e = configs.SelectSingleNode("StartDatumDruck");
                    if (e != null)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(e.InnerText, out dt) == true)
                            loadedSettings.StartDatumDruck = dt;
                    }

                    e = configs.SelectSingleNode("EndeDatumDruck");
                    if (e != null)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(e.InnerText, out dt) == true)
                            loadedSettings.EndeDatumDruck = dt;
                    }

                    e = configs.SelectSingleNode("MitarbeiterDisplayWithBezeichnung");
                    if (e != null)
                    {
                        bool b;
                        if (bool.TryParse(e.InnerText, out b))
                            loadedSettings.MitarbeiterDisplayWithBezeichnung = b;
                    }

                    e = configs.SelectSingleNode("MitarbeiterDisplayWithName");
                    if (e != null)
                    {
                        bool b;
                        if (bool.TryParse(e.InnerText, out b))
                            loadedSettings.MitarbeiterDisplayWithName = b;
                    }

                    e = configs.SelectSingleNode("MitarbeiterDisplayWithVorname");
                    if (e != null)
                    {
                        bool b;
                        if (bool.TryParse(e.InnerText, out b))
                            loadedSettings.MitarbeiterDisplayWithVorname = b;
                    }

                    e = configs.SelectSingleNode("ShowKolonnePrefix");
                    if (e != null)
                    {
                        bool b;
                        if (bool.TryParse(e.InnerText, out b))
                            loadedSettings.ShowKolonnePrefix = b;
                    }

                    e = configs.SelectSingleNode("DayBrush");
                    if (e != null)
                    {
                        string brushString = e.InnerText;
                        loadedSettings.DayBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(brushString);
                    }

                    e = configs.SelectSingleNode("SeitenAbstandX");
                    if (e != null)
                    {
                        int i;
                        if (int.TryParse(e.InnerText, out i) == true)
                            loadedSettings.Print.PageOffsetX = i;
                    }

                    e = configs.SelectSingleNode("SeitenAbstandY");
                    if (e != null)
                    {
                        int i;
                        if (int.TryParse(e.InnerText, out i) == true)
                            loadedSettings.Print.PageOffsetY = i;
                    }
                }
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("Fehler beim Laden der Configs", exp.Message);
            }
            return loadedSettings;
        }
    }
}