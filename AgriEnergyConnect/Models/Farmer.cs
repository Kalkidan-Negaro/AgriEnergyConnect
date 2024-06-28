using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Models;

public partial class Farmer
{
    [Key]
    public int FarmerID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    public string ContactInfo { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    [InverseProperty("Farmer")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Farmer")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string PasswordHash { get; set; }
}