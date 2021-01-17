using System;
using System.Collections.Generic;
using System.Linq;


namespace findEquation
{
    class SystemofEquation
    {
        private Dictionary<int, List<Equation>> tree = new Dictionary<int, List<Equation>>();
        private List<Equation> baselist;
        private int[] inputdata;
        private double[] unknown;

        public int[] Inputdata
        {
            get
            {
                return inputdata;
            }
            set
            {
                inputdata = value;
                Unknown = new double[inputdata.Length];
                setBaseList();
                settingSystemOfEquations();
            }
        }
        private void setBaseList()
        {
            baselist = new List<Equation>();
            int x = 1;
            int y = 1;
            for (x = 1; x <= inputdata.Length; x++)
            {
                double[] basearr = new double[inputdata.Length];
                for (y = 1; y <= inputdata.Length; y++)
                {
                    basearr[y - 1] = (double)Math.Pow(x, y);
                }
                Equation equation = new Equation();
                equation.Equations = basearr;
                equation.Solution = inputdata[x - 1];

                baselist.Add(equation);
            }
        }
        public Dictionary<int, List<Equation>> Tree { get => tree; }
        public double[] Unknown { get => unknown; set => unknown = value; }

        private void settingSystemOfEquations()
        {
            tree.Clear();
            tree.Add(0, baselist); //제일 하위 기본 식

            for (int i = 1; i < baselist.Count; i++)
            {
                List<Equation> list = tree[i - 1];
                List<Equation> equations = new List<Equation>();

                for (int j = 0; j < list.Count - 1; j++)
                {
                    Equation equation1 = list[0];
                    Equation equation2 = list[j + 1];
                    Equation equation3 = excuteSystem(equation1, equation2);

                    equations.Add(equation3);


                }
                tree.Add(i, equations);
                if (tree[i].Count == 1)
                {
                    Unknown[baselist.Count - 1] = equations[0].Solution / equations[0].Equations.Last();
                    if (double.IsNaN(Unknown[baselist.Count - 1]))
                    {
                        Unknown[baselist.Count - 1] = 0;
                    }
                }
            }
        }

        public void solve()
        {
            int i = 0;
            int j = 0;
            for (i = 0; i < baselist.Count - 1; i++)
            {
                int index = baselist.Count - 2;
                List<Equation> list = tree[index - i];
                double sum = 0;
                for (j = 0; j < list.Count - 1; j++)
                {
                    sum += Unknown[index + 1 - j] * list[0].Equations[index + 1 - j];
                }
                Unknown[index - i] = (list[0].Solution - sum) / list[0].Equations[index + 1 - j];
                if (double.IsNaN(Unknown[index - i]))
                {
                    Unknown[index - i] = 0;
                }
            }
        }

        public class Equation
        {
            private double[] equations;
            private double solution;

            public double[] Equations { get => equations; set => equations = value; }
            public double Solution { get => solution; set => solution = value; }
        }
        private Equation excuteSystem(Equation originData1, Equation originData2)
        {
            Equation returnEquation = new Equation();
            double[] targetData = new double[originData1.Equations.Length];
            bool findRemovePoint = true;
            double high = 0;
            double low = 0;
            double solution = 0;
            for (int i = 0; i < originData1.Equations.Length; i++)
            {
                if (originData1.Equations[i] != 0 && findRemovePoint)
                {
                    high = originData1.Equations[i];
                    low = originData2.Equations[i];
                    targetData[i] = high * originData2.Equations[i] - low * originData1.Equations[i];
                    findRemovePoint = false;
                }
                else
                {
                    targetData[i] = high * originData2.Equations[i] - low * originData1.Equations[i];
                }
            }
            solution = originData2.Solution * high - originData1.Solution * low;

            returnEquation.Solution = solution;
            returnEquation.Equations = targetData;

            return returnEquation;
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                List<int> strArr = new List<int>();

                SystemofEquation systemofEquation = new SystemofEquation();
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                string[] array = input.Split(",");

                for (int i = 0; i < array.Length; i++)
                {
                    strArr.Add(Convert.ToInt32(array[i]));
                }

                systemofEquation.Inputdata = strArr.ToArray();
                systemofEquation.solve();
                double[] coefficient = systemofEquation.Unknown;
                double test = 0;
                for (int i = coefficient.Length - 1; i > -1; i--)
                {
                    if (coefficient[i] != 0 && i == coefficient.Length - 1)
                    {
                        Console.Write(coefficient[i] + "x^" + (i + 1));
                        //test += (double)coefficient[i] * (double)Math.Pow(5, i + 1);

                    }
                    else if (coefficient[i] != 0)
                    {
                        string sign = coefficient[i] > 0 ? "+" : "";
                        Console.Write(sign + coefficient[i] + "x^" + (i + 1));
                        //test += (double)coefficient[i] * (double)Math.Pow(5, i + 1);
                    }



                }

                Console.WriteLine();
                Console.WriteLine();
            }


        }
    }
}
