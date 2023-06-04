using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Montageplan.Model
{
    /// <summary>
    /// Holt das UIElement mit dem angegebenen Namen. Das UIElement wird im ParentControl gesucht.
    /// Wird z. B. nötig, wenn man die Referenz auf ein MenuItem vom ContextMenu in einem ListView erhalten möchte.
    /// </summary>
    public class SubElementGetter
    {
        /// <summary>
        /// Holt das UIElement abhängig vom übergebenen Namen. Das UIElement wird in einer der Elternobjekte gesucht.
        /// Es muss sich nicht direkt um das Elternobjekt handelt. Das gefundene UIElement wird zurückgegeben.
        /// </summary>
        /// <param name="name">Name vom UIElement</param>
        /// <param name="parentUIObject">Elternobjekt</param>
        /// <returns></returns>
        public static UIElement Get(string name, DependencyObject parentUIObject)
        {
            UIElement element = null;
            try
            {
                element = Window.GetWindow(parentUIObject).FindName(name) as UIElement;
            }
            catch (Exception)
            {
            }
            return element;
        }


    }
}
