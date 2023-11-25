using NetTopologySuite.Geometries;

namespace ColoboTree.Context.Entities;

public partial class AreaChunk
{
    public int Id { get; set; }
    public Point? UpperLeftVertex2180 { get; set; }
    public Point? UpperLeftVertex4326 { get; set; }
    public Point? BottomRightVertex2180 { get; set; }
    public Point? BottomRightVertex4326 { get; set; }
    public decimal? TreeCoveragePercentage { get; set; }
    public int? TreeClassification { get; set; }
    public int? TreeId { get; set; }
    public int Resolution { get; set; }
}
