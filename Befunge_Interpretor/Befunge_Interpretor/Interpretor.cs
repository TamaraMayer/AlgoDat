﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Befunge.Test")]

namespace Befunge_Interpretor
{
    public class Interpretor
    {
        //the direction in which the program moves, by default right (set in constructor)
        private Directions directionToMove;

        //a boolean determinating if the next char in the program shall be read, or not
        private bool end;

        //the code as string
        private string input;

        //the code as multidimensional char[]
        private char[,] inputLines;

        //the stack
        public List<int> Stack { get; private set; }

        //a visitor for the getting user input
        private IInputVisitor visitor;

        //a boolean to determine if a string is read"
        private bool isReadingString;

        //the index in line(row) and of character(column) where the program is currently at
        private int lineIndex;
        private int characterIndex;

        //an event for showing that a new ouput occured
        public event EventHandler<OnOutpuEventArgs> OnNewOutput;

        public Interpretor(string inputString, IInputVisitor inputVisitor)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentNullException(nameof(inputString), "The specified paramter must not be null, empty or only whitespace!");
            }

            if (inputVisitor == null)
            {
                throw new ArgumentNullException(nameof(inputVisitor), "The specified paramter must not be null!");

            }

            //setting the instances
            this.input = inputString;
            this.visitor = inputVisitor;
            this.Stack = new List<int>();
            this.lineIndex = 0;
            this.characterIndex = 0;
            this.end = true;
            this.isReadingString = false;
            this.directionToMove = Directions.Right;

