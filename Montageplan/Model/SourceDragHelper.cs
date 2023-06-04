using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Montageplan.Model
{
    public abstract class SourceDragHelper
    {
        private static readonly List<SourceDragHelper> helpers = new List<SourceDragHelper>();
        public static void AddHelper(SourceDragHelper helper)
        {
            helpers.Add(helper);
        }
        public static void DisposeHelpers()
        {
            foreach (var helper in helpers)
                helper.Dispose();
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref PointUser32 lpPoint);
        public struct PointUser32
        {
            public int X;
            public int Y;
        }


        private readonly UIElement source;
        private readonly DataTemplate adornerTemplate;
        private DragAdorner adorner = null;

        public SourceDragHelper(UIElement source, DataTemplate adornerTemplate)
        {
            this.source = source;
            this.source.PreviewMouseMove += Source_PreviewMouseMove;
            this.source.PreviewGiveFeedback += Source_PreviewGiveFeedback;
            this.adornerTemplate = adornerTemplate;
            this.DragDropDataName = "Data";
        }
        ~SourceDragHelper()
        {
            this.Dispose();
        }

        public string DragDropDataName { get; set; }

        /// <summary>
        /// Holt die Daten von der OriginalSource, die per DragDrop an das Drop-Ziel übergeben werden soll.
        /// </summary>
        /// <param name="originalSource">OriginalSource vom Drag-Vorgang</param>
        /// <returns></returns>
        protected abstract object GetDataForDragDrop(object originalSource);
        /// <summary>
        /// Holt die Daten, die an dem DragAdorner gebunden und angezeigt werden sollen.
        /// </summary>
        /// <returns></returns>
        protected abstract object GetDataForAdornerTemplate();


        public void Dispose()
        {
            this.source.PreviewMouseMove -= Source_PreviewMouseMove;
            this.source.PreviewGiveFeedback -= Source_PreviewGiveFeedback;
        }

        private void DestroyAdorner()
        {
            if (this.adorner != null)
            {
                this.adorner.Destroy();
                this.adorner = null;
            }
        }

        // EventHandler

        /// <summary>
        // Handler wird aufgerufen: 
        // a) Permanent bei Bewegungen mit der Maus, Maustaste nicht gedrückt
        // b) Einmalig, wenn die Maustaste gedrückt wurde. Der Handler wird erst wieder aufgerufen, wenn die Maustaste losgelassen wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Source_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Hier nur bei Linksklick (gedrückt) reinspringen
                object draggedData = this.GetDataForDragDrop(e.OriginalSource);
                if (draggedData != null)
                {
                    // Drag&Drop wird eingeleitet
                    DataObject dragData = new DataObject(this.DragDropDataName, draggedData);
                    DragDropEffects effect = DragDrop.DoDragDrop(this.source, dragData, DragDropEffects.Move); // Drag&Drop starten
                    // Wenn der Drag&Drop-Vorgang abgeschlossen ist (egal mit welchem Ergebnis), dann den Adorner entfernen
                    this.DestroyAdorner();
                }
            }
        }

        /// <summary>
        /// Drag wird gestartet ('DoDragDrop' wurde aufgerufen). Dieser Handler wird ständig aufgerufen,
        /// während Daten gedraggt wurden und die linke Maustaste gedrückt bleibt (auch ohne Mausbewegung). 
        /// Hier wird der Adorner erstellt und angezeigt.
        /// Falls kein AdornerTemplate übergeben wurde, ist dieser EventHandler unwichtig.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Source_PreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (this.adornerTemplate != null)
            {
                object dataForAdorner = this.GetDataForAdornerTemplate();
                if (dataForAdorner != null)
                {
                    if (this.adorner == null)
                        this.adorner = new DragAdorner(dataForAdorner, this.adornerTemplate, this.source, AdornerLayer.GetAdornerLayer(this.source));

                    // Hier wird die Mausposition ermittelt und dem Adorner übergeben, damit der Adorner an dem Cursor "hängen" bleibt.
                    // Zur Hilfe musste hier eine DllImport-Methode benutzt werden.
                    PointUser32 p = new PointUser32();
                    GetCursorPos(ref p);
                    Point point = new Point(p.X, p.Y);
                    point = this.source.PointFromScreen(point);

                    this.adorner.UpdatePosition(point.X, point.Y);
                }
            }
        }


    }
}
