using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class FullAuditedEntity<TKey> : AuditedEntity<TKey>
    {
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        [MaxLength(50)]
        public string? DeletedBy { get; set; }
    }
}
