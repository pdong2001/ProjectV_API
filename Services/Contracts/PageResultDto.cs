using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public class PageResultDto<TData>
    {
        public ICollection<TData> Items { get; set; }

        public PageResultDto()
        {
            Items = new List<TData>();
        }

        public long Total { get; set; }

    }
}
