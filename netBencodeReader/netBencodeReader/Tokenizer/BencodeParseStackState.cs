namespace netBencodeReader.Tokenizer
{
    internal class BencodeParseStackState
    {
        public BencodeToken CurrenToken { get; set; }

        public bool DictionaryKeyExpected { get; set; }

        public BencodeParseStackState(BencodeToken token)
        {
            this.CurrenToken = token;
        }
    }
}
