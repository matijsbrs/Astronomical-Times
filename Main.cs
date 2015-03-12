using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using AstronomicalTimes;

namespace AstronomicalTimes
{
	class MainClass
	{
		static void Main (string[] args)
		{


			//astro cal2 = new astro (52.0f, 5.0f, 1, 1, 2014);
			NSApplication.Init ();
			NSApplication.Main (args);

		}
	}
}

