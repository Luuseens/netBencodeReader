// -----------------------------------------------------------------------
// <copyright file="BencodeWriterTests.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using netBencodeReader.Tokenizer;
using netBencodeReader.Types;

namespace netBencodeReaderUnitTests
{
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using netBencodeReader.InternalDeser;

    [TestClass]
    public class IntermediateDeserializerTests
    {
        /// <summary>
        /// Verifies GetBaseObject parses numbers.
        /// </summary>
        [TestMethod]
        public void IntermediateDeserializer_UnitTest_Numbers()
        {
            var tokenizer = BencodeReader.Create(new StringReader("i24e"));
            var deser = new Deserializer();
            var num = deser.GetBaseObject(tokenizer);
            
            Assert.IsInstanceOfType(num, typeof(BENumber));
            Assert.AreEqual(24, (num as BENumber).ToInt());
            Assert.AreEqual(24L, (num as BENumber).ToLong());
            Assert.AreEqual("24", num.ToString());

            tokenizer = BencodeReader.Create(new StringReader("i-1e"));
            num = deser.GetBaseObject(tokenizer);

            Assert.IsInstanceOfType(num, typeof(BENumber));
            Assert.AreEqual(-1, (num as BENumber).ToInt());
            Assert.AreEqual(-1L, (num as BENumber).ToLong());
            Assert.AreEqual("-1", num.ToString());
        }

        /// <summary>
        /// Verifies GetBaseObject parses strings.
        /// </summary>
        [TestMethod]
        public void IntermediateDeserializer_UnitTest_Strings()
        {
            var tokenizer = BencodeReader.Create(new StringReader("5:hello"));
            var deser = new Deserializer();
            var str = deser.GetBaseObject(tokenizer);

            Assert.IsInstanceOfType(str, typeof(BEByteString));
            Assert.AreEqual("hello", Encoding.ASCII.GetString((str as BEByteString).Value));
            Assert.AreEqual("hello", str.ToString());
        }

        /// <summary>
        /// TBD
        /// </summary>
        [TestMethod]
        public void IntermediateDeserializer_UnitTest_TBD()
        {
            var tokenizer = BencodeReader.Create(new StringReader("d3:key5:valuee"));
            var deser = new Deserializer();
            var dict = deser.GetBaseObject(tokenizer);

            tokenizer = BencodeReader.Create(new StringReader("d1:a1:b1:cd2:ca2:cb2:ccd3:ccadee2:cdl3:cda3:cdb3:cdcee1:d1:ee"));
            dict = deser.GetBaseObject(tokenizer);
        }
    }
}
