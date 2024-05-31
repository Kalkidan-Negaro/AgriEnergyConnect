
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Models;

public partial class Product
{
    [Key]
    public int ProductID { get; set; }

    public int? FarmerID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(50)]
    public string Category { get; set; }

    public DateTime? ProductionDate { get; set; }

    [ForeignKey("FarmerID")]
    [InverseProperty("Products")]
    public virtual Farmer Farmer { get; set; }
}