﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ViennaMaps.Models
{
    public partial class LayerGroup
    {
        public LayerGroup()
        {
            Layer = new HashSet<Layer>();
        }

        public int LayerGroupId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<Layer> Layer { get; set; }
    }
}