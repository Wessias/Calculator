using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Controls;

namespace HowToNotMakeACalc
{
    class Operator
    {

        public virtual double Operation(double num1, double num2)
        {
            return 0;
        }
    }
}
