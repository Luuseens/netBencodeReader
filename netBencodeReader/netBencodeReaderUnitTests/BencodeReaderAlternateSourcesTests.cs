// -----------------------------------------------------------------------
// <copyright file="BencodeReaderAlternateSourcesTests.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReaderUnitTests
{
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using netBencodeReader;

    [TestClass]
    public class BencodeReaderAlternateSourcesTests
    {
        /// <summary>
        /// Attempts to tokenize the Bencode document using a byte array
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifySimpleMixOfAllEntityTypes_ByteArray()
        {
            var tokenizer = BencodeReader.Create(Encoding.ASCII.GetBytes("d4:name5:randy3:agei29e4:miscli1e2:lvee"));

            ValidateAllEntityMix(tokenizer);
        }
        
        /// <summary>
        /// Attempts to tokenize the Bencode document using a string
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifySimpleMixOfAllEntityTypes_String()
        {
            var tokenizer = BencodeReader.Create("d4:name5:randy3:agei29e4:miscli1e2:lvee");

            ValidateAllEntityMix(tokenizer);
        }

        /// <summary>
        /// Attempts to tokenize the Bencode document using a stream
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifySimpleMixOfAllEntityTypes_Stream()
        {
            var tokenizer = BencodeReader.Create(new MemoryStream(Encoding.ASCII.GetBytes("d4:name5:randy3:agei29e4:miscli1e2:lvee")));

            ValidateAllEntityMix(tokenizer);
        }

        /// <summary>
        /// Attempts to tokenize the Bencode document using a stream reader
        /// </summary>
        [TestMethod]
        public void BencodeReader_UnitTests_VerifySimpleMixOfAllEntityTypes_StreamReader()
        {
            var tokenizer = BencodeReader.Create(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes("d4:name5:randy3:agei29e4:miscli1e2:lvee"))));

            ValidateAllEntityMix(tokenizer);
        }

        private static void ValidateAllEntityMix(BencodeReader tokenizer)
        {
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

            // Ensure the reader is in the correct end state
            Assert.IsFalse(tokenizer.Read());
            Assert.AreEqual(ReadState.EndOfFile, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.None, tokenizer.TokenType);
            Assert.AreEqual(string.Empty, tokenizer.TokenStringValue);
        }
    }
}
