namespace Core.Domain
{
    public class Product
    {
        public Product()
        {
        }
        public Product(int id, float cost, string name, int version)
        {
            Id = id;
            Cost = cost;
            Name = name;
            Version = version;
        }

        public int Id { get; set; }

        public float Cost { get; set; }

        public string Name { get; set; }

        public int Version { get; set; }
    }
}
