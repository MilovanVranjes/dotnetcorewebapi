using iMisho.API.Data;

namespace iMisho.API.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
    }
}
