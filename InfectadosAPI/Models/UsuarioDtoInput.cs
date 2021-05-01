using System.ComponentModel.DataAnnotations;

namespace InfectadosAPI.Models
{
    public class UsuarioDtoInput
    {
        [Required(ErrorMessage = "Login: campo obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha: campo obrigatório")]
        public string Senha { get; set; }
    }
}
