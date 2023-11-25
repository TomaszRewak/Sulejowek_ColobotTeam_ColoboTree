namespace ColoboTree.Inputs;

public record GetPlotInput(PlotPoint[] polygon);

public record PlotPoint(double X, double Y);