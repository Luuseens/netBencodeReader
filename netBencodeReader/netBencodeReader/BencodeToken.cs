// -----------------------------------------------------------------------
// <copyright file="BencodeToken.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader
{
    public enum BencodeToken
    {
        None,

        StartDictionary,

        StartArray,

        Integer,

        String,

        DictionaryKey,

        EndDictionary,

        EndArray,
    }
}
