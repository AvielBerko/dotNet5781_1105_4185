using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_00_1105_4185
{
	partial class Program
	{
		static void Main(string[] args)
		{
			Welcome1105();
			Welcome4185();
			Console.ReadKey();
		}

		static partial void Welcome4185();
		private static void Welcome1105()
		{
			Console.WriteLine("Enter your name: ");
			string name = Console.ReadLine();
			Console.WriteLine("{0}, welcome to my first console application", name);
		}
	}
}
