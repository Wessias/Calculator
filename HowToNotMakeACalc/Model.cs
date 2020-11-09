using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HowToNotMakeACalc
{

    class Model
    {
        private readonly Regex sc_allowedInputs = new Regex("[^0-9./=+*√(^)-]");

        public bool IsTextAllowed(string text)
        {
            return sc_allowedInputs.IsMatch(text);
        }
    }

    class Addition : Operator
    {


    }

    class Multiplication : Operator
    {

    }

    class Exponentiation : Operator
    {

    }

    class Equals : Operator
    {

    }

}
