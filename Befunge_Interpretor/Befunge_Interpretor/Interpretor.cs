﻿using System;
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
        private string[] inputLines;
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

        private string[] SetLines()
        {
            List<string> temp = new List<string>();

            using (StringReader rd = new StringReader(this.input))
            {
                string line;

                do
                {
                    line = rd.ReadLine();
                    temp.Add(line);
                }
                while (line != null);
            }

            return temp.ToArray();
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
            throw new NotImplementedException();
        }

        private void HandleCharacter(char readCharacter)
        {
            if (this.isReadingString)
            {
                this.PushToStack(GetASCIIValue(readCharacter));
            }

            if(char.IsDigit(readCharacter))
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
                case'A':
                    this.HandleA();
                    break;
                case 'B':
                    this.HandleB();
                    break;
                case 'C':
                    this.HandleC();
                    break;
                case 'D':
                    this.HandleD();
                    break;
                case 'E':
                    this.HandleE();
                    break;
                case 'F':
                    this.HandleF();
                    break;
                    // I did not include a case for space, since it does nothing
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

        private void HandleA()
        {
            this.PushToStack(10);
        }

        private void HandleF()
        {
            this.PushToStack(15);
        }

        private void HandleE()
        {
            this.PushToStack(14);
        }

        private void HandleD()
        {
            this.PushToStack(13);
        }

        private void HandleC()
        {
            this.PushToStack(12);
        }

        private void HandleB()
        {
            this.PushToStack(11);
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
            string input = this.visitor.GetUserInput("Please insert a ASCII character.");

            while (!string.IsNullOrEmpty(input))
            {
                input = this.visitor.GetUserInput("Please insert a ASCII character!");
            }

            this.PushToStack(GetASCIIValue(input[0]));
        }

        private void HandleAtSign()
        {
            this.end = true;
        }

        private void HandleTilde()
        {
          string input = this.visitor.GetUserInput("Please insert a ASCII character.");

            while (!string.IsNullOrEmpty(input))
            {
                input = this.visitor.GetUserInput("Please insert a ASCII character!");
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
            //TODO maybe erst überprüfen ob es diesen character gibt?

            return inputLines[lineIndex][characterIndex];
        }
    }
}
