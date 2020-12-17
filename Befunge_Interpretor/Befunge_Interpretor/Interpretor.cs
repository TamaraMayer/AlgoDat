using System;
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

        public string OUtput;

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

            this.OUtput = "";
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
                    this.SetMoveRandom();
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
        internal void DivideLastTwoValues()
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

        /// <summary>
        /// Pops two values and pushes either 0 or 1.
        /// </summary>
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

        /// <summary>
        /// Pop a value. If the value is zero, push 1; otherwise, push zero.
        /// </summary>
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

        /// <summary>
        /// Starts or Stops the string reading mode.
        /// </summary>
        private void HandleQuotationMark()
        {
            this.isReadingString = !this.isReadingString;
        }

        /// <summary>
        /// Duplicate value on top of the stack.
        /// </summary>
        private void HandleColon()
        {
            this.PushToStack(this.PeekFromStack());
        }

        /// <summary>
        /// Swap two values on top of the stack
        /// </summary>
        private void HandleBackSlash()
        {
            int a = this.PopFromStack();
            int b = this.PopFromStack();

            this.PushToStack(a);
            this.PushToStack(b);
        }

        /// <summary>
        /// Pop value from the stack and discard it
        /// </summary>
        private void HandleDollarSign()
        {
            this.PopFromStack();
        }

        /// <summary>
        /// Pop value and output as an integer followed by a space.
        /// </summary>
        private void HandleFullStop()
        {
            int a = this.PopFromStack();

            this.FireOutputEvent($"{a} ");
        }

        /// <summary>
        /// Pop value and output as ASCII character.
        /// </summary>
        private void HandleComma()
        {
            int a = this.PopFromStack();

            char temp = (char)a;

            this.FireOutputEvent($"{temp}");
        }

        /// <summary>
        /// Makes an extra move to jump over one instruction.
        /// </summary>
        private void HandleHash()
        {
            Move();
        }

        /// <summary>
        /// Pop y, x, and v, then change the character at (x,y) in the program to the character with ASCII value v.
        /// </summary>
        private void HandlePutCall()
        {
            int x = this.PopFromStack();
            int y = this.PopFromStack();
            int v = this.PopFromStack();

            if (x > this.inputLines.GetLength(0))
            {
                return;
            }
            if (y > this.inputLines.GetLength(1))
            {
                return;
            }


                this.inputLines[x, y] = (char)v;

        }

        /// <summary>
        /// Pop y and x, then push ASCII value of the character at that position in the program.
        /// </summary>
        private void HandleGetCall()
        {
            int x = this.PopFromStack();
            int y = this.PopFromStack();

            if (x >= this.inputLines.GetLength(0))
            {
                this.PushToStack(32);
                return;
            }
            if (y >= this.inputLines.GetLength(1))
            {
                this.PushToStack(32);
                return;
            }

            this.PushToStack(this.inputLines[x, y]);
        }

        /// <summary>
        /// Askes a number from the user and pushes it to the stack.
        /// </summary>
        internal void HandleAmpersand()
        {
            bool exit = false;
            string input;

            while (!exit)
            {
                input = this.visitor.GetUserInput("Please enter a number!");

                if (Int32.TryParse(input, out int result))
                {
                    exit = true;
                    this.PushToStack(result);
                }
            }
        }

        /// <summary>
        /// Sets the end of the program.
        /// </summary>
        private void HandleAtSign()
        {
            this.end = false;
        }

        /// <summary>
        /// Askes a ASCII character from the user and pushes the ascii value to the stack.
        /// </summary>
        internal void HandleTilde()
        {
            bool exit = false;
            string input = this.visitor.GetUserInput("Please enter a ASCII character.");

            while (!exit)
            {
                input = this.visitor.GetUserInput("Please enter a ASCII character!");

                if(!string.IsNullOrEmpty(input) && input.Length == 1)
                {
                    exit = true;
                }
            }

            this.PushToStack(GetASCIIValue(input[0]));
        }

        /// <summary>
        /// Pops a value from the stack and changed direction based on that value.
        /// </summary>
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

        /// <summary>
        /// Pops a value from the stack and changed direction based on that value.
        /// </summary>
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
        internal void ModuloLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            PushToStack(b % a);
        }

        /// <summary>
        /// Sets the direction to move to the bottom.
        /// </summary>
        private void SetMoveDown()
        {
            this.directionToMove = Directions.Down;
        }

        /// <summary>
        /// Sets the direction to move to the left.
        /// </summary>
        private void SetMoveLeft()
        {
            this.directionToMove = Directions.Left;
        }

        /// <summary>
        /// Sets the direction to move to a random direction.
        /// </summary>
        private void SetMoveRandom()
        {
            Random random = new Random();
            this.directionToMove = (Directions)random.Next(1, 5);
        }

        /// <summary>
        /// Sets the direction to move to the right.
        /// </summary>
        private void SetMoveRight()
        {
            this.directionToMove = Directions.Right;
        }

        /// <summary>
        /// Sets the direction to move to the top.
        /// </summary>
        private void SetMoveUp()
        {
            this.directionToMove = Directions.Up;
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes them multiplied with each other.
        /// </summary>
        internal void MultiplyLastTwoValues()
        {
            int a = PopFromStack();
            int b = PopFromStack();

            PushToStack(a * b);
        }

        /// <summary>
        /// Pops the last two values from the stack and pushes them subtracted.
        /// </summary>
        internal void SubtractLastTwoValues()
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

            this.OUtput += output;
        }
    }
}
