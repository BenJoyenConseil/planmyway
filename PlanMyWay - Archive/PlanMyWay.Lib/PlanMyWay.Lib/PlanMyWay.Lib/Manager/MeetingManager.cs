using Microsoft.Phone.UserData;
using PlanMyWay.Lib.GeocodeService;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Phone.Controls.Maps;

namespace PlanMyWay.Lib.Manager
{
    /// <summary>
    /// Accède à l'API BingMap pour controler les rendez-vous et en retirer des objets de type DataModel.Meeting
    /// </summary>
    public class MeetingManager : IMeetingManager
    {

        public EventHandler<LocationCheckedEventArgs> LocationChecked { get; set; }

        public EventHandler<AllMeetingsReceivedEventArgs> AllMeetingsRetreived { get; set; }

        public EventHandler MeetingResolved { get; set; }


        /// <summary>
        /// Verifie les coordonnées géographiques d'un rendez-vous du Calendrier
        /// </summary>
        /// <param name="appointment">Rendez-vous</param>
        /// <remarks>Abonnez vous d'abord à l'évènement LocationChecked avant l'appel de cette méthode</remarks>
        public void CheckLocationAsync(Appointment appointment)
        {
            //Procédure de connexion et préparation de la requete
            GeocodeRequest request = new GeocodeRequest() { Credentials = new Credentials { ApplicationId = BingMapCredential.CREDENTIAL } };
            if (appointment.Location == null)
                return;
            request.Query = appointment.Location;
            GeocodeServiceClient service = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            
            //Lorsque bing map nous envoie une réponse
            service.GeocodeCompleted += (o, e) =>
            {
                //Construction du Meeting qui sera envoyé en résultat
                Meeting m = new Meeting();
                m.Subject = appointment.Subject;
                m.DateTime = appointment.StartTime;
                m.Duration = (appointment.EndTime - appointment.StartTime).TotalMinutes;

                //Si dans le service bing map a trouvé les latitude et longitude de la requete
                if (e.Result == null || 
                    e.Result.Results.Count < 0 || 
                    e.Result.Results.Any(obj => obj.Locations == null && obj.Locations.Any()))
                {
                    throw new Exception("Pas de géolocalisation possible");
                }

                m.IsLocationFail = true;
                if (e.Result.Results.FirstOrDefault().Confidence == Confidence.High)
                {
                    m.IsLocationFail = false;
                    m.Location.Latitude = e.Result.Results.FirstOrDefault().Locations.FirstOrDefault().Latitude;
                    m.Location.Longitude = e.Result.Results.FirstOrDefault().Locations.FirstOrDefault().Longitude;
                }

                LocationCheckedEventArgs eventToSend = new LocationCheckedEventArgs();
                eventToSend.Meeting = m;
                //On notifie l'observateur (dans ce cas, la méthode GetAllMeetingsAsync)
                LocationChecked(this, eventToSend);
            };
            service.GeocodeAsync(request);
        }

        /// <summary>
        /// Récupère les rendez-vous d'un jour précis
        /// </summary>
        /// <value> <list type="List<Meeting>">Results</list></value>
        /// <param name="date">Jour à partir duquel récupèrer les rendez-vous</param>
        public void GetAllMeetingsAsync(DateTime date)
        {
            Appointments appointments = new Appointments();
            
            //Lorsque les rendez-vous sont reçus de la plateforme Calendar WP7
            appointments.SearchCompleted += (o, e) =>
            {
                int countSended = 0;
                //Liste finale, des meetings récupérés, envoyée à la fin du traitement
                List<Meeting> mettings = new List<Meeting>();

                //Si il n'y a aucun résultat
                if (!e.Results.Any())
                {
                    Debug.WriteLine("No Apointments Found");
                    return;
                }

                //Lorsqu'on reçoit une réponse de recherche de location
                LocationChecked += (sender, locationEvent) =>
                {
                    mettings.Add(locationEvent.Meeting);
                    Debug.WriteLine("réponse " + locationEvent.Meeting.Subject + " ->" + locationEvent.Meeting.Location.Latitude + " " + locationEvent.Meeting.Location.Longitude);

                    //Si cette réponse est la dernière parmis toutes celles lancées
                    if (countSended == mettings.Count)
                    {
                        Debug.WriteLine("AllMeetingsRetreived!!");
                        AllMeetingsReceivedEventArgs eventToSend = new AllMeetingsReceivedEventArgs();
                        //On range les réponses par date
                        eventToSend.Meetings = mettings.OrderBy(m => m.DateTime);
                        //Et on notifie les observateurs
                        AllMeetingsRetreived(this, eventToSend);

                        //On se dé-abonne de l'event
                        LocationChecked = null;
                    }
                };

                //Pour charque rendez-vous trouvés on lance la méthode de check location
                foreach (Appointment a in e.Results)
                {
                    if (a.Location != null)
                    {
                        countSended++;
                        CheckLocationAsync(a);
                    }
                }
            };
            Debug.WriteLine("Date de début : " + date);
            Debug.WriteLine("Date de fin : " + date.AddDays(1));
            appointments.SearchAsync(date, date.AddDays(1), null);
        }



        /// <summary>
        /// Cherche la latitude et la longitude d'une adresse sous forme de chaine de caractère
        /// </summary>
        /// <param name="address"></param>
        /// <remarks>Deprecated</remarks>
        public void SearchStringAsync(string address)
        {
            GeocodeRequest request = new GeocodeRequest() { Credentials = new Credentials { ApplicationId = BingMapCredential.CREDENTIAL } };
            request.Query = address;
            GeocodeServiceClient service = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            service.GeocodeCompleted += (o, e) =>
            {
                if (e.Result != null && e.Result.Results.Any(obj => obj.Locations != null && obj.Locations.Any()))
                {
                    double la = e.Result.Results.FirstOrDefault().Locations.FirstOrDefault().Latitude;
                    double lon = e.Result.Results.FirstOrDefault().Locations.FirstOrDefault().Longitude;
                    LocationChecked(this, new LocationCheckedEventArgs());
                }
                else
                {
                    LocationChecked(this, new LocationCheckedEventArgs());
                }
            };
            service.GeocodeAsync(request);
        }
    }
}
