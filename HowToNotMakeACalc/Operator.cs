using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Controls;

namespace HowToNotMakeACalc
{
    class Operator
    {

        public virtual BigInteger Operation(BigInteger num1, BigInteger num2)
        {
            return 0;
        }
    }
}
