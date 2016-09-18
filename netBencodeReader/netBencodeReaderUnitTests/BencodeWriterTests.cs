// -----------------------------------------------------------------------
// <copyright file="BencodeWriterTests.cs">
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
    public class BencodeWriterTests
    {
        /// <summary>
        /// Verifies integers can be written.
        /// </summary>
        [TestMethod]
        public void BencodeWriter_UnitTests_VerifyIntegers()
        {
            var textStream = new MemoryStream();
            var writer = BencodeWriter.Create(textStream);

            writer.Write(1);
            writer.Flush();
            
            textStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("i1e", new StreamReader(textStream).ReadToEnd());
        }
        
        /// <summary>
        /// Verifies longs can be written.
        /// </summary>
        [TestMethod]
        public void BencodeWriter_UnitTests_VerifyLongs()
        {
            var textStream = new MemoryStream();
            var writer = BencodeWriter.Create(textStream);

            writer.Write(1L);
            writer.Flush();
            
            textStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("i1e", new StreamReader(textStream).ReadToEnd());
        }
        
        /// <summary>
        /// Verifies strings can be written.
        /// </summary>
        [TestMethod]
        public void BencodeWriter_UnitTests_VerifyStrings()
        {
            var textStream = new MemoryStream();
            var writer = BencodeWriter.Create(textStream);

            writer.Write("hello world");
            writer.Flush();
            
            textStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("11:hello world", new StreamReader(textStream).ReadToEnd());
        }
     
        /// <summary>
        /// Verifies byte strings can be written.
        /// </summary>
        [TestMethod]
        public void BencodeWriter_UnitTests_VerifyByteStrings()
        {
            var textStream = new MemoryStream();
            var writer = BencodeWriter.Create(textStream);

            writer.Write(Encoding.ASCII.GetBytes("hello world"));
            writer.Flush();
            
            textStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("11:hello world", new StreamReader(textStream).ReadToEnd());
        }

        /// <summary>
        /// Verifies dictionaries can be written.
        /// </summary>
        [TestMethod]
        public void BencodeWriter_UnitTests_VerifyDictionaries()
        {
            var textStream = new MemoryStream();
            var writer = BencodeWriter.Create(textStream);

            writer.WriteStartDictionary();
            writer.Write("key");
            writer.Write("value");
            writer.WriteEndDictionary();
            writer.Flush();

            textStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("d3:key5:valuee", new StreamReader(textStream).ReadToEnd());
        }

        /// <summary>
        /// Verifies arrays can be written.
        /// </summary>
        [TestMethod]
        public void BencodeWriter_UnitTests_VerifyArrays()
        {
            var textStream = new MemoryStream();
            var writer = BencodeWriter.Create(textStream);

            writer.WriteStartArray();
            writer.Write("apple");
            writer.Write("orange");
            writer.WriteEndArray();
            writer.Flush();

            textStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("l5:apple6:orangee", new StreamReader(textStream).ReadToEnd());
        }
    }
}
