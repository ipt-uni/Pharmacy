using System.ComponentModel.DataAnnotations;

namespace pharmacy.Data.Models;

/// <summary>
/// Profile extension for users with the Staff role.
/// No extra fields beyond MyUser.
/// </summary>
public class NonCustomer : MyUser { }
