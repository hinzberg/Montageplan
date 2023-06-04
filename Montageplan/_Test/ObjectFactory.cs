using System;
using System.Collections.Generic;
using System.Windows.Media;
using Montageplan.Model;

namespace Montageplan._Test
{
    public static class ObjectFactory
    {
        public static List<Mitarbeiter> CreateMitarbeiterList(IList<MitarbeiterFunktion> funktionen)
        {
            List<Mitarbeiter> list = new List<Mitarbeiter>();
            list.Add(new Mitarbeiter("Norbert", "Mustermann", true));
            list.Add(new Mitarbeiter("Daniel", "Mustermann", true));
            list.Add(new Mitarbeiter("Sinan", "Mustermann", true));
            list.Add(new Mitarbeiter("Manuel", "Mustermann", true));
            list.Add(new Mitarbeiter("Josi", "Mustermann", true));
            list.Add(new Mitarbeiter("Timo", "Mustermann", false));
            list.Add(new Mitarbeiter("Alex", "Mustermann", true));
            list.Add(new Mitarbeiter("Giuseppe", "Mustermann", true));
            list.Add(new Mitarbeiter("Pascal", "Mustermann", true));
            list.Add(new Mitarbeiter("Ricardo", "Mustermann"));
            list.Add(new Mitarbeiter("David", "Mustermann"));
            list.Add(new Mitarbeiter("Maui", "Mustermann"));
            list.Add(new Mitarbeiter("Franc", "Mustermann"));
            list.Add(new Mitarbeiter("Felipe", "Mustermann"));

            int funkIndex = 0;
            foreach (var item in list)
            {
                if (funkIndex < funktionen.Count - 1)
                    funkIndex++;
                else
                    funkIndex = 0;

                item.Funktion = funktionen[funkIndex];
            }

            return list;
        }

        public static List<Projekt> CreateProjektListe()
        {
            List<Projekt> list = new List<Projekt>();

            Projekt werkstatt = new Projekt("Werkstatt");
            werkstatt.ProjectColorBrush = new SolidColorBrush(Color.FromArgb(80, 255, 255, 120));
            list.Add(werkstatt);

            Projekt elektro = new Projekt("Elektroverkabelung");
            elektro.Startdatum = DateTime.Now.AddDays(-20);
            elektro.Endedatum = DateTime.Now.AddDays(20);
            elektro.ProjectColorBrush = new SolidColorBrush(Color.FromArgb(80, 200, 255, 120));
            list.Add(elektro);

            Projekt rohre = new Projekt("Heizung & Sanitär");
            rohre.Startdatum = DateTime.Now;
            rohre.Endedatum = DateTime.Now.AddDays(50);
            rohre.ProjectColorBrush = new SolidColorBrush(Color.FromArgb(80, 125, 120, 255));
            list.Add(rohre);

            Projekt innen = new Projekt("Innenausbau");
            innen.Startdatum = DateTime.Now.AddDays(-50);
            innen.Endedatum = DateTime.Now.AddDays(50);
            innen.ProjectColorBrush = new SolidColorBrush(Color.FromArgb(80, 255, 80, 80));
            list.Add(innen);

            Projekt ende = new Projekt("Endmontage");
            ende.Startdatum = DateTime.Now.AddDays(-50);
            ende.Endedatum = DateTime.Now.AddDays(80);
            ende.ProjectColorBrush = new SolidColorBrush(Color.FromArgb(80, 80, 180, 80));
            list.Add(ende);

            return list;
        }

        public static List<MitarbeiterFunktion> CreateFunktionen()
        {
            List<MitarbeiterFunktion> funktionen = new List<MitarbeiterFunktion>();
            funktionen.Add(new MitarbeiterFunktion("Meister"));
            funktionen.Add(new MitarbeiterFunktion("Elektriker"));
            funktionen.Add(new MitarbeiterFunktion("Schlosser"));
            funktionen.Add(new MitarbeiterFunktion("Schrauber"));
            funktionen.Add(new MitarbeiterFunktion("Fliesenleger"));
            return funktionen;
        }

        public static List<Kolonne> CreateKolonnenListe()
        {
            List<Kolonne> list = new List<Kolonne>();
            list.Add(new Kolonne("#A11", "Statik", new Mitarbeiter()));
            list.Add(new Kolonne("#B_22", "Verkabelungen", new Mitarbeiter()));
            return list;
        }

        public static List<User> CreateUsers(IList<User> user)
        {
            List<User> list = new List<User>();
            list.Add(new User("Holger", "xxx", true));
            list.Add(new User("Felipe", "xxx", false));
            list.Add(new User("Andreas", "xxx", true));
            list.Add(new User("Mike", "xxx", false));
            return list;
        }
    }
}