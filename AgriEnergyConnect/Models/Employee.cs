using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Models;

[Table("Employee")]
public partial class Employee
{
    [Key]
    public int EmployeeID { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; }

    public int? FarmerID { get; set; }

    [ForeignKey("FarmerID")]
    [InverseProperty("Employees")]
    public virtual Farmer Farmer { get; set; }
}