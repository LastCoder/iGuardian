﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGuardian.GUI
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class LimitAttribute : Attribute
    {
        public LimitAttribute(double low, double high)
        {
            Low = low;
            High = high;
        }

        public double High { get; set; }
        public double Low { get; set; }
    }
}
