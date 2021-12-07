using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab1
{
    public class GeneticAlgorithm1
    {
        private const int PopulationSize = 50;
        private double mutationRate = 0.8; //percent
        private int numOfSwaps = 1;
        private string Alphabet;
        private string encryptedText;
        private List<Chromosome1> population;
        private int KEY;

        public Dictionary<string, double> onegramDistribution;
        public Dictionary<string, double> bigramDistribution;
        public Dictionary<string, double> trigramDistribution;

        private Chromosome1 bestKey;
        private int withoutNewBest;

        public GeneticAlgorithm1(string encryptedText)
        {
            this.encryptedText = encryptedText;
            population = new List<Chromosome1>();
            Alphabet = "";
            withoutNewBest = 0;
            KEY = 4;

            CalculateOnegramDistribution();
            CalculateBigramDistribution();
            CalculateTrigramDistribution();

            for (var i = 0; i < 26; i++)
            {
                Alphabet += (char) ('A' + i);
            }
            
            for (var i = 0; i < PopulationSize; i++)
            {
                var keys = new List<string>();
                for (var j = 0; j < KEY; j++)
                {
                    var randomKey = GenerateRandomKeys();
                    keys.Add(randomKey);
                }
                population.Add(new Chromosome1(keys, encryptedText,onegramDistribution,bigramDistribution,trigramDistribution));
            }
            
            bestKey = population[0];
            Run();
        }

        public void Run()
        {
            for (var i = 0; i < 100000; i++)
            {
                Crossover();
                if (i % 10 == 0)
                {
                    Console.WriteLine("=========== " + i + "==============");
                    Console.WriteLine("Without New Best: " + withoutNewBest);
                    Console.WriteLine("BestFitness: " + bestKey.fitness);
                    foreach (var key in bestKey.keys)
                    {
                        Console.WriteLine("Key: " + key);
                    }
                    Console.WriteLine("Text: " + bestKey.decryptedText);
                }
                
                if (withoutNewBest > 300)
                {
                    withoutNewBest = 0;
                    population.Sort((a, b) => a.fitness >= b.fitness ? 1 : -1);
                    var list = new List<Chromosome1>();
                    for (var j = 0; j < PopulationSize; j++)
                    {
                        if (j < 0.3 * PopulationSize)
                        {
                            list.Add(population[j]);
                        }
                        else
                        {
                            var keys = new List<string>();
                            for (var p = 0; p < KEY; p++)
                            {
                                var randomKey = GenerateRandomKeys();
                                keys.Add(randomKey);
                            }
                            list.Add(new Chromosome1(keys,
                            encryptedText,onegramDistribution,bigramDistribution,trigramDistribution));
                        }
                    }

                    population = list;
                }
            }
        }
        
        private void CalculateOnegramDistribution()
        {
            onegramDistribution = new Dictionary<string, double>();
            var sr =
                new StreamReader(
                    "C:\\Users\\MaksymAndroshchuk\\OneDrive - Ladoburn Europe Ltd\\Desktop\\security\\Lab1\\Lab1\\Files\\english_monograms.txt");

            var line = sr.ReadLine();
            
            while (line != null)
            {
                var str = line.Split(" ");
                onegramDistribution[str[0]] = Convert.ToDouble(str[1]);
                line = sr.ReadLine();
            }

            var sum = onegramDistribution.Sum(x => x.Value);

            foreach (var key in onegramDistribution.Keys.ToList())
            {
                onegramDistribution[key] = onegramDistribution[key] / sum;
            }
        }


        private void CalculateBigramDistribution()
        {
            bigramDistribution = new Dictionary<string, double>();
            var sr =
                new StreamReader(
                    "C:\\Users\\MaksymAndroshchuk\\OneDrive - Ladoburn Europe Ltd\\Desktop\\security\\Lab1\\Lab1\\Files\\english_bigrams.txt");

            var line = sr.ReadLine();
            
            while (line != null)
            {
                var str = line.Split(" ");
                bigramDistribution[str[0]] = Convert.ToDouble(str[1]);
                line = sr.ReadLine();
            }

            var sum = bigramDistribution.Sum(x => x.Value);

            foreach (var key in bigramDistribution.Keys.ToList())
            {
                bigramDistribution[key] = bigramDistribution[key] / sum;
            }
        }
        
        private void CalculateTrigramDistribution()
        {
            trigramDistribution = new Dictionary<string, double>();
            var sr =
                new StreamReader(
                    "C:\\Users\\MaksymAndroshchuk\\OneDrive - Ladoburn Europe Ltd\\Desktop\\security\\Lab1\\Lab1\\Files\\english_trigrams.txt");
            
            var line = sr.ReadLine();
            
            while (line != null)
            {
                var str = line.Split(" ");
                trigramDistribution[str[0]] = Convert.ToDouble(str[1]);
                line = sr.ReadLine();
            }

            var sum = trigramDistribution.Sum(x => x.Value);

            foreach (var key in trigramDistribution.Keys.ToList())
            {
                trigramDistribution[key] = trigramDistribution[key] / sum;
            }
        }

        public List<string> CrossTwoParent(Chromosome1 parent1, Chromosome1 parent2)
        {
            var keys = new List<string>();
            for (var i = 0; i < KEY; i++)
            {
                var r = new Random();
                var rInt = r.Next(1, 26);

                var newKey = parent1.keys[i][..rInt];
                foreach (var ch in parent2.keys[i])
                {
                    if (!newKey.Contains(ch))
                        newKey += ch;
                }
                
                keys.Add(newKey);
            }

            return keys;
        }
        
        
        
        public void Crossover()
        {
            var children = new List<Chromosome1>();
            for (var i = 0; i < PopulationSize; i++)
            {
                var (parent1, parent2) = TournamentSelection();
                
                var childKey1 = CrossTwoParent(parent1, parent2);
                while(children.Any(ch => isEqual(ch.keys,childKey1)))
                {
                    (parent1, parent2) = TournamentSelection();
                    childKey1 = CrossTwoParent(parent1, parent2);
                }
                children.Add(new Chromosome1(childKey1,encryptedText,onegramDistribution,bigramDistribution, trigramDistribution));
                
                var childKey2 = CrossTwoParent(parent2, parent1);

                while(children.Any(ch => isEqual(ch.keys, childKey2)))
                {
                    (parent1, parent2) = TournamentSelection();
                    childKey2 = CrossTwoParent(parent2, parent1);
                }
                
                children.Add(new Chromosome1(childKey2,encryptedText,onegramDistribution,bigramDistribution, trigramDistribution));
            }

            var childrenAndParent = children.Concat(population).ToList();
            //var childrenAndParent = children;
            childrenAndParent.Sort((a, b) => a.fitness >= b.fitness ? 1 : -1);

            var newGeneration = new List<Chromosome1>();
            for (var i = 0; i < PopulationSize; i++)
            {
                newGeneration.Add(childrenAndParent[i]);
            }

            if (bestKey.fitness > newGeneration[0].fitness)
            {
                bestKey = newGeneration[0];
                withoutNewBest = 0;
            }
            else
            {
                withoutNewBest += 1;
            }
            
            

            
            var rnd = new Random();
            population = newGeneration.OrderBy(item => rnd.Next()).ToList();
           
            for (var i = 0; i < PopulationSize * mutationRate; i++)
            {
                mutation();
            }
        }

        public void mutation()
        {
            var r = new Random();
            var rChrom = r.Next(0, PopulationSize);
            var keys = new List<string>(population[rChrom].keys);
            for (var i = 0; i < 2; i++)
            {
                var keyIndex = r.Next(0, KEY);
                //Console.WriteLine(population[rChrom].keys.Count);
                var newKey = swapGenes(population[rChrom].keys[keyIndex]);
                keys[keyIndex] = newKey;
            }
            population[rChrom] = new Chromosome1(keys, encryptedText, onegramDistribution,bigramDistribution, trigramDistribution);
        }
        
        /*public void mutation()
        {
            var r = new Random();
            var rChrom = r.Next(0, PopulationSize);
            var keys = new List<string>();
            for (var i = 0; i < KEY; i++)
            {
                var newKey = swapGenes(population[rChrom].keys[i]);
                keys.Add(newKey);
            }
            population[rChrom] = new Chromosome1(keys, encryptedText, onegramDistribution,bigramDistribution, trigramDistribution);
        }
        */

        public string swapGenes(string key)
        {
            var r = new Random();
            var str = new StringBuilder(key);

            for (var i = 0; i < numOfSwaps; i++)
            {
                var rGen = r.Next(0, 26);
                var rGen1 = r.Next(0, 26);
                while (rGen == rGen1)
                {
                    rGen1 = r.Next(0, 26);
                }
                var temp = str[rGen];
                str[rGen] = str[rGen1];
                str[rGen1] = temp;
            }

            return str.ToString();
        }

        public (Chromosome1, Chromosome1) TournamentSelection()
        {
            var r = new Random();
            var rInt1 = r.Next(0, PopulationSize);
            var rInt2 = r.Next(0, PopulationSize);
            var parent1 = population[rInt1].fitness >= population[rInt2].fitness ?
                population[rInt2] : population[rInt1];
            
            rInt1 = r.Next(0, PopulationSize);
            rInt2 = r.Next(0, PopulationSize);
            
            var parent2 = population[rInt1].fitness >= population[rInt2].fitness ?
                population[rInt2] : population[rInt1];

            var count = 0;
            while (isEqual(parent1.keys,parent2.keys) && count < 5)
            {
                rInt1 = r.Next(0, PopulationSize);
                rInt2 = r.Next(0, PopulationSize);
                parent1 = population[rInt1].fitness >= population[rInt2].fitness ? population[rInt2] : population[rInt1];
                count++;
            }

            return (parent1, parent2);
        }

        private bool isEqual(List<string> ch1, List<string> ch2)
        {
            var isEqual = true;
            for (var i = 0; i < KEY; i++)
            {
                isEqual = isEqual && ch1[i] == ch2[i];
            }

            return isEqual;
        }
        private string GenerateRandomKeys()
        {
            var q = from c in Alphabet.ToCharArray()
                orderby Guid.NewGuid()
                select c;
            var s = string.Empty;
            foreach (var r in q)
                s += r;

            return s;
        }
    }
}