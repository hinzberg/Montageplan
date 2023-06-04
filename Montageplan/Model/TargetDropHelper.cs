using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Montageplan.Model
{
    public abstract class TargetDropHelper
    {
        private static readonly List<TargetDropHelper> helpers = new List<TargetDropHelper>();
        public static void AddHelper(TargetDropHelper helper)
        {
            helpers.Add(helper);
        }
        public static void DisposeHelpers()
        {
            foreach (var helper in helpers)
                helper.Dispose();
        }


        public delegate void DropCompletedHandler(object data);

        private readonly UIElement target;

        public TargetDropHelper(UIElement target)
        {
            this.target = target;
            this.target.PreviewDragEnter += Target_PreviewDragEnter;
            this.target.PreviewDragOver += Target_PreviewDragOver;
            this.target.PreviewDrop += Target_PreviewDrop;
            this.DropCompleted = null;
            this.DragDropDataName = "Data";
        }
        ~TargetDropHelper()
        {
            this.Dispose();
        }

        public DropCompletedHandler DropCompleted { get; set; }
        public string DragDropDataName { get; set; }

        /// <summary>
        /// Dürfen die Daten auf dem Zielobjekt losgelassen werden ?
        /// </summary>
        /// <param name="draggedData"></param>
        /// <returns></returns>
        protected abstract bool IsDropAllowed(object draggedData);

        public void Dispose()
        {
            this.target.PreviewDragEnter -= Target_PreviewDragEnter;
            this.target.PreviewDragOver -= Target_PreviewDragOver;
            this.target.PreviewDrop -= Target_PreviewDrop;
        }

        private void CheckDropAllowedForEventHandler(DragEventArgs e)
        {
            object draggedData = e.Data.GetData(this.DragDropDataName);
            if (draggedData != null && this.IsDropAllowed(draggedData))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        // EventHandler

        /// <summary>
        /// Überprüfen, ob hier an dem Zielobjekt Daten abgelegt werden dürfen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Target_PreviewDragOver(object sender, DragEventArgs e)
        {
            this.CheckDropAllowedForEventHandler(e);
        }

        /// <summary>
        /// Überprüfen, ob hier an dem Zielobjekt Daten abgelegt werden dürfen. Wenn dieser Handler nicht benutzt wird,
        /// dann flackert beim Ziehen eines Objekts manchmal das Symbol, das nicht gedraggt werden darf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Target_PreviewDragEnter(object sender, DragEventArgs e)
        {
            this.CheckDropAllowedForEventHandler(e);
        }

        /// <summary>
        /// Linke Maustaste über dem Zielobjekt losgelassen. Gültige Daten wurden über dem Zielobjekt abgelegt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Target_PreviewDrop(object sender, DragEventArgs e)
        {
            object draggedData = e.Data.GetData(this.DragDropDataName);
            if (draggedData != null)
            {
                if (this.DropCompleted != null)
                    this.DropCompleted(draggedData);
            }
        }

    }
}
