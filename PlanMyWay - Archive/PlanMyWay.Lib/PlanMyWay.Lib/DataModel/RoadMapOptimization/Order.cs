using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel.RoadMapOptimization
{
    public class Order : List<Node>
    {
        public double Sum
        {
            get
            {
                if (this.Count <= 0)
                    return 0;

                double sum = 0;
                for (int i = 0; i < this.Count - 1; i++)
                {
                    sum += this.getMeetingCollection().CalculateGeoCoordinatesDistanceBetween(i, i + 1);
                }
                return sum;
            }
        }

        public MeetingCollection getMeetingCollection()
        {
            MeetingCollection mc = new MeetingCollection();
            foreach (var item in this)
                mc.Add(item.MeetingRef);
            return mc;
        }
    }
}
