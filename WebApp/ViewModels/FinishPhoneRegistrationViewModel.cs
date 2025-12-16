using System.ComponentModel.DataAnnotations;

public class FinishPhoneRegistrationViewModel
{
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
