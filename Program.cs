﻿using System;

namespace tec_xx
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();

            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
