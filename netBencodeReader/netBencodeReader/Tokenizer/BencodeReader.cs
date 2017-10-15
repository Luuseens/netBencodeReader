// -----------------------------------------------------------------------
// <copyright file="BencodeReader.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader.Tokenizer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using netBencodeReader.Exceptions;

    public sealed class BencodeReader
    {
        /// <summary>
        /// Stack to keep track how deep in a List/Dictionary structure we are.
        /// </summary>
        private readonly Stack<BencodeParseStackState> tokenTypeStack = new Stack<BencodeParseStackState>();

        /// <summary>
        /// Last found token type.
        /// </summary>
        public BencodeToken TokenType { get; private set; }
    
        /// <summary>
        /// Last found token value.
        /// </summary>
        public byte[] TokenByteStringValue { get; private set; }    
        
        /// <summary>
        /// Last found token value.
        /// </summary>
        public string TokenStringValue { get; private set; }

        /// <summary>
        /// Sets the read state of the reader.
        /// </summary>
        public ReadState ReadState { get; private set; }

        /// <summary>
        /// Text reader wrapping the Bencode string.
        /// </summary>
        private TextReader textReader;

        /// <summary>
        /// Creates a new instance of BencodeReader.
        /// </summary>
        /// <param name="sourceReader">TextReader to read the string from.</param>
        /// <returns>A new instance of BencodeReader.</returns>
        public static BencodeReader Create(TextReader sourceReader)
        {
            return new BencodeReader
            {
                textReader = sourceReader,
                ReadState = ReadState.Initial
            };
        }

        /// <summary>
        /// Creates a new instance of BencodeReader.
        /// </summary>
        /// <param name="sourceString">String to read the string from.</param>
        /// <returns>A new instance of BencodeReader.</returns>
        public static BencodeReader Create(string sourceString)
        {
            return new BencodeReader
            {
                textReader = new StringReader(sourceString),
                ReadState = ReadState.Initial
            };
        }

        /// <summary>
        /// Creates a new instance of BencodeReader.
        /// </summary>
        /// <param name="sourceBytes">Bytes to read the string from.</param>
        /// <returns>A new instance of BencodeReader.</returns>
        public static BencodeReader Create(byte[] sourceBytes)
        {
            return new BencodeReader
            {
                textReader = new StreamReader(new MemoryStream(sourceBytes)),
                ReadState = ReadState.Initial
            };
        }

        /// <summary>
        /// Creates a new instance of BencodeReader.
        /// </summary>
        /// <param name="sourceStream">Stream to read the string from.</param>
        /// <returns>A new instance of BencodeReader.</returns>
        public static BencodeReader Create(Stream sourceStream)
        {
            return new BencodeReader
            {
                textReader = new StreamReader(sourceStream),
                ReadState = ReadState.Initial
            };
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
            this.ReadState = ReadState.InProgress;
            this.TokenType = BencodeToken.None;
            this.TokenByteStringValue = new byte[0];
            this.TokenStringValue = string.Empty;
            this.SwapDictionaryKeyState();

            var readCharInt = this.textReader.Read();
            if (readCharInt < 0)
            {
                // We have reached the end of the document.
                this.ReadState = ReadState.EndOfFile;
                return false;
            }

            var readChar = Convert.ToChar(readCharInt);
            
            // Handle a dictionary, e.g. "d4:name3:jone"
            if (readChar == 'd')
            {
                if (this.DictionaryKeyExpected())
                {
                    this.ReadState = ReadState.Error;
                    throw new BencodeParseException("Expected a string for dictionary's key, found a new dictionary instead.");
                }

                this.tokenTypeStack.Push(new BencodeParseStackState(BencodeToken.StartDictionary));
                this.TokenType = BencodeToken.StartDictionary;
                return true;
            }

            // Handle a list, e.g. "l5:helloi123ee"
            if (readChar == 'l')
            {
                if (this.DictionaryKeyExpected())
                {
                    this.ReadState = ReadState.Error;
                    throw new BencodeParseException("Expected a string for dictionary's key, found an array instead.");
                }

                this.tokenTypeStack.Push(new BencodeParseStackState(BencodeToken.StartArray));
                this.TokenType = BencodeToken.StartArray;
                return true;
            }

            // Handle the end of a list or a dictionary
            if (readChar == 'e')
            {
                var dictionaryKeyExpected = this.DictionaryKeyExpected();

                if (!this.tokenTypeStack.Any())
                {
                    this.ReadState = ReadState.Error;
                    throw new BencodeParseException("Unexpected termination character 'e'.");
                }

                var currentType = this.tokenTypeStack.Pop();

                switch (currentType.CurrenToken)
                {
                    case BencodeToken.StartDictionary:
                        if (!dictionaryKeyExpected)
                        {
                            this.ReadState = ReadState.Error;
                            throw new BencodeParseException("Expected a value for the key-value pair in dictionary, instead found end of dictionary.");
                        }

                        this.TokenType = BencodeToken.EndDictionary;
                        break;
                    case BencodeToken.StartArray:
                        this.TokenType = BencodeToken.EndArray;
                        break;
                    default:
                        // Nothing else should have an ending 
                        this.ReadState = ReadState.Error;
                        throw new BencodeParseException("Unexpected termination character 'e'.");
                }

                return true;
            }

            // Handle a string, e.g. "4:name"
            if (IsDigit(readChar))
            {
                // Check whether we are in a dictionary and expecting a key.
                this.TokenType = this.DictionaryKeyExpected() ? BencodeToken.DictionaryKey : BencodeToken.String;

                var numString = readChar + this.ReadDigitsToEnd();

                this.ReadAndVerifyExpectedCharacter(':');
                
                var strLen = int.Parse(numString);

                var chars = new byte[strLen];

                for (var i = 0; i < strLen; i++)
                {
                    readCharInt = this.textReader.Read();
                    if (readCharInt < 0)
                    {
                        this.ReadState = ReadState.Error;
                        throw new BencodeParseException("Unexpected end of BEncode document.");
                    }

                    chars[i] = (byte)readCharInt;
                }

                this.TokenByteStringValue = chars;

                return true;
            }

            // Handle an integer, e.g. "i2345e"
            if (readChar == 'i')
            {
                if (this.DictionaryKeyExpected())
                {
                    this.ReadState = ReadState.Error;
                    throw new BencodeParseException("Expected a string for dictionary's key, found an int instead.");
                }

                this.TokenType = BencodeToken.Integer;

                string num;

                // Peek at the textReader to see if the integer starts with '-'.
                var peekValue = this.textReader.Peek();
                if (peekValue > -1 && Convert.ToChar(peekValue) == '-')
                {
                    this.textReader.Read();
                    num = "-" + this.ReadDigitsToEnd();
                }
                else
                {
                    num = this.ReadDigitsToEnd();
                }

                if (num.Length == 0 || num == "-")
                {
                    this.ReadState = ReadState.Error;
                    throw new BencodeParseException("Unexpected value while extracting a Bencode integer.");
                }

                this.ReadAndVerifyExpectedCharacter('e');

                this.TokenStringValue = num;
                
                return true;
            }

            this.ReadState = ReadState.Error;
            throw new BencodeParseException($"Unexpected character '{readChar}' found while parsing Bencode document.");
        }

        /// <summary>
        /// Identifies whether the current key is expected to be a 
        /// </summary>
        /// <returns>True if the next token is expected to be a dictionary key, false otherwise.</returns>
        private bool DictionaryKeyExpected()
        {
            // If the parser is not inside any structure, return false.
            if (!this.tokenTypeStack.Any())
            {
                return false;
            }

            var topOfStack = this.tokenTypeStack.Peek();
            return topOfStack.CurrenToken == BencodeToken.StartDictionary && topOfStack.DictionaryKeyExpected;
        }

        /// <summary>
        /// If the reader is currently reading through a dictionary, 
        /// the type of token needs to be inverted every other time to switch between key and value type.
        /// </summary>
        private void SwapDictionaryKeyState()
        {
            if (this.tokenTypeStack.Any())
            {
                var topOfStack = this.tokenTypeStack.Peek();

                if (topOfStack.CurrenToken == BencodeToken.StartDictionary)
                {
                    topOfStack.DictionaryKeyExpected = !topOfStack.DictionaryKeyExpected;
                }
            }
        }

        /// <summary>
        /// Reads all the characters from the textReader as long as they are digits. 
        /// After this has run, textReader will be pointing at the first following non-digit value.
        /// </summary>
        /// <returns></returns>
        private string ReadDigitsToEnd()
        {
            var str = new StringBuilder();
            var peek = this.textReader.Peek();

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
                        this.textReader.Read();

                        // Peek at the next value
                        peek = this.textReader.Peek();
                    }
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// Reads a single character from the textReader and ensures it exists and matches the expected one.
        /// </summary>
        /// <param name="expected">Expected character.</param>
        private void ReadAndVerifyExpectedCharacter(char expected)
        {
            var readCharInt = this.textReader.Read();
            if (readCharInt < 0)
            {
                this.ReadState = ReadState.Error;
                throw new BencodeParseException("Unexpected end of Bencode document.");
            }

            var readChar = Convert.ToChar(readCharInt);

            if (readChar != expected)
            {
                this.ReadState = ReadState.Error;
                throw new BencodeParseException($"Unexpected character found while parsing Bencode document. Expected '{expected}', encountered '{readChar}'.");
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
 