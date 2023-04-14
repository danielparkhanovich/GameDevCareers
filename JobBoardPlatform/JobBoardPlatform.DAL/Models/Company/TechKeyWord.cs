using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Index(nameof(Name), IsUnique = true)]
    [Table("TechKeywords")]
    public class TechKeyword : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
