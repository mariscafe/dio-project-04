using System;
using System.ComponentModel.DataAnnotations;

namespace InfectadosAPI.Models
{
    public class InfectadoDtoInput
    {
        [Required(ErrorMessage = "Dt. nascimento: campo obrigatório")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Sexo: campo obrigatório")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Latitude: campo obrigatório")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude: campo obrigatório")]
        public double Longitude { get; set; }
    }
}
