using NetTopologySuite.Geometries;

namespace ColoboTree.Functions;

public static class DistanceCalculator
{
    public static double CalculateHaversineDistance(Coordinate point1, Coordinate point2)
    {
        const double earthRadiusInMeters = 6371000;

        double dLat = Math.PI / 180 * (point2.Y - point1.Y);
        double dLon = Math.PI / 180 * (point2.X - point1.X);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(Math.PI / 180 * point1.Y) * Math.Cos(Math.PI / 180 * point2.Y) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusInMeters * c;
    }
}