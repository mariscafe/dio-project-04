namespace InfectadosAPI.Models
{
    public class SuccessModel
    {
        public string Message { get; set; }

        public SuccessModel(string msg)
        {
            this.Message = msg;
        }
    }
}
