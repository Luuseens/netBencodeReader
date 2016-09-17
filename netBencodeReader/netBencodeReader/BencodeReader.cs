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
    using System.Text;

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
            
            // Handle a dictionary, e.g. "d4:name3:jone"
            if (readChar == 'd')
            {
                this.tokenTypeStack.Push(BencodeToken.StartDictionary);
                this.TokenType = BencodeToken.StartDictionary;
                return true;
            }

            // Handle a list, e.g. "l5:helloi123ee"
            if (readChar == 'l')
            {
                this.tokenTypeStack.Push(BencodeToken.StartArray);
                this.TokenType = BencodeToken.StartArray;
                return true;
            }

            // Handle the end of a list or a dictionary
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

            // Handle a string, e.g. "4:name"
            if (IsDigit(readChar))
            {
                this.TokenType = BencodeToken.String;
                var numString = readChar + this.ReadDigitsToEnd();

                this.ReadAndVerifyExpectedCharacter(':');
                
                var strLen = int.Parse(numString);

                var chars = new char[strLen];

                for (int i = 0; i < strLen; i++)
                {
                    readCharInt = this.stringReader.Read();
                    if (readCharInt < 0)
                    {
                        throw new ArgumentException();
                    }

                    chars[i] = Convert.ToChar(readCharInt);
                }

                this.TokenStringValue = new string(chars);

                return true;
            }

            // Handle an integer, e.g. "i2345e"
            if (readChar == 'i')
            {
                this.TokenType = BencodeToken.Integer;
                var num = this.ReadDigitsToEnd();

                if (num.Length == 0)
                {
                    throw new ArgumentException();
                }

                this.ReadAndVerifyExpectedCharacter('e');

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
        private string ReadDigitsToEnd()
        {
            var str = new StringBuilder();
            var peek = this.stringReader.Peek();

            var keepGoing = true;

            while (keepGoing)
            {
                keepGoing = false;

                if (peek > 0)
                {
                    var asChar = Convert.ToChar(peek);

                    if (IsDigit(asChar))
                    {
                        // This char was a number, run through at least once to check the next one.
                        keepGoing = true;

                        str.Append(asChar);

                        // Advance the pointer
                        this.stringReader.Read();

                        // Peek at the next value
                        peek = this.stringReader.Peek();
                    }
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// Reads a single character from the stringReader and ensures it exists and matches the expected one.
        /// </summary>
        /// <param name="expected">Expected character.</param>
        private void ReadAndVerifyExpectedCharacter(char expected)
        {
            var readCharInt = this.stringReader.Read();
            if (readCharInt < 0)
            {
                throw new ArgumentException();
            }

            var readChar = Convert.ToChar(readCharInt);

            if (readChar != expected)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Checks whether the character is a digit or not.
        /// </summary>
        /// <param name="c">Character to check.</param>
        /// <returns>True if the character is in the range from '0' to '9', false otherwise.</returns>
        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
 