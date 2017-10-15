// -----------------------------------------------------------------------
// <copyright file="BencodeReaderTests.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

namespace netBencodeReaderUnitTests
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using netBencodeReader.Tokenizer;

    [TestClass]
    public class BencodeReaderTests
    {
        /// <summary>
        /// Verifies integers can be read.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyIntegers()
        {
            var tokenizer = BencodeReader.Create(new StringReader("i24e"));
            tokenizer.Read();
            Assert.AreEqual("24", tokenizer.TokenStringValue);
            Assert.AreEqual(BencodeToken.Integer, tokenizer.TokenType);
        }
        
        /// <summary>
        /// Verifies strings can be read.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyStrings()
        {
            var tokenizer = BencodeReader.Create(new StringReader("5:hello"));
            tokenizer.Read();
            Assert.AreEqual("hello", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
        }

        /// <summary>
        /// Verifies dictionaries can be read.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyDictionaries()
        {
            var tokenizer = BencodeReader.Create(new StringReader("d3:key5:valuee"));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("key", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("value", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));
        }

        /// <summary>
        /// Verifies lists can be read.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyLists()
        {
            var tokenizer = BencodeReader.Create(new StringReader("l5:apple6:orangee"));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.StartArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("apple", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("orange", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.EndArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);
        }

        /// <summary>
        /// Attempts to tokenize the Bencode document "d4:name5:randy3:agei29e4:miscli1e2:lvee", 
        /// verifies the:
        /// - Correct bool is returned by Read() as it is called,
        /// - ReadState is returned correctly,
        /// - Correct TokenType is returned,
        /// - Correct TokenStringValue is returned
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifySimpleMixOfAllEntityTypes()
        {
            var tokenizer = BencodeReader.Create(new StringReader("d4:name5:randy3:agei29e4:miscli1e2:lvee"));

            Assert.AreEqual(ReadState.Initial, tokenizer.ReadState);

            // Pop the first 'd' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            // Pop the string '4:name' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("name", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            // Pop the string '5:randy' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("randy", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            // Pop the string '3:age' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("age", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            // Pop the int 'i29e' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.Integer, tokenizer.TokenType);
            Assert.AreEqual("29", tokenizer.TokenStringValue);

            // Pop the string '4:misc' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("misc", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            // Pop the beginning of the list 'l' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.StartArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            // Pop the integer 'i1e' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.Integer, tokenizer.TokenType);
            Assert.AreEqual("1", tokenizer.TokenStringValue);

            // Pop the sring '2:lv' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("lv", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            // Pop the end of array 'e' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.EndArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            // Pop the end of dictionary 'e' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            // Ensure the reader is in the correct end state
            Assert.IsFalse(tokenizer.Read());
            Assert.AreEqual(ReadState.EndOfFile, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.None, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);
        }

        /// <summary>
        /// Verifies the value of negative integers can be read.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyNegativeIntegers()
        {
            var tokenizer = BencodeReader.Create(new StringReader("i-24e"));
            tokenizer.Read();
            Assert.AreEqual("-24", tokenizer.TokenStringValue);
            Assert.AreEqual(BencodeToken.Integer, tokenizer.TokenType);
        }

        /// <summary>
        /// Verifies empty lists are considered valid.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyEmptyList()
        {
            var tokenizer = BencodeReader.Create(new StringReader("le"));
            tokenizer.Read();
            Assert.AreEqual(BencodeToken.StartArray, tokenizer.TokenType);

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.EndArray, tokenizer.TokenType);
        }

        /// <summary>
        /// Verifies empty dictionaries are considered valid.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyEmptyDictionary()
        {
            var tokenizer = BencodeReader.Create(new StringReader("de"));
            tokenizer.Read();
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
        }

        /// <summary>
        /// Verifies empty strings are considered valid.
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyEmptyStrings()
        {
            var tokenizer = BencodeReader.Create(new StringReader("0:"));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsFalse(tokenizer.Read());

            tokenizer = BencodeReader.Create(new StringReader("1:a0:2:bb"));
            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("a", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("bb", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsFalse(tokenizer.Read());
        }

        /// <summary>
        /// Attempts to tokenize the bencode document with deeper dictionary structure. 
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifyDeepDictionaries()
        {
            /*  d
                  1:a 
                  1:b

                  1:c 
                  d
                    2:ca
                    2:cb
  
                    3:cc
                    d
                      3:cca
                      de
                    e
  
                    2:cd
                    l 
                      3:cda
                      3:cdb
                      3:cdc
                    e
                  e

                  1:d
                  1:e
                e            */

            var tokenizer = BencodeReader.Create(new StringReader("d1:a1:b1:cd2:ca2:cb2:ccd3:ccadee2:cdl3:cda3:cdb3:cdcee1:d1:ee"));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("a", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("b", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("c", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("ca", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cb", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("cc", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("cca", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("cd", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cda", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cdb", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cdc", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("d", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("e", Encoding.ASCII.GetString(tokenizer.TokenByteStringValue));

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsFalse(tokenizer.Read());
            Assert.AreEqual(ReadState.EndOfFile, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.None, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);
        }
    }
}
