// -----------------------------------------------------------------------
// <copyright file="BencodeException.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader.Exceptions
{
    using System;

    public class BencodeException : Exception
    {
        public BencodeException(string message)
            : base(message)
        {
        }
    }
}
