namespace ColoboTree.Functions;

public static class TemperatureReductionFunctions
{
    public static double CalculateTemperatureReduction(double crownDiameter)
    {
        double crownRadius = crownDiameter / 2.0;

        double shadeArea = Math.PI * Math.Pow(crownRadius, 2);

        double evapotranspiration = 1.0; 
        double shadeCoolingFactor = 0.01;
        double evapotranspirationCoolingFactor = 0.05;

        double temperatureReduction = shadeCoolingFactor * shadeArea + evapotranspirationCoolingFactor * evapotranspiration;

        return temperatureReduction;
    }
}
