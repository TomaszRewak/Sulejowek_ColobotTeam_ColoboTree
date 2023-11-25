using ColoboTree.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColoboTree.Context;

public partial class ColoboTreeContext : DbContext
{
    public ColoboTreeContext()
    {
    }

    public ColoboTreeContext(DbContextOptions<ColoboTreeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AreaChunk> AreaChunks { get; set; }

    public virtual DbSet<Tree> Trees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<AreaChunk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("area_chunks_pkey");

            entity.ToTable("area_chunks");

            entity.HasIndex(e => e.CenterPoint2180, "idx_area_chunks_center_point_2180").HasMethod("gist");

            entity.HasIndex(e => e.CenterPoint4326, "idx_area_chunks_center_point_4326").HasMethod("gist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CenterPoint2180)
                .HasColumnType("geometry(Point,2180)")
                .HasColumnName("center_point_2180");
            entity.Property(e => e.CenterPoint4326)
                .HasColumnType("geometry(Point,4326)")
                .HasColumnName("center_point_4326");
            entity.Property(e => e.TreeClassification).HasColumnName("tree_classification");
            entity.Property(e => e.TreeCoveragePercentage)
                .HasPrecision(10, 2)
                .HasColumnName("tree_coverage_percentage");
        });

        modelBuilder.Entity<Tree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trees_pkey");

            entity.ToTable("trees");

            entity.HasIndex(e => e.Coordinates2180, "idx_trees_coordinates_2180").HasMethod("gist");

            entity.HasIndex(e => e.Coordinates4326, "idx_trees_coordinates_4326").HasMethod("gist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coordinates2180)
                .HasColumnType("geometry(Point,2180)")
                .HasColumnName("coordinates_2180");
            entity.Property(e => e.Coordinates4326)
                .HasColumnType("geometry(Point,4326)")
                .HasColumnName("coordinates_4326");
            entity.Property(e => e.TreeDiameter)
                .HasPrecision(10, 2)
                .HasColumnName("tree_diameter");
            entity.Property(e => e.TreeHeight)
                .HasPrecision(10, 2)
                .HasColumnName("tree_height");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
