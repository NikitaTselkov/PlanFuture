﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Core
{
    public interface ICard
    {
        public string Title { get; }
        public int Index { get; set; }
    }
}
