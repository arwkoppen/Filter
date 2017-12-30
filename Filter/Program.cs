// C#
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using System;
using Microsoft.Toolkit;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static System.Math;
using static System.Console;

namespace Filter_UWP2
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            AppCenter.Start("0430dee9-f26c-4b11-b0e4-f6abf670cbca", typeof(Analytics));
            Boolean input_boolean;
            string input;
            double n, ripple, f, cap1, cap2,
                      eps, help, ks, kw;
            double[,] array = new double[256, 2];
            int k, poles, counter;
            WriteLine("Enter the ripple of the filter in dB (min>0, max<=2): ");
            do
            {
                input = ReadLine();
                input_boolean = Double.TryParse(input, out ripple);
            }
            while ((ripple <= 0) || (ripple > 2) || (input_boolean == false));
            eps = Sqrt(Pow(10.0, y: (0.1 * ripple)) - 1);
            WriteLine("Enter the order in number of poles (max255): ");
            do
            {
                input = ReadLine();
                input_boolean = Double.TryParse(input, out n);
                poles = (int)n;
            }
            while ((poles <= 0) || (poles > 255) || (input_boolean == false));
            help = 1 / eps;
            help = help + Sqrt((help * help) + 1);
            help = Pow(help, y: (1 / (double)poles));
            ks = (help - (1 / help)) / (-2);
            kw = (help - (1 / help)) / 2;
            WriteLine("\neps: {0}, ks: {1}, kw: {2}\n\n", eps, ks, kw);
            for (k = 0; k < poles; k++)
            {
                array[k, 0] = ks * Sin((PI * (1 + (2 * k))) / (2 * (double)poles));
                array[k, 1] = kw * Cos((PI * (1 + (2 * k))) / (2 * (double)poles));
                WriteLine("Pole {0} @: sigma= {1}; omega= {2}\n", (k + 1), array[k, 0], array[k, 1]);
            }
            WriteLine("\nEnter the -3 dB frequency:");
            do
            {
                input = ReadLine();
                input_boolean = Double.TryParse(input, out f);
            }
            while ((f <= 0) || (input_boolean = false));
            WriteLine("\nThe capacitor-values are normalized for use with 1 kOhm resistors:\n");
            counter = (int)((n - 1) / 2);
            for (k = counter; k >= 0; k--)
            {
                cap1 = (-1 / array[k, 0]);
                cap2 = (1 / ((array[k, 0] * array[k, 0]) + (array[k, 1] * array[k, 1] * cap1)));
                WriteLine("C{0}feedback= {1} F, C{2}regular= {3} F\n", (k + 1), (cap1 / f / 2000 / PI), (k + 1), (cap2 / f / 2000 / PI));
            }
            WriteLine("Press <ENTER> to Abort the Program");
            input = ReadLine();
        }
    }
}



