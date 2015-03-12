using System;
using MonoMac.CoreLocation;
using MonoMac.Foundation;

namespace AstronomicalTimes
{
	public class myLocation
	{

		public static int meCounter;

		CLLocationManager pos = null;
		private double latitude, longtitude;

		Func<bool,bool> callMethod;


	public myLocation ()
	{
				Console.WriteLine ("hoppa " + meCounter);
				meCounter++;
			pos = new CLLocationManager ();
			pos.DesiredAccuracy = 1000;


				pos.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
					//UpdateLocation (mainScreen, e.Locations [e.Locations.Length - 1]);
					CLLocation loc = e.Locations[e.Locations.Length - 1];
					CLLocation home = new CLLocation(52.00,5.00);
					double afstand = home.DistanceFrom(loc) / 1000;

					Console.WriteLine("longtitude: " + e.Locations[e.Locations.Length - 1].Coordinate.Longitude.ToString());
					Console.WriteLine("latitude  : " + e.Locations[e.Locations.Length - 1].Coordinate.Latitude.ToString());
					Console.WriteLine("Accuracy  : " + e.Locations[e.Locations.Length - 1].HorizontalAccuracy.ToString());
					Console.WriteLine("Distance  : " + afstand + " km");


					longtitude 	=  loc.Coordinate.Longitude;
					latitude	=  loc.Coordinate.Latitude;

					callMethod(true);
					pos.StopUpdatingLocation();
				};

				pos.StartUpdatingLocation ();

			}

		public void RunTheMethod(Func<bool,bool> myMethodName)
		{
			//... do stuff
			callMethod = myMethodName;

		}

			public double getLatitude() {
				return latitude;
			}

			public double getLongtitude() {
				return longtitude;
			}

		}
}

