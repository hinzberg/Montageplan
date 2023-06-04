using Montageplan.Model;
using Montageplan.View;
using Montageplan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Montageplan.Presenter
{
    public class CalendarMitarbeiterPresenter
    {
        private readonly CalendarMitarbeiterView view;
        private readonly DateTime day;
        private List<DayMitarbeiterViewModel> dayMitarViewModels;

        public CalendarMitarbeiterPresenter(CalendarMitarbeiterView view, DateTime d)
        {
            this.view = view;
            this.day = d;
            this.view.mitarbeiterListBox.AddHandler(
                ScrollBar.PreviewMouseMoveEvent, new MouseEventHandler(this.ScrollBar_PreviewMouseMove), true);
        }

        public void SetMitarbeiter(IEnumerable<DayMitarbeiter> mitarbeiterDayLinks)
        {
            this.dayMitarViewModels = DayMitarbeiterViewModel.CreateViewModels(mitarbeiterDayLinks, this.day);
            this.UpdateView();
        }

        public void UpdateView()
        {
            this.view.mitarbeiterListBox.ItemsSource = null;
            this.view.mitarbeiterListBox.ItemsSource = this.dayMitarViewModels;
        }

        public DayMitarbeiterViewModel GetDayMitarbeiter(int mitarbeiterId)
        {
            DayMitarbeiterViewModel matchedViewModel = null;
            foreach (var viewModel in this.dayMitarViewModels)
            {
                if (viewModel.GetModel().Mitarbeiter.Id == mitarbeiterId)
                {
                    matchedViewModel = viewModel;
                    break;
                }
            }
            return matchedViewModel;
        }

        private void ScrollBar_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Klappt nicht!

            // Verhindert, dass der DragDrop Helper den Event der DragSource 'PreviewMouseMove' aufruft.
            // Hierbei wird verhindert, dass man ausversehen draggt, während man eigentlich die ScrollBar ziehen wollte
          //  e.Handled = true;
        }
    }
}
