using NetTopologySuite.Geometries;

namespace ColoboTree.Context.Entities;

public partial class AreaChunk
{
    public int Id { get; set; }

    public Point? CenterPoint2180 { get; set; }

    public Point? CenterPoint4326 { get; set; }

    public decimal? TreeCoveragePercentage { get; set; }

    public int? TreeClassification { get; set; }
}
