// Written by Joe Zachary for CS 4150, January 2016

using System;
using System.Diagnostics;

namespace ArraySearch
{
    /// <summary>
    /// Provides a timing demo.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// The number of repetitions used in versions 2 and 3
        /// </summary>
        public const long REPTS = 1000;

        /// <summary>
        /// The minimum duration of a timing experiment (in msecs) in versions 4 and 5
        /// </summary>
        public const int DURATION = 1000;

        /// <summary>
        /// The size of the array in versions 1 through 5
        /// </summary>
        public const int SIZE = 1023;

        /// <summary>
        /// Drives the timing demo.
        /// </summary>
        public static void Main()
        {
            // Let's look at how precise the Stopwatch is
            Console.WriteLine("Is high resolution: " + Stopwatch.IsHighResolution);
            Console.WriteLine("Ticks per second: " + Stopwatch.Frequency);
            Console.WriteLine();

            // Now do an experiment.
            Console.Write("Enter choice (1-10): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            RunExperiment(choice);
            Console.Read();
        }

        /// <summary>
        /// Runs different experiments depending on the value of approach.
        /// </summary>
        /// 
        public static void RunExperiment(int approach)
        {
            if (approach == 1)
            {
                // Time a single lookup using binary search and report the result.
                Console.WriteLine("Time single binary search for last element in array");
                Console.WriteLine(TimeBinarySearch1(SIZE).ToString("G2") + " msecs");
            }

            else if (approach == 2)
            {
                // Do REPT lookups, time each individually.  Sum the timings
                // and report an average.
                Console.WriteLine("Time " + REPTS + " searches, average them");
                Console.WriteLine(TimeBinarySearch2(SIZE).ToString("G2") + " msecs");
            }

            else if (approach == 3)
            {
                // Time a loop that does REPT lookups.  Report the average
                Console.WriteLine("Time loop that does " + REPTS + " searches, compute average");
                Console.WriteLine(TimeBinarySearch3(SIZE).ToString("G2") + " msecs");
            }

            else if (approach == 4)
            {
                // Time a loop that does REPT lookups.  Then time a copy of the loop
                // (with the lookups commented out).  Find the difference and report
                // an average.
                Console.WriteLine("Time loop that does " + REPTS + " searches, time same loop without searches, compute averages, subtract");
                Console.WriteLine(TimeBinarySearch4(SIZE).ToString("G2") + " msecs");
            }

            else if (approach == 5)
            {
                // Time a loop that takes at least DURATION msecs to do lookups, and
                // compute an average.  Then time a copy of the loop (with the lookups
                // commented out) that runs for at least DURATION msecs, and compute
                // an average.  Report the difference.
                Console.WriteLine("Time loop that does searches for " + DURATION + " msecs, time same loop without search for " + DURATION + " msecs, subtract the averages");
                Console.WriteLine(TimeBinarySearch5(SIZE, true).ToString("G2") + " msecs");
            }

            else if (approach == 6)
            {
                // Uses the best practices identified above to determine the average time
                // required to do a successful lookup using binary search.  
                Console.WriteLine("Use best practices from approach 5 to compute average time for successful lookup");
                Console.WriteLine(TimeBinarySearch6(SIZE, true).ToString("G2") + " msecs");
            }

            else if (approach == 7)
            {
                // Uses the best practices identified above to determine the average time
                // required to do a successful lookup using binary search.  
                Console.WriteLine("Use best practices from approach 5 to compute average time for successful lookup using a process timer");
                Console.WriteLine(TimeBinarySearch7(SIZE, true).ToString("G2") + " msecs");
            }

            else if (approach == 8)
            {
                // Report the average time required to do a binary search for various sizes
                // of arrays.
                Console.WriteLine("Average time for binary search for last element for various sizes");

                int size = 32;
                Console.WriteLine("\nSize\tTime (msec)\tDelta (msec)");
                double previousTime = 0;
                for (int i = 0; i <= 17; i++)
                {
                    size = size * 2;
                    double currentTime = TimeBinarySearch5(size - 1);
                    Console.Write((size - 1) + "\t" + currentTime.ToString("G3"));
                    if (i > 0)
                    {
                        Console.WriteLine("   \t" + (currentTime - previousTime).ToString("G3"));
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                    previousTime = currentTime;
                }
            }

            else if (approach == 9)
            {
                // Report the average time required to do a binary search for various sizes
                // of arrays, using a different timing measurement.
                Console.WriteLine("Average time for successful binary search for various sizes");

                int size = 32;
                Console.WriteLine("\nSize\tTime (msec)\tDelta (msec)");
                double previousTime = 0;
                for (int i = 0; i <= 17; i++)
                {
                    size = size * 2;
                    double currentTime = TimeBinarySearch6(size - 1);
                    Console.Write((size - 1) + "\t" + currentTime.ToString("G3"));
                    if (i > 0)
                    {
                        Console.WriteLine("   \t" + (currentTime - previousTime).ToString("G3"));
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                    previousTime = currentTime;
                }
            }

            else if (approach == 10)
            {
                // Report the average time required to do a linear search for various sizes
                // of arrays.
                Console.WriteLine("Average time for linear search for last element for various sizes");

                int size = 32;
                Console.WriteLine("\nSize\tTime (msec)\tRatio (msec)");
                double previousTime = 0;
                for (int i = 0; i <= 17; i++)
                {
                    size = size * 2;
                    double currentTime = TimeLinearSearch1(size - 1);
                    Console.Write((size - 1) + "\t" + currentTime.ToString("G3"));
                    if (i > 0)
                    {
                        Console.WriteLine("   \t" + (currentTime / previousTime).ToString("G3"));
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                    previousTime = currentTime;
                }
            }
        }

        /// <summary>
        /// Returns an index of elt within data, or -1 if it isn't there.
        /// </summary>
        public static int LinearSearch(int[] data, int elt)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == elt) return i;
            }
            return -1;
        }

        /// <summary>
        /// Implements binary search.  If elt is found in data, return its
        /// index.  Otherwise, find the index i at which elt should have appeared
        /// and return -(i+1).
        /// </summary>
        public static int BinarySearch(int[] data, int elt)
        {
            int lo = 0;
            int hi = data.Length - 1;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (data[mid] == elt)
                {
                    return mid;
                }
                else if (data[mid] < elt)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
            }
            return -(lo + 1);
        }

