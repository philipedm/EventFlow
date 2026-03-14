namespace Contract
{
    public class OrderCreated
    {
        public Guid Id { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public string User { get; set; }
    }
}
