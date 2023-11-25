using NetTopologySuite.Geometries;

namespace ColoboTree.Functions;

public static class PolygonExtension
{
    public static (double width, double height) GetRectangleWidthAndHeightInMeters(this Polygon rectangle)
    {
        var (bottomLeft, topLeft, bottomRight) = rectangle.GetRectangleBaseCoordinates();
        
        return (
            DistanceCalculator.CalculateHaversineDistance(bottomLeft, topLeft), 
            DistanceCalculator.CalculateHaversineDistance(bottomLeft, bottomRight)
            );
    }
    
    public static (double, double) GetRectangleDegreeWidthAndHeightInDegrees(this Polygon rectangle)
    {
        var (bottomLeft, topLeft, bottomRight) = rectangle.GetRectangleBaseCoordinates();
        return (Math.Abs(bottomLeft.X - bottomRight.X), Math.Abs(bottomLeft.Y - topLeft.Y));
    }
    
    private static (Coordinate bottomLeft, Coordinate topLeft, Coordinate bottomRight) GetRectangleBaseCoordinates(this Polygon rectangle)
    {
        var bottomLeft = rectangle.Coordinates 
            .OrderBy(c => c.Y)
            .ThenBy(c => c.X)
            .First();
        
        var topLeft = rectangle.Coordinates
            .OrderBy(c => c.X)
            .ThenByDescending(c => c.Y)
            .First();
        
        var bottomRight = rectangle.Coordinates
            .OrderByDescending(c => c.X)
            .ThenBy(c => c.Y)
            .First();
        
        return (bottomLeft, topLeft, bottomRight);
    }
}