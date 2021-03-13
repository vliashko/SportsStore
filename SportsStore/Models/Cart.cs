using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> lineColletion = new List<CartLine>();
        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineColletion
                                .Where(p => p.Product.ProductID == product.ProductID)
                                .FirstOrDefault();
            if(line == null)
            {
                lineColletion.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Product product) => 
            lineColletion.RemoveAll(l => l.Product.ProductID == product.ProductID);
        public virtual decimal ComputeTotalValue() =>
            lineColletion.Sum(e => e.Product.Price * e.Quantity);
        public virtual void Clear() => lineColletion.Clear();
        public virtual IEnumerable<CartLine> Lines => lineColletion;
    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
