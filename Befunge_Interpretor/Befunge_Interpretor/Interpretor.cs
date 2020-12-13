using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Befunge_Interpretor
{
    public class Interpretor
    {

        private Directions directionToMove;
        private bool end;
        private string input;
        private char[,] inputLines;
        //public string Output { get; private set; }

        public List<char> Stack { get; private set; }

        private IInputVisitor visitor;

        private bool isReadingString;

        private int lineIndex;
        private int characterIndex;

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

            this.input = inputString;
            this.visitor = inputVisitor;
            this.Stack = new List<char>();

            this.lineIndex = 0;
            this.characterIndex = 0;
            this.end = false;
            this.isReadingString = false;
            this.directionToMove = Directions.Right;

            inputLines = SetLines();
        }

        private char[,] SetLines()
        {
            int amountOfLines=0;
            int amountOfCharacters=0;
            char[,] temp;
            string line;

            using (StringReader rd = new StringReader(this.input))
            {
                line = rd.ReadLine();

                do
                {
                    amountOfLines++;

                    if(amountOfCharacters<line.Length)
                    {
                        amountOfCharacters = line.Length;
                    }

                    line = rd.ReadLine();
                }
                while (line != null);
            }

            using (StringReader rd = new StringReader(this.input))
            {
                temp = new char[amountOfLines, amountOfCharacters];
                lineIndex = 0;
                line = rd.ReadLine().PadLeft(amountOfCharacters);

                do
                {
                    for (int i = 0; i < amountOfCharacters; i++)
                    {
                        temp[lineIndex, i]=line[i];
                    }

                    line = rd.ReadLine();
                    lineIndex++;
                }
                while (line != null);
            }

            return temp;
        }

        public void Run()
        {
            char readCharacter;

            while (end)
            {
                this.Move();

                readCharacter = ReadCharacter();

                HandleCharacter(readCharacter);
            }
        }

        private void Move()
        {
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

        private void MoveRight()
        {
            this.characterIndex++;

            if (this.characterIndex >= this.inputLines.GetLength(1))
            {
                this.characterIndex = 0;
            }
        }

        private void MoveLeft()
        {
            this.characterIndex--;

            if(this.characterIndex < 0)
            {
                this.characterIndex = this.inputLines.GetLength(1) - 1;
            }
        }

        private void MoveUp()
        {
            this.lineIndex--;

            if (this.lineIndex <0)
            {
                this.lineIndex = this.inputLines.GetLength(0)-1;
            }
        }

        private void MoveDown()
        {
            this.lineIndex++;

            if (this.lineIndex >= this.inputLines.GetLength(0))
            {
                this.lineIndex = 0;
            }
        }

        private void HandleCharacter(char readCharacter)
        {
            if (this.isReadingString)
            {
                this.PushToStack(GetASCIIValue(readCharacter));
            }

            if (char.IsDigit(readCharacter))
            {
                PushToStack(readCharacter);
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
                case '"':
                    this.HandleQuotationMark();
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
                    throw new ArgumentOutOfRangeException($"Came accross some invalid code! In line {this.lineIndex} at position {this.characterIndex}");
            }
        }

        private void DivideLastTwoValues()
        {
            throw new NotImplementedException();
        }

        private void HandleBacktick()
        {
            throw new NotImplementedException();
        }

        private void HandleExclamationMark()
        {
            throw new NotImplementedException();
        }

        private void HandleQuotationMark()
        {
            this.isReadingString = !this.isReadingString;
        }

        private void HandleColon()
        {
            throw new NotImplementedException();
        }

        private void HandleBackSlash()
        {
            throw new NotImplementedException();
        }

        private void HandleDollarSign()
        {
            throw new NotImplementedException();
        }

        private void HandleFullStop()
        {
            throw new NotImplementedException();
        }

        private void HandleComma()
        {
            throw new NotImplementedException();
        }

        private void HandleHash()
        {
            throw new NotImplementedException();
        }

        private void HandlePutCall()
        {
            throw new NotImplementedException();
        }

        private void HandleGetCall()
        {
            throw new NotImplementedException();
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
            this.end = true;
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
            throw new NotImplementedException();
        }

        private void HandleVerticalBar()
        {
            throw new NotImplementedException();
        }

        private void ModuloLastTwoValues()
        {
            throw new NotImplementedException();
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

        private void MultiplyLastTwoValues()
        {
            throw new NotImplementedException();
        }

        private void SubtractLastTwoValues()
        {
            throw new NotImplementedException();
        }

        private void AddLastTwoValues()
        {
            throw new NotImplementedException();
        }

        private int GetASCIIValue(char character)
        {
            byte[] charByte = Encoding.ASCII.GetBytes(character.ToString());

            return charByte[0];
        }

        private void PushToStack(int toPush)
        {
            throw new NotImplementedException();
        }

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

        private char ReadCharacter()
        {
            return inputLines[lineIndex,characterIndex];
        }
    }
}
