using EF6_QueryTaker.Models.Common;
using System.Collections.Generic;

namespace EF6_QueryTaker.Common
{
    public class StaticCollections
    {
        public static List<CommonProxy<long>> QueryStatuses()
        {
            var temp = new List<CommonProxy<long>>()
            {
                new CommonProxy<long>() {Name = "", Id = 0 },
                new CommonProxy<long>() {Name = "To Be Processed", Id = 7 },
                new CommonProxy<long>() {Name = "In Progress", Id = 8 },
                new CommonProxy<long>() {Name = "Processed", Id = 9 },
                new CommonProxy<long>() {Name = "Cancelled", Id = 10 }
            };

            return temp;
        }
    }
}