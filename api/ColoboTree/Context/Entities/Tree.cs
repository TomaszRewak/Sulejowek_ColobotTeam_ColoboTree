using NetTopologySuite.Geometries;

namespace ColoboTree.Context.Entities;

public partial class Tree
{
    public int Id { get; set; }

    public Point? Coordinates2180 { get; set; }

    public Point? Coordinates4326 { get; set; }

    public decimal? TreeDiameter { get; set; }

    public decimal? TreeHeight { get; set; }
}
