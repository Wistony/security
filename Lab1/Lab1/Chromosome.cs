using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
           
            CalculateBigramDistribution();
            CalculateTrigramDistribution();
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
        private void CalculateFitness(Dictionary<string, double> englishTextBigramDistribution,Dictionary<string, double> englishTextTrigramDistribution)
        {
            fitness = 0;
            foreach (var bigram in englishTextBigramDistribution.Keys)
            {
                if (bigramDistribution.ContainsKey(bigram))
                {
                    fitness += Math.Abs(englishTextBigramDistribution[bigram] - bigramDistribution[bigram]) * 100;
                }
                else
                {
                    fitness += englishTextBigramDistribution[bigram] * 100;
                }
            }
            
            foreach (var trigram in englishTextTrigramDistribution.Keys)
            {
                if (trigramDistribution.ContainsKey(trigram))
                {
                    fitness += Math.Abs(englishTextTrigramDistribution[trigram] - trigramDistribution[trigram]) * 200;
                }
                else
                {
                    fitness += englishTextTrigramDistribution[trigram] * 200;
                }
            }
        }
    }
}