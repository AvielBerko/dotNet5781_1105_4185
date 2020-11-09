using System;
using System.Collections.Generic;
using System.Linq;

namespace dotNet_5781_02_1105_4185
{
	class Program
	{
		/// <summary>
		/// Menu of actions for the buses list.
		/// </summary>
		enum Menu { Add = 1, Delete, Search, Print, Exit }
		/// <summary>
		/// Two cases for each choice in the menu.
		/// </summary>
		enum TwoCases { Case1 = 1, Case2 }
		/// <summary>
		/// BusList collection of all buses.
		/// </summary>
		static BusList buses;
		static Random rand = new Random();

		static void Main()
		{
			GenerateStations();
			GenerateFirstBuses();

			Console.WriteLine("1: Add a new bus-line or a new station to an existing bus-line\n" +
							"2: Delete a bus-line or a station from an existing bus-line\n" +
							"3: Search bus-lines by station code or buses by route\n" +
							"4: Print all bus-lines or all stations\n" +
							"5: Exit");
			Menu choice = Menu.Exit;
			do
			{
				Console.Write("\nSelect 1-5: ");

				try
				{
					Execute(out choice);
				}
				catch (Exception e)
				{
					PrintError(e);
				}
			} while (choice != Menu.Exit);
		}

		/// <summary>
		/// Handles the menu of actions.
		/// </summary>
		/// <param name="choice">User's action of choice.</param>
		static void Execute(out Menu choice)
		{
			if (Enum.TryParse(Console.ReadLine(), out choice))
			{
				switch (choice)
				{
					case Menu.Add:
						Add();
						break;
					case Menu.Delete:
						Delete();
						break;
					case Menu.Search:
						Search();
						break;
					case Menu.Print:
						Print();
						break;
					case Menu.Exit:
						Console.WriteLine("Bye bye");
						break;
					default:
						throw new InvalidOperationException("Unknown choice");
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		/// <summary>
		/// Adds a new bus-line to the list or adds a station to an existing bus-line. 
		/// </summary>
		static void Add()
		{
			Console.Write("Enter 1 to add a new bus-line or 2 to add a station to an existing bus-line: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					// Adds a new bus-line to the list
					case TwoCases.Case1:
						{
							GetBusIdentifier(out uint lineNum, out Direction dir);
							Console.Write("Enter bus area (0 - General, 1 - North, 2 - South, 3 - Center, 4 - Jerusalem, 5 - Eilat): ");
							if (!Enum.TryParse(Console.ReadLine(), out Areas area))
								throw new ArgumentException("Couldn't parse area");

							Console.WriteLine("Enter route of stations:\n");
							// A list of bus-stations for the new bus
							List<BusStation> busStations = new List<BusStation>();
							string input = string.Empty;
							do
							{
								// Keeps the loop of getting bus station going (in case of an error)
								try
								{
									// Adds a station to bus' route
									// For the first station - no need for distance and time from last station
									busStations.Add(ReceiveBusStation(busStations.Count == 0));
								}
								catch (ArgumentException e)
								{
									PrintError(e);
								}
								Console.WriteLine();
								// Mininum 2 station - ask the user for more (Optional)
								if (busStations.Count >= 2)
								{
									do
									{
										Console.Write("Add another station? [Y/n]: ");
										input = Console.ReadLine().ToLower();
									} while (input != "y" && input != "n");
								}
							} while (input != "n");

							// Adds the bus to the busList
							buses.AddBus(new Bus(lineNum, area, dir, busStations));
							Console.WriteLine("Added Seccessfuly");
							break;
						}
					//adds a station to an existing bus-line
					case TwoCases.Case2:
						{
							GetBusIdentifier(out uint lineNum, out Direction dir);
							var bus = buses[lineNum, dir];

							Console.Write("Enter station code to add after (Keep empty to add as the first station): ");
							string codeInput = Console.ReadLine();
							// If afterStation is null - inserts the station at the begining of the route
							Station afterStation = null;
							// if the input is empty - no ask for afterStation
							if (codeInput != string.Empty)
							{
								if (!uint.TryParse(codeInput, out uint code))
									throw new ArgumentException("Couldn't parse Code");

								// Finds the station to add after - if null add first
								afterStation = Station.Stations.Find((item) => item.Code == code);
							}

							// Receive a new station and insert it to bus' route
							BusStation newStation = ReceiveBusStation(afterStation == null);
							bus.InsertStation(newStation, afterStation);

							Console.WriteLine("Added Seccessfuly");
							break;
						}
					default:
						throw new InvalidOperationException("Unknown choice");
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		/// <summary>
		/// Deletes an existing bus-line or removes a station from an existing bus-line. 
		/// </summary>
		static void Delete()
		{
			Console.Write("Enter 1 to delete a bus-line or 2 to remove a station from an existing bus-line: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					// Deletes an existing bus-line.
					case TwoCases.Case1:
						{
							GetBusIdentifier(out uint lineNum, out Direction dir);

							// Removes the bus from buses list
							buses.RemoveBus(buses[lineNum, dir]);
							Console.WriteLine("Deleted Seccessfuly");
							break;
						}

					// Removes a station from an existing bus-line. 
					case TwoCases.Case2:
						{
							GetBusIdentifier(out uint lineNum, out Direction dir);

							Console.Write("Enter station code to remove: ");
							if (!uint.TryParse(Console.ReadLine(), out uint code))
								throw new ArgumentException("Couldn't parse Code");

							// Finds the station and removes it
							Station station = Station.Stations.Find((item) => item.Code == code);
							buses[lineNum, dir].RemoveStation(station);

							// If station has no buses - remove it from stations list
							if (buses.BusesOfStation(station).Count == 0)
								Station.Stations.Remove(station);

							Console.WriteLine("Removed Seccessfuly");
							break;
						}
					default:
						throw new InvalidOperationException("Unknown choice");
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		/// <summary>
		/// Finds all buses stoping at a station or search a bus for a route (listed by time).
		/// </summary>
		static void Search()
		{
			Console.Write("Enter 1 to find buses stoping at a station or 2 to search a bus for a route: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					// Finds all buses stoping at a station
					case TwoCases.Case1:
						{
							Console.Write("Enter station code: ");
							if (!uint.TryParse(Console.ReadLine(), out uint code))
								throw new ArgumentException("Couldn't parse Code");

							// Gets all buses lines stoping at the station
							var stationBuses = buses.BusesOfStation(code);
							var lines = (from bus in stationBuses select bus.Line).ToArray();

							Console.Write("Buses: ");
							Console.WriteLine(string.Join(", ", lines) + "\n");
							break;
						}

					// Search a bus for a route (listed by time)
					case TwoCases.Case2:
						{
							Console.Write("Enter starting station code: ");
							if (!uint.TryParse(Console.ReadLine(), out uint startingCode))
								throw new ArgumentException("Couldn't parse Code");
							Station startingStation = Station.Stations.Find((item) => item.Code == startingCode);

							Console.Write("Enter ending station code: ");
							if (!uint.TryParse(Console.ReadLine(), out uint endingCode))
								throw new ArgumentException("Couldn't parse Code");
							Station endingStation = Station.Stations.Find((item) => item.Code == endingCode);

							var routes = new BusList();

							// Adds all buses with subroute between the 2 station
							foreach (Bus bus in buses)
							{
								// If there is no subroute - don't break
								try
								{
									routes.AddBus(bus.GetSubRoute(startingStation, endingStation));
								}
								catch { }
							}

							foreach (var bus in routes.BusesByTime())
								Console.WriteLine($"bus-line: {bus.Line,3}, time: {bus.RouteTime():n2} minutes");
							break;
						}
					default:
						throw new InvalidOperationException("Unknown choice");
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		/// <summary>
		/// Prints all bus lines or all stations data.
		/// </summary>
		static void Print()
		{
			Console.Write("Enter 1 to print all bus-lines or 2 to print all stations data: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					// Print all bus lines
					case TwoCases.Case1:
						{
							Console.WriteLine();
							foreach (var bus in buses)
								Console.WriteLine(bus + "\n");
							break;
						}
					// Print all stations data
					case TwoCases.Case2:
						{
							Console.WriteLine();
							foreach (var station in Station.Stations)
							{
								Console.WriteLine(station);

								// List of buses stoping at the station
								var stationBuses = buses.BusesOfStation(station);
								var lines = (from bus in stationBuses select bus.Line).ToArray();
								Console.Write("Buses: ");
								Console.WriteLine(string.Join(", ", lines) + "\n");
							}
							break;
						}
					default:
						throw new InvalidOperationException("Unknown choice");
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		/// <summary>
		/// Prints an error to the console (Red)
		/// </summary>
		/// <param name="e">An exeption to print</param>
		static void PrintError(Exception e)
		{
			var temp = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(e.Message);
			Console.ForegroundColor = temp;
		}

		/// <summary>
		/// Receives from the user a bus' line and direction
		/// </summary>
		/// <param name="lineNum">Bus' line number</param>
		/// <param name="dir">Bus' direction</param>
		private static void GetBusIdentifier(out uint lineNum, out Direction dir)
		{
			Console.Write("Enter bus line-number: ");
			if (!uint.TryParse(Console.ReadLine(), out lineNum))
				throw new ArgumentException("Couldn't parse line number");
			Console.Write("Enter bus direction (1 - Go, 2 - Return): ");
			if (!Enum.TryParse(Console.ReadLine(), out dir))
				throw new ArgumentException("Couldn't parse direction");
		}

		/// <summary>
		/// Receives a bus station from the user.
		/// </summary>
		/// <param name="isFirst">If station is the first in the route</param>
		/// <returns></returns>
		static BusStation ReceiveBusStation(bool isFirst = false)
		{
			Console.Write("Enter station code: ");
			if (!uint.TryParse(Console.ReadLine(), out uint code))
				throw new ArgumentException("Couldn't parse Code");

			// If station doesn't exist - creates a new one
			Station station = Station.Stations.Find((item) => item.Code == code)
				?? new Station(code, addresses[rand.Next(0, addresses.Length)]);

			// If the station is first - no ask for distance and time
			if (!isFirst)
			{
				Console.Write("Enter distance to last station (Meters): ");
				if (!double.TryParse(Console.ReadLine(), out double distance))
					throw new ArgumentException("Couldn't parse Distance");
				Console.Write("Enter time to last station (Minutes): ");
				if (!double.TryParse(Console.ReadLine(), out double time))
					throw new ArgumentException("Couldn't parse Time");
				return new BusStation(station, distance, time);
			}
			else
				return new BusStation(station, 0, 0);

		}

		/// <summary>
		/// Randomly generates a list of stations.
		/// </summary>
		static void GenerateStations()
		{
			int numOfStations = rand.Next(40, 60);
			for (int i = 0; i < numOfStations; i++)
			{
				try
				{
					new Station((uint)rand.Next(100000, 999999), addresses[rand.Next(0, addresses.Length)]);
				}
				catch
				{
					i--;
				}
			}
		}

		/// <summary>
		/// Randomly generates a list of buses.
		/// </summary>
		/// <returns>A busList of the created buses.</returns>
		static void GenerateFirstBuses()
		{
			buses = new BusList();

			var lstStations = new List<Station>(Station.Stations);
			for (int i = 0; i < 10; i++)
			{
				try
				{
					Bus bus = new Bus((uint)rand.Next(1, 1000), (Areas)rand.Next(0, 6), Direction.Go, GenerateRoute(ref lstStations));
					buses.AddBus(bus);
					// Adds the opposite direction bus
					var oppositeRoute = new List<BusStation>(bus.Route);
					oppositeRoute.Reverse();
					buses.AddBus(new Bus(bus.Line, bus.Area, Direction.Return, oppositeRoute));
				}
				// Retry to generate a bus when a bus with the same line already exists.
				catch { i--; }
			}
		}

		/// <summary>
		/// Generates a random route to a bus.
		/// </summary>
		/// <param name="lst">List of least used stations</param>
		/// <returns>Route (list of bus stations)</returns>
		static List<BusStation> GenerateRoute(ref List<Station> lst)
		{
			var numOfStations = rand.Next(10, 20);
			List<BusStation> result = new List<BusStation>();
			for (int i = 0; i < numOfStations; i++)
			{
				int temp = rand.Next(0, lst.Count);
				result.Add(new BusStation(lst[temp], 1000 * rand.NextDouble(), 10 * rand.NextDouble() + 0.5));
				lst.RemoveAt(temp);
				if (lst.Count == 0)
					lst = new List<Station>(Station.Stations);
			}
			return result;
		}

		/// <summary>
		/// Array of addresses.
		/// </summary>
		static string[] addresses = new string[]
		{
			"777 Brockton Avenue, Abington MA 2351",
			"30 Memorial Drive, Avon MA 2322",
			"250 Hartford Avenue, Bellingham MA 2019",
			"700 Oak Street, Brockton MA 2301",
			"66-4 Parkhurst Rd, Chelmsford MA 1824",
			"591 Memorial Dr, Chicopee MA 1020",
			"55 Brooksby Village Way, Danvers MA 1923",
			"137 Teaticket Hwy, East Falmouth MA 2536",
			"42 Fairhaven Commons Way, Fairhaven MA 2719",
			"374 William S Canning Blvd, Fall River MA 2721",
			"121 Worcester Rd, Framingham MA 1701",
			"677 Timpany Blvd, Gardner MA 1440",
			"337 Russell St, Hadley MA 1035",
			"295 Plymouth Street, Halifax MA 2338",
			"1775 Washington St, Hanover MA 2339",
			"280 Washington Street, Hudson MA 1749",
			"20 Soojian Dr, Leicester MA 1524",
			"11 Jungle Road, Leominster MA 1453",
			"301 Massachusetts Ave, Lunenburg MA 1462",
			"780 Lynnway, Lynn MA 1905",
			"70 Pleasant Valley Street, Methuen MA 1844",
			"830 Curran Memorial Hwy, North Adams MA 1247",
			"1470 S Washington St, North Attleboro MA 2760",
			"506 State Road, North Dartmouth MA 2747",
			"742 Main Street, North Oxford MA 1537",
			"72 Main St, North Reading MA 1864",
			"200 Otis Street, Northborough MA 1532",
			"180 North King Street, Northhampton MA 1060",
			"555 East Main St, Orange MA 1364",
			"555 Hubbard Ave-Suite 12, Pittsfield MA 1201",
			"300 Colony Place, Plymouth MA 2360",
			"301 Falls Blvd, Quincy MA 2169",
			"36 Paramount Drive, Raynham MA 2767",
			"450 Highland Ave, Salem MA 1970",
			"1180 Fall River Avenue, Seekonk MA 2771",
			"1105 Boston Road, Springfield MA 1119",
			"100 Charlton Road, Sturbridge MA 1566",
			"262 Swansea Mall Dr, Swansea MA 2777",
			"333 Main Street, Tewksbury MA 1876",
			"550 Providence Hwy, Walpole MA 2081",
			"352 Palmer Road, Ware MA 1082",
			"3005 Cranberry Hwy Rt 6 28, Wareham MA 2538",
			"250 Rt 59, Airmont NY 10901",
			"141 Washington Ave Extension, Albany NY 12205",
			"13858 Rt 31 W, Albion NY 14411",
			"2055 Niagara Falls Blvd, Amherst NY 14228",
			"101 Sanford Farm Shpg Center, Amsterdam NY 1201",
			"297 Grant Avenue, Auburn NY 13021",
			"4133 Veterans Memorial Drive, Batavia NY 14020",
			"6265 Brockport Spencerport Rd, Brockport NY 144",
			"5399 W Genesse St, Camillus NY 13031",
			"3191 County rd 10, Canandaigua NY 14424",
			"30 Catskill, Catskill NY 12414",
			"161 Centereach Mall, Centereach NY 11720",
			"3018 East Ave, Central Square NY 13036",
			"100 Thruway Plaza, Cheektowaga NY 14225",
			"8064 Brewerton Rd, Cicero NY 13039",
			"5033 Transit Road, Clarence NY 14031",
			"3949 Route 31, Clay NY 13041",
			"139 Merchant Place, Cobleskill NY 12043",
			"85 Crooked Hill Road, Commack NY 11725",
			"872 Route 13, Cortlandville NY 13045",
			"279 Troy Road, East Greenbush NY 12061",
			"2465 Hempstead Turnpike, East Meadow NY 11554",
			"6438 Basile Rowe, East Syracuse NY 13057",
			"25737 US Rt 11, Evans Mills NY 13637",
			"901 Route 110, Farmingdale NY 11735",
			"2400 Route 9, Fishkill NY 12524",
			"10401 Bennett Road, Fredonia NY 14063",
			"1818 State Route 3, Fulton NY 13069",
			"4300 Lakeville Road, Geneseo NY 14454",
			"990 Route 5 20, Geneva NY 14456",
			"311 RT 9W, Glenmont NY 12077",
			"200 Dutch Meadows Ln, Glenville NY 12302",
			"100 Elm Ridge Center Dr, Greece NY 14626",
			"1549 Rt 9, Halfmoon NY 12065",
			"5360 Southwestern Blvd, Hamburg NY 14075",
			"103 North Caroline St, Herkimer NY 13350",
			"1000 State Route 36, Hornell NY 14843",
			"1400 County Rd 64, Horseheads NY 14845",
			"135 Fairgrounds Memorial Pkwy, Ithaca NY 14850",
			"2 Gannett Dr, Johnson City NY 13790",
			"233 5th Ave Ext, Johnstown NY 12095",
			"601 Frank Stottile Blvd, Kingston NY 12401",
			"350 E Fairmount Ave, Lakewood NY 14750",
			"4975 Transit Rd, Lancaster NY 14086",
			"579 Troy-Schenectady Road, Latham NY 12110",
			"5783 So Transit Road, Lockport NY 14094",
			"7155 State Rt 12 S, Lowville NY 13367",
			"425 Route 31, Macedon NY 14502",
			"3222 State Rt 11, Malone NY 12953",
			"200 Sunrise Mall, Massapequa NY 11758",
			"43 Stephenville St, Massena NY 13662",
			"750 Middle Country Road, Middle Island NY 11953",
			"470 Route 211 East, Middletown NY 10940",
			"3133 E Main St, Mohegan Lake NY 10547",
			"288 Larkin, Monroe NY 10950",
			"41 Anawana Lake Road, Monticello NY 12701",
			"4765 Commercial Drive, New Hartford NY 13413",
			"1201 Rt 300, Newburgh NY 12550"
		};
	}
}
