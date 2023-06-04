using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class MitarbeiterChooseViewModel : MitarbeiterViewModel
    {
        /// <summary>
        /// Erstellt eine Liste von 'MitarbeiterChooseViewModels'. Alle Mitarbeiter, die bereits einer bestimmten Kolonne zugeordnet sind
        /// ('kolonneMitarbeiter'), werden "angehakt".
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiterliste</param>
        /// <param name="kolonneMitarbeiter">Mitarbeiter einer bestimmen Kolonne</param>
        /// <returns></returns>
        public static List<MitarbeiterChooseViewModel> CreateChooseViewModels(
            IEnumerable<Mitarbeiter> mitarbeiter, IEnumerable<Mitarbeiter> kolonneMitarbeiter)
        {
            List<MitarbeiterChooseViewModel> viewModels = new List<MitarbeiterChooseViewModel>();
            foreach (var mitItem in mitarbeiter)
            {
                bool isChecked = false;
                foreach (var kolMitItem in kolonneMitarbeiter)
                {
                    if (kolMitItem.Id == mitItem.Id)
                    {
                        isChecked = true;
                        break;
                    }
                }
                viewModels.Add(new MitarbeiterChooseViewModel(mitItem, isChecked));
            }
            return viewModels;
        }


        private readonly Mitarbeiter model;
        private readonly bool isChecked;

        public MitarbeiterChooseViewModel(Mitarbeiter model, bool isChecked)
            : base(model)
        {
            this.model = model;
            this.isChecked = isChecked;
            this.IsChecked = isChecked;
        }

        public bool IsChecked { get; set; }

        public bool HasChanges()
        {
            bool hasChanges = (this.IsChecked != this.isChecked);
            return hasChanges;
        }

        public Mitarbeiter GetModel()
        {
            return this.model;
        }

    }
}
