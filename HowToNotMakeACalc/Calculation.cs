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
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private readonly Addition sc_add = new Addition();
        private readonly Subtraction sc_sub = new Subtraction();
        private readonly Multiplication sc_mult = new Multiplication();
        private readonly Division sc_div = new Division();
        private readonly Exponentiation sc_expo = new Exponentiation();
        private readonly SquareRoot sc_sqrt = new SquareRoot();
        private readonly Regex sc_allowedInputs = new Regex("[^0-9,/+*√(^)-]");
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Checks if text is allowed with the help of a regular expression.
        public bool IsTextAllowed(string text)
        {
            return sc_allowedInputs.IsMatch(text);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Dont enter only 1 kind of bracket it will fuck up. 
        //Exponentiation doesnt work with brackets and will only do the exponentiation/sqrt on the number to the right of it no adding/subtracting and shiet like dat ;(
        public string EvaluateExpression(string expression)
        {
            var splitExpression = SplitExpressionOnNonNumber(expression);
            if (DoesCharExistTwiceInElement(splitExpression, ','))
            {
                MessageBox.Show("Invalid Data: Too many decimal signs");
                return "";
            }

            while(DoesListContainChar(splitExpression, '('))
            {
                splitExpression = EvaluateBrackets(splitExpression);
            }

            splitExpression = RemoveEmptyStringInList(splitExpression);

            while (DoesListContainChar(splitExpression, '^') | DoesListContainChar(splitExpression, '√'))
            {
                splitExpression = EvaluateExponentiationAndRoots(splitExpression);
            }

            splitExpression = RemoveEmptyStringInList(splitExpression);

            while (DoesListContainChar(splitExpression, '*') | DoesListContainChar(splitExpression, '/'))
            {
                splitExpression = EvaluateMultiplicationAndDivision(splitExpression);
            }

            splitExpression = RemoveEmptyStringInList(splitExpression);

            while (DoesListContainChar(splitExpression, '+') | DoesListContainChar(splitExpression, '-') && splitExpression.Count != 1)
            {
                splitExpression = EvaluateAdditionAndSubtraction(splitExpression);
            }

            splitExpression = RemoveEmptyStringInList(splitExpression);

            return splitExpression[0];
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public List<String> RemoveSetAmountOfElementsInListAtIndex(List<String> list, int startElement, int amountToRemove)
        {
            list.RemoveRange(startElement, amountToRemove);
            return list;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public List<String> RemoveEmptyStringInList(List<String> list)
        {

            list = list.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            return list;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------







        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //I know bracket doesnt fucking mean parenthesis but I couldnt be arsed writing parenthesis everytime and YES I realize I could just use the function to replace everywhere bracket is typed with parenthesis but now I've written this shit and refuse to do so. :)

        //Does the operation inside the bracket, couldn't get it to work with exponentiation and didn't bother to make it work for multiple operations inside the bracket.
        //if the input is a-(-b) or a+(+b) it faka up too. Works with multiple brackets as long as you don't multiply or divise those mofos. Can't multiply a bracket with a number either.
        //It is possible to take a bracket to the power of something correctly.

        //Works by finding where a bracket starts then checking if a closing bracket is 2 spaces ahead in the list or if its 4 spaces ahead.
        //If the closing bracket is two spaces ahead it means there is just a number inside the bracket like (a) if its 4 spaces ahead it means the bracket looks like (a+b) with random operation inside.
        //At first it enters a switch case which operates on what is 2 steps ahead of the opening bracket, which is either an operation or a closing bracket.
        //Then if it is an operator it does the calculation saves it in a variabel removes all of the elements used and inserts the calculated value at the same index as the opening bracket used to be. (Same as all the other evaluting shieet)
        //If the switch operator is a closing bracket it then enters an if statement which is true if the element behind the opening bracket is empty "" for some reason
        //when the original expression is split it leaves whitespace between the operator and bracket when the expression looks like a+(a) and I hadnt cared enough about it
        //to make the method which removed elements which were whitespace or empty at this point. If its not empty and instead a digit to the left in the form of a(b)
        //This is interpreted as a * b. If it is like mentioned an empty element behind the opening bracket and it looks like a+(b) it will simplify the expression to a+b.
        //Was gonna make it so you could do sqrt(a), a^(b+c) and a^(b) but couldn't be bothered which is why some code is marked as a comment.
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
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 5);
                                    splitExpression.Insert(i, temp.ToString());
                                break;
                                case "-":
                                    var temp1 =sc_sub.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 5);
                                    splitExpression.Insert(i, temp1.ToString());
                                break;
                                case "*":
                                    var temp2 =sc_mult.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 5);
                                    splitExpression.Insert(i, temp2.ToString());
                                break;
                                case "/":
                                    var temp3 = sc_div.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 5);
                                    splitExpression.Insert(i, temp3.ToString());
                                break;
                                case "^":
                                var temp69 = sc_expo.Operation(Convert.ToDouble(splitExpression[i + 1]), Convert.ToDouble(splitExpression[i + 3]));
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 5);
                                    splitExpression.Insert(i, temp69.ToString());
                                break;

                            case ")":

                                if (splitExpression[i - 1] == "")
                                {
                                    switch (splitExpression[i - 2])
                                    {
                                        //case "√":
                                            //var temp4 = sc_sqrt.Operation(Convert.ToDouble(splitExpression[i + 1]), (1/2));
                                            //MessageBox.Show(temp4.ToString() + "Hello");
                                           // RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 2, 4);
                                            //splitExpression.Insert(i, temp4.ToString());
                                            //return splitExpression;
                                        //case "^":
                                           // var temp5 = sc_expo.Operation(Convert.ToDouble(splitExpression[i - 3]), Convert.ToDouble(splitExpression[i + 1]));
                                         //   RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 3, 3);
                                          //  splitExpression.Insert(i, temp5.ToString());
                                          //  return splitExpression;
                                        case "+":
                                        case "-":
                                        case "/":
                                            var temp6 = splitExpression[i + 1];
                                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 3);
                                            splitExpression.Insert(i, temp6);
                                            return splitExpression;
                                        default:
                                            break;

                                    }
                                }
                                else {
                                    var temp7 = splitExpression[i + 1];
                                    RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 3);
                                    splitExpression.Insert(i, temp7.ToString());
                                    splitExpression.Insert(i, "*");
                                }
                                break;
                                default:
                                splitExpression.Clear();
                                break;
                            }
                        }
                }
            }

            return splitExpression;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------






        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Checks at which index the exponentiation or root operator is an element then does the operation with the help of the element beore and after. 
        //In the case of sqrt it doesnt need the element before since I only allow the square root.
        //Then it saves the calculated number in a variable and removes the operator sign, and the two (one in sqrt case) numbers used from the list.
        //Proccedes to insert the calculated temp variable into the list at index one step before the operator.
        public List<String> EvaluateExponentiationAndRoots(List<String> splitExpression)
        {
            for (var i = 0; i < splitExpression.Count - 1; i++)
            {
                if (splitExpression[i] == "^" | splitExpression[i] == "√")
                {
                    switch (splitExpression[i])
                    {
                        case "√":
                            var temp = sc_sqrt.Operation(Convert.ToDouble(splitExpression[i + 1]), 2);
                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i, 2);
                            splitExpression.Insert(i, temp.ToString());
                            break;
                        case "^":
                            var temp1 = sc_expo.Operation(Convert.ToDouble(splitExpression[i - 1]), Convert.ToDouble(splitExpression[i + 1]));
                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, 3);
                            splitExpression.Insert(i - 1, temp1.ToString());
                            break;
                        default:
                            splitExpression.Clear();
                            break;
                    }
                }
            }

            return splitExpression;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Finds the index the element matches the operator and then does the operation with the help of the elements before and after.
        //Saves the calculation of the expression in a temp variabel and then removes all the elemnts that we're used to calculate it.
        //Inserts the calculation one step before the index of the operator.
        public List<String> EvaluateMultiplicationAndDivision(List<String> splitExpression)
        {
            for (var i = 0; i < splitExpression.Count - 1; i++)
            {
                if (splitExpression[i] == "*" | splitExpression[i] == "/")
                {
                    switch (splitExpression[i])
                    {
                        case "*":
                            var temp = sc_mult.Operation(Convert.ToDouble(splitExpression[i - 1]), Convert.ToDouble(splitExpression[i + 1]));
                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, 3);
                            splitExpression.Insert(i - 1, temp.ToString());
                            break;
                        case "/":
                            var temp1 = sc_div.Operation(Convert.ToDouble(splitExpression[i - 1]), Convert.ToDouble(splitExpression[i + 1]));
                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, 3);
                            splitExpression.Insert(i - 1, temp1.ToString());
                            break;
                        default:
                            splitExpression.Clear();
                            break;
                    }
                }
            }



            return splitExpression;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Does the same thing as the other evaluate methods except with addition and subtraction.
        //If the expression has more than 1 subtraction in a row it fugs up i.e a-a-a-a = 0. I.e 500-50-50=400 but 500-50-50-50=450
        //If you write a negative sign as the first character it will fug up cause of the regex expression which will create a substring of it i.e -a will become {-}, {a} and crash. 
        public List<String> EvaluateAdditionAndSubtraction(List<String> splitExpression)
        {
            for (var i = 0; i < splitExpression.Count - 1; i++)
            {
                if (splitExpression[i] == "-" | splitExpression[i] == "+" )
                {
                    switch (splitExpression[i])
                    {
                        case "+":
                            var temp = sc_add.Operation(Convert.ToDouble(splitExpression[i - 1]), Convert.ToDouble(splitExpression[i + 1]));
                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, 3);
                            splitExpression.Insert(i - 1, temp.ToString());
                            break;
                        case "-":
                            var temp1 = sc_sub.Operation(Convert.ToDouble(splitExpression[i - 1]), Convert.ToDouble(splitExpression[i + 1]));
                            RemoveSetAmountOfElementsInListAtIndex(splitExpression, i - 1, 3);
                            splitExpression.Insert(i - 1, temp1.ToString());
                            break;       
                        default:
                            splitExpression.Clear();
                            break;
                    }
                }
            }
            return splitExpression;

        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void test(List<String> list)
        {
            foreach(string item in list)
            {
                MessageBox.Show(item);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Uses a regular expression to split the expression whenever it matches the pattern and then converts it into a list of strings.
        public List<String> SplitExpressionOnNonNumber(string expression)
        {
            //char[] charToSplitOn = { '/', '+', '*', '√', '(', '^', ')', '-' }; 
            //Regex toSplitOn = new Regex(@"([*()\^\/\√]|[\+\-])");  <--- Regex is confusing as fuck but worth it lmao. 
            var splitExpression = Regex.Split(expression, @"([*()\^\/\√]|[\+\-])").ToList();
            return splitExpression;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    }




    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Addition : Operator
    {
        public override double Operation(double num1, double num2)
        {
            var result = num1 + num2;
            return result;

        }

    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Subtraction : Addition
    {
        public override double Operation(double num1, double num2)
        {
            return base.Operation(num1, -num2);
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Multiplication : Operator
    {
        public override double Operation(double num1, double num2)
        {
            var result = num1 * num2;
            return result;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Division : Multiplication
    {
        public override double Operation(double num1, double num2)
        {
            return base.Operation(num1, (1 / (num2)));
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    class Exponentiation : Operator
    {
        public override double Operation(double num1, double num2)
        {
            var result = Math.Pow(num1, num2);
            return result;
        }

    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    class SquareRoot : Exponentiation
    {
        public override double Operation(double num1, double num2)
        {
            return base.Operation(num1, 1 / num2);
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------







}
