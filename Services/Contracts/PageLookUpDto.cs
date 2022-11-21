using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public class PageLookUpDto
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public string? Columns { get; set; }
        public string? Search { get; set; }
    }
}
