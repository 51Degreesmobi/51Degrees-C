﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiftyOne.Mobile.Detection.Provider.Interop.Pattern;
using System.IO;
using System.Threading;

namespace FiftyOne.Example.Illustration.CSharp.Reload_Data_File
{
    class Program
    {
        // A memory-resident data file initialised with specified properties, 
        // a cache and a workset pool.
        static Provider provider;
        // Location of the 51Degrees data file.
        string deviceDataFile;
        // Location of the file containing User-Agent strings.
        string userAfentsFile;
        // A list of comma-separated properties to initialise provider with.
        string propertiesToUse;
        // Indicates how many threads have finished executing.
        static int threadsFinished = 0;

        /// <summary>
        /// Performs dataset reload tests. A number of threads run in the 
        /// background constantly performing device detection while the main 
        /// thread does the data file reloads.
        /// 
        /// The main thread will carry on doing the dataset reloads while at 
        /// least one thread has not finished.
        /// </summary>
        public void Run()
        {
            // Threads that run device detection in the background.
            int numberOfThreads = 4;
            // Contains references to background threads.
            Thread[] threads;

            Console.WriteLine("Starting the Reload Data File Example.");

            provider = new Provider(deviceDataFile, propertiesToUse);
            threads = new Thread[numberOfThreads];
            // Start detection threads.
            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(new ThreadStart(threadPayload));
                threads[i].Start();
            }

            // Reload data file until at least one thread is still not done.
            while (threadsFinished < numberOfThreads)
            {
                provider.reloadFromFile();
                Console.WriteLine("Provider reloaded.");
                Thread.Sleep(50);
            }

            // Wait for all detection threads to complete.
            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }

            // Release resources held by the provider.
            provider.Dispose();

            // Report the end of the program and exit.
            Console.WriteLine("Program execution complete. Press Enter to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Represents payload for a thread. Function opens the file with 
        /// User-Agent strings and runs device detection for each User-Agent.
        /// Hash is computed for each detection and the XORed with the 
        /// existing hash to verify that reloading the dataset did not affect 
        /// the detection.
        /// </summary>
        public void threadPayload()
        {
            // Local variables.
            long hash = 0L;
            int recordsProcessed = 0;
            Match match;

            // Open file containing User-Agent strings for read.
            using (FileStream fs = File.Open(userAfentsFile, 
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                // Read next line.
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Perform detection.
                    match = provider.getMatch(line);
                    // Compute hash for this match.
                    hash ^= getHash(match);
                    // Release match.
                    match.Dispose();
                    // Update count of processed User-Agent lines.
                    recordsProcessed++;
                }
            }
            // When thread is finished increment threadsFinished counter and
            // Report on the progress
            Interlocked.Increment(ref threadsFinished);
            Console.WriteLine("Thread complete with hash code: " + hash + 
                              " and records processed: " + recordsProcessed);
        }

        /// <summary>
        /// Computes a hash based on values of the 51Degrees properties that 
        /// were passed as part of the Match object. Only property values are 
        /// used to compute hash.
        /// </summary>
        /// <param name="match">
        /// Object containing 51Degrees device detection results.
        /// </param>
        /// <returns>
        /// Hash value of the provided Match object.
        /// </returns>
        public static long getHash(Match match)
        {
            long hash = 0L;
            foreach(var property in provider.getAvailableProperties()) 
            {
                hash += match.getValue(property).GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Constructs the new instance of the Program with provided parameters.
        /// </summary>
        /// <param name="deviceDataFile"> 
        /// Location of the 51Degrees data file.
        /// </param>
        /// <param name="userAfentsFile"> 
        /// Location of the file with User-Agent strings.
        /// </param>
        /// <param name="propertiesToUse"> 
        /// Comma-separated string of properties to initialise the Provider 
        /// with.
        /// </param>
        public Program(string deviceDataFile,
                       string userAfentsFile,
                       string propertiesToUse)
        {
            this.deviceDataFile = deviceDataFile;
            this.userAfentsFile = userAfentsFile;
            this.propertiesToUse = propertiesToUse;
        }

        /// <summary>
        /// Main entry point for this program.
        /// </summary>
        /// <param name="args">
        /// 0: location of the 51Degrees device data file. The data file must 
        /// be of version 3.2. Lite data file can be found within the data 
        /// folder.
        /// 1: A file containing User-Agent strings. Does not have to be a CSV 
        /// file. Can have any extension as long as one line contains exactly 
        /// one User-Agent.
        /// 2: A string of comma-separated properties.
        /// All parameters are optional.
        /// </param>
        public static void Main(string[] args)
        {
            string fileName = args.Length > 0 ? args[0] : "../../../../../../data/51Degrees-LiteV3.2.dat";
            string userAgents = args.Length > 1 ? args[1] : "../../../../../../data/20000 User Agents.csv";
            string properties = args.Length > 2 ? args[2] : "IsMobile,BrowserName";
            Program program = new Program(fileName, userAgents, properties);
            program.Run();
        }
    }
}
