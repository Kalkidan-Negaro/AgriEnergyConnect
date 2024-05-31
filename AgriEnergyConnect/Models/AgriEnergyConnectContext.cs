
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Models;

public partial class AgriEnergyConnectContext : DbContext
{
    public AgriEnergyConnectContext(DbContextOptions<AgriEnergyConnectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Farmer> Farmers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID).HasName("PK__Employee__7AD04FF18B4F1DFF");

            entity.HasOne(d => d.Farmer).WithMany(p => p.Employees).HasConstraintName("FK__Employee__Farmer__4F7CD00D");
        });

        modelBuilder.Entity<Farmer>(entity =>
        {
            entity.HasKey(e => e.FarmerID).HasName("PK__Farmers__731B88E8943B1462");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("PK__Products__B40CC6ED7B947331");

            entity.HasOne(d => d.Farmer).WithMany(p => p.Products).HasConstraintName("FK__Products__Farmer__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}