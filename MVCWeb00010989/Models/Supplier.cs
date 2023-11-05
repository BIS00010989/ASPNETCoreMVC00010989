using System;

namespace MVCWeb00010989.Model
{
    public class Supplier
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public int QuantityOfProductsSupplied { get; set; }
        public string Company { get; set; }
    }
}
