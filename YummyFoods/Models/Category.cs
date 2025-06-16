using System.ComponentModel.DataAnnotations;

namespace YummyFoods.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You should enter a name.")]
        public string Name { get; set; }
    }
}
