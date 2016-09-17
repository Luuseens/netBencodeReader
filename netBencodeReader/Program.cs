using System;

namespace netBencodeReader
{
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            var tokenizer = BencodeReader.Create(new StringReader("d4:name5:randy3:agei29e4:miscli1e2:lvee"));

            while (tokenizer.Read())
            {
                Console.WriteLine($"Type: {tokenizer.TokenType}, Value: {tokenizer.TokenStringValue}");
            }

            Console.ReadLine();
        }
    }
}
