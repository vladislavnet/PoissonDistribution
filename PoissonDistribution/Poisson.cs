using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoissonDistribution
{
    public class Poisson
    {
        public Poisson(int countTests, double probability, int m)
        {
            CountTests = countTests;
            Probability = probability;
            M = m;
            //double[] factmas = new double[20];
            //for (int i = 0; i < factmas.Length; i++)
            //{
            //    factmas[i] = factorial(i + 1);
            //}
        }
        public int CountTests { get; }
        public double Probability { get; }
        public int M { get; }
        public double Disperion => CountTests * Probability;

        public double[] getProbabilities()
        {
            double[] probalities = new double[M];
            for(int i = 0; i < M; i++)
            {
                probalities[i] = Math.Pow(Disperion, i + 1) * Math.Exp(-1.0 * Disperion) / factorial(i + 1);
            }
            return probalities;
        }
        public string[] getMCollection()
        {
            string[] mStrings = new string[M];
            for(int i = 1; i <= M; i++)
            {
                mStrings[i - 1] = i.ToString();
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
