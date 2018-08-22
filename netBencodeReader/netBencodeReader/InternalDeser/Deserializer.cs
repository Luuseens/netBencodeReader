namespace netBencodeReader.InternalDeser
{
    using System;
    using System.Diagnostics;
    
    using netBencodeReader.Types;
    using netBencodeReader.Tokenizer;

    public class Deserializer
    {
        public BEBaseObject GetBaseObject(BencodeReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
            {
                reader.Read();
            }

            this.AssertReaderState(reader);

            switch (reader.TokenType)
            {
                case BencodeToken.StartDictionary:
                    var dict = this.GetDictionary(reader);
                    Debug.WriteLine(dict);
                    return dict;
                case BencodeToken.String:
                    var str = this.GetByteString(reader);
                    Debug.WriteLine(str);
                    return str;
                case BencodeToken.Integer:
                    var num = this.GetNumber(reader);
                    Debug.WriteLine(num);
                    return num;
                case BencodeToken.StartArray:
                    var arr = this.GetList(reader);
                    Debug.WriteLine(arr);
                    return arr;
            }

            throw new Exception("TBD");
        }

        public BEByteString GetByteString(BencodeReader reader)
        {
            this.AssertReaderState(reader);

            if (reader.TokenType != BencodeToken.String)
            {
                throw new Exception("TBD");
            }

            var result = new BEByteString(reader.TokenByteStringValue);

            // Advance to the next token
            reader.Read();

            return result;
        }

        public BENumber GetNumber(BencodeReader reader)
        {
            this.AssertReaderState(reader);

            if (reader.TokenType != BencodeToken.Integer)
            {
                throw new Exception("TBD");
            }

            var result = new BENumber(reader.TokenStringValue);

            // Advance to the next token
            reader.Read();

            return result;
        }

        public BEList GetList(BencodeReader reader)
        {
            this.AssertReaderState(reader);

            if (reader.TokenType != BencodeToken.StartArray)
            {
                throw new Exception("TBD");
            }

            var list = new BEList();

            // Advance past "StartArray" token
            reader.Read();

            while (reader.TokenType != BencodeToken.EndArray)
            {
                var value = this.GetBaseObject(reader);
                list.Add(value);
            }

            // Pop off the EndArray
            reader.Read();

            return list;
        }

        public BEDictionary GetDictionary(BencodeReader reader)
        {
            this.AssertReaderState(reader);

            if (reader.TokenType != BencodeToken.StartDictionary)
            {
                throw new Exception("TBD");
            }

            var dict = new BEDictionary();
            
            reader.Read();
            while (reader.TokenType != BencodeToken.EndDictionary)
            {
                if (reader.TokenType != BencodeToken.DictionaryKey)
                {
                    throw new Exception("TBD");
                }

                var key = new BEByteString(reader.TokenByteStringValue);
                reader.Read();
                
                var value = this.GetBaseObject(reader);

                dict.Add(key, value);
            }

            // Pop off the EndDictionary
            reader.Read();

            return dict;
        }

        private void AssertReaderState(BencodeReader reader)
        {
            if (reader.ReadState != ReadState.Initial && reader.ReadState != ReadState.InProgress)
            {
                throw new Exception("TBD");
            }
        }
    }
}
