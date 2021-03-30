using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.Devices.Geolocation;

namespace TestService
{
    public partial class GeolocationService : ServiceBase
    {
        private EventLog eventLog;
        private Timer timer;

        public GeolocationService()
        {
            InitializeComponent();
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog.Source = "MySource";
            eventLog.Log = "MyNewLog";
        }
    

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 15000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            var position = await geolocator.GetGeopositionAsync();
            string result = $"Coordinates - Latitude: {position.Coordinate.Point.Position.Latitude} / Longitude: {position.Coordinate.Point.Position.Longitude}";
            eventLog.WriteEntry(result);
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
