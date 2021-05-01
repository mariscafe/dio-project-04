using System.Collections.Generic;

namespace InfectadosAPI.Models
{
    public class ErrorModel
    {
        public IEnumerable<string> Errors { get; set; }

        public ErrorModel(IEnumerable<string> erros)
        {
            this.Errors = erros;
        }
    }
}
