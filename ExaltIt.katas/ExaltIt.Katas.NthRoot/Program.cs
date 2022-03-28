using System;
using System.Threading.Tasks;

namespace ExaltIt.Katas.NthRoot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nth Root Calculator");

            ShowMenu();
        }

        static void ShowMenu()
        {
            double number = 0;
            while (number == 0)
            {
                Console.WriteLine("Please enter a positive real number for nth calculation");

                if (double.TryParse(Console.ReadLine(), out number) && number > 0)
                {
                    Console.WriteLine("Thank you");
                    break;
                }
                else
                {
                    Console.WriteLine("Error. Please try again");
                }
            }

            int root = 0;
            while (root == 0)
            {
                Console.WriteLine("Please enter a positive Natural number as a root");

                if (int.TryParse(Console.ReadLine(), out root) && root > 0)
                {
                    Console.WriteLine("Thank you");
                    break;
                }
                else
                {
                    Console.WriteLine("Error. Please try again");
                }
            }

            Console.WriteLine("Calculation in progress...");
            double? rst = GetNthRoot(number, root);

            if (rst.HasValue)
            {
                Console.WriteLine($"Result : {rst.Value}");
                Console.WriteLine("Please try again");
                ShowMenu();
            }
            else
            {
                Console.WriteLine("Calculation Error. Please try again");
                ShowMenu();
            }
        }

        static double? GetNthRoot(double number, int n)
        {
            if (number <= 0 || n <= 0)
            {
                return null;
            }

            var task = new Task<double>(() => GetNthRootNewtonMethod(number, n, .000000000000001),
                    TaskCreationOptions.LongRunning);
            task.Start();

            return task.Result;
        }

        static double GetNthRootNewtonMethod(double number, int root, double p)
        {
            double rst = number;
            double temp = number / root;

            while (Abs(rst - temp) > p)
            {
                temp = rst;
                rst = (1 / (double)root) * (((root - 1) * rst) + (number / Power(rst, root - 1)));
            }
            return rst;
        }


        static double Power(double number, int n)
        {
            double rst = 1.0;

            Parallel.For(1, n + 1,
                   index =>
                   {
                       rst = rst * number;
                   });

            return rst;
        }

        static double Abs(double number)
        {
            if (number < 0) number = -number;
            return number;
        }
    }
}
