using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysSeguridad2G05.EntidadesNegocio
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre es Obligatorio")]
        [StringLength(30, ErrorMessage ="Tamaño maximo 30 caracteres")]
        public string Nombre { get; set; }

        public List<Usuario> Usuarios { get; set; }

        [NotMapped]
        public int Top_Aux { get; set; }
    }
}
