using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab1
{
    public class Chromosome
    {
        public string key;
        public string decryptedText;
        private Dictionary<string, double> bigramDistribution;
        private Dictionary<string, double> trigramDistribution;
        public double fitness;

        public Chromosome(string key, string encryptedText, Dictionary<string, double> englishTextBigramDistribution,
            Dictionary<string, double> englishTextTrigramDistribution)
        {
            this.key = key;
            DecodeText(encryptedText);

            bigramDistribution = new Dictionary<string, double>();
            trigramDistribution = new Dictionary<string, double>();
            /*foreach (var t in englishTextBigramDistribution.Keys)
            {
                bigramDistribution[t] = 0;
            }*/
            CalculateBigramDistribution(englishTextBigramDistribution.Keys.ToList());
            CalculateTrigramDistribution(englishTextTrigramDistribution.Keys.ToList());
            CalculateFitness(englishTextBigramDistribution, englishTextTrigramDistribution);
        }

        private void DecodeText(string encryptedText)
        {
            var charArray = new char[encryptedText.Length];
            for (var i = 0; i < key.Length; i++)
            {
                for (var j = 0; j < encryptedText.Length; j++)
                {
                    if (key[i] == encryptedText[j])
                    {
                        charArray[j] = (char) ('A' + i);
                    }
                }
            }

            decryptedText = new string(charArray);
        }

        private void CalculateBigramDistribution(List<string> allBigram)
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

            foreach (var bigram in allBigram)
            {
                if (bigramDistribution.ContainsKey(bigram))
                {
                    bigramDistribution[bigram] = bigramDistribution[bigram] / bigramCount;
                }
                else
                {
                    bigramDistribution[bigram] = 0;
                }
            }
        }
        
        private void CalculateTrigramDistribution(List<string> allTrigram)
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
            foreach (var trigram in allTrigram)
            {
                if (trigramDistribution.ContainsKey(trigram))
                {
                    trigramDistribution[trigram] = trigramDistribution[trigram] / trigramCount;
                }
                else
                {
                    trigramDistribution[trigram] = 0;
                }
            }
        }

        private void CalculateFitness(Dictionary<string, double> englishTextBigramDistribution,Dictionary<string, double> englishTextTrigramDistribution)
        {
            fitness = 0;
            foreach (var k in englishTextBigramDistribution.Keys)
            {
                fitness += Math.Abs(englishTextBigramDistribution[k] * 100 - bigramDistribution[k] * 100);
            }
            foreach (var k in englishTextTrigramDistribution.Keys)
            {
                fitness += Math.Abs(englishTextTrigramDistribution[k] * 100 - trigramDistribution[k] * 100);
            }
        }
    }
}