﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public class EntityDto<TKey>
    {
        public TKey Id { get; set; }
    }
    public class EntityDto : EntityDto<Guid>
    { }
}
