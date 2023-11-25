namespace ColoboTree.Functions;

//https://www.ecomatcher.com/how-to-calculate-co2-sequestration
public static class Co2SequestrationFunctions
{
	private const double AboveBelowGroundTreeRatio = 1.2;
	private const double ThresholdDiameter = 0.28;
	private const double AverageDryMatterRatio = 0.725;
	private const double AverageCarbonInDryWeightRatio = 0.5;
	private const double Co2ToCarbonRatio = 3.67;
	
    public static double CalculateTreeLifetimeCo2Sequestration(double height, double diameter)
    { 
	    var greenWeightToHeightRatio = diameter >= ThresholdDiameter ? 0.15 : 0.25;
        var greenWeightAboveGround = greenWeightToHeightRatio * Math.Pow(diameter, 2) * height;
        var totalGreenWeight = greenWeightAboveGround * AboveBelowGroundTreeRatio;
        var dryWeight = AverageDryMatterRatio * totalGreenWeight;
        var carbonWeight = AverageCarbonInDryWeightRatio * dryWeight;
        var co2Weight = Co2ToCarbonRatio * carbonWeight;
		
        return co2Weight;
    }
}