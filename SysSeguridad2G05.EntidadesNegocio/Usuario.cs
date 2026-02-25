using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysSeguridad2G05.EntidadesNegocio
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Rol")]
        [Required(ErrorMessage = "Rol es Obligatorio")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Nombre es obligatorio")]
        [StringLength(30, ErrorMessage = "Maximo 30 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Apellido es Obligatorio")]
        [StringLength(30, ErrorMessage ="Maximo 30 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Login es Obligatorio")]
        [StringLength(25, ErrorMessage = "Maximo 25 caracteres")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password es Obligatorio")]
        [DataType(DataType.Password)]
        [StringLength(34, ErrorMessage = "Maximo 34 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Estatus es Obligatorio")]
        public byte Estatus { get; set; }

        [Display(Name ="Fecha Registro")]
        public DateTime FechaRegistro { get; set; }

        public Rol Rol { get; set; }

        [NotMapped]
        public int Top_Aux { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirmar Password es Obligatorio")]
        [StringLength(34, ErrorMessage = "Password de estar entre 5 y 34 caracteres")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password y Confirmar password deben ser iguales")]
        [Display(Name ="Confirmar Password")]
        public string ConfirmPassword_aux { get; set; }
    }

    public enum Estatus_Usuario
    { 
        ACTIVO = 1,
        INACTIVO = 2
    }
}
