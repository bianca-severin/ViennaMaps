﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ViennaMaps.Models
{
    public partial class Layer
    {
        public Layer()
        {
            Project = new HashSet<Project>();
        }

        public int LayerId { get; set; }
        public int LayerGroupId { get; set; }
        public string LayerName { get; set; }
        public string LayerLabel { get; set; }
        public string ArcGisuri { get; set; }
        public string LayerDataSource { get; set; }

        public virtual LayerGroup LayerGroup { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}