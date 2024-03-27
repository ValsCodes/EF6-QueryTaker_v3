using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace EF6_QueryTaker.Models.Enums
{
    public enum StatusEnums
    {
        ToBeProcessed = 7,
        InProgress = 8,
        Processed = 9,
        Declined = 10,
    }
}