using System;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Understanding AWAIT vs NO AWAIT ===\n");

            // WITHOUT await - Fire and Forget
            Console.WriteLine("--- WITHOUT await ---");
            Console.WriteLine("Before calling async method");
#pragma warning disable CS4014 // Intentional: demonstrating fire-and-forget behavior
            DoWorkAsync("Task 1");
#pragma warning restore CS4014
            Console.WriteLine("After calling async method");
            Console.WriteLine("Notice: 'After' printed BEFORE the task completed!\n");

            // Wait a bit to see the async work complete
            await Task.Delay(2000);
            Console.WriteLine();

            // WITH await - Proper waiting
            Console.WriteLine("--- WITH await ---");
            Console.WriteLine("Before calling async method");
            await DoWorkAsync("Task 2");
            Console.WriteLine("After calling async method");
            Console.WriteLine("Notice: 'After' printed AFTER the task completed!");
        }

        static async Task DoWorkAsync(string taskName)
        {
            Console.WriteLine($"  [ASYNC] {taskName} - Starting work...");
            await Task.Delay(1500); // Simulate 1.5 seconds of work
            Console.WriteLine($"  [ASYNC] {taskName} - Work completed!");
        }
    }
}
