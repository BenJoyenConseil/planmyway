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

        public RouteService.ItineraryItem Itinary;
        public bool IsTripFail { get; set; }

        public Trip()
        {
            Duration = 0;
            Distance = 0;
            IsTripFail = false;
        }
    }
}
