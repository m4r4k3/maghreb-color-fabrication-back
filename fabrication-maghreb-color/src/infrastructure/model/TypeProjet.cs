using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
       [Table("TypeProjet")]

    public class TypeProjet   
    {
        [Key]
        public Int32 Id { get; set; }
        public string Intitule { get; set; }
        public string Abrege {get; set;}  
        public string Reference { get; set; }
        
    }

}


