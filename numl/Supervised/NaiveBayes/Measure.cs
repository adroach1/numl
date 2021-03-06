using numl.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace numl.Supervised.NaiveBayes
{
    public class Measure
    {
        public string Label { get; set; }
        public bool Discrete { get; set; }
        public Statistic[] Probabilities { get; set; }

        internal void Increment(double x)
        {
            var p = GetStatisticFor(x);
            if (p == null) throw new InvalidOperationException("Range not found!");
            p.Count++;
        }

        internal double GetProbability(double x)
        {
            var p = GetStatisticFor(x);
            if (p == null) throw new InvalidOperationException("Range not found!");
            return p.Probability;
        }

        internal Statistic GetStatisticFor(double x)
        {
            if (Probabilities == null || Probabilities.Length == 0)
                throw new IndexOutOfRangeException("Invalid statistics");

            var p = Probabilities.Where(s => s.X.Test(x)).FirstOrDefault();

            return p;
        }

        internal void Normalize()
        {
            double total = Probabilities.Select(p => p.Count).Sum();
            for (int i = 0; i < Probabilities.Length; i++)
                Probabilities[i].Probability = (double)Probabilities[i].Count / total;
        }

        public Measure Clone()
        {
            var m = new Measure
            {
                Label = Label,
                Discrete = Discrete
            };

            if (Probabilities != null && Probabilities.Length > 0)
            {
                m.Probabilities = new Statistic[Probabilities.Length];
                for (int i = 0; i < m.Probabilities.Length; i++)
                    m.Probabilities[i] = Probabilities[i].Clone();
            }

            return m;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Label, Discrete ? "Discrete" : "Continuous");
        }
    }
}
