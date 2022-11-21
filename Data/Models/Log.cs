using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Log : Entity<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public string? Exception { get; set; }
        public string? Url { get; set; }
        public string? Trace { get; set; }
        public string? Payload { get; set; }
        public string? Params { get; set; }
        public string? User { get; set; }
        public string? Method { get; set; }
    }
}
