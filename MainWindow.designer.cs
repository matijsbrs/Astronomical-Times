// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace AstronomicalTimes
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSPopUpButton Altitude_Item { get; set; }

		[Outlet]
		MonoMac.AppKit.NSDatePicker datum { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField DayLength_value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton DLS_Value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField Latitude { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField Lontitude { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButtonCell myPosButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SolarAltitude_Value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SolarAzimut_Value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SolarDistance_value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SolarRadius_Value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SouthTime_value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SunRise_value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SunSet_Value { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField TimeZone_Value { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Altitude_Item != null) {
				Altitude_Item.Dispose ();
				Altitude_Item = null;
			}

			if (datum != null) {
				datum.Dispose ();
				datum = null;
			}

			if (DayLength_value != null) {
				DayLength_value.Dispose ();
				DayLength_value = null;
			}

			if (Latitude != null) {
				Latitude.Dispose ();
				Latitude = null;
			}

			if (Lontitude != null) {
				Lontitude.Dispose ();
				Lontitude = null;
			}

			if (myPosButton != null) {
				myPosButton.Dispose ();
				myPosButton = null;
			}

			if (SolarDistance_value != null) {
				SolarDistance_value.Dispose ();
				SolarDistance_value = null;
			}

			if (SouthTime_value != null) {
				SouthTime_value.Dispose ();
				SouthTime_value = null;
			}

			if (SunRise_value != null) {
				SunRise_value.Dispose ();
				SunRise_value = null;
			}

			if (SunSet_Value != null) {
				SunSet_Value.Dispose ();
				SunSet_Value = null;
			}

			if (TimeZone_Value != null) {
				TimeZone_Value.Dispose ();
				TimeZone_Value = null;
			}

			if (SolarRadius_Value != null) {
				SolarRadius_Value.Dispose ();
				SolarRadius_Value = null;
			}

			if (SolarAltitude_Value != null) {
				SolarAltitude_Value.Dispose ();
				SolarAltitude_Value = null;
			}

			if (SolarAzimut_Value != null) {
				SolarAzimut_Value.Dispose ();
				SolarAzimut_Value = null;
			}

			if (DLS_Value != null) {
				DLS_Value.Dispose ();
				DLS_Value = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
