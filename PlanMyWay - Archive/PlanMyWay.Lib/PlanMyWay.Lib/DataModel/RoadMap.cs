using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.DataModel.RoadMapOptimization;
using PlanMyWay.Lib.RouteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel
{
    public class RoadMap
    {
        MeetingCollection _meetings = new MeetingCollection();
        List<Trip> _trips = new List<Trip>();
        List<Node> _pathes = new List<Node>();
        DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public List<Meeting> Meetings { get { return _meetings; } }
        public List<Meeting> ValidMeetings
        {
            get
            {
                List<Meeting> lTemp = new List<Meeting>();
                lTemp.AddRange(_meetings.Where(o => o.IsLocationFail == false));
                return lTemp;
            }
        }

        public List<Meeting> LocationFailMeetings
        {
            get
            {
                List<Meeting> lTemp = new List<Meeting>();
                lTemp.AddRange(_meetings.Where(o => o.IsLocationFail == true));
                return lTemp;
            } 
        }

        public List<Trip> Trips { get { return _trips; } }
        public List<Trip> FailTrips
        {
            get
            {
                List<Trip> lTemp = new List<Trip>();
                lTemp.AddRange(_trips.Where(o => o.IsTripFail == true));
                return lTemp;
            }
        }
        /// <summary>
        /// Récupère la distance globale de la roadmap (somme des différentes routes entre les rendez-vous)
        /// </summary>
        public double Distance
        {
            get
            {
                if (_trips.Count <= 0)
                    return 0;
                double sum = 0;
                foreach (var trip in _trips)
                    sum += trip.Distance;
                return sum;
            }
        }

        /// <summary>
        /// Consommation de carburant du roadtrip entier.
        /// </summary>
        /// <param name="consumptionAverage"></param>
        /// <returns></returns>
        public double GetConsumption(float consumptionAverage)
        {
            return (consumptionAverage * this.Distance / 100);
        }

        public double GetTotalCost(float consumptionAverage, float gasCost)
        {
            return (consumptionAverage * this.Distance / 100 * gasCost);
        }

        public RouteService.RouteResult RouteResult { get; set; }


        #region optimization
        /// <summary>
        /// Construit un arbre équilibré et retourne le premier noeud, la racine.
        /// </summary>
        /// <returns>La racine de l'arbre équilibré</returns>
        private Node buildTree()
        {
            List<Meeting> meetingsWhithoutStartEnd = new List<Meeting>();
            meetingsWhithoutStartEnd.AddRange(_meetings.Where(o => o.IsLocationFail == false));
            meetingsWhithoutStartEnd.Remove(meetingsWhithoutStartEnd.First());
            meetingsWhithoutStartEnd.Remove(meetingsWhithoutStartEnd.Last());
            Meeting start = _meetings.First();
            Meeting end = _meetings.Last();

            Node tree = new Node();
            tree.MeetingRef = start;
            tree.Children.AddRange(getChildren(meetingsWhithoutStartEnd, end));
            foreach (var child in tree.Children)
                child.Parent = tree;

            return tree;
        }
        /// <summary>
        /// Fonction d'optimisation recherchant le meilleur ordonnancement des rendez-vous de manière à parcourir le moins de chemin
        /// </summary>
        /// <returns>L'ordre de meeting possèdant le chemin global le plus court</returns>
        public MeetingCollection OptimizedMeetingOrder
        {
            get
            {
                buildTree();
                List<Order> orderList = new List<Order>();
                foreach (var path in _pathes)
                {
                    Order order = new Order();
                    getParent(order, path);
                    orderList.Add(order);
                }
                Order higher = orderList.OrderByDescending(o => o.Sum).FirstOrDefault();
                Order lower = orderList.OrderBy(o => o.Sum).FirstOrDefault();
                lower.Reverse();
                MeetingCollection mc = lower.getMeetingCollection();
                //lower = null;
                //higher = null;
                //orderList = null;
                //GC.Collect();
                return mc;
            }
        }
        /// <summary>
        /// Récupère les meetings d'un chemin possible dans l'arbre de manière récursive
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="child"></param>
        private void getParent(Order parents, Node child)
        {
            parents.Add(child);
            if (child.Parent == null)
                return;
            getParent(parents, child.Parent);
        }
        /// <summary>
        /// Récupère tous les enfants d'un noeud de manière récursive
        /// </summary>
        /// <param name="unusedMeetings">Les rendez-vous pas encore référencés dans l'arbre</param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<Node> getChildren(List<Meeting> unusedMeetings, Meeting end)
        {
            List<Node> children = new List<Node>();

            foreach (var meeting in unusedMeetings)
            {
                Node node = new Node();
                node.MeetingRef = meeting;
                if (unusedMeetings.Count <= 1)
                {
                    Node endNode = new Node();
                    endNode.MeetingRef = end;
                    endNode.Parent = node;
                    node.Children.Add(endNode);
                    _pathes.Add(endNode);
                }
                else
                {
                    List<Meeting> unusedMeetingsbis = new List<Meeting>();
                    unusedMeetingsbis.AddRange(unusedMeetings);
                    unusedMeetingsbis.Remove(meeting);
                    node.Children.AddRange(getChildren(unusedMeetingsbis, end));
                }
                children.Add(node);
                foreach(var child in node.Children)
                    child.Parent = node;
            }

            //unusedMeetings = null;
            return children;
        }
        #endregion
    }
}
