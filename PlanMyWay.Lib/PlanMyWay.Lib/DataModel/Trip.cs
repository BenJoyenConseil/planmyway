using Microsoft.Phone.Controls.Maps.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel
{
    public class Trip
    {
        /// <summary>
        /// Position de départ
        /// </summary>
        public Meeting Start { get; set; }
        /// <summary>
        /// Position d'arrivée
        /// </summary>
        public Meeting End { get; set; }
        /// <summary>
        /// Durée en minutes
        /// </summary>
        public double Duration { get; set; }
        /// <summary>
        /// En Kilomètre
        /// </summary>
        public double Distance { get; set; }

        public List<Location> RoutePath { get; set; }

        /// <summary>
        /// Test si le temps de déplacement entre deux rendez-vous est suffisant
        /// </summary>
        public bool IsTripFail
        {
            get
            {
                if (End == null || Start == null)
                    return false;
                if (End.GetType().Equals(typeof(ReferenceMeeting)) || Start.GetType().Equals(typeof(ReferenceMeeting)))
                    return false;
                var testTripFail = this.End.DateTime < getMeetingPlusTripDuration();
                return testTripFail;
            }
        }

        DateTime getMeetingPlusTripDuration()
        {
            return this.Start.DateTime.AddMinutes(this.Start.Duration + this.Duration);
        }
        /// <summary>
        /// Temps suplémentaire (en minutes) requis pour le trajet entre le 1er RDV et le 2ème.
        /// </summary>
        public int TimeNeeded
        {
            get
            {
                var underated = getMeetingPlusTripDuration() - this.End.DateTime;
                return underated.Minutes + underated.Hours * 60;
            }
        }

        public Trip()
        {
            Duration = 0;
            Distance = 0;
            RoutePath = new List<Location>();
        }
    }
}
