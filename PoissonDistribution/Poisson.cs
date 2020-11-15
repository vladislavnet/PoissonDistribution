using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoissonDistribution
{
    public class Poisson
    {
        private List<double> probabilities;
        public Poisson(int countTests, double probability)
        {
            CountTests = countTests;
            Probability = probability;
           
            //double[] factmas = new double[20];
            //for (int i = 0; i < factmas.Length; i++)
            //{
            //    factmas[i] = factorial(i + 1);
            //}
        }
        public int CountTests { get; }
        public double Probability { get; }
        public double Disperion => CountTests * Probability;

        public double[] getProbabilities()
        {
            //double[] probalities = new double[M];
            //for(int i = 0; i < M; i++)
            //{
            //    probalities[i] = Math.Pow(Disperion, i + 1) * Math.Exp(-1.0 * Disperion) / factorial(i + 1);
            //}
            //return probalities;
            probabilities = new List<double>();
            int countM = 0;
            double probMax = 1.0;
            while(probMax > 0.000009)
            {
                double prob = Math.Pow(Disperion, countM) * Math.Exp(-1.0 * Disperion) / factorial(countM);
                probabilities.Add(prob);
                countM++;
                probMax -= prob;
            }
            return probabilities.ToArray();
        }
        public string[] getMCollection()
        {
            string[] mStrings = new string[probabilities.Count()];
            for(int i = 0; i < probabilities.Count(); i++)
            {
                mStrings[i] = i.ToString();
            }
            return mStrings;
        }

        private double factorial(double x)
        {
            if (x == 0)
                return 1;
            else
                return x * factorial(x - 1);
        }


    }
}
