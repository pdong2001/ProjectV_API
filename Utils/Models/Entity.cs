using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public abstract class Entity<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }
    }

    public abstract class Entity : Entity<Guid>
    {
    }
}
