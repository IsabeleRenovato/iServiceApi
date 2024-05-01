using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace iServiceRepositories.Repositories.Models
{
    public class Service
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int EstablishmentProfileId { get; set; }

        [Required]
        public int ServiceCategoryId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "O nome do serviço deve ter no máximo 255 caracteres.")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "A descrição não pode exceder 255 caracteres.")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O tempo estimado deve ser maior que zero.")]
        public int EstimatedDuration { get; set; }

        public string? Photo { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public IFormFile? File { get; set; }
    }
}
