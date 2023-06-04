using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class DayKolonneViewModel
    {
        public static List<KolonneViewModel> CreateViewModels(IEnumerable<Kolonne> kolonnen)
        {
            List<KolonneViewModel> viewModels = new List<KolonneViewModel>();
            foreach (var kol in kolonnen)
                viewModels.Add(new KolonneViewModel(kol));

            return viewModels;
        }

        private readonly Engagement model;
        private readonly KolonneViewModel kolonne;

        public DayKolonneViewModel(Engagement model)
        {
            this.model = model;
            this.kolonne = new KolonneViewModel(this.model.Kolonne);
        }

        public KolonneViewModel Kolonne
        {
            get { return this.kolonne; }
        }



    }
}
