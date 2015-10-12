﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Direct;

namespace RT.Generexes.Tests
{

#warning TODO: Test recursive

    class TestsProgram
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            NUnitDirect.RunTestsOnAssembly(typeof(TestsProgram).Assembly);

            if (args.Contains("--wait"))
            {
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
