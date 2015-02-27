using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GlimpseDemo.CustomCode.QuoteOfTheDayExample
{
    public class QuoteOfTheDayService
    {
        // Random with time as seed
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);


        public string GetQuote()
        {
            // Simulate some load time
            Thread.Sleep(200);

            // Simulate a quite unstable service 40 % chance of failure
            if (_random.NextDouble() >= 0.6)
            {
                throw new Exception("Something really bad happend");
            }
        
            
            return new List<string>()
            {
                "Glimpse is nice",
                "Be strong",
                "Dynamicweb Tech Conference 2015!"
            }
            .Skip(_random.Next(0,3))
            .FirstOrDefault();
        }
    }
}