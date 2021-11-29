using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace Lab1
{
    class Program
    {
        private static Dictionary<char, double> occurrence_english = new Dictionary<char, double>()
        {
            {'a', 8.2389258}, {'b', 1.5051398}, {'c', 2.8065007}, {'d', 4.2904556},
            {'e', 12.813865}, {'f', 2.2476217}, {'g', 2.0327458}, {'h', 6.1476691},
            {'i', 6.1476691}, {'j', 0.1543474}, {'k', 0.7787989}, {'l', 4.0604477},
            {'m', 2.4271893}, {'n', 6.8084376}, {'o', 7.5731132}, {'p', 1.9459884},
            {'q', 0.0958366}, {'r', 6.0397268}, {'s', 6.3827211}, {'t', 9.1357551},
            {'u', 2.7822893}, {'v', 0.9866131}, {'w', 2.3807842}, {'x', 0.1513210},
            {'y', 1.9913847}, {'z', 0.0746517}
        };
        
        
        static void Main(string[] args)
        {
            //Task1();
            Task2();



        }

        private static string Xor(string text, string key)
        {
            var result = new StringBuilder();

            for (int c = 0; c < text.Length; c++)
                result.Append((char)((text[c] ^ (uint)key[c % key.Length])));

            return result.ToString();
        }


        static void Task1()
        {
            string text =
                "Now to the actual tasks. All of them are graded based on the code you write, so no point in stealing deciphered text from your classmates.";
            for (var i = 0; i < 256; i++)
            {
                Console.WriteLine("Expected key - " + i);
                Decode_Xor(Xor(text, i.ToString()));
                Console.WriteLine("------------------------------------------------------------");

            }
        }

        private static void Decode_Xor(string encryptedText)
        {
            var fittingCoef = new Dictionary<int, double>();
            for (var i = 0; i < 256; i++)
            {
                var decryptedText = Xor(encryptedText, i.ToString());
                var onlyEnglishCharacter = delete_non_english_character(decryptedText);
                fittingCoef[i] = calculate_fitting_coef(onlyEnglishCharacter, decryptedText.Length);
            }

            var min = fittingCoef.Min(x => x.Value);

            foreach (var pair in fittingCoef)
            {
                if (pair.Value == min)
                {
                    Console.WriteLine("Key: " + pair.Key);
                    Console.WriteLine(Xor(encryptedText, pair.Key.ToString()));
                }
            }
        }

        private static string delete_non_english_character(string text)
        {
            return Regex.Replace(text, @"[^a-zA-Z]+", string.Empty);
        }

        private static double calculate_fitting_coef(string text, int len)
        {
            double sumDeviation = 0;
            foreach (var letter in occurrence_english.Keys)
            {
                var letterOccurrence = text.Count(c => c == letter) * 100.0 / len;
                sumDeviation += Math.Abs(occurrence_english[letter] - letterOccurrence);
            }

            return sumDeviation / occurrence_english.Count;
        }
        
        static void Task2()
        {
            var text =
                "7958401743454e1756174552475256435e59501a5c524e176f786517545e475f5245191772195019175e4317445f58425b531743565c521756174443455e595017d5b7ab5f525b5b58174058455b53d5b7aa175659531b17505e41525917435f52175c524e175e4417d5b7ab5c524ed5b7aa1b174f584517435f5217515e454443175b524343524517d5b7ab5fd5b7aa17405e435f17d5b7ab5cd5b7aa1b17435f5259174f584517d5b7ab52d5b7aa17405e435f17d5b7ab52d5b7aa1b17435f525917d5b7ab5bd5b7aa17405e435f17d5b7ab4ed5b7aa1b1756595317435f5259174f58451759524f4317545f564517d5b7ab5bd5b7aa17405e435f17d5b7ab5cd5b7aa175650565e591b17435f525917d5b7ab58d5b7aa17405e435f17d5b7ab52d5b7aa1756595317445817585919176e5842175a564e17424452175659175e5953524f1758511754585e59545e53525954521b177f565a5a5e595017535e4443565954521b177c56445e445c5e17524f565a5e5956435e58591b17444356435e44435e54565b17435244434417584517405f564352415245175a52435f5853174e5842175152525b174058425b5317445f584017435f52175552444317455244425b4319";
            string s = Encoding.ASCII.GetString(FromHex(text));

            for (int i = 0; i < 256; i++)
            {
                Console.WriteLine("Key: " + i);
                Console.WriteLine(Xor(s,i.ToString()));
                Console.WriteLine("--------------------------------------------");
            }
        }
        
        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}