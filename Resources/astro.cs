using System;

namespace AstronomicalTimes
{
	static class Constants {
		public const double RADEG = 57.29577951308233; //!< Constant used to convert radians to degrees
		public const double DEGRAD = 0.01745329251994;  //!< Constant used to convert degrees to radians
	}



	public class astro
	{
		public double Longtitude;      	//!< longtitude postive is East, negative is West
		public double Latitude;       	//!< latitude positive is North, negative is South
		public double timeZone = +1.0; 	// Amsterdam
		int year,month,day,hour,minute; //!< date and time
		double days;           			//!< Days since 2000 Jan 0.0 (negative before)
		double sunTime;         		//!< Local sidereal time
		public string sunSetStr;
		public string sunRiseStr;
		public Double SouthTime;
		public bool DayLightSavings;

		Sun zon;

		public class Sun {
			public double Latitude;
			public double Longitude;
			public double Duration; //!< Solar duration
			public double Distance; 
			public double Radius;
			public double Diff;
			public double Altitude;
			public double days;
			public double Declination;
			public double Equatorial;
			public double solarLongtitude;
			public double solarLatitude;

			public double Azimut;
			public double SolarAngle;
			public int year,month,day,hour,minute;

			public Sun() {
				solarLatitude 	= 0.0;
				Altitude 		= -0.55; // normal
				//Altitude		= -6.0f; // Civil
			}

			public Sun(double altitude) {
				Altitude = altitude;
			}

			public double Position() {


				double days = (month-1)*(30.4375f) + day + (hour) / 23.75f + (minute/1425.0f);
				double Declination = -23.45 * Math.Cos((Constants.DEGRAD * 360.0f * (days + 10.0f)) / 365.0f);
				double EOT = 60.0f * (-0.171f * Math.Sin((0.0337f*days+0.465f)) - 0.1299f * Math.Sin((0.01787f*days-0.168f)));
				double LHA = 15.0f * (hour + (minute/60.0f) - (15.0f-Longitude)/15.0f - 12.0f + EOT/60.0f);

				double x = Math.Sin((Constants.DEGRAD * Latitude)) * Math.Sin((Constants.DEGRAD * Declination)) + Math.Cos((Constants.DEGRAD * Latitude)) * Math.Cos((Constants.DEGRAD * Declination)) * Math.Cos((Constants.DEGRAD * LHA));
				double y = - (Math.Sin((Constants.DEGRAD * Latitude)) * x - Math.Sin((Constants.DEGRAD * Declination))) / (Math.Cos((Constants.DEGRAD * Latitude)) * Math.Sin(Math.Acos(x)));
				SolarAngle = Math.Asin(x) / Constants.DEGRAD;
				//double reverseX = sin(SolarAngle * K);
				//printf("\tSolarAngle: %f orignal(x):%f x:%f\n",SolarAngle,x, reverseX );

				double stop = 12.0 + (15.0-Longitude)/15.0 - EOT/60.0;

				 
				if ( (hour + (minute/60.0)) <= stop) {
					Azimut = Math.Acos(y) / Constants.DEGRAD;
				} else {
					Azimut = 360.0 - Math.Acos(y) / Constants.DEGRAD;
				}

				//Console.WriteLine("Azimut: %f of %f of %f decl:%f EOT:%f x:%f\n",Azimut, (Math.Acos(y) / Constants.DEGRAD), (360.0 - Math.Acos(y) / Constants.DEGRAD), Declination, EOT,x);
				return SolarAngle;
			}


			public double Position(double day) {
				double M,  //!< Mean anomaly of the Sun
				w,         //!< Mean longitude of perihelion Note: Sun's mean longitude = M + w
				e,         //!< Eccentricity of Earth's orbit
				E,         //!< Eccentric anomaly
				x, y,      //!< x, y coordinates in orbit
				v;         //!< True anomaly

				/* Compute mean elements */
				M = revolution( 356.0470 + 0.9856002585 * day );
				w = 282.9404 + 4.70935E-5 * day;
				e = 0.016709 - 1.151E-9 * day;

				/* Compute true longitude and radius vector */
				E = M + e * Constants.RADEG * Math.Sin(M*Constants.DEGRAD) * ( 1.0 + e * Math.Cos(M*Constants.DEGRAD) );
				x = Math.Cos(E*Constants.DEGRAD) - e;
				y = Math.Sqrt (1.0 - e * e) * Math.Sin (E * Constants.DEGRAD);
				Distance = Math.Sqrt( x*x + y*y );              /* Solar distance */
				v = Math.Atan2 (y, x) * Constants.RADEG;  // True anomaly
				solarLongtitude = v + w;                        /* True solar longitude */
				if ( solarLongtitude >= 360.0 )
					solarLongtitude -= 360.0;                   /* Make it 0..360 degrees */

				Radius       = 0.2666 / Distance;     // Compute the Sun's apparent radius in degrees

				//Console.WriteLine ("days{0}, x{1}, y{2} ,angle:{3}", day, x, y, (Math.Asin(x) / Constants.DEGRAD));

				return solarLongtitude;
			}
				