            //setting the lines to the char[,]
            inputLines = SetLines();
        }

        /// <summary>
        /// Sets the given code in string to a char array.
        /// </summary>
        /// <returns>The code seperated into a char array.</returns>
        private char[,] SetLines()
        {
            int amountOfLines = 0;
            int amountOfCharacters = 0;
            char[,] temp;
            string line;

            //figuring out the number of rows and the max number of characters in one line
            using (StringReader rd = new StringReader(this.input))
            {
                line = rd.ReadLine();

                do
                {
                    amountOfLines++;

                    if (amountOfCharacters < line.Length)
                    {
                        amountOfCharacters = line.Length;
                    }

                    line = rd.ReadLine();
                }
                while (line != null);
            }

            //writing each character of each line into the char[,]
            using (StringReader rd = new StringReader(this.input))
            {
                temp = new char[amountOfLines, amountOfCharacters];
                int tempLineIndex = 0;
                line = rd.ReadLine();

                do
                {
                    line = line.PadRight(amountOfCharacters);

                    for (int i = 0; i < amountOfCharacters; i++)
                    {
                        temp[tempLineIndex, i] = line[i];
                    }

                    line = rd.ReadLine();
                    tempLineIndex++;
                }
                while (line != null);
            }

            return temp;
        }

        /// <summary>
        /// Runs the code.
        /// </summary>
        public void Run()
        {
            char readCharacter;

            //read the character, do whatever is supposed to happen, then move to the next spot; repeat
            while (end)
            {
                readCharacter = ReadCharacter();

                HandleCharacter(readCharacter);

                this.Move();
            }
        }

        /// <summary>
        /// Determines the direction in which to move and does the move
        /// </summary>
        private void Move()
        {
            //determines in which direction to move

            switch (this.directionToMove)
            {
                case Directions.Down:
                    this.MoveDown();
                    break;
                case Directions.Up:
                    this.MoveUp();
                    break;
                case Directions.Left:
                    this.MoveLeft();
                    break;
                case Directions.Right:
                    this.MoveRight();
                    break;
            }
        }

        /// <summary>
        /// Moves one step to the right.
        /// </summary>
        private void MoveRight()
        {
            //increment characterIndex, if characterIndex is greather than the amount of chars per line, jump back to character 0

            this.characterIndex++;

            if (this.characterIndex >= this.inputLines.GetLength(1))
            {
                this.characterIndex = 0;
            }
        }

        /// <summary>
        /// Moves one step to the left.
        /// </summary>
        private void MoveLeft()
        {
            //decrement characterIndex, if smaller than 0, jumt to the end of the the line

            this.characterIndex--;

            if (this.characterIndex < 0)
            {
                this.characterIndex = this.inputLines.GetLength(1) - 1;
            }
        }

        /// <summary>
        /// Moves one step to the top.
        /// </summary>
        private void MoveUp()
        {
            //decrement lineIndex, if smaller than 0, jump to last line

            this.lineIndex--;

            if (this.lineIndex < 0)
            {
                this.lineIndex = this.inputLines.GetLength(0) - 1;
            }
        }

        /// <summary>
        /// MOves one step to the bottom.
        /// </summary>
        private void MoveDown()
        {
            //increment lineIndex, if lineIndex is greather than the amount of lines, jump back to line 0

            this.lineIndex++;

            if (this.lineIndex >= this.inputLines.GetLength(0))
            {
                this.lineIndex = 0;
            }
        }

        /// <summary>
        /// Chooses what action to do with the given character.
        /// </summary>
        /// <param name="readCharacter">The given character.</param>
        private void HandleCharacter(char readCharacter)
        {
            if (readCharacter == '"')
            {
                this.HandleQuotationMark();
                return;
            }

            if (this.isReadingString)
            {
                this.PushToStack(GetASCIIValue(readCharacter));
                return;
            }

            if (char.IsDigit(readCharacter))
            {
                PushToStack(Int32.Parse(readCharacter.ToString()));
                return;
            }

            switch (readCharacter)
            {
                case '+':
                    this.AddLastTwoValues();
                    break;
                case '-':
                    this.SubtractLastTwoValues();
                    break;
                case '*':
                    this.MultiplyLastTwoValues();
                    break;
                case '/':
                    this.DivideLastTwoValues();
                    break;
                case '%':
                    this.ModuloLastTwoValues();
                    break;
                case '!':
                    this.HandleExclamationMark();
                    break;
                case '`':
                    this.HandleBacktick();
                    break;
                case '>':
                    this.SetMoveRight();
                    break;
                case '<':
                    this.SetMoveLeft();
                    break;
                case '^':
                    this.SetMoveUp();
                    break;
                case 'v':
                    this.SetMoveDown();
                    break;
                case '?':
                    this.MoveRandom();
                    break;
                case '_':
                    this.HandleUnderscore();
                    break;
                case '|':
                    this.HandleVerticalBar();
                    break;
                case ':':
                    this.HandleColon();
                    break;
                case '\\':
                    this.HandleBackSlash();
                    break;
                case '$':
                    this.HandleDollarSign();
                    break;
                case '.':
                    this.HandleFullStop();
                    break;
                case ',':
                    this.HandleComma();
                    break;
                case '#':
                    this.HandleHash();
                    break;
                case 'p':
                    this.HandlePutCall();
                    break;
                case 'g':
                    this.HandleGetCall();
                    break;
                case '&':
                    this.HandleAmpersand();
                    break;
                case '~':
                    this.HandleTilde();
                    break;
                case '@':
                    this.HandleAtSign();
                    break;
                case ' ':
                    break;
                default:
                    throw new Exception($"Came accross some invalid code! In line {this.lineIndex} at position {this.characterIndex}");
            }
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes the result after a divison.
        /// </summary>
        private void DivideLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            if (a == 0)
            {
                this.PushToStack(0);
            }
            else
            {
                PushToStack(b / a);
            }
        }


        private void HandleBacktick()
        {
            int a = this.PopFromStack();
            int b = this.PopFromStack();

            if (b > a)
            {
                this.PushToStack(1);
            }
            else
            {
                this.PushToStack(0);
            }
        }

        private void HandleExclamationMark()
        {
            int a = PopFromStack();

            if (a == 0)
            {
                this.PushToStack(1);
            }
            else
            {
                this.PushToStack(0);
            }
        }

        private void HandleQuotationMark()
        {
            this.isReadingString = !this.isReadingString;
        }

        private void HandleColon()
        {
            this.PushToStack(this.PeekFromStack());
        }

        private void HandleBackSlash()
        {
            int a = this.PopFromStack();
            int b = this.PopFromStack();

            this.PushToStack(b);
            this.PushToStack(a);
        }

        private void HandleDollarSign()
        {
            this.PopFromStack();
        }

        private void HandleFullStop()
        {
            int a = this.PopFromStack();

            this.FireOutputEvent($"{a} ");
        }

        private void HandleComma()
        {
            int a = this.PopFromStack();

            if (a < 0)
            {
                this.FireOutputEvent("?");
            }

            char temp = (char)a;

            this.FireOutputEvent($"{temp}");
        }

        private void HandleHash()
        {
            Move();
        }

        private void HandlePutCall()
        {
            int y = this.PopFromStack();
            int x = this.PopFromStack();
            int v = this.PopFromStack();

            if (x > this.inputLines.GetLength(0))
            {
                return;
            }
            if (y > this.inputLines.GetLength(1))
            {
                return;
            }

            if (v < 0)
            {
                //desided to do (char)0 if it is a negative int since it is null, and i don't quite know what else to do
                this.inputLines[x, y] = (char)0;
            }
            else
            {
                this.inputLines[x, y] = (char)v;
            }
        }

        private void HandleGetCall()
        {
            int y = this.PopFromStack();
            int x = this.PopFromStack();

            if (x > this.inputLines.GetLength(0))
            {
                this.PushToStack(0);
                return;
            }
            if (y > this.inputLines.GetLength(1))
            {
                this.PushToStack(0);
                return;
            }

            this.PushToStack(this.inputLines[x, y]);
        }

        private void HandleAmpersand()
        {
            bool exit = false;
            string input;

            while (exit)
            {
                input = this.visitor.GetUserInput("Please enter a number!");

                if (input.Length != 1)
                {
                    continue;
                }

                if (Int32.TryParse(input, out int result))
                {
                    exit = true;
                    this.PushToStack(result);

                }
            }
        }

        private void HandleAtSign()
        {
            this.end = false;
        }

        private void HandleTilde()
        {
            string input = this.visitor.GetUserInput("Please enter a ASCII character.");

            while (!string.IsNullOrEmpty(input) || input.Length != 1)
            {
                input = this.visitor.GetUserInput("Please enter a ASCII character!");
            }

            this.PushToStack(GetASCIIValue(input[0]));
        }

        private void HandleUnderscore()
        {
            int a = this.PopFromStack();

            if (a == 0)
            {
                this.directionToMove = Directions.Right;
            }
            else
            {
                this.directionToMove = Directions.Left;
            }
        }

        private void HandleVerticalBar()
        {
            int a = this.PopFromStack();

            if (a == 0)
            {
                this.directionToMove = Directions.Down;
            }
            else
            {
                this.directionToMove = Directions.Up;
            }
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes the left over from a division.
        /// </summary>
        private void ModuloLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            PushToStack(b % a);
        }

        private void SetMoveDown()
        {
            this.directionToMove = Directions.Down;
        }

        private void SetMoveLeft()
        {
            this.directionToMove = Directions.Left;
        }

        private void MoveRandom()
        {
            Random random = new Random();
            this.directionToMove = (Directions)random.Next(1, 5);
        }

        private void SetMoveRight()
        {
            this.directionToMove = Directions.Right;
        }

        private void SetMoveUp()
        {
            this.directionToMove = Directions.Up;
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes them multiplied with each other.
        /// </summary>
        private void MultiplyLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            PushToStack(a * b);
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes them subtracted.
        /// </summary>
        private void SubtractLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            PushToStack(b - a);
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes them added together.
        /// </summary>
        private void AddLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            PushToStack(a + b);
        }

        /// <summary>
        /// Converts a char into its ascii value.
        /// </summary>
        /// <param name="character">The ascii value.</param>
        /// <returns></returns>
        private int GetASCIIValue(char character)
        {
            byte[] charByte = Encoding.ASCII.GetBytes(character.ToString());

            return charByte[0];
        }

        /// <summary>
        /// Puts a new element to the top of the Stack.
        /// </summary>
        /// <param name="toPush"></param>
        private void PushToStack(int toPush)
        {
            this.Stack.Add(toPush);
        }

        /// <summary>
        /// Takes the top element from the stack. The element gets deleted from the stack
        /// </summary>
        /// <returns>The element on top of the stack.</returns>
        private int PopFromStack()
        {
            if (Stack.Count == 0)
            {
                return 0;
            }

            int temp = this.Stack[this.Stack.Count - 1];
            this.Stack.RemoveAt(this.Stack.Count - 1);

            return temp;
        }

        /// <summary>
        /// Takes a look at the top element on the stack, but does not delete it.
        /// </summary>
        /// <returns>The element on top of the stack.</returns>
        private int PeekFromStack()
        {
            if (Stack.Count == 0)
            {
                return 0;
            }

            int temp = this.Stack[this.Stack.Count - 1];

            return temp;
        }

        /// <summary>
        /// reads the next character in the given code
        /// </summary>
        /// <returns>The char at the lineIndex and characterIndex.</returns>
        private char ReadCharacter()
        {
            return inputLines[lineIndex, characterIndex];
        }

        private void FireOutputEvent(string output)
        {
            this.OnNewOutput?.Invoke(this, new OnOutpuEventArgs(output));
        }
    }
}
