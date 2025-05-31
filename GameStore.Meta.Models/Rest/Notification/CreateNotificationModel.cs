﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Notification
{
    public class CreateNotificationModel
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        public string Type { get; set; }
        public string? ContentType { get; set; }
        public List<Guid>? TargetUsers { get; set; }
    }
}
