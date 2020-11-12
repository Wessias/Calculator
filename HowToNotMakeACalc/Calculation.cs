using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;

namespace HowToNotMakeACalc
{

    class Calculation
    {
        private readonly Regex sc_allowedInputs = new Regex("[^0-9./+*√(^)-]");

        //Checks if text is allowed with the help of a regular expression.
        public bool IsTextAllowed(string text)
        {
            return sc_allowedInputs.IsMatch(text);
        }

        public string EvaluateExpression(string expression)
        {
            var splitExpression = SplitExpressionOnNonNumber(expression);
            if (DoesCharExistTwiceInElement(splitExpression, '.'))
            {
                MessageBox.Show("Invalid Data: Too many decimal signs");
                return "";
            }
            test(splitExpression);


            return "1";
        }

        public bool DoesCharExistTwiceInElement(List<String> list, char c)
        {
            
            foreach(String text in list) {
                if (text.Contains(c)){
                    char[] splitText = text.ToCharArray();
                    for (var i = 0; i < splitText.Length - 1; i++)
                    {
                        if (splitText[i] == c)
                        {
                            for (var j = i+1;j < splitText.Length - 1; j++)
                            {
                                if (splitText[i] == splitText[j])
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public List<String> EvaluateBrackets(List<String> splitExpression)
        {
            for (var i = 0; i < splitExpression.Count - 1; i++)
            {
                if (0==0)
                {

                }
            }

            return splitExpression;
        }



        public void test(List<String> list)
        {
            foreach(string text in list)
            {
                MessageBox.Show(text);
            }
        }



        public List<String> SplitExpressionOnNonNumber(string expression)
        {
            //char[] charToSplitOn = { '/', '+', '*', '√', '(', '^', ')', '-' };
            //Regex toSplitOn = new Regex(@"([*()\^\/\√]|[\+\-])");
            var splitExpression = Regex.Split(expression, @"([*()\^\/\√]|[\+\-])").ToList();
            return splitExpression;
        }











        class Addition : Operator
        {
            public override BigInteger Operation(BigInteger num1, BigInteger num2)
            {
                var result = num1 + num2;
                return result;

            }

        }

        class Multiplication : Operator
        {
            public override BigInteger Operation(BigInteger num1, BigInteger num2)
            {
                var result = num1 * num2;
                return result;
            }

        }

        class Exponentiation : Operator
        {
            public override BigInteger Operation(BigInteger num1, BigInteger num2)
            {
                var result = Math.Pow((double)num1, (double)num2);
                return (BigInteger)result;
            }

        }


    }
}
