using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class LibreriaMaterial
    {
        [Key]
        public Guid? LibreriaMateriaID { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? AutorLibro { get; set; }
    }
}
