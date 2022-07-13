using UnityEngine;

namespace IGUIDResources
{
    public class GUIDResourceLocator
    {
        public BikeModels Bikes { get; private set; }
        public Career Career { get; private set; }
        
        private GUIDResourceLocator(BikeModels bikes, Career career)
        {
            Bikes = bikes;
            Career = career;
        }

        public static GUIDResourceLocator Initialize()
        {
            BikeModels bikes = Resources.Load<BikeModels>("GUIDResources/Bikes/BikesRegistry");
            Career career = Resources.Load<Career>("GUIDResources/Career/Career");
            return new GUIDResourceLocator(bikes, career);
        }
    }
}