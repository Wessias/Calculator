using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HowToNotMakeACalc
{

    class Model
    {
        private Regex sc_AllowedInputs = new Regex("[0-9./=+*√(^)-]");

        public bool IsTextAllowed(string text)
        {
            return !sc_AllowedInputs.IsMatch(text);
        }
    }
}
