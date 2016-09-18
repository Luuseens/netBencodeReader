// -----------------------------------------------------------------------
// <copyright file="BencodeWriter.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader
{
    using System.IO;
    using System.Text;

    public sealed class BencodeWriter
    {
        /// <summary>
        /// Stream this writer will be writing to.
        /// </summary>
        private Stream writeStream;

        /// <summary>
        /// Creates a new instance of BencodeWriter.
        /// </summary>
        /// <param name="output">Output stream to write to.</param>
        /// <returns>A new instance of BencodeWriter.</returns>
        public static BencodeWriter Create(Stream output)
        {
            return new BencodeWriter
            {
                writeStream = output
            };
        }

        /// <summary>
        /// Hide the default constructor.
        /// </summary>
        private BencodeWriter()
        {
        }

        /// <summary>
        /// Writes a string to the Bencode document.
        /// </summary>
        /// <param name="s">String to write.</param>
        public void Write(string s)
        {
            var fullString = $"{s.Length}:{s}";
            var bytes = Encoding.ASCII.GetBytes(fullString);

            this.writeStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes an array of bytes as a byte string to the Bencode document.
        /// </summary>
        /// <param name="s">String to write.</param>
        public void Write(byte[] b)
        {
            var fullString = $"{b.Length}:";
            var bytes = Encoding.ASCII.GetBytes(fullString);

            this.writeStream.Write(bytes, 0, bytes.Length);
            this.writeStream.Write(b, 0, b.Length);
        }

        /// <summary>
        /// Writes a long to the Bencode document.
        /// </summary>
        /// <param name="l">Long to write.</param>
        public void Write(long l)
        {
            var fullString = $"i{l}e";
            var bytes = Encoding.ASCII.GetBytes(fullString);

            this.writeStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes an int to the Bencode document.
        /// </summary>
        /// <param name="i">Int to write.</param>
        public void Write(int i)
        {
            var fullString = $"i{i}e";
            var bytes = Encoding.ASCII.GetBytes(fullString);

            this.writeStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the dictionary-start token to the Bencode document.
        /// </summary>
        public void WriteStartDictionary()
        {
            var bytes = Encoding.ASCII.GetBytes("d");
            this.writeStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the dictionary-end token to the Bencode document.
        /// </summary>
        public void WriteEndDictionary()
        {
            var bytes = Encoding.ASCII.GetBytes("e");
            this.writeStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the array-start token to the Bencode document.
        /// </summary>
        public void WriteStartArray()
        {
            var bytes = Encoding.ASCII.GetBytes("l");
            this.writeStream.Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// Writes the dictionary-end token to the Bencode document.
        /// </summary>
        public void WriteEndArray()
        {
            var bytes = Encoding.ASCII.GetBytes("e");
            this.writeStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Flushes the underlying stream.
        /// </summary>
        public void Flush()
        {
            this.writeStream.Flush();
        }
    }
}
 