using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Montageplan.Model
{
    public class ListBoxDragHelper : SourceDragHelper
    {
        private object data;

        public ListBoxDragHelper(UIElement source, DataTemplate adornerTemplate)
            : base(source, adornerTemplate)
        {
            this.data = null;
        }

        protected override object GetDataForDragDrop(object originalSource)
        {
            // Drag & Drop erlauben ?
            if (AppConfig.UserCanModify)
            {
                ListBoxItem item;
                item = AnchestorElementGetter.FindAnchestor<ListBoxItem>((DependencyObject)originalSource);
                if (item != null)
                    data = item.Content;
            }
            return data;
        }

        protected override object GetDataForAdornerTemplate()
        {
            return this.data;
        }

    }
}
