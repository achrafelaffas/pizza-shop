using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PizzaStore.Areas.Identity.Data;

// Add profile data for application users by adding properties to the PizzaStoreUser class
public class PizzaStoreUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(30)")]
    public string Firstname { get; set; } = string.Empty;
    [PersonalData]
    [Column(TypeName = "nvarchar(30)")]
    public string Lastname { get; set; } = string.Empty;
}

