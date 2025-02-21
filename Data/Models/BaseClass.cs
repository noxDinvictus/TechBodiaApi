using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBodiaApi.Data.Models
{
    public class BaseClass
    {
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public Guid CreatedByUserId { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? UpdatedByUserId { get; set; }

        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
