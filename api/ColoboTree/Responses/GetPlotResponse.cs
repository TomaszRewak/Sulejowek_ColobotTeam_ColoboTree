namespace ColoboTree.Responses;

public record GetPlotResponse(double TotalArea, double TreeCoverage, List<AreaChunkResponse> Chunks, decimal Co2Sequestration);
