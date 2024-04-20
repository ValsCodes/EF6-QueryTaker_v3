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
                new CommonProxy<long>() {Name = string.Empty, Id = 0 },
                new CommonProxy<long>() {Name = "To Be Processed", Id = 7 },
                new CommonProxy<long>() {Name = "In Progress", Id = 8 },
                new CommonProxy<long>() {Name = "Processed", Id = 9 },
                new CommonProxy<long>() {Name = "Cancelled", Id = 10 }
            };

            return temp;
        }

        public static List<CommonProxy<long>> QueryCatgories()
        {
            var temp = new List<CommonProxy<long>>()
            {
                new CommonProxy<long>() {Name = string.Empty, Id = 0 },
                new CommonProxy<long>() {Name = "Home Security", Id = 1 },
                new CommonProxy<long>() {Name = "Plumbing", Id = 2 },
                new CommonProxy<long>() {Name = "Electricity", Id = 3 },
                new CommonProxy<long>() {Name = "Interior Design", Id = 4 },
                new CommonProxy<long>() {Name = "Cyber Security", Id = 5 },
                new CommonProxy<long>() {Name = "Exterior Design", Id = 6 },
                new CommonProxy<long>() {Name = "Hardware", Id = 7 },
            };

            return temp;
        }
    }
}