using System;

namespace TrabalhoInterdisciplinar.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Mensagem { get; set; }

        public ErrorViewModel(string mensagem)
        {
            Mensagem = mensagem;
        }

        public ErrorViewModel()
        {

        }
    }
}
