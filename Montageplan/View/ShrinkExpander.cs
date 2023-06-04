using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Montageplan.View
{
    public class ShrinkExpander : Expander
    {
        private void RotateHeaderHorizontal()
        {
            this.LayoutTransform = null;
        }

        private void RotateHeaderVerticalSmall()
        {
            if (this.ActualHeight >= 1 && this.ActualWidth >= 1)
            {
                double height = this.ActualHeight / 2;
                double width = this.ActualWidth / 2;
                RotateTransform rotate = new RotateTransform(-90, width, height);
                this.LayoutTransform = rotate;
            }
        }

        // EventHandler

        protected override void OnCollapsed()
        {
            base.OnCollapsed();
            this.RotateHeaderVerticalSmall();
        }

        protected override void OnExpanded()
        {
            base.OnExpanded();
            this.RotateHeaderHorizontal();
        }



    }
}
