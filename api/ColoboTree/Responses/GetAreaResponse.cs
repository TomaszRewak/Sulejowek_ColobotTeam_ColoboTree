using NetTopologySuite.Geometries;

namespace ColoboTree.Responses;

public record GetAreaResponse(double TotalArea, double TreeCoverage,List<AreaChunkResponse> Chunks);

public record AreaChunkResponse(int Id,
    Point? UpperLeftVertex2180, Point? UpperLeftVertex4326, int? TreeId
    );