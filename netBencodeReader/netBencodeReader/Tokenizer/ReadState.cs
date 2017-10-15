// -----------------------------------------------------------------------
// <copyright file="ReadState.cs">
//   Copyright Rendijs Smukulis.
// </copyright>
// -----------------------------------------------------------------------

namespace netBencodeReader.Tokenizer
{
    public enum ReadState
    {
        /// <summary>
        /// Initial state, before the first read has hapened.
        /// </summary>
        Initial,

        /// <summary>
        /// The reading of the Bencode document has started and the document has not been exhausted yet.
        /// </summary>
        InProgress,

        /// <summary>
        /// EndOfFile, when the end of the Bencode document has been reached successfully.
        /// </summary>
        EndOfFile,

        /// <summary>
        /// Error has occured while parsing the Bencode document, and further reading can not take place.
        /// </summary>
        Error, 
    }
}
