using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab1
{
    public class Chromosome1
    {
        public List<string> keys;
        public string decryptedText;
        private int KEY = 4;
        
        //private Dictionary<char, List<double>> onegramDistribution;
        private Dictionary<char, double> onegramDistribution;
        private Dictionary<string, double> bigramDistribution;
        private Dictionary<string, double> trigramDistribution;
        public double fitness;

        public Chromosome1(List<string> keys, string encryptedText, 
            Dictionary<string, double> englishTextOnegramDistribution,
            Dictionary<string, double> englishTextBigramDistribution,
            Dictionary<string, double> englishTextTrigramDistribution)
        {
            this.keys = new List<string>(keys);

            DecodeText(encryptedText);

            onegramDistribution = new Dictionary<char, double>();
            bigramDistribution = new Dictionary<string, double>();
            trigramDistribution = new Dictionary<string, double>();
            
            CalculateOnegramDistribution();
            CalculateBigramDistribution();
            CalculateTrigramDistribution();
            
            CalculateFitness(englishTextOnegramDistribution,englishTextBigramDistribution, englishTextTrigramDistribution);
        }
        private void DecodeText(string encryptedText)
        {
            var charArray = new char[encryptedText.Length];
            
            for (var k = 0; k < keys.Count; k++)
            {
                for (var i = 0; i < keys[k].Length; i++)
                {
                    for (var j = k; j < encryptedText.Length; j += KEY) 
                    {
                        if (keys[k][i] == encryptedText[j])
                        {
                            charArray[j] = (char) ('A' + i);
                        }
                    }
                }
            }

            decryptedText = new string(charArray);
        }

        private void CalculateOnegramDistribution()
        {
            for (var i = 0; i < 26; i++)
            {
                var letter = (char) ('A' + i);
                onegramDistribution[letter] = 0;
            }
            
            foreach (var t in decryptedText)
            {
                onegramDistribution[t] += 1;
            }
            
            var count = decryptedText.Length;
            foreach (var letter in onegramDistribution.Keys.ToList())
            {
                onegramDistribution[letter] = onegramDistribution[letter] / count;
            }
        }

        private void CalculateBigramDistribution()
        {
            for (var i = 0; i < decryptedText.Length - 1; i++)
            {
                var bigram = decryptedText.Substring(i, 2);
                if (!bigramDistribution.ContainsKey(bigram))
                {
                    bigramDistribution[bigram] = 1;
                }
                else
                {
                    bigramDistribution[bigram] += 1;
                }
            }
            var bigramCount = decryptedText.Length - 1;
            
            foreach (var bigram in bigramDistribution.Keys.ToList())
            {
                bigramDistribution[bigram] = bigramDistribution[bigram] / bigramCount;
            }
        }
        
        private void CalculateTrigramDistribution()
        {
            for (var i = 0; i < decryptedText.Length - 2; i++)
            {
                var trigram = decryptedText.Substring(i, 3);
                if (!trigramDistribution.ContainsKey(trigram))
                {
                    trigramDistribution[trigram] = 1;
                }
                else
                {
                    trigramDistribution[trigram] += 1;
                }
            }
            var trigramCount = decryptedText.Length - 2;
            foreach (var trigram in trigramDistribution.Keys.ToList())
            {
                trigramDistribution[trigram] = trigramDistribution[trigram] / trigramCount;  
            }
        }
        private void CalculateFitness(Dictionary<string, double> englishTextOnegramDistribution,
            Dictionary<string, double> englishTextBigramDistribution,Dictionary<string, double> englishTextTrigramDistribution)
        {
            fitness = 0;
            foreach (var letter in onegramDistribution.Keys)
            {
                    fitness += Math.Abs(englishTextOnegramDistribution[letter.ToString()] -
                                        onegramDistribution[letter]) * 100;
            }
            
            foreach (var bigram in englishTextBigramDistribution.Keys)
            {
                if (bigramDistribution.ContainsKey(bigram))
                {
                    fitness += Math.Abs(englishTextBigramDistribution[bigram] - bigramDistribution[bigram]) * 200;
                }
                else
                {
                    //fitness += Math.Pow(englishTextBigramDistribution[bigram],2) * 200;
                    fitness += englishTextBigramDistribution[bigram] * 200;
                }
            }
            
            foreach (var trigram in englishTextTrigramDistribution.Keys)
            {
                if (trigramDistribution.ContainsKey(trigram))
                {
                    fitness += Math.Abs(englishTextTrigramDistribution[trigram] - trigramDistribution[trigram]) * 300;
                }
                else
                {
                    //fitness += Math.Pow(englishTextTrigramDistribution[trigram],2) * 300;
                    fitness += englishTextTrigramDistribution[trigram] * 300;

                }
            }
        }
    }
}