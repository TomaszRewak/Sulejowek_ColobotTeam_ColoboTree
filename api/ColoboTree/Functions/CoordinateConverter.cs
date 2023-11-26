using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace ColoboTree.Functions;

public static class CoordinateConverter
{
    public static (double Easting, double Northing) ConvertLatLonToUTM(double latitude, double longitude)
    {
        var geographicCoordinateSystem = GeographicCoordinateSystem.WGS84;

        // UTM Zone calculation - you can adjust this according to your specific need
        int utmZone = (int)Math.Floor((longitude + 180.0) / 6.0) + 1;
        var utmProjection = ProjectedCoordinateSystem.WGS84_UTM(utmZone, latitude >= 0);

        var coordinateTransformationFactory = new CoordinateTransformationFactory();
        var transform = coordinateTransformationFactory.CreateFromCoordinateSystems(geographicCoordinateSystem, utmProjection);

        double[] fromPoint = new double[] { longitude, latitude };
        double[] toPoint = transform.MathTransform.Transform(fromPoint);

        return (Easting: toPoint[0], Northing: toPoint[1]);
    }
}