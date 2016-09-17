namespace netBencodeReader
{
    public enum BencodeToken
    {
        None,
        StartDictionary,
        StartArray,
        Integer,
        String,
        EndDictionary,
        EndArray,
    }
}
