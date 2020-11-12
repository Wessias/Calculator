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
        private readonly Addition sc_add = new Addition();
        private readonly Subtraction sc_sub = new Subtraction();
        private readonly Multiplication sc_mult = new Multiplication();
        private readonly Division sc_div = new Division();
        private readonly Exponentiation sc_expo = new Exponentiation();
        private readonly SquareRoot sc_sqrt = new SquareRoot();
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
            while(DoesListContainChar(splitExpression, '('))
            {
                splitExpression = EvaluateBrackets(splitExpression);
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






        public bool DoesListContainChar(List<String> list, char c)
        {
            foreach(String text in list)
            {
                if (text.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        public List<String> RemoveSetAmountOfElementsInListAtIndex(List<String> list, int startElement, int amountToRemove)
        {
            list.RemoveRange(startElement, startElement + amountToRemove);
            return list;
        }


        //Does the operation inside the bracket or sqrt/power of, cba doing brackets inside of brackets, only works with one operation inside the bracket.
        public List<String> EvaluateBrackets(List<String> splitExpression)
        {
            for (var i = 0; i < splitExpression.Count - 1; i++)
            {
                if (splitExpression[i] == "(")
                {
                        if (splitExpression[i + 2] == ")" || splitExpression[i+4] == ")")
                        {
                            switch (splitExpression[i + 2])
                            {
                                ///+*√(^)-
                                case "+":
                                    var temp =sc_add.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, i + 4);
                                    splitExpression.Insert(i, temp.ToString());
                                break;
                                case "-":
                                    var temp1 =sc_sub.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, i + 4);
                                    splitExpression.Insert(i, temp1.ToString());
                                break;
                                case "*":
                                    var temp2 =sc_mult.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, i + 4);
                                    splitExpression.Insert(i, temp2.ToString());
                                break;
                                case "/":
                                    var temp3 = sc_div.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, i + 4);
                                    splitExpression.Insert(i, temp3.ToString());
                                break;

                                case ")":
                                    if (splitExpression[i - 1] == "√")
                                    {
                                        var temp4 =sc_sqrt.Operation(Convert.ToDouble(splitExpression[i + 1]), (1 / 2));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, i + 2);
                                    splitExpression.Insert(i, temp4.ToString());
                                    }
                                    else if (splitExpression[i - 1] == "^")
                                    {
                                        var temp5 = sc_expo.Operation(Convert.ToDouble(splitExpression[i - 2]), Convert.ToDouble(splitExpression[i+1]));
                                        RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, i + 2);
                                        splitExpression.Insert(i, temp5.ToString());
                                    }
                                    else if (splitExpression[i - 1 ] != "+" || splitExpression[i - 1] != "-" || splitExpression[i - 1] != "/")
                                    {
                                    var temp6 = splitExpression[i + 1];
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, i + 2);
                                    splitExpression.Insert(i, temp6.ToString());
                                    splitExpression.Insert(i, "*");
                                }
                                break;
                                default:
                                break;
                            }
                        }
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
            public override double Operation(double num1, double num2)
            {
                var result = num1 + num2;
                return result;

            }

        }

        class Subtraction : Addition
        {
            public override double Operation(double num1, double num2)
            {
                return base.Operation(num1, -num2);
            }
        }

        class Multiplication : Operator
        {
            public override double Operation(double num1, double num2)
            {
                var result = num1 * num2;
                return result;
            }

        }

        class Division : Multiplication
        {
            public override double Operation(double num1, double num2)
            {
                return base.Operation(num1, (1 / (num2)));
            }
        }

        class Exponentiation : Operator
        {
            public override double Operation(double num1, double num2)
            {
                var result = Math.Pow(num1, num2);
                return result;
            }

        }

        class SquareRoot : Exponentiation
        {
            public override double Operation(double num1, double num2)
            {
                return base.Operation(num1, (1 / 2));
            }
        }


    }
}
