using System.ComponentModel.DataAnnotations;

public class RegisterByPhoneViewModel
{
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    public string? Code { get; set; }

    public bool IsCodeSent { get; set; }
}
