using dotNet_5781_02_1105_4185.src;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_02_1105_4185
{
	class Program
	{
		enum Menu { Add = 1, Delete, Search, Print, Exit }
		enum TwoCases { Case1 = 1, Case2 }

		static BusList buses;
		static Random rand = new Random();

		static void Main(string[] args)
		{
			GenerateStations();
			buses = GenerateFirstBuses();
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
					Console.WriteLine(e.Message);
				}
			} while (choice != Menu.Exit);
		}

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
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		static void Add()
		{
			Console.Write("Enter 1 to add a new bus-line or 2 to add a station to an existing bus-line: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					case TwoCases.Case1:
						{
							Console.Write("Enter bus line-number: ");
							if (!uint.TryParse(Console.ReadLine(), out uint lineNum))
								throw new ArgumentException("Line number is invalid");
							Console.Write("Enter bus area (0 - General, 1 - North, 2 - South, 3 - Center, 4 - Jerusalem, 5 - Eilat): ");
							if (!Enum.TryParse(Console.ReadLine(), out Areas area))
								throw new ArgumentException("Area is invalid");
							Console.Write("Enter bus direction (1 - Go, 2 - Return): ");
							if (!Enum.TryParse(Console.ReadLine(), out Direction dir))
								throw new ArgumentException("Direction is invalid");

							List<BusStation> busStations = new List<BusStation>();
							string input = string.Empty;
							do
							{
								busStations.Add(CreateBusStation());

								do
								{
									Console.Write("Continue? [Y/n]: ");
									input = Console.ReadLine().ToLower();
								} while (input != "y" && input != "n");
							} while (input != "n");

							buses.AddBus(new Bus(lineNum, area, dir, busStations));
							Console.WriteLine("Added Seccessfuly");
							break;
						}

					case TwoCases.Case2:
						{
							Console.Write("Enter bus line-number: ");
							if (!uint.TryParse(Console.ReadLine(), out uint lineNum))
								throw new ArgumentException("Line number is invalid");
							Console.Write("Enter bus direction (1 - Go, 2 - Return): ");
							if (!Enum.TryParse(Console.ReadLine(), out Direction dir))
								throw new ArgumentException("Direction is invalid");

							Console.Write("Enter station code to add after (Keep empty to add as the first station): ");
							if (!uint.TryParse(Console.ReadLine(), out uint code))
								throw new ArgumentException("Could'nt parse Code");
							Station afterStation = Station.Stations.Find((item) => item.Code == code);

							Console.Write("Enter station code for bus route: ");
							BusStation newStation = CreateBusStation();

							buses[lineNum, dir].InsertStation(newStation, afterStation);
							Console.WriteLine("Added Seccessfuly");
							break;
						}
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		static void Delete()
		{
			Console.Write("Enter 1 to delete a bus-line or 2 to remove a station from an existing bus-line: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					case TwoCases.Case1:
						{
							Console.Write("Enter bus line-number: ");
							if (!uint.TryParse(Console.ReadLine(), out uint lineNum))
								throw new ArgumentException("Line number is invalid");
							Console.Write("Enter bus direction (1 - Go, 2 - Return): ");
							if (!Enum.TryParse(Console.ReadLine(), out Direction dir))
								throw new ArgumentException("Direction is invalid");
							buses.RemoveBus(buses[lineNum, dir]);
							Console.WriteLine("Deleted Seccessfuly");
							break;
						}

					case TwoCases.Case2:
						{
							Console.Write("Enter bus line-number: ");
							if (!uint.TryParse(Console.ReadLine(), out uint lineNum))
								throw new ArgumentException("Line number is invalid");
							Console.Write("Enter bus direction (1 - Go, 2 - Return): ");
							if (!Enum.TryParse(Console.ReadLine(), out Direction dir))
								throw new ArgumentException("Direction is invalid");

							Console.Write("Enter station code to remove: ");
							if (!uint.TryParse(Console.ReadLine(), out uint code))
								throw new ArgumentException("Could'nt parse Code");
							Station station = Station.Stations.Find((item) => item.Code == code);

							buses[lineNum, dir].RemoveStation(station);
							Console.WriteLine("Removed Seccessfuly");
							break;
						}
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		static void Search()
		{
			Console.Write("Enter 1 to find buses stoping at a station or 2 to search a bus for a route: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					case TwoCases.Case1:
						{
							Console.Write("Enter station code: ");
							if (!uint.TryParse(Console.ReadLine(), out uint code))
								throw new ArgumentException("Could'nt parse Code");
							Station station = Station.Stations.Find((item) => item.Code == code);

							Console.Write("Buses: ");
							List<uint> lst = new List<uint>();
							foreach (Bus bus in buses)
							{
								if (!lst.Contains(bus.BusLine) && bus.BusRoute.Any((item) => item.Station == station))
									lst.Add(bus.BusLine);
							}
							Console.WriteLine(string.Join(", ", lst));
							Console.WriteLine();
							break;
						}

					case TwoCases.Case2:
						{
							Console.Write("Enter starting station code: ");
							if (!uint.TryParse(Console.ReadLine(), out uint startingCode))
								throw new ArgumentException("Could'nt parse Code");
							Station startingStation = Station.Stations.Find((item) => item.Code == startingCode);

							Console.Write("Enter ending station code: ");
							if (!uint.TryParse(Console.ReadLine(), out uint endingCode))
								throw new ArgumentException("Could'nt parse Code");
							Station endingStation = Station.Stations.Find((item) => item.Code == endingCode);

							List<Bus> routes = new List<Bus>();
							foreach (Bus bus in buses)
							{
								try
								{
									routes.Add(bus.GetSubRoute(startingStation, endingStation));
								}
								catch { }
							}

							var query = from bus in routes orderby bus.RouteTime() select bus;
							foreach (var bus in query)
								Console.WriteLine($"bus-line: {bus.BusLine,3}, time: {bus.RouteTime():n2} minutes");
							break;
						}
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		static void Print()
		{
			Console.Write("Enter 1 to print all bus-lines or 2 to print all stations data: ");
			if (Enum.TryParse(Console.ReadLine(), out TwoCases caseChoice))
			{
				switch (caseChoice)
				{
					case TwoCases.Case1:
						{
							foreach (var bus in buses)
								Console.WriteLine(bus + "\n");
							break;
						}

					case TwoCases.Case2:
						{
							foreach (var station in Station.Stations)
							{
								Console.WriteLine(station);
								Console.Write("Buses: ");
								List<uint> lst = new List<uint>();
								foreach (Bus bus in buses)
								{
									if (!lst.Contains(bus.BusLine) && bus.BusRoute.Any((item) => item.Station == station))
										lst.Add(bus.BusLine);
								}
								Console.WriteLine(string.Join(", ", lst));
								Console.WriteLine();
							}
							break;
						}
				}
			}
			else
				throw new InvalidOperationException("Invalid choice");
		}

		static BusStation CreateBusStation()
		{
			Console.Write("Enter station code: ");
			if (!uint.TryParse(Console.ReadLine(), out uint code))
				throw new ArgumentException("Could'nt parse Code");

			Station station = Station.Stations.Find((item) => item.Code == code)
				?? new Station(code, addresses[rand.Next(0, addresses.Length)]);

			Console.Write("Enter distance to last station (Meters): ");
			if (!double.TryParse(Console.ReadLine(), out double distance))
				throw new ArgumentException("Could'nt parse Distance");
			Console.Write("Enter time to last station (Minutes): ");
			if (!double.TryParse(Console.ReadLine(), out double time))
				throw new ArgumentException("Could'nt parse Time");

			return new BusStation(station, distance, time);
		}

		static void GenerateStations()
		{
			for (int i = 0; i < 40; i++)
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

		static BusList GenerateFirstBuses()
		{
			var lstStations = new List<Station>(Station.Stations);
			var buses = new BusList();
			for (int i = 0; i < 10; i++)
			{
				Bus bus = new Bus((uint)rand.Next(1, 1000), (Areas)rand.Next(0, 6), Direction.Go, GenerateRoute(ref lstStations));
				buses.AddBus(bus);
				var oppositeRoute = new List<BusStation>(bus.BusRoute);
				oppositeRoute.Reverse();
				buses.AddBus(new Bus(bus.BusLine, bus.Area, Direction.Return, oppositeRoute));
			}
			return buses;
		}

		static List<BusStation> GenerateRoute(ref List<Station> lst)
		{
			var numOfStations = rand.Next(10, 20);
			List<BusStation> result = new List<BusStation>();
			for (int i = 0; i < numOfStations; i++)
			{
				int temp = rand.Next(0, lst.Count);
				result.Add(new BusStation(lst[temp], 1000 * rand.NextDouble(), 10 * rand.NextDouble()));
				lst.RemoveAt(temp);
				if (lst.Count == 0)
					lst = new List<Station>(Station.Stations);
			}
			return result;
		}

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
