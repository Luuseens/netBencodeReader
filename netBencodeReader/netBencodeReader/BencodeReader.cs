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

            var readCharInt = this.stringReader.Read();
            if (readCharInt < 0)
            {
                return false;
            }

            var readChar = Convert.ToChar(readCharInt);
            
            if (readChar == 'd')
            {
                this.tokenTypeStack.Push(BencodeToken.StartDictionary);
                this.TokenType = BencodeToken.StartDictionary;
                return true;
            }

            if (readChar == 'l')
            {
                this.tokenTypeStack.Push(BencodeToken.StartArray);
                this.TokenType = BencodeToken.StartArray;
                return true;
            }

            if (readChar == 'e')
            {
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

            if (IsDigit(readChar))
            {
                // TODO: optimize this!
                this.TokenType = BencodeToken.String;
                string num = "" + readChar;

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

        /// <summary>
        /// Reads all the characters from the stringReader as long as they are digits. 
        /// After this has run, stringReader will be pointing at the first following non-digit value.
        /// </summary>
        /// <returns></returns>
        private string ReadNumber()
        {
            var str = string.Empty;
            var peek = 
        }

        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

    }
}
