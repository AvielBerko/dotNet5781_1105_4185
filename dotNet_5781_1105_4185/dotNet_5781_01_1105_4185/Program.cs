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
		static Random rand = new Random();
		enum Menu { INSERT = 1 , BUS_SELECT, REFUEL_TREATMENT, LAST_TREATMENT_KM, EXIT }
		enum ActionsMenu { TREATMENT = 1, REFUEL}
		static void Main(string[] args)
		{
			Console.WriteLine("Select 1-5:\n" + "1: Insert a new bus to the database\n" +
							 "2: Select a bus to drive\n" + "3: Refuel or send to treatment\n" +
							 "4: Print all buses last treatment Kilometrage\n" + "5: Exit");
			List<Bus> buses = new List<Bus>();
			Menu choice = Menu.EXIT;
			do
			{
				try
				{
					if (Enum.TryParse(Console.ReadLine(), out choice))
					{

						switch (choice)
						{
							case Menu.INSERT:
								{
									Console.Write("Enter registration-number: ");
									string regnum = Console.ReadLine();
									Console.Write("Enter date registrated: ");
									if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
										buses.Add(new Bus(regnum, date));
									else
										throw new Exception("Date format is ivalid, try dd/mm/yyyy");
									break;
								}
							case Menu.BUS_SELECT:
								{
									Console.Write("Enter registration-number: ");
									string regnum = Console.ReadLine();
									Bus selectedBus = buses.Find((bus) => (regnum == bus.Registration));
									if (selectedBus != null)
										selectedBus.Drive((uint)rand.Next(1, 1200));
									else
										throw new Exception("Bus was not found");
									break;
								}
							case Menu.REFUEL_TREATMENT:
								{
									Console.Write("Enter registration-number: ");
									string regnum = Console.ReadLine();
									Bus selectedBus = buses.Find((bus) => (regnum == bus.Registration));
									if (selectedBus != null)
									{
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
												default:
													break;
											}
										}
										else
											throw new Exception("Invalid choice");
									}
									else
										throw new Exception("Bus was not found");
									
									break;
								}
							case Menu.LAST_TREATMENT_KM:
								{
									foreach (Bus bus in buses)
										Console.WriteLine(bus);
									break;
								}
							case Menu.EXIT:
								Console.WriteLine("Program terminated with errors, Exit Code (-13452i)");
								return;
						}
					}
					else
						throw new Exception("Invalid choice");
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				Console.Write("Select 1-5: ");
			} while (choice != Menu.EXIT);
		}
	}
}
