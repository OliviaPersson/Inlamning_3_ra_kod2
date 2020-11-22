using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_ra_kod
{
    /* CLASS: Letter
     * PURPOSE: A class Letter with two variables and a constructor to 
     *   store information about a letter and it´s value
     */
    public class Letter
    {
        public string letters;
        public double value = 0;

        public Letter(string letters)
        {
            this.letters = letters;
        }

    }
    /* CLASS: CStack
     * PURPOSE: Is essentially a RPN-calculator with four registers X, Y, Z, T
     *   like the HP RPN calculators. Numeric values are entered in the entry
     *   string by adding digits and one comma. For test purposes the method
     *   RollSetX can be used instead. Operations can be performed on the
     *   calculator preferrably by using one of the methods
     *     1. BinOp - merges X and Y into X via an operation and rolls down
     *        the stack
     *     2. Unop - operates X and puts the result in X with overwrite
     *     3. Nilop - adds a known constant on the stack and rolls up the stack
     */
    public class CStack
    {
        public double X, Y, Z, T;
        public string entry;
        public string selectedLetter;
        public List<Letter> letterValues = new List<Letter>();
        public string fileName = @"C:\Users\Olivi\molkfreecalc.clc";
        /* CONSTRUCTOR: CStack
         * PURPOSE: create a new stack and init X, Y, Z, T and the text entry
         * PARAMETERS: --
         */
        public CStack()
        {
            X = Y = Z = T = 0;
            entry = "";

            AddLettersToList();
            ReadFile();
        }
        /* METHOD: AddLettersToList
         * PURPOSE: Creates letter object and adds to list
         * PARAMETERS: --
         * RETURNS: --
         */
        private void AddLettersToList()
        {
            Letter A = new Letter("A");
            letterValues.Add(A);
            Letter B = new Letter("B");
            letterValues.Add(B);
            Letter C = new Letter("C");
            letterValues.Add(C);
            Letter D = new Letter("D");
            letterValues.Add(D);
            Letter E = new Letter("E");
            letterValues.Add(E);
            Letter F = new Letter("F");
            letterValues.Add(F);
            Letter G = new Letter("G");
            letterValues.Add(G);
            Letter H = new Letter("H");
            letterValues.Add(H);
        }
        /* METHOD: ReadFile
         * PURPOSE: Reads file if if file exist and updates the variables X, Y, Z, T, 
         * A, B, C, D, E, F, G, H
         * PARAMETERS: --
         * RETURNS: --
         */
        private void ReadFile()
        {
            if (File.Exists(fileName))
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line = file.ReadLine();
                    while (line != null)
                    {
                        //string line = file.ReadLine();
                        string[] words = line.Split('#');
                        double number = double.Parse(words[1]);
                        Console.WriteLine(line);
                        line = file.ReadLine();

                        switch (words[0])
                        {
                            case "X": X = number; break;
                            case "Y": Y = number; break;
                            case "Z": Z = number; break;
                            case "T": T = number; break;
                            case "A": letterValues[0].value = number; break;
                            case "B": letterValues[1].value = number; break;
                            case "C": letterValues[2].value = number; break;
                            case "D": letterValues[3].value = number; break;
                            case "E": letterValues[4].value = number; break;
                            case "F": letterValues[5].value = number; break;
                            case "G": letterValues[6].value = number; break;
                            case "H": letterValues[7].value = number; break;
                            default: break;
                        }
                    }
                }
            }
        }
        /* METHOD: Exit
         * PURPOSE: called on exit and saves the values in file for variables X, Y, Z, T,
         * A, B, C, D, E, F, G, H
         * PARAMETERS: --
         * RETURNS: --
         */
        public void Exit()
        {
            using (StreamWriter writer = new StreamWriter(fileName))

                writer.WriteLine($"X#{X}\nY#{Y}\nZ#{Z}\nT#{T}");
        }
        /* METHOD: StackString
         * PURPOSE: construct a string to write out in a stack view
         * PARAMETERS: --
         * RETURNS: the string containing the values T, Z, Y, X with newlines 
         *   between them
         */
        public string StackString()
        {
            return $"{T}\n{Z}\n{Y}\n{X}\n{entry}";
        }
        /* METHOD: VarString
         * PURPOSE: construct a string to write out in a variable list
         * PARAMETERS: --
         * RETURNS: NOT YET IMPLEMENTED
         */
        public string VarString()
        {
            string valuesInList = "";
            for (int i = 0; i < letterValues.Count(); i++)
            {
                if (letterValues[i].value != 0)
                {
                    valuesInList += letterValues[i].value.ToString() + "\n";
                }
                else
                {
                    valuesInList += "\n";
                }
            }
            return $"{valuesInList}";
        }
        /* METHOD: SetX
         * PURPOSE: set X with overwrite
         * PARAMETERS: double newX - the new value to put in X
         * RETURNS: --
         */
        public void SetX(double newX)
        {
            X = newX;
        }
        /* METHOD: EntryAddNum
         * PURPOSE: add a digit to the entry string
         * PARAMETERS: string digit - the candidate digit to add at the end of the
         *   string
         * RETURNS: --
         * FAILS: if the string digit does not contain a parseable integer, nothing
         *   is added to the entry
         */
        public void EntryAddNum(string digit)
        {
            int val;
            if (int.TryParse(digit, out val))
            {
                entry = entry + val;
            }
        }
        /* METHOD: EntryAddComma
         * PURPOSE: adds a comma to the entry string
         * PARAMETERS: --
         * RETURNS: --
         * FAILS: if the entry string already contains a comma, nothing is added
         *   to the entry
         */
        public void EntryAddComma()
        {
            if (entry.IndexOf(",") == -1)
                entry = entry + ",";
        }
        /* METHOD: EntryChangeSign
         * PURPOSE: changes the sign of the entry string
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: if the first char is already a '-' it is exchanged for a '+',
         *   if it is a '+' it is changed to a '-', otherwise a '-' is just added
         *   first
         */
        public void EntryChangeSign()
        {
            char[] cval = entry.ToCharArray();
            if (cval.Length > 0)
            {
                switch (cval[0])
                {
                    case '+': cval[0] = '-'; entry = new string(cval); break;
                    case '-': cval[0] = '+'; entry = new string(cval); break;
                    default: entry = '-' + entry; break;
                }
            }
            else
            {
                entry = '-' + entry;
            }
        }
        /* METHOD: Enter
         * PURPOSE: converts the entry to a double and puts it into X
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: the entry is cleared after a successful operation
         */
        public void Enter()
        {
            if (entry != "")
            {
                RollSetX(double.Parse(entry));
                entry = "";
            }
        }
        /* METHOD: Drop
         * PURPOSE: drops the value of X, and rolls down
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: Z gets the value of T
         */
        public void Drop()
        {
            X = Y; Y = Z; Z = T;
        }
        /* METHOD: DropSetX
         * PURPOSE: replaces the value of X, and rolls down
         * PARAMETERS: double newX - the new value to assign to X
         * RETURNS: --
         * FEATURES: Z gets the value of T
         * NOTES: this is used when applying binary operations consuming
         *   X and Y and putting the result in X, while rolling down the
         *   stack
         */
        public void DropSetX(double newX)
        {
            X = newX; Y = Z; Z = T;
        }
        /* METHOD: BinOp
         * PURPOSE: evaluates a binary operation
         * PARAMETERS: string op - the binary operation retrieved from the
         *   GUI buttons
         * RETURNS: --
         * FEATURES: the stack is rolled down
         */
        public void BinOp(string op)
        {
            switch (op)
            {
                case "+": DropSetX(Y + X); break;
                case "−": DropSetX(Y - X); break;
                case "×": DropSetX(Y * X); break;
                case "÷": DropSetX(Y / X); break;
                case "yˣ": DropSetX(Math.Pow(Y, X)); break;
                case "ˣ√y": DropSetX(Math.Pow(Y, 1.0 / X)); break;
            }
        }
        /* METHOD: Unop
         * PURPOSE: evaluates a unary operation
         * PARAMETERS: string op - the unary operation retrieved from the
         *   GUI buttons
         * RETURNS: --
         * FEATURES: the stack is not moved, X is replaced by the result of
         *   the operation
         */
        public void Unop(string op)
        {
            switch (op)
            {
                // Powers & Logarithms:
                case "x²": SetX(X * X); break;
                case "√x": SetX(Math.Sqrt(X)); break;
                case "log x": SetX(Math.Log10(X)); break;
                case "ln x": SetX(Math.Log(X)); break;
                case "10ˣ": SetX(Math.Pow(10, X)); break;
                case "eˣ": SetX(Math.Exp(X)); break;

                // Trigonometry:
                case "sin": SetX(Math.Sin(X)); break;
                case "cos": SetX(Math.Cos(X)); break;
                case "tan": SetX(Math.Tan(X)); break;
                case "sin⁻¹": SetX(Math.Asin(X)); break;
                case "cos⁻¹": SetX(Math.Acos(X)); break;
                case "tan⁻¹": SetX(Math.Atan(X)); break;
            }
        }
        /* METHOD: Nilop
         * PURPOSE: evaluates a "nilary operation" (insertion of a constant)
         * PARAMETERS: string op - the nilary operation (name of the constant)
         *   retrieved from the GUI buttons
         * RETURNS: --
         * FEATURES: the stack is rolled up, X is preserved in Y that is preserved in
         *   Z that is preserved in T, T is erased
         */
        public void Nilop(string op)
        {
            switch (op)
            {
                case "π": RollSetX(Math.PI); break;
                case "e": RollSetX(Math.E); break;
            }
        }
        /* METHOD: Roll
         * PURPOSE: rolls the stack up
         * PARAMETERS: --
         * RETURNS: --
         */
        public void Roll()
        {
            double tmp = T;
            T = Z; Z = Y; Y = X; X = tmp;
        }
        /* METHOD: Roll
         * PURPOSE: rolls the stack up and puts a new value in X
         * PARAMETERS: double newX - the new value to put into X
         * RETURNS: --
         * FEATURES: T is dropped
         */
        public void RollSetX(double newX)
        {
            T = Z; Z = Y; Y = X; X = newX;
        }
        /* METHOD: SetAddress
         * PURPOSE: Store the selected letter in variable
         * PARAMETERS: string name - variable name
         * RETURNS: --
         * FEATURES: NOT YET IMPLEMENTED
         */
        public void SetAddress(string name)
        {
            switch (name)
            {
                case "A": selectedLetter = name; break;
                case "B": selectedLetter = name; break;
                case "C": selectedLetter = name; break;
                case "D": selectedLetter = name; break;
                case "E": selectedLetter = name; break;
                case "F": selectedLetter = name; break;
                case "G": selectedLetter = name; break;
                case "H": selectedLetter = name; break;
            }
        }
        /* METHOD: SetVar
         * PURPOSE: Looks for the same string value and adds the value of X to the list object
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: NOT YET IMPLEMENTED
         */
        public void SetVar()
        {
            for (int i = 0; i < letterValues.Count(); i++)
            {
                if (letterValues[i].letters == selectedLetter)
                {
                    letterValues[i].value = X;
                }
            }
        }
        /* METHOD: GetVar
         * PURPOSE: Move the value from list object to x
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: NOT YET IMPLEMENTED
         */
        public void GetVar()
        {
            for (int i = 0; i < letterValues.Count(); i++)
            {
                if (letterValues[i].letters == selectedLetter)
                {
                    RollSetX(letterValues[i].value);
                    letterValues[i].value = 0;
                }
            }
        }
    }
}
