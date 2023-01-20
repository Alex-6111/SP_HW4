

using System.Threading;

namespace SP_HW4._1
{
    internal class Program
    {

        //
        // task 1
        //

        static List<Thread> threads = new List<Thread>();

        static void Main(string[] args)
        {
            ThreadInfo();
        }

        public static void ThreadInfo()
        {
            for (int i = 0; i < 10; i++)
            {
                threads.Add(new(ShowRandomNumbers) { Name = $"Lists thread {i}" });
            }
            Semaphore semaphore = new(3, 3);
            foreach (Thread thread in threads)
            {
                thread.Start(semaphore);
            }
        }

        public static void ShowRandomNumbers(object? obj)
        {
            Semaphore? semaphore = obj as Semaphore;
            semaphore?.WaitOne();
            try
            {
                Random random = new Random();
                int[] nums = new int[3] {random.Next(0,1000), random.Next(0,1000), random.Next(0,1000)};
                Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} out numbers-> {nums[0]}, {nums[1]}, {nums[2]}");
                Thread.Sleep(3000);
                Console.WriteLine();
            }
            finally
            {
                semaphore?.Release();
            }
        }

    }
}