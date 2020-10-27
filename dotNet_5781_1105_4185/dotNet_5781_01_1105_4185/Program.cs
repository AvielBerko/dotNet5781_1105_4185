// Names: Aviel Berkovich (211981105), Meir Klemfner(211954185)
// Project in Windows
// Exercise 1

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_01_1105_4185
{
	class Program
	{
		enum Menu { INSERT = 1, BUS_SELECT, REFUEL_TREATMENT, LAST_TREATMENT_KM, EXIT }
		enum ActionsMenu { TREATMENT = 1, REFUEL }

		static Random rand = new Random();
		static List<Bus> buses = new List<Bus>();
		static void Main()
		{
			Console.WriteLine("1: Insert a new bus to the database\n" +
							"2: Select a bus to drive\n" +
							"3: Refuel or send to treatment\n" +
							"4: Print all buses last treatment Kilometrage\n" +
							"5: Exit");
			Menu choice = Menu.EXIT;
			do
			{
				Console.Write("Select 1-5: ");

				try
				{
					Execute(out choice);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			} while (choice != Menu.EXIT);
		}

		/// <summary>
		/// Getting the user choice and executing it.
		/// </summary>
		/// <param name="choice">The user choice</param>
		/// <exception cref="Exception">When the execution gone wrong.</exception>
		static void Execute(out Menu choice)
		{
			if (Enum.TryParse(Console.ReadLine(), out choice))
			{

				switch (choice)
				{
					case Menu.INSERT:
						{
							string regnum = GetRegistration();
							if (buses.Any((bus) => bus.Registration == regnum))
								throw new Exception("Bus is already registered in the system");

							Console.Write("Enter date registrated: ");
							if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
								buses.Add(new Bus(regnum, date));
							else
								throw new Exception("Date format is invalid, try dd/mm/yyyy");
							break;
						}
					case Menu.BUS_SELECT:
						{
							string regnum = GetRegistration();
							Bus selectedBus = FindBus(regnum);
							selectedBus.Drive((uint)rand.Next(1, 1200));
							break;
						}
					case Menu.REFUEL_TREATMENT:
						{
							string regnum = GetRegistration();
							Bus selectedBus = FindBus(regnum);

							Console.WriteLine("Enter 1 for treatment OR 2 to refuel:");
							if (Enum.TryParse(Console.ReadLine(), out ActionsMenu action))
							{
								switch (action)
								{
									case ActionsMenu.TREATMENT:
										selectedBus.Treatment();
										break;
									case ActionsMenu.REFUEL:
										selectedBus.Refuel();
										break;
								}
							}
							else
								throw new Exception("Invalid choice");
							break;
						}
					case Menu.LAST_TREATMENT_KM:
						{
							foreach (Bus bus in buses)
								Console.WriteLine(bus);
							break;
						}
					case Menu.EXIT:
						Console.WriteLine("Bye, bye");
						break;
				}
			}
			else
				throw new Exception("Invalid choice");
		}

		/// <summary>
		/// Finds a bus by the registration.
		/// </summary>
		/// <param name="regnum">The bus's registration.</param>
		/// <returns>The found bus.</returns>
		/// <exception cref="Exception">When no bus was found.</exception>
		private static Bus FindBus(string regnum)
		{
			Bus selectedBus = buses.Find((bus) => (regnum == bus.Registration));
			if (selectedBus != null)
				return selectedBus;
			else
				throw new Exception("Bus was not found");
		}

		/// <summary>
		/// Gets a registration for the user.
		/// </summary>
		/// <returns>The given registration.</returns>
		private static string GetRegistration()
		{
			Console.Write("Enter registration-number: ");
			string regnum = Console.ReadLine();
			return regnum;
		}
	}
}
