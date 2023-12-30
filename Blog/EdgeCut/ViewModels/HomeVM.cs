using EdgeCut.Models;

namespace EdgeCut.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Furniture> Furnitures { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public Message Message { get; set; }
    }
}
