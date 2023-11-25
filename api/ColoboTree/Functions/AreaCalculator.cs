using NetTopologySuite.Geometries;

namespace ColoboTree.Functions;

public static class AreaCalculator
{
    // private const double DegreeToMeterRatio = 111139d;
    public static double CalculateArea(Polygon polygon)
    {
        var (width, height) = polygon.GetRectangleWidthAndHeightInMeters();
        
        return height * width;
    }
}