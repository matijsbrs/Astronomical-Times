using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
//using AstronomicalTimes;
using Behrens.Solar;

namespace AstronomicalTimes
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{

		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{


		}

		#endregion

		public myLocation myPos = null;

		public void update_astroTimes() {

			//DateTime date = new DateTime(datum.DateValue);
			//	Console.WriteLine("datum: " + datum.DateValue.ToString());
			var dateTime = DateTime.SpecifyKind(this.datum.DateValue, DateTimeKind.Unspecified);


			//var str = string.Format ("{0} + {1}" ,dateTime.ToString("dd/MM/yyyy"), dateTime.Day);
			//this.label.StringValue = str;
			// double lat, double lon, int dayIn, int monthIn, int yearIn
			double altitude = 0;
			switch (Altitude_Item.IndexOfSelectedItem) {
			case 0: 
				altitude = -0.3;
				break;
			case 1:
				altitude = 0.0;
				break;
			case 2: 
				altitude = -6.0;
				break;
			case 3: 
				altitude = -12.0;
				break;
			case 4: 
				altitude = -18.0;
				break;

			}

			double lat = Double.Parse(Latitude.ObjectValue.ToString());
			double lon = Double.Parse(Lontitude.ObjectValue.ToString());


			int TimeZone = Int16.Parse (TimeZone_Value.ObjectValue.ToString ());
			bool dls = false;
			if (DLS_Value.ObjectValue.ToString ().CompareTo ("1") == 0)
				dls = true;


			astro tijd = new astro(lat,lon, dateTime.Hour, dateTime.Minute, dateTime.Day, dateTime.Month, dateTime.Year, altitude,TimeZone,dls);
			astro.Sun sun = tijd.getSun ();

			SunRise_value.StringValue = tijd.sunRiseStr;
			SunSet_Value.StringValue = tijd.sunSetStr;
			DayLength_value.StringValue = "" + sun.Diff * 2.0;
			SouthTime_value.StringValue =  "" + (int)tijd.SouthTime + ":" + (int)(60.0 * (tijd.SouthTime - ((int)tijd.SouthTime)));
			SolarDistance_value.StringValue = "" + ((int)(sun.Distance * 149597870.700)) + " km" ;

			SolarAzimut_Value.StringValue = tijd.getSun().Azimut.ToString();
			SolarAltitude_Value.StringValue = tijd.getSun ().SolarAngle.ToString();
			SolarRadius_Value.StringValue = "" + ((int)(tijd.getSun ().Radius * 1391684)) + " km" ; 



			//Console.WriteLine("date: " + date.Day ());
		}

		public bool updateLocations(bool state) {
			if ( state == true ) {
				Latitude.StringValue = myPos.getLatitude ().ToString ();
				Lontitude.StringValue = myPos.getLongtitude ().ToString ();
				update_astroTimes ();
			}
			return true;
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			Altitude_Item.RemoveAllItems ();

			Altitude_Item.AddItem ("Normal (-0.3)");
			Altitude_Item.AddItem ("No altitude (0.0)");
			Altitude_Item.AddItem ("Civil (-6.0)");
			Altitude_Item.AddItem ("Nautical (-12.0)");
			Altitude_Item.AddItem ("Astronimical (-18.0)");

			datum.Activated += (object sender, EventArgs e) =>  { update_astroTimes(); };
			Latitude.Activated += (object sender, EventArgs e) => {	update_astroTimes(); };
			Lontitude.Activated += (object sender, EventArgs e) => { update_astroTimes(); };
			Altitude_Item.Activated += (object sender, EventArgs e) => { update_astroTimes (); };
			DLS_Value.Activated += (object sender, EventArgs e) => { update_astroTimes (); };
			TimeZone_Value.Activated += (object sender, EventArgs e) => { update_astroTimes (); };


			//myPos = new myLocation ();
			//myPos.RunTheMethod (updateLocations);

			//datum.DateValue = DateTime.Today ;
			datum.DateValue = DateTime.Now;


			myPosButton.Activated += (object sender, EventArgs e) => {
				myPos = new myLocation ();
				myPos.RunTheMethod (updateLocations);
			};
			update_astroTimes ();
		}


		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}
	}
}

