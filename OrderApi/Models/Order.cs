namespace OrderAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string OrderName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
