using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Calculator.WPF.WPFControls
{
    public class GraphControl : FrameworkElement
    {
        protected override void OnRender(DrawingContext dc) // idk why but it wouldnt work unelss i made it protected
        {
            base.OnRender(dc);


            double width = ActualWidth;
            double height = ActualHeight;

            dc.DrawRectangle(Brushes.White, null, new Rect(0,0, width, height));


        }


    }
}
