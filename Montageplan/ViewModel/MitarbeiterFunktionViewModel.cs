using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class MitarbeiterFunktionViewModel : NotificationModel
    {
        public static List<MitarbeiterFunktionViewModel> CreateViewModels(IEnumerable<MitarbeiterFunktion> models)
        {
            List<MitarbeiterFunktionViewModel> viewModels = new List<MitarbeiterFunktionViewModel>();
            foreach (var model in models)
                viewModels.Add(new MitarbeiterFunktionViewModel(model));

            return viewModels;
        }


        private MitarbeiterFunktion model;

        public MitarbeiterFunktionViewModel(MitarbeiterFunktion model)
        {
            this.model = model;
        }

        public string Name
        {
            get { return this.model.Name; }
        }

        public MitarbeiterFunktion GetModel()
        {
            return this.model;
        }


    }
}
