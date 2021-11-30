using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace Lab1
{
    class Program
    {
        private static readonly Dictionary<char, double> OccurrenceEnglish = new Dictionary<char, double>()
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
            //Task2();
            Task3();


        }
        static void Task1()
        {
            const string text = "Now to the actual tasks. All of them are graded based on the code you write, so no point in stealing deciphered text from your classmates.";
            for (var i = 0; i < 256; i++)
            {
                Console.WriteLine("Expected key - " + i);
                decode_xor(Xor(text, i));
                Console.WriteLine("------------------------------------------------------------");

            }
        }
        
        static void Task2()
        {
            const string text = "7958401743454e1756174552475256435e59501a5c524e176f786517545e475f5245191772195019175e4317445f58425b531743565c521756174443455e595017d5b7ab5f525b5b58174058455b53d5b7aa175659531b17505e41525917435f52175c524e175e4417d5b7ab5c524ed5b7aa1b174f584517435f5217515e454443175b524343524517d5b7ab5fd5b7aa17405e435f17d5b7ab5cd5b7aa1b17435f5259174f584517d5b7ab52d5b7aa17405e435f17d5b7ab52d5b7aa1b17435f525917d5b7ab5bd5b7aa17405e435f17d5b7ab4ed5b7aa1b1756595317435f5259174f58451759524f4317545f564517d5b7ab5bd5b7aa17405e435f17d5b7ab5cd5b7aa175650565e591b17435f525917d5b7ab58d5b7aa17405e435f17d5b7ab52d5b7aa1756595317445817585919176e5842175a564e17424452175659175e5953524f1758511754585e59545e53525954521b177f565a5a5e595017535e4443565954521b177c56445e445c5e17524f565a5e5956435e58591b17444356435e44435e54565b17435244434417584517405f564352415245175a52435f5853174e5842175152525b174058425b5317445f584017435f52175552444317455244425b4319";
            var str = Encoding.ASCII.GetString(FromHex(text));
            decode_xor(str);
        }
        
        static void Task3()
        {
            const string encrypted_text =
                "G0IFOFVMLRAPI1QJbEQDbFEYOFEPJxAfI10JbEMFIUAAKRAfOVIfOFkYOUQFI15ML1kcJFUeYhA4IxAeKVQZL1VMOFgJbFMDIUAAKUgFOElMI1ZMOFgFPxADIlVMO1VMO1kAIBAZP1VMI14ANRAZPEAJPlMNP1VMIFUYOFUePxxMP19MOFgJbFsJNUMcLVMJbFkfbF8CIElMfgZNbGQDbFcJOBAYJFkfbF8CKRAeJVcEOBANOUQDIVEYJVMNIFwVbEkDORAbJVwAbEAeI1INLlwVbF4JKVRMOF9MOUMJbEMDIVVMP18eOBADKhALKV4JOFkPbFEAK18eJUQEIRBEO1gFL1hMO18eJ1UIbEQEKRAOKUMYbFwNP0RMNVUNPhlAbEMFIUUALUQJKBANIl4JLVwFIldMI0JMK0INKFkJIkRMKFUfL1UCOB5MH1UeJV8ZP1wVYBAbPlkYKRAFOBAeJVcEOBACI0dAbEkDORAbJVwAbF4JKVRMJURMOF9MKFUPJUAEKUJMOFgJbF4JNERMI14JbFEfbEcJIFxCbHIJLUJMJV5MIVkCKBxMOFgJPlVLPxACIxAfPFEPKUNCbDoEOEQcPwpDY1QDL0NCK18DK1wJYlMDIR8II1MZIVUCOB8IYwEkFQcoIB1ZJUQ1CAMvE1cHOVUuOkYuCkA4eHMJL3c8JWJffHIfDWIAGEA9Y1UIJURTOUMccUMELUIFIlc=";
            var byteArr = Convert.FromBase64String(encrypted_text);
            var text = Encoding.UTF8.GetString(byteArr);
            
            //get_index_of_coincidence(text);
            /*Console.WriteLine("text --- ");
            for (var i = 0; i < text.Length; i++)

            { 
                Console.WriteLine(byteArr[i] + " - " + (int)text[i]);
            }*/
            /*Console.WriteLine(text);
            for (var i = 0; i < text.Length; i++)
            {
                Console.WriteLine(i + " - " + text[i]);
            }*/
            get_each_n_character(text,3);
        }
        private static string Xor(string text, int key)
        {
            var result = new StringBuilder();

            for (var c = 0; c < text.Length; c++)
            {
                //result.Append(text[c] ^ (uint)key[c % key.Length]);
                result.Append((char) (text[c] ^ key));
                //Console.WriteLine("Res:" + ((int) (text[c] ^  key)) + " text: "
                 //                 + text[c] + " text int: " + (int)text[c] + " key:"+ key );
                
            }

            return result.ToString();
        }

        private static void decode_xor(string encryptedText)
        {
            var fittingCoef = new Dictionary<int, double>();
            for (var i = 0; i < 256; i++)
            {
                var decryptedText = Xor(encryptedText, i);
                //Console.WriteLine("-------" + i + "------------");
                //Console.WriteLine(decryptedText);
                var onlyEnglishCharacter = delete_non_english_character(decryptedText);
                fittingCoef[i] = calculate_fitting_coef(onlyEnglishCharacter, decryptedText.Length);
            }

            var min = fittingCoef.Min(x => x.Value);

            foreach (var pair in fittingCoef.Where(pair => pair.Value == min))
            {
                Console.WriteLine("Key: " + pair.Key);
                //Console.WriteLine(Encoding.ASCII.GetString(FromHex(Xor(encryptedText, pair.Key.ToString()))));
                Console.WriteLine(Xor(encryptedText, pair.Key));
                Console.WriteLine("========================================");
            }
        }

        private static string delete_non_english_character(string text)
        {
            return Regex.Replace(text, @"[^a-zA-Z]+", string.Empty);
        }

        private static double calculate_fitting_coef(string text, int len)
        {
            double sumDeviation = 0;
            foreach (var letter in OccurrenceEnglish.Keys)
            {
                var letterOccurrence = text.Count(c => c == letter) * 100.0 / len;
                sumDeviation += Math.Abs(OccurrenceEnglish[letter] - letterOccurrence);
            }

            return sumDeviation / OccurrenceEnglish.Count;
        }
        
        public static void get_each_n_character(string text, int len)
        {
            var one = "";
            var two = "";
            var three = "";
            
            for (var i = 0; i < text.Length; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        one += text[i];
                        Console.Write(text[i] + " ");
                        break;
                    case 1:
                        two += text[i];
                        break;
                    case 2:
                        three += text[i];
                        break;
                }
            }
            Console.WriteLine("Two ---- ");
            Console.WriteLine(two);
            decode_xor(one);
            //decode_xor(two);
            //decode_xor(three);
        }
        
        public static byte[] FromHex(string hex)
        {
            var raw = new byte[hex.Length / 2];
            for (var i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static void calculate_index_of_coincidence(string text)
        {
            var uniqueCharacter = new string(text.Distinct().ToArray());
            var dict = new Dictionary<int, float>();
            var letterRepeat = new Dictionary<char, int>();
            foreach (var ch in uniqueCharacter)
            {
                letterRepeat[ch] = 0;
            }
            
            for (var keyLen = 1;  keyLen < text.Length;  keyLen++)
            {
                for (var j = 0; j < text.Length; j++)
                {
                    if (text[j] == text[(keyLen + j) % text.Length])
                    {
                        letterRepeat[text[j]] += 1;
                    }
                }

                var indexOfCoincidence = 0;
                foreach (var ch in uniqueCharacter)
                {
                    var repeatCount = letterRepeat[ch];
                    letterRepeat[ch] = 0;
                    indexOfCoincidence += repeatCount * repeatCount;
                }

                dict[keyLen] = indexOfCoincidence / (float)(text.Length * text.Length);
            }

            foreach (var (key, value) in dict)
            {
                Console.WriteLine(key + " => " + value);
            }
        }
    }
}