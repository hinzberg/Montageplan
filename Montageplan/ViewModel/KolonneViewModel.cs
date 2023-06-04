using System.Windows.Media;
using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class KolonneViewModel : NotificationModel
    {
        public static List<KolonneViewModel> CreateViewModels(IEnumerable<Kolonne> kolonnen)
        {
            List<KolonneViewModel> viewModels = new List<KolonneViewModel>();
            foreach (var kol in kolonnen)
                viewModels.Add(new KolonneViewModel(kol));

            return viewModels;
        }


        private readonly Kolonne model;

        public KolonneViewModel(Kolonne model)
        {
            this.model = model;
        }

        public string Nummer
        {
            get { return this.model.Nummer; }
        }

        public string Bezeichnung
        {
            get { return this.model.Bezeichnung; }
        }

        public string FuehrerFullName
        {
            get { return this.model.Fuehrer.GetFullName(); }
        }

        public string FirstMitarbeiterFullName
        {
            get { return "-"; }
        }

        public SolidColorBrush KolonneColorBrush
        {
            get { return this.model.KolonneColorBrush; }
        }

        public Kolonne GetModel()
        {
            return this.model;
        }

    }
}
