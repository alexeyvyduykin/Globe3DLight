using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Data;
using GlmSharp;

namespace Globe3DLight.DataProvider.Science
{
    public class ScienceDataProvider : ObservableObject, IDataProvider
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IDataFactory _dataFactory;

        public ScienceDataProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
           // _dataFactory = serviceProvider.GetService<IDataFactory>();
        }


        public J2000Data CreateJ2000Data(DateTime begin)
        {
            var Angle0DEG = AngleDeg(begin);

            return new J2000Data()
            {
                Epoch = begin,
                AngleDeg = Angle0DEG,
            };
        }

        public SunData CreateSunData(DateTime begin, TimeSpan duration)
        {
            var position1 = GetSunPosition(begin);
            var position2 = GetSunPosition(begin + duration);

            //  var res = SunPositionTools.CalculateSunPosition(_begin, 0.0, 0.0);
            //return _databaseFactory.CreateSunDatabase(0.0, duration.TotalSeconds, position1, position2);
            return new SunData()
            {
                Position0 = position1,
                Position1 = position2,
                TimeBegin = 0.0,
                TimeEnd = duration.TotalSeconds,
            };
        }

        private double AngleDeg(DateTime time)
        {
            double JD = ToJulianDate(time);

            double d = JD - 2451545.0;

            double T = d / 36525;

            // gmst, secs
            double gmst = 24110.54841 + 8640184.812866 * T + 0.093104 * T * T - 0.0000062 * T * T * T;

            // double deg = gmst * 360.0 / 86400.0;

            double deg = gmst / 3600.0;

            return deg;
        }
        private double ToJulianDate(DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }
        private dvec3 GetSunPosition(DateTime time)
        {
            double sunDistance = 0.989 * 1.496E+8;
            var sunPosition = GetSunDirection(time.ToUniversalTime());

            sunPosition = sunPosition * sunDistance;

            return sunPosition;
        }
        // ECI
        private dvec3 GetSunDirection(DateTime time)
        {
            time = time.ToUniversalTime();
            double JD = 367 * time.Year - Math.Floor(7.0 * (time.Year + Math.Floor((time.Month + 9.0) / 12.0)) / 4.0) + Math.Floor(275.0 * time.Month / 9.0) + time.Day + 1721013.5 + time.Hour / 24.0 + time.Minute / 1440.0 + time.Second / 86400.0;
            double pi = 3.14159265359;
            double UT1 = (JD - 2451545) / 36525;
            double longMSUN = 280.4606184 + 36000.77005361 * UT1;
            double mSUN = 357.5277233 + 35999.05034 * UT1;
            double ecliptic = longMSUN + 1.914666471 * Math.Sin(mSUN * pi / 180) + 0.918994643 * Math.Sin(2 * mSUN * pi / 180);
            double eccen = 23.439291 - 0.0130042 * UT1;

            double x = Math.Cos(ecliptic * pi / 180);
            double y = Math.Cos(eccen * pi / 180) * Math.Sin(ecliptic * pi / 180);
            double z = Math.Sin(eccen * pi / 180) * Math.Sin(ecliptic * pi / 180);

            return new dvec3(x, y, z);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


}
