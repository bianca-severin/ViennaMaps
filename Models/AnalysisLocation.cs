﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ViennaMaps.Models
{
    public partial class AnalysisLocation
    {
        public int AnalysisLocationId { get; set; }
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
    }
}