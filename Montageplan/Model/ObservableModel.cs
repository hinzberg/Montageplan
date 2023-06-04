using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class ObservableModel
    {
        private List<Action> observers;

        public ObservableModel()
        {
            this.observers = null;
        }

        public void AppendObserver(Action observer)
        {
            if (this.observers == null)
                this.observers = new List<Action>();
            this.observers.Add(observer);
        }

        public void RemoveObserver(Action observer)
        {
            if (this.observers != null)
                this.observers.Remove(observer);
        }

        public void ClearObservers()
        {
            if (this.observers != null)
                this.observers.Clear();
        }

        public void NotifyObservers()
        {
            if (this.observers != null)
            {
                foreach (var observer in this.observers)
                    observer();
            }
        }

    }
}
