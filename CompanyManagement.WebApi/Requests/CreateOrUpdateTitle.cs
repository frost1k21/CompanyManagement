using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.WebApi.Requests;

public class CreateOrUpdateTitle
{
    [Required]
    public string Title { get; set; }
}