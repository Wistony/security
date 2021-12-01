using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab1
{
    public class GeneticAlgorithm
    {
        private const int PopulationSize = 70;
        private double mutationRate = 0.5; //percent
        private int numOfSwaps = 2;
        private string Alphabet;
        private string encryptedText;
        private List<Chromosome> population;

        private Dictionary<string, double> bigramDistribution;
        private Dictionary<string, double> trigramDistribution;

        private Chromosome bestKey;
        private int withoutNewBest;

        public GeneticAlgorithm(string encryptedText)
        {
            this.encryptedText = encryptedText;
            population = new List<Chromosome>();
            Alphabet = "";
            withoutNewBest = 0;

            CalculateBigramDistribution();
            CalculateTrigramDistribution();

            for (var i = 0; i < 26; i++)
            {
                Alphabet += (char) ('A' + i);
            }
            for (var i = 0; i < PopulationSize; i++)
            {
                var randomKey = GenerateRandomKeys();
                population.Add(new Chromosome(randomKey, encryptedText,bigramDistribution,trigramDistribution));
            }

            bestKey = population[0];
            run();
        }

        public void run()
        {
            for (var i = 0; i < 100000; i++)
            {
                Crossover();
                if (i % 10 == 0)
                {
                    Console.WriteLine("=========== " + i + "==============");
                    Console.WriteLine("Without New Best: " + withoutNewBest);
                    Console.WriteLine("BestFitness: " + bestKey.fitness);
                    Console.WriteLine("Key: " + bestKey.key);
                    Console.WriteLine("Text: " + bestKey.decryptedText);
                }

                if (withoutNewBest > 50 && numOfSwaps < 10)
                {
                    withoutNewBest = 0;
                    numOfSwaps += 1;
                }

                if (withoutNewBest > 200)
                {
                    withoutNewBest = 0;
                    population.Sort((a, b) => a.fitness >= b.fitness ? 1 : -1);
                    var list = new List<Chromosome>();
                    for (var j = 0; j < PopulationSize; j++)
                    {
                        if (j < 0.2 * PopulationSize)
                        {
                            list.Add(population[j]);
                        }
                        else
                        {
                            var randomKey = GenerateRandomKeys();
                            list.Add(new Chromosome(randomKey, encryptedText,bigramDistribution,trigramDistribution));
                        }
                    }

                    population = list;
                }
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

        public string CrossTwoParent(Chromosome parent1, Chromosome parent2)
        {
            var r = new Random();
            var rInt = r.Next(1, 26);
            
            var newKey = parent1.key[..rInt];
            foreach (var ch in parent2.key)
            {
                if (!newKey.Contains(ch))
                    newKey += ch;
            }

            return newKey;
        }
        
        public void Crossover()
        {
            var children = new List<Chromosome>();
            for (var i = 0; i < PopulationSize; i++)
            {
                var (parent1, parent2) = TournamentSelection();
                
                var childKey1 = CrossTwoParent(parent1, parent2);
                while(children.Any(ch => ch.key == childKey1))
                {
                    (parent1, parent2) = TournamentSelection();
                    childKey1 = CrossTwoParent(parent1, parent2);
                }
                children.Add(new Chromosome(childKey1,encryptedText,bigramDistribution, trigramDistribution));
                
                var childKey2 = CrossTwoParent(parent2, parent1);

                while(children.Any(ch => ch.key == childKey2))
                {
                    (parent1, parent2) = TournamentSelection();
                    childKey2 = CrossTwoParent(parent2, parent1);
                }
                
                children.Add(new Chromosome(childKey2,encryptedText,bigramDistribution, trigramDistribution));
            }

            var childrenAndParent = children.Concat(population).ToList();
            childrenAndParent.Sort((a, b) => a.fitness >= b.fitness ? 1 : -1);

            var newGeneration = new List<Chromosome>();
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

            for (var i = 0; i < numOfSwaps; i++)
            {
                swapGenes(rChrom);
            }
            
        }

        public void swapGenes(int rChrom)
        {
            var r = new Random();

            var rGen = r.Next(0, 26);
            var rGen1 = r.Next(0, 26);
            while (rGen == rGen1)
            {
                rGen1 = r.Next(0, 26);
            }

            var ch = population[rChrom].key;

            StringBuilder str = new StringBuilder(ch);
            var temp = str[rGen];
            str[rGen] = str[rGen1];
            str[rGen1] = temp;

            population[rChrom] = new Chromosome(str.ToString(), encryptedText, bigramDistribution,trigramDistribution);
        }

        public (Chromosome, Chromosome) TournamentSelection()
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
            while (parent1.key == parent2.key && count < 3)
            {
                rInt1 = r.Next(0, PopulationSize);
                rInt2 = r.Next(0, PopulationSize);
                parent1 = population[rInt1].fitness >= population[rInt2].fitness ? population[rInt2] : population[rInt1];
                count++;
            }

            return (parent1, parent2);
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