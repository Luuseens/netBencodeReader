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
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("name", tokenizer.TokenStringValue);

            // Pop the string '5:randy' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("randy", tokenizer.TokenStringValue);

            // Pop the string '3:age' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
            Assert.AreEqual("age", tokenizer.TokenStringValue);

            // Pop the int 'i29e' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.Integer, tokenizer.TokenType);
            Assert.AreEqual("29", tokenizer.TokenStringValue);

            // Pop the string '4:misc' off the document
            Assert.IsTrue(tokenizer.Read());
            Assert.AreEqual(ReadState.InProgress, tokenizer.ReadState);
            Assert.AreEqual(BencodeToken.String, tokenizer.TokenType);
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
    }
}
