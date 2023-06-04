using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Montageplan.View
{
    /// <summary>
    /// Diese Klasse ist ein erweiteres horizontales StackPanel, wo die Breite der Items je nach Option gesetzt werden
    /// </summary>
    public class CalendarDaysHorizontalStackPanel : StackPanel
    {
        /// <summary>
        /// Die Referenzen vom ItemsStackPanel werden gesammelt, um leichter auf die Objekte (als ItemsPanel der Kalendertage-ListBoxen) zugreifen zu können.
        /// </summary>
        private static List<CalendarDaysHorizontalStackPanel> instances = new List<CalendarDaysHorizontalStackPanel>();
        /// <summary>
        /// Information, dass das StackPanel erzeugt wurde, weil die Items gebunden wurden
        /// </summary>
        public static Action PanelCreated = null;
        public static void SetAllItemsAlignment(DaysAlignment alignment)
        {
            foreach (var item in instances)
                item.ItemsAlignment = alignment;
        }
        public static void UpdateAllItemsWidth()
        {
            foreach (var item in instances)
                item.UpdateItemsWidth();
        }


        public CalendarDaysHorizontalStackPanel()
        {
            this.Orientation = System.Windows.Controls.Orientation.Horizontal;
            this.ItemsAlignment = DaysAlignment.FixedWidth;

            this.SizeChanged += CalenderDaysHorizontalStackPanel_SizeChanged;

            instances.Add(this);
            if (PanelCreated != null)
                PanelCreated();
        }

        public DaysAlignment ItemsAlignment { get; set; }

        public void UpdateItemsWidth()
        {
            List<ListBoxItem> items = this.GetChildrenItems();
            if (items.Count > 0)
            {
                if (this.ItemsAlignment == DaysAlignment.FixedWidth)
                    this.SetFixedWidth(items);
                else if (this.ItemsAlignment == DaysAlignment.DynamicWidth)
                    this.SetDynamicWidth(items);
            }
        }

        private List<ListBoxItem> GetChildrenItems()
        {
            List<ListBoxItem> items = new List<ListBoxItem>();
            foreach (var child in base.Children)
            {
                if (child is ListBoxItem)
                    items.Add(child as ListBoxItem);
            }

            return items;
        }

        private void SetFixedWidth(IList<ListBoxItem> items)
        {
            foreach (var item in items)
                item.Width = AppConfig.FIXED_DAY_ITEMS_WIDTH;
        }

        private void SetDynamicWidth(IList<ListBoxItem> items)
        {
            double width = (this.ActualWidth / (double)items.Count) - 1; // -1 = Damit die horizontale ScrollBar nicht angezeigt wird
            if (width < AppConfig.MIN_DAY_ITEMS_WIDTH)
                width = AppConfig.MIN_DAY_ITEMS_WIDTH;

            foreach (var item in items)
                item.Width = width;
        }

        // EventHandler

        void CalenderDaysHorizontalStackPanel_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            this.UpdateItemsWidth();
        }






    }
}
