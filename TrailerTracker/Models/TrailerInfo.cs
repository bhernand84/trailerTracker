﻿using System;

namespace TrailerTracker.Models
{
    public class TrailerInfo
    {
        public string esn { get; set; }
        public DateTime lastIdReportTime { get; set; }
        public string trailerNumber { get; set; }
        public TrailerInfo()
        {
        }
    }
}