			public void SunEquatorialDeclination()
			/******************************************************/
			/* Computes the Sun's equatorial coordinates RA, Decl */
			/* and also its distance, at an instant given in d,   */
			/* the number of days since 2000 Jan 0.0.             */
			/******************************************************/
			{
				double obl_ecl, x, y, z;

				/* Compute ecliptic rectangular coordinates (z=0) */
				x = Distance * Math.Cos(solarLongtitude * Constants.DEGRAD);
				y = Distance * Math.Sin(solarLongtitude * Constants.DEGRAD);

				/* Compute obliquity of ecliptic (inclination of Earth's axis) */
				obl_ecl = 23.4393 - (3.563E-7 * days);

				/* Convert to equatorial rectangular coordinates - x is unchanged */
				z = y * Math.Sin(obl_ecl*Constants.DEGRAD);
				y = y * Math.Cos(obl_ecl*Constants.DEGRAD);
				//Console.WriteLine ("solarlong: {4}, obl:{0}, x:{1} y:{2} z:{3}", obl_ecl, x, y, z, solarLongtitude);

				/* Convert to spherical coordinates */
				Equatorial     = Constants.RADEG*Math.Atan2( y, x );
				Declination    = Constants.RADEG*Math.Atan2( z, Math.Sqrt(x*x + y*y) );
    
			}  /* sun_RA_dec */



			/*! 
			    @fn double sunDiff(double altitude, double latitude, double sunDeclanation, double * sunDuration)
			    @brief calculate the solarhigh point. and solar arc duration
			*/
			public double Difference() {
				//Console.WriteLine ("Altitude:{0}, Radius: {1}",Altitude, Radius);
				double a, b, c, d, e;
				a = Math.Sin ((Altitude-Radius) * Constants.DEGRAD);
				b = Math.Sin ((Latitude) * Constants.DEGRAD);
				c = Math.Sin (Declination * Constants.DEGRAD);
				d = Math.Cos (Latitude*Constants.DEGRAD);
				e = Math.Cos (Declination * Constants.DEGRAD);
				//Console.WriteLine("a:{0}, b:{1}, c{2}, d{3}, e{4}", a,b,c,d,e );
				Duration = (a - b * c) / (d * e);
				//Duration    = ( Math.Sin((Altitude-Radius)*Constants.DEGRAD) - Math.Sin((Latitude)*Constants.DEGRAD) * Math.Sin(Declination*Constants.DEGRAD) ) 
				//	/ ( Math.Cos(Latitude*Constants.DEGRAD) * Math.Cos(Declination*Constants.DEGRAD) );

				Diff = (Constants.RADEG * Math.Acos(Duration)) / 15.0; // The diurnal arc in hours
				//Console.WriteLine ("Duration: {0}, Diff: {1}", Duration, Diff);
				return Diff;
			}

			public double GMST0( double d )
			{
				double sidtim0;
				/* sunTime at 0h UT = L (Sun's mean longitude) + 180.0 degr  */
				/* L = M + w, as defined in SunPosition().  Since I'm too lazy to */
				/* add these numbers, I'll let the C compiler do it for me.  */
				/* Any decent C compiler will add the constants at compile   */
				/* time, imposing no runtime or code overhead.               */
				sidtim0 = revolution( ( 180.0 + 356.0470 + 282.9404 ) + ( 0.9856002585 + 4.70935E-5 ) * d );
				return sidtim0;
			}  /* GMST0 */


			// return 0..360 for a given angle/revolutions
			 
			public double revolution(double angle) {
				//double angle = GMST0 (days) + 180.0f + Longtitude;
				return( angle - 360.0 * Math.Floor( angle * (1.0/360.0) ) );
			}

			/*
			 revlutions -180..+180
			 */
			public double revolution_180(double x ) {
				//double x = (sunTime - Equatorial);

				return( x - 360.0 * Math.Floor( x * (1.0/360.0) + 0.5 ) );
			}
		}


		public astro ( double lat, double lon, int hourIn, int minuteIn, int dayIn, int monthIn, int yearIn, double altitudeIn, int timeZoneIn, bool useDayLightSavingsIn) {
			Latitude = lat;
			Longtitude = lon;
			day = dayIn;
			month = monthIn;
			year = yearIn;
			hour = hourIn; // 12 hour default time.
			minute = minuteIn;
			timeZone = timeZoneIn;
			zon = new Sun (altitudeIn);
			DayLightSavings = useDayLightSavingsIn;
			superCalc ();
		}

		public astro ( double lat, double lon, int dayIn, int monthIn, int yearIn, double altitudeIn ) {
			Latitude = lat;
			Longtitude = lon;
			day = dayIn;
			month = monthIn;
			year = yearIn;
			hour = 12; // 12 hour default time.
 			minute = 0;
			timeZone = 1; // default is home ;-)
			DayLightSavings = true;
			zon = new Sun (altitudeIn);
			superCalc ();
		}

