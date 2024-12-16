using System;
using System.Threading;

namespace ConsoleStopwatch
{
    // Delegate declaration
    public delegate void StopwatchEventHandler(string message);

    public class Stopwatch
    {
        // Fields
        private int timeElapsed; // Time in seconds
        private bool isRunning;

        // Events
        public event StopwatchEventHandler? OnStarted;
        public event StopwatchEventHandler? OnStopped;
        public event StopwatchEventHandler? OnReset;

       
        public int TimeElapsed => timeElapsed;
        public bool IsRunning => isRunning;

        // Methods
        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                OnStarted?.Invoke("Stopwatch Started!");
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                OnStopped?.Invoke("Stopwatch Stopped!");
            }
        }

        public void Reset()
        {
            timeElapsed = 0;
            isRunning = false;
            OnReset?.Invoke("Stopwatch Reset!");
        }

        public void Tick()
        {
            if (isRunning)
            {
                timeElapsed++;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Subscribing to events
            stopwatch.OnStarted += message => DisplayMessage(message);
            stopwatch.OnStopped += message => DisplayMessage(message);
            stopwatch.OnReset += message => DisplayMessage(message);

            bool exit = false;

            Console.Clear();
            Console.WriteLine("========================");
            Console.WriteLine("  Console Stopwatch   ");
            Console.WriteLine("========================");
            Console.WriteLine("Press S to Start");
            Console.WriteLine("Press T to Stop");
            Console.WriteLine("Press R to Reset");

            string previousMessage = string.Empty;
            int previousTimeElapsed = -1;

            while (!exit)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    switch (key)
                    {
                        case ConsoleKey.S:
                            stopwatch.Start();
                            break;
                        case ConsoleKey.T:
                            stopwatch.Stop();
                            break;
                        case ConsoleKey.R:
                            stopwatch.Reset();
                            break;
                        default:
                            DisplayMessage("Invalid key. Please use S, T or R.");
                            break;
                    }
                }
                if (stopwatch.TimeElapsed != previousTimeElapsed)
                {
                    previousTimeElapsed = stopwatch.TimeElapsed;
                    Console.SetCursorPosition(0, 7); // Keeps the elapsed time in a fixed position
                    Console.Write($"Time Elapsed: {stopwatch.TimeElapsed}s      ");
                }

                // Small delay for smooth performance
                Thread.Sleep(100);
                stopwatch.Tick();
            }

        }
        static void DisplayMessage(string message)
        {
            Console.SetCursorPosition(0, 8); // Display messages in a fixed position
            Console.WriteLine(message.PadRight(Console.WindowWidth)); // Clear the line with padding
        }
    }
}
