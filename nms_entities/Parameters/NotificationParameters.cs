﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.Parameters
{
    public class NotificationParameters: QueryStringParameters
    {
        public NotificationParameters()
        {
            OrderBy = "title";
        }

        public string Title { get; set; }
    }
}
