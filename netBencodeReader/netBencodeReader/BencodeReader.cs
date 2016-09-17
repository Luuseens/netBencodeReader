// -----------------------------------------------------------------------
// <copyright file="BencodeReader.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public sealed class BencodeReader
    {
        private string tempSrcString = "d4:name5:randy3:agei29e4:miscli1e2:lvee";

        private int pointer;

        private Stack<BencodeToken> tokenTypeStack = new Stack<BencodeToken>();

        public BencodeToken TokenType { get; private set; }
    
        public string TokenStringValue { get; private set; }

        private StringReader stringReader;

        /// <summary>
        /// Creates a new instance of BencodeReader.
        /// </summary>
        /// <param name="stringRead">StringReader to read the string from.</param>
        /// <returns>A new instance of BencodeReader.</returns>
        public static BencodeReader Create(StringReader stringRead)
        {
            return new BencodeReader { stringReader = stringRead };
        }

        /// <summary>
        /// Hide the default constructor.
        /// </summary>
        private BencodeReader()
        {
            
        }

        /// <summary>
        /// Reads the next token from the Bencoded string. 
        /// Sets the TokenType to the type of token found,
        /// and the 
        /// </summary>
        /// <returns>True if something was read, false if the end of the string has been reached.</returns>
        public bool Read()
        {

            this.TokenType = BencodeToken.None;
            this.TokenStringValue = string.Empty;

            if (pointer >= this.tempSrcString.Length)
            {
                return false;
            }

            if (this.tempSrcString[this.pointer] == 'd')
            {
                this.pointer++;
                this.tokenTypeStack.Push(BencodeToken.StartDictionary);
                this.TokenType = BencodeToken.StartDictionary;
                return true;
            }

            if (this.tempSrcString[this.pointer] == 'l')
            {
                this.pointer++;
                this.tokenTypeStack.Push(BencodeToken.StartArray);
                this.TokenType = BencodeToken.StartArray;
                return true;
            }

            if (this.tempSrcString[this.pointer] == 'e')
            {
                this.pointer++;
                var currentType = this.tokenTypeStack.Peek();

                if (currentType == BencodeToken.StartDictionary)
                {
                    this.tokenTypeStack.Pop();
                    this.TokenType = BencodeToken.EndDictionary;
                } else if (currentType == BencodeToken.StartArray)
                {
                    this.tokenTypeStack.Pop();
                    this.TokenType = BencodeToken.EndArray;
                }
                else
                {
                    // Nothing else should have an ending 
                    throw new ArgumentException();
                }

                this.tokenTypeStack.Push(BencodeToken.StartDictionary);
                return true;
            }

            if (IsDigit(this.tempSrcString[this.pointer]))
            {
                this.TokenType = BencodeToken.String;
                string num = "" + this.tempSrcString[this.pointer];

                while (IsDigit(this.tempSrcString[++this.pointer]))
                {
                    num += this.tempSrcString[this.pointer];
                }

                if (this.tempSrcString[this.pointer++] != ':')
                {
                    throw new ArgumentException();
                }

                var strLen = int.Parse(num);
                this.TokenStringValue = this.tempSrcString.Substring(this.pointer, strLen);
                this.pointer += strLen;

                return true;
            }

            if (this.tempSrcString[this.pointer] == 'i')
            {
                this.TokenType = BencodeToken.Integer;
                var num = string.Empty;

                while (IsDigit(this.tempSrcString[++this.pointer]))
                {
                    num += this.tempSrcString[this.pointer];
                }

                if (this.tempSrcString[this.pointer] != 'e')
                {
                    throw new ArgumentException();
                }
                
                this.pointer++;

                this.TokenStringValue = num;


                return true;
            }

            return false;
        }

        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

    }
}
