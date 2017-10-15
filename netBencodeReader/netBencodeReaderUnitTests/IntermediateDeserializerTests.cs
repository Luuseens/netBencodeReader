// -----------------------------------------------------------------------
// <copyright file="BencodeWriterTests.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using netBencodeReader.Tokenizer;

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
        /// Verifies GetBaseObject doesn't throw.
        /// </summary>
        [TestMethod]
        public void IntermediateDeserializer_UnitTest_GoldenPath()
        {
            var tokenizer = BencodeReader.Create(new StringReader("i24e"));
            var deser = new Deserializer();
            var num = deser.GetBaseObject(tokenizer);

            tokenizer = BencodeReader.Create(new StringReader("5:hello"));
            var str = deser.GetBaseObject(tokenizer);

            tokenizer = BencodeReader.Create(new StringReader("d3:key5:valuee"));
            var dict = deser.GetBaseObject(tokenizer);

            tokenizer = BencodeReader.Create(new StringReader("d1:a1:b1:cd2:ca2:cb2:ccd3:ccadee2:cdl3:cda3:cdb3:cdcee1:d1:ee"));
            dict = deser.GetBaseObject(tokenizer);
        }
    }
}
