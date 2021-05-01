namespace InfectadosAPI.Data.Collections
{
    public class Usuario
    {
        public Usuario(string login, string senha)
        {
            this.Login = login;
            this.Senha = senha;
        }

        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
