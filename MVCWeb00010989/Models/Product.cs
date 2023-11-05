using System;

namespace MVCWeb00010989.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Supplier ProductCategory { get; set; }
        public int Weight { get; set; }
        public string Units { get; set; }
        public DateTime SuppliedDate { get; set; }
    }
}
