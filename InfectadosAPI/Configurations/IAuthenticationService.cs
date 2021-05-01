using InfectadosAPI.Models;

namespace InfectadosAPI.Configurations
{
    public interface IAuthenticationService
    {
        string GerarToken(UsuarioDtoOutput usuarioModelOutput);
    }
}
