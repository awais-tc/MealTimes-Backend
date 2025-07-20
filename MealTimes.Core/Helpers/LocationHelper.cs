namespace MealTimes.Core.Helpers
{
    public static class LocationHelper
    {
        /// <summary>
        /// Calculate distance between two points using Haversine formula
        /// </summary>
        /// <param name="lat1">Latitude of first point</param>
        /// <param name="lon1">Longitude of first point</param>
        /// <param name="lat2">Latitude of second point</param>
        /// <param name="lon2">Longitude of second point</param>
        /// <returns>Distance in kilometers</returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Check if a point is within a certain radius of another point
        /// </summary>
        public static bool IsWithinRadius(double lat1, double lon1, double lat2, double lon2, double radiusKm)
        {
            var distance = CalculateDistance(lat1, lon1, lat2, lon2);
            return distance <= radiusKm;
        }

        /// <summary>
        /// Calculate bounding box for efficient database queries
        /// </summary>
        public static (double minLat, double maxLat, double minLon, double maxLon) GetBoundingBox(
            double centerLat, double centerLon, double radiusKm)
        {
            const double R = 6371; // Earth's radius in kilometers

            var latRadian = ToRadians(centerLat);
            var deltaLat = radiusKm / R;
            var deltaLon = radiusKm / (R * Math.Cos(latRadian));

            var minLat = centerLat - ToDegrees(deltaLat);
            var maxLat = centerLat + ToDegrees(deltaLat);
            var minLon = centerLon - ToDegrees(deltaLon);
            var maxLon = centerLon + ToDegrees(deltaLon);

            return (minLat, maxLat, minLon, maxLon);
        }

        private static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}