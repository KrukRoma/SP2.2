using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SP2_2
{
    class Program
    {
        static List<int> numbers = new List<int>();
        static int max = int.MinValue;
        static int min = int.MaxValue;
        static double avg = 0;

        static void Method(int threadNumber, int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                Console.WriteLine($"\tThread {threadNumber}: {i}");
                lock (numbers)
                {
                    numbers.Add(i);  
                }
            }
        }

        static void FindMax()
        {
            max = numbers.Max();
            Console.WriteLine($"Max: {max}");
        }

        static void FindMin()
        {
            min = numbers.Min();
            Console.WriteLine($"Min: {min}");
        }

        static void FindAvg()
        {
            avg = numbers.Average();
            Console.WriteLine($"Average: {avg}");
        }

        static void WriteToFile()
        {
            string path = "numbers_and_results.txt";
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Numbers:");
                foreach (int num in numbers)
                {
                    writer.Write(num + " ");
                }

                writer.WriteLine($"\nMax: {max}");
                writer.WriteLine($"Min: {min}");
                writer.WriteLine($"Average: {avg}");
            }

            Console.WriteLine($"Results written to {path}");
        }

        static void Main(string[] args)
        {
            Console.Write("Enter number of threads: ");
            int threadCount = int.Parse(Console.ReadLine());

            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                Console.Write($"Enter start of range for thread {i + 1} (between 0 and 50): ");
                int start = int.Parse(Console.ReadLine());

                Console.Write($"Enter end of range for thread {i + 1} (between {start} and 50): ");
                int end = int.Parse(Console.ReadLine());

                int threadNumber = i + 1; 
                threads[i] = new Thread(() => Method(threadNumber, start, end));
                threads[i].Start();

                threads[i].Join();
            }

            Thread maxThread = new Thread(FindMax);
            Thread minThread = new Thread(FindMin);
            Thread avgThread = new Thread(FindAvg);
            Thread writeThread = new Thread(WriteToFile);

            maxThread.Start();
            minThread.Start();
            avgThread.Start();

            maxThread.Join();
            minThread.Join();
            avgThread.Join();

            writeThread.Start();
        }
    }
}