		public astro ()	{
			// ascension island		: -7.945200,-14.368068
			// Centrum Nederland 	: 52.0,5,0

			Latitude 	= 52.0;	
			Longtitude 	= 5.0;
			minute 		= 0;
			hour 		= 12;
			day 		= 1;
			month	 	= 1;
			year 		= 2014;
			zon = new Sun ();
			superCalc ();
		}

		int dayCount(int year, int month, int day) {
			double output = (year - 2000.0) * 365.2425;
			output += (month - 1.0) * 30.436857;
			output += day;
			return (int) output;
		}

		public Sun getSun() {
			return zon;
		}

		public bool isSummer(int year, int month, int day, int hour, int minutes) {
			bool output = false;
			DateTime a = new DateTime (year, month, day);
			int dow = (int) a.DayOfWeek;

			if ( (month >= 10 ) || (month < 3 ) ) {
				if ( month == 10 ) {
					if ( (day >= 25) && (dow == 6) && (hour >= 2))
						output = false;
					else
						output = true;
				} else {
					output = false;
				}
			} 
			if ( (month >= 3 ) && (month < 10 ) ) {
				if ( month == 3 ) {
					if ( ((day >= 25) && (dow == 6) && (hour >= 2)) || 
						((day - dow) > 25) )
						output = true;
					else 
						output = false;
				} else {
					output = true;	
				}
			} 
			return output; // 
		}

		public bool isSummer(int year, int month, int day) {
			return isSummer (year, month, day, 12, 0); // assume midday.
		}

		void superCalc() {
			days     = dayCount(year, month, day)+(12/24) - Longtitude/360.0;
			//sunTime  = revolution( GMST0(input->result.days) + 180.0f + input->me.longtitude );       // Time in respect to sun
			zon.Longitude 	= Longtitude;
			zon.Latitude	= Latitude;
			zon.days 		= days;
			zon.year 		= year;
			zon.month 		= month;
			zon.day 		= day;
			if ((isSummer (year, month, day) == true) && DayLightSavings) {
				zon.hour = hour + (int)timeZone;
			} else {
				zon.hour = hour + (int)timeZone+1;
			}
			zon.minute 		= minute;
			zon.Position (); // calculate the actual position of the sun at the exact given time.

			sunTime = zon.revolution (zon.GMST0(days) + 180.0 + Longtitude);
			//Console.WriteLine ("sunTime: " + sunTime + " long: " + Longtitude + " days " + days);

			zon.Position (days);
			//Console.WriteLine ("sun: longtitude:{0}, distance{1}", zon.solarLongtitude, zon.Distance);

			zon.SunEquatorialDeclination ();


			//Console.WriteLine ("sun: equatorial:{0}, declanation{1}", zon.Equatorial, zon.Declination);
			//SunPosition             (input->result.days, &input->sun.longtitude, &input->sun.distance);
			//SunEquatorialDeclination(input->result.days, input->sun.longtitude, input->sun.distance, &input->sun.equatorial, &input->sun.declination);

			//input->sun.southTime    = 12.0f - (revolution_180(input->sun.time - input->sun.equatorial)/15.0f);         // Calc when the sun is South (Universal Time)
			SouthTime = 12.0 - (zon.revolution_180 (sunTime - zon.Equatorial ) / 15.0); // Calculate the sun south (universal) 
			//Console.WriteLine ("sun: southtime:{0}", SouthTime);
			//input->sun.radius       = 0.2666f / input->sun.distance;                                                 // Compute the Sun's apparent radius in degrees

			//input->sun.diffTime     = sunDiff((input->me.altitude - input->sun.radius), input->me.latitude, input->sun.declination, &input->sun.solarDuration);
			zon.Difference ();
			SouthTime  += timeZone; // compensate for timezone
			if ((isSummer (year, month, day) == true) && DayLightSavings)
				SouthTime += 1; // summertime

			double SunRise = SouthTime - zon.Diff;
			double SunSet = SouthTime + zon.Diff;
			//double dayLength = zon.Diff * 2.0f;

			// compute easy readable values:

			//int uren = (int) SunRise;
			//double minutes = 60.0f * (SunRise - (double)uren);	
			//int minuten = (int) minutes;
			//Console.WriteLine ("dagen {0}, uren: {1} minuten:{2} ({3})", days, uren, minuten, SunRise);
			//Console.WriteLine ("onder: " + (int) SunSet + ":" + (int)(60.0f * (SunSet - ((int) SunSet))) + " sunset:" + SunSet + " sunrise:" + SunRise + " south:" + SouthTime + " diff:" + zon.Diff); 
			//Console.WriteLine ("Dag lengthe: " + dayLength);

			sunSetStr 	= "" + (int)SunSet + ":" + (int)(60.0 * (SunSet - ((int)SunSet)));
			sunRiseStr 	= "" + (int)SunRise + ":" + (int)(60.0 * (SunRise - ((int)SunRise)));

		}




	}
}

