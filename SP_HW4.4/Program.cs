using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace SP_HW4._4
{
    internal class Program
    {
        private static Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            Problem4();
        }

        public static void Problem4()
        {
            Thread thread1 = new Thread(() =>
            {
                mutex.WaitOne();
                using (StreamWriter writer = new StreamWriter("numbers.txt"))
                {
                    Random random = new Random();
                    for (int i = 0; i < 10; i++)
                    {
                        int num = random.Next(1, 100);
                        writer.WriteLine(num);
                    }
                }
                mutex.ReleaseMutex();
            });

             
            Thread thread2 = new Thread(() =>
            {
                thread1.Join();
                mutex.WaitOne();
                using (StreamReader reader = new StreamReader("numbers.txt"))
                {
                    using (StreamWriter writer = new StreamWriter("prime_numbers.txt"))
                    {
                        while (!reader.EndOfStream)
                        {
                            int num = int.Parse(reader.ReadLine());
                            if (IsPrime(num))
                            {
                                writer.WriteLine(num);
                            }
                        }
                    }
                }
                mutex.ReleaseMutex();
            });

            
            Thread thread3 = new Thread(() =>
            {
                thread2.Join();
                mutex.WaitOne();
                using (StreamReader reader = new StreamReader("prime_numbers.txt"))
                {
                    using (StreamWriter writer = new StreamWriter("last_7_prime_numbers.txt"))
                    {
                        List<int> primeNumbers = new List<int>();
                        while (!reader.EndOfStream)
                        {
                            primeNumbers.Add(int.Parse(reader.ReadLine()));
                        }
                        int startIndex = Math.Max(0, primeNumbers.Count - 7);
                        for (int i = startIndex; i < primeNumbers.Count; i++)
                        {
                            writer.WriteLine(primeNumbers[i]);
                        }
                    }
                }
                mutex.ReleaseMutex();
            });

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread3.Join();
            Console.WriteLine("All threads have finished execution");
        }


        static bool IsPrime(int num)
        {
            if (num <= 1) return false;
            if (num == 2) return true;
            if (num % 2 == 0) return false;

            for (int i = 3; i <= Math.Sqrt(num); i += 2)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}