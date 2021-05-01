using MongoDB.Driver.GeoJsonObjectModel;
using System;

namespace InfectadosAPI.Models
{
    public class InfectadoDtoOutput
    {
        public String Id { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }
    }
}
