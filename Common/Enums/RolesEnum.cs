
namespace EF6_QueryTaker.Models.Enums
{
    public class RolesEnum
    {
        public static readonly RolesEnum Admin = new RolesEnum(1, "Administrator");
        public static readonly RolesEnum Engineer = new RolesEnum(2, "Engineer");
        public static readonly RolesEnum Operator = new RolesEnum(3, "Operator");
        public static readonly RolesEnum User = new RolesEnum(4, "User");

        private RolesEnum(long value, string name)
        {
            Number = value;
            Name = name;
        }

        private long Number { get; }
        private string Name { get; }

        public string GetString()
        {
            return Name;
        }

        public long GetEnum()
        {
            return Number;
        }
    }
}