using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    class Program
    {
        
        // global object
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        static void Main(string[] args)
        {

            // List of messages selscted at random when CPU is MAXED out
            List<string> cpuMaxedOutMessages = new List<string>();

            cpuMaxedOutMessages.Add("Do I smell smoke?");
            cpuMaxedOutMessages.Add("Holy Crap your CPU is about to catch fire!");
            cpuMaxedOutMessages.Add("What are you even doing?");
            cpuMaxedOutMessages.Add("What are you downloading!");
            cpuMaxedOutMessages.Add("WARNING! WARNING!  WARNING! I Farted");

            // rolls the dice (creates a random number)
            Random rand = new Random();

            #region My Performance Counters

            // using text to speech to say hello
            Console.WriteLine("Welcome to Jarvis version 0.1 (Created by: Cory)");
            synth.Speak("This was sent by GOD, you must read this till the end and good fortune will come your way please like and share or else you will die aha aha aha aha");
            
           
            // Pulls the current CPU Load in percentage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            // pulls the current Ram Load in MegaBytes
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            // pulls the current up time in seconds
            PerformanceCounter perfUpTimeCount = new PerformanceCounter("System", "System Up Time");
            perfUpTimeCount.NextValue();

       #endregion

            TimeSpan upTimeSpan = TimeSpan.FromSeconds(perfUpTimeCount.NextValue());

            string systemUpTimeMessage = string.Format("The current system up time is {0}, days {1} hours {2} minutes {3} seconds", 
                
                    (int)upTimeSpan.TotalDays,
                    (int)upTimeSpan.Days,
                    (int)upTimeSpan.Hours,
                    (int)upTimeSpan.Minutes,
                    (int)upTimeSpan.Seconds
                               
                );

            // tell the current up time message 
           Speak(systemUpTimeMessage, VoiceGender.Female, 2);

            int speechSpeed = 1;

            //   variable for openening chrome.exe
            bool isChromeOpenedAlready = false;

            // While loop
            while (true)
            {
           
                float currentCpuPercentage = (int)perfCpuCount.NextValue();

                float currentAvailableMemory = (int)perfMemCount.NextValue();
                
                // Every one second print cpu and mem to the screen

                //  {0} this will be replaced with this , >  perfCpuCount.NextValue()
                Console.WriteLine("CPU Load:         {0}%", currentCpuPercentage);
                // Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);

               

                #region   LOGIC
                // speaks a message when CPU usage is above 5%
                if(currentCpuPercentage > 5)
                {

                    if (currentCpuPercentage >= 6)
                    {

                     // makes speech speed not exceed 5x normal
                        if(speechSpeed < 5)
                        {

                            speechSpeed++;
                        }


                        // using TTS to say the current values
                        string cpuLoadVocalMessage = cpuMaxedOutMessages[rand.Next(5)];
                       
                        if (isChromeOpenedAlready == false)
                        {
                            OpenWebsite("https://youtu.be/D67jM8nO7Ag");
                            isChromeOpenedAlready = true;
                        }
                        // making TTS say these messages
                        Speak(cpuLoadVocalMessage, VoiceGender.Female, speechSpeed);
                    } else
                    {
                      
                        // using TTS to say the current values
                        string cpuLoadVocalMessage = String.Format("The Current CPU load is {0} percent", currentCpuPercentage);
                        // making TTS say these messages
                        Speak(cpuLoadVocalMessage, VoiceGender.Female, 5);
                    }
                    
                }



                // speaks a message when RAM is Below 15 Gigs
                if (currentAvailableMemory < 15000)
                {
                  
                    string memAvailableVocalMessage = String.Format("You currently have {0} Megabytes Available", currentAvailableMemory);
                    Speak(memAvailableVocalMessage, VoiceGender.Male);           

                }


                // every one second print cpu load to console
                Thread.Sleep(1000);

            }// loop end
            #endregion


        }

        /// <summary>
        /// Speaks with a selected voice
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }


        /// <summary>
        ///  OVERLOADING the function and adds speed of TTS
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        /// <param name="rate"></param>
        public static void Speak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            Speak(message, voiceGender);
        }


        // opens a website
        public static void OpenWebsite(string URL)
        {
            // starts a new chrome browser
            Process p1 = new Process();

            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.Start();

        }
         
    }
}
