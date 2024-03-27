namespace EF6_QueryTaker.Models.Common
{
    public class CommonProxy<T>
    {
        public T Id { get; set; }

        public string Name { get; set; }

        public CommonProxy(string name, T id = default)
        {
            Id = id;
            Name = name;
        }

        public CommonProxy() { }
    }
}