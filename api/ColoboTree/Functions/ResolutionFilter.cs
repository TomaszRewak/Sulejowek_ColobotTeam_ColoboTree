using NetTopologySuite.Geometries;

namespace ColoboTree.Functions;

public static class ResolutionFilter
{
    private const double BaseRectangleWidthInDegrees = 0.0085474408498d;  //X
    private static readonly int[] Resolutions = new[]{1, 2, 5, 10, 20, 50};
    
    public static int GetResolutionForRectangle(Polygon rectangle)
    {
        var (width, height) = rectangle.GetRectangleDegreeWidthAndHeightInDegrees();
        
        var resolution = (int) Math.Floor(width / BaseRectangleWidthInDegrees);

        var res = Math.Max(1, resolution);
        
        return Resolutions.First(r => r >= res);
    }
}