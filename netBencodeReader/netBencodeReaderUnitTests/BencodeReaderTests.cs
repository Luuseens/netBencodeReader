// -----------------------------------------------------------------------
// <copyright file="BencodeReaderTests.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReaderUnitTests
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using netBencodeReader;

    [TestClass]
    public class BencodeReaderTests
    {
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
            Assert.AreEqual("name", tokenizer.TokenStringValue);

            // Pop the string '5:randy' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("randy", tokenizer.TokenStringValue);

            // Pop the string '3:age' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("age", tokenizer.TokenStringValue);

            // Pop the int 'i29e' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.Integer, tokenizer.TokenType);
            Assert.AreEqual("29", tokenizer.TokenStringValue);

            // Pop the string '4:misc' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("misc", tokenizer.TokenStringValue);

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
            Assert.AreEqual("lv", tokenizer.TokenStringValue);

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

            Assert.IsFalse(tokenizer.Read());
            Assert.AreEqual(ReadState.EndOfFile, tokenizer.ReadState);
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
            Assert.AreEqual("a", tokenizer.TokenStringValue);

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            tokenizer.Read();
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("bb", tokenizer.TokenStringValue);

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
            Assert.AreEqual("a", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("b", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("c", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("ca", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cb", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("cc", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("cca", tokenizer.TokenStringValue);

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
            Assert.AreEqual("cd", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.StartArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cda", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cdb", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("cdc", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndArray, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.EndDictionary, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.DictionaryKey, tokenizer.TokenType);
            Assert.AreEqual("d", tokenizer.TokenStringValue);

            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("e", tokenizer.TokenStringValue);

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