        /// <summary>
        /// Returns the number of milliseconds that have elapsed on the Stopwatch.
        /// </summary>
        public static double msecs(Stopwatch sw)
        {
            return (((double)sw.ElapsedTicks) / Stopwatch.Frequency) * 1000;
        }

        /// <summary>
        /// Returns the time required to find the last element of an array
        /// of the given size using binary search.  Operates by timing a
        /// single lookup operation.
        /// </summary>
        public static double TimeBinarySearch1(int size)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Create a stopwatch
            Stopwatch sw = new Stopwatch();

            // Time the operation
            sw.Start();
            BinarySearch(data, size-1);
            sw.Stop();

            // Return the number of milliseconds that elapsed
            return msecs(sw);
        }

        /// <summary>
        /// Returns the time required to find the last element of an array
        /// of the given size using binary search.  Operates by timing REPTS
        /// operations and averaging. 
        /// </summary>
        public static double TimeBinarySearch2(int size)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Create a stopwatch
            Stopwatch sw = new Stopwatch();

            // Time REPTS operations
            for (long i = 0; i < REPTS; i++)
            {
                sw.Start();
                BinarySearch(data, size-1);
                sw.Stop();
            }

            // Return the average number of milliseconds that elapsed
            return msecs(sw) / REPTS;

        }

        /// <summary>
        /// Returns the time required to find the last element of an array
        /// of the given size using binary search.  Operates by timing a loop
        /// that does the operation REPTS times, then calculating the loop overhead,
        /// and averaging.
        /// </summary>
        public static double TimeBinarySearch3(int size)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Create a stopwatch
            Stopwatch sw = new Stopwatch();

            // Make a single measurement of REPTS operations
            sw.Start();
            for (long i = 0; i < REPTS; i++)
            {
                BinarySearch(data, size - 1);
            }
            sw.Stop();
            double totalAverage = msecs(sw) / REPTS;

            // Return the average
            return totalAverage;
        }

        /// <summary>
        /// Returns the time required to find the last element of an array
        /// of the given size using binary search.  Operates by timing a loop
        /// that does the operation REPTS times, then calculating the loop overhead,
        /// and averaging.
        /// </summary>
        public static double TimeBinarySearch4(int size)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Create a stopwatch
            Stopwatch sw = new Stopwatch();

            // Make a single measurement of REPTS operations
            sw.Start();
            for (long i = 0; i < REPTS; i++)
            {
                BinarySearch(data, size-1);
            }
            sw.Stop();
            double totalAverage = msecs(sw) / REPTS;

            // Create a new stopwatch
            sw = new Stopwatch();

            // Repeat, but don't actually do the binary search
            sw.Start();
            for (long i = 0; i < REPTS; i++)
            {
                //BinarySearch(data, size-1);
            }
            sw.Stop();
            double overheadAverage = msecs(sw) / REPTS;

            // Display the raw data as a sanity check
            Console.WriteLine("   Total avg:    " + totalAverage.ToString("G2") + " msecs");
            Console.WriteLine("   Overhead avg: " + overheadAverage.ToString("G2") + " msecs");

            // Return the difference
            return totalAverage - overheadAverage;
        }

        /// <summary>
        /// Returns the time required to find the last element of an array
        /// of the given size using binary search.  Operates by timing a loop
        /// that does the operation until one second elapses, then times the
        /// overhead, then computes and returns an average.
        /// </summary>
        public static double TimeBinarySearch5(int size, bool displaySanity = false)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Create a stopwatch
            Stopwatch sw = new Stopwatch();

            // Keep increasing the number of repetitions until one second elapses.
            double elapsed = 0;
            long repetitions = 1;
            do
            {
                repetitions *= 2;
                sw.Restart();
                for (long i = 0; i < repetitions; i++)
                {
                    BinarySearch(data, size-1);
                }
                sw.Stop();
                elapsed = msecs(sw);
            } while (elapsed < DURATION);
            double totalAverage = elapsed / repetitions;

            // Create a stopwatch
            sw = new Stopwatch();

            // Keep increasing the number of repetitions until one second elapses.
            elapsed = 0;
            repetitions = 1;
            do
            {
                repetitions *= 2;
                sw.Restart();
                for (long i = 0; i < repetitions; i++)
                {
                    //BinarySearch(data, size-1);
                }
                sw.Stop();
                elapsed = msecs(sw);
            } while (elapsed < DURATION);
            double overheadAverage = elapsed / repetitions;

            // Display the raw data as a sanity check
            if (displaySanity)
            {
                Console.WriteLine("   Total avg:    " + totalAverage.ToString("G2") + " msecs");
                Console.WriteLine("   Overhead avg: " + overheadAverage.ToString("G2") + " msecs");
            }

            // Return the difference
            return totalAverage - overheadAverage;
        }

        /// <summary>
        /// Returns the average time required to find an element in an array of
        /// the given size using binary search, assuming that the element actually
        /// appears in the array.
        /// </summary>
        public static double TimeBinarySearch6(int size, bool displaySanity = false)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Create a stopwatch
            Stopwatch sw = new Stopwatch();

            // Keep increasing the number of repetitions until one second elapses.
            double elapsed = 0;
            long repetitions = 1;
            do
            {
                repetitions *= 2;
                sw.Restart();
                for (long i = 0; i < repetitions; i++)
                {
                    for (int d = 0; d < size; d++)
                    {
                        BinarySearch(data, d);
                    }
                }
                sw.Stop();
                elapsed = msecs(sw);
            } while (elapsed < DURATION);
            double totalAverage = elapsed / repetitions / size;

            // Create a stopwatch
            sw = new Stopwatch();

            // Keep increasing the number of repetitions until one second elapses.
            elapsed = 0;
            repetitions = 1;
            do
            {
                repetitions *= 2;
                sw.Restart();
                for (long i = 0; i < repetitions; i++)
                {
                    for (int d = 0; d < size; d++)
                    {
                        //BinarySearch(data, d);
                    }
                }
                sw.Stop();
                elapsed = msecs(sw);
            } while (elapsed < DURATION);
            double overheadAverage = elapsed / repetitions / size;

            // Display the raw data as a sanity check
            if (displaySanity)
            {
                Console.WriteLine("   Total avg:    " + totalAverage.ToString("G2") + " msecs");
                Console.WriteLine("   Overhead avg: " + overheadAverage.ToString("G2") + " msecs");
            }

            // Return the difference
            return totalAverage - overheadAverage;
        }


        /// <summary>
        /// Returns the average time required to find an element in an array of
        /// the given size using binary search, assuming that the element actually
        /// appears in the array.  Uses a different timer than Search6.
        /// </summary>
        public static double TimeBinarySearch7(int size, bool displaySanity = false)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Get the process
            Process p = Process.GetCurrentProcess();

            // Keep increasing the number of repetitions until one second elapses.
            double elapsed = 0;
            long repetitions = 1;
            do
            {
                repetitions *= 2;
                TimeSpan start = p.TotalProcessorTime;
                for (long i = 0; i < repetitions; i++)
                {
                    for (int d = 0; d < size; d++)
                    {
                        BinarySearch(data, d);
                    }
                }
                TimeSpan stop = p.TotalProcessorTime;
                elapsed = stop.TotalMilliseconds - start.TotalMilliseconds;
            } while (elapsed < DURATION);
            double totalAverage = elapsed / repetitions / size;

            // Keep increasing the number of repetitions until one second elapses.
            elapsed = 0;
            repetitions = 1;
            do
            {
                repetitions *= 2;
                TimeSpan start = p.TotalProcessorTime;
                for (long i = 0; i < repetitions; i++)
                {
                    for (int d = 0; d < size; d++)
                    {
                        //BinarySearch(data, d);
                    }
                }
                TimeSpan stop = p.TotalProcessorTime;
                elapsed = stop.TotalMilliseconds - start.TotalMilliseconds;
            } while (elapsed < DURATION);
            double overheadAverage = elapsed / repetitions / size;

            // Display the raw data as a sanity check
            if (displaySanity)
            {
                Console.WriteLine("   Total avg:    " + totalAverage.ToString("G2") + " msecs");
                Console.WriteLine("   Overhead avg: " + overheadAverage.ToString("G2") + " msecs");
            }

            // Return the difference
            return totalAverage - overheadAverage;
        }

        /// <summary>
        /// Returns the average time required to find an element in an array of
        /// the given size using binary search, assuming that the element actually
        /// appears in the array.  Uses a different timer than Search5.
        /// </summary>
        public static double TimeLinearSearch1(int size, bool displaySanity = false)
        {
            // Construct a sorted array
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }

            // Get the process
            Process p = Process.GetCurrentProcess();

            // Keep increasing the number of repetitions until one second elapses.
            double elapsed = 0;
            long repetitions = 1;
            do
            {
                repetitions *= 2;
                TimeSpan start = p.TotalProcessorTime;
                for (long i = 0; i < repetitions; i++)
                {
                    for (int d = 0; d < size; d++)
                    {
                        LinearSearch(data, d);
                    }
                }
                TimeSpan stop = p.TotalProcessorTime;
                elapsed = stop.TotalMilliseconds - start.TotalMilliseconds;
            } while (elapsed < DURATION);
            double totalAverage = elapsed / repetitions / size;

            // Keep increasing the number of repetitions until one second elapses.
            elapsed = 0;
            repetitions = 1;
            do
            {
                repetitions *= 2;
                TimeSpan start = p.TotalProcessorTime;
                for (long i = 0; i < repetitions; i++)
                {
                    for (long d = 0; d < size; d++)
                    {
                        //LinearSearch(data, d);
                    }
                }
                TimeSpan stop = p.TotalProcessorTime;
                elapsed = stop.TotalMilliseconds - start.TotalMilliseconds;
            } while (elapsed < DURATION);
            double overheadAverage = elapsed / repetitions / size;

            // Display the raw data as a sanity check
            if (displaySanity)
            {
                Console.WriteLine("   Total avg:    " + totalAverage.ToString("G2") + " msecs");
                Console.WriteLine("   Overhead avg: " + overheadAverage.ToString("G2") + " msecs");
            }

            // Return the difference
            return totalAverage - overheadAverage;
        }
    }
}




