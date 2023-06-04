using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class Repository<T> where T : class
    {
        public class UniformResult<T>
        {
            public UniformResult()
            {
                this.NewItems = new List<T>();
                this.DeletedItems = new List<T>();
            }

            public List<T> NewItems { get; set; }
            public List<T> DeletedItems { get; set; }
        }


        private readonly List<T> items;

        public Repository()
        {
            this.items = new List<T>();
        }

        public virtual bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public virtual void ClearAll()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Überprüft die übergebene Auflistung, ob neue Items (unbekannte) übergeben wurden und fügt sie
        /// der Repository hinzu. Zusätzlich wird überprüft, ob Items in der Auflistung nicht mehr vorhanden sind,
        /// diese werden aus der Repository entfernt. Ergebnis ist, das die Repository die gleichen Items
        /// beinhaltet, wie die übergebene Auflistung. Zusätzlich wird das Vergleichsergebnis zurückgegeben, 
        /// wo neue  und entfernt Einträge als Information vorhanden sind.
        /// </summary>
        /// <param name="collection">Auflistung</param>
        /// <returns>Vergleichsergebnis (neue und entfernte Items)</returns>
        public virtual UniformResult<T> UniformToCollection(IEnumerable<T> collection)
        {
            UniformResult<T> result = new UniformResult<T>();
            List<T> currentItems = new List<T>(this.items);
            // Überprüfen, ob Items entfernt wurden
            foreach (var currItem in currentItems)
            {
                if (!collection.Contains(currItem))
                {
                    this.Remove(currItem);
                    result.DeletedItems.Add(currItem);
                }
            }
            // Überprüfen, ob Items neu (noch unbekannt) sind
            foreach (var item in collection)
            {
                if (!currentItems.Contains(item))
                {
                    this.Add(item);
                    result.NewItems.Add(item);
                }
            }
            return result;
        }

        public virtual void SetCollection(IEnumerable<T> collection)
        {
            this.items.Clear();
            this.items.AddRange(collection);
        }

        public virtual void Insert(int index, T item)
        {
            this.items.Insert(index, item);
        }

        public virtual void AddRange(IEnumerable<T> collection)
        {
            this.items.AddRange(collection);
        }

        public virtual void Add(T item)
        {
            this.items.Add(item);
        }

        public virtual void RemoveRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                this.items.Remove(item);
            }
        }

        public virtual void Remove(T item)
        {
            this.items.Remove(item);
        }

        public virtual int GetCount()
        {
            int count = this.items.Count;
            return count;
        }

        public virtual List<T> GetAll()
        {
            List<T> allItems = new List<T>(this.items);
            return allItems;
        }

        public virtual T GetItemAt(int index)
        {
            T item = this.items[index];
            return item;
        }

    }
}
