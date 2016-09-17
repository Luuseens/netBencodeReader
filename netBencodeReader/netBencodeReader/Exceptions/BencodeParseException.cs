// -----------------------------------------------------------------------
// <copyright file="BencodeParseException.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader.Exceptions
{
    public class BencodeParseException : BencodeException
    {
        public BencodeParseException(string message)
            : base(message)
        {
        }
    }
}
