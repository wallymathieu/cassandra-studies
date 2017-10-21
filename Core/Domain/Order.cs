using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Order
    {
        public Order()
        {
        }
        public Order(int id, int customer, DateTime orderDate, IEnumerable<int> products, int version)
        {
            Id = id;
            Customer = customer;
            OrderDate = orderDate;
            Products = products;
            Version = version;
        }
        public int Id { get; set; }

        public int Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public IEnumerable<int> Products { get; set; }

        public int Version { get; set; }
    }
}
