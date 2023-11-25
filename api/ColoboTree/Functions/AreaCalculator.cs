using NetTopologySuite.Geometries;

namespace ColoboTree.Functions;

public static class AreaCalculator
{
    private const double DegreeToMeterRatio = 111139d;
    
    public static double CalculateArea(Polygon polygon)
    {
        var bottomLeft = polygon.Coordinates 
            .OrderBy(c => c.Y)
            .ThenBy(c => c.X)
            .First();
        
        var topLeft = polygon.Coordinates
            .OrderBy(c => c.Y)
            .ThenByDescending(c => c.X)
            .First();
        
        var bottomRight = polygon.Coordinates
            .OrderByDescending(c => c.Y)
            .ThenBy(c => c.X)
            .First();
        
        var height = DistanceCalculator.CalculateHaversineDistance(bottomLeft, topLeft);
        var width = DistanceCalculator.CalculateHaversineDistance(bottomLeft, bottomRight);
        
        return height * width;
    }
}