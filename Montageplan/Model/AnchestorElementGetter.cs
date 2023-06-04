using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Montageplan.Model
{
    /// <summary>
    /// Sucht ein Elternobjekt des übergebenen Typs.
    /// </summary>
    public static class AnchestorElementGetter
    {
        /// <summary>
        /// Sucht ein Elternobjekt des übergebenen Typs. Wenn der Typ übereinstimmt, wird das Elternobjekt zurückgegeben.
        /// </summary>
        /// <typeparam name="T">Der zu suchende Typ</typeparam>
        /// <param name="child">Kinderobjekt</param>
        /// <returns></returns>
        public static T FindAnchestor<T>(DependencyObject child) where T : DependencyObject
        {
            T parent = null;
            DependencyObject tempChild = child;
            do
            {
                if (tempChild is T)
                {
                    parent = (T)tempChild;
                    break;
                }
                else
                {
                    tempChild = VisualTreeHelper.GetParent(tempChild);
                    if (tempChild == null)
                        break;
                }
            }
            while (parent == null);

            return parent;
        }

    }
}
