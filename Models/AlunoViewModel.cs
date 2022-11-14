using Microsoft.AspNetCore.Http;
using System;

namespace TrabalhoInterdisciplinar.Models
{
    public class AlunoViewModel:PadraoViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public int IdBiometria { get; set; }
        public IFormFile Imagem { get; set; }
        public byte[] ImagemEmByte { get; set; }
        public string ImagemEmBase64
        {
            get
            {
                if (ImagemEmByte != null)
                    return Convert.ToBase64String(ImagemEmByte);
                else
                    return string.Empty;
            }
        }
    }
}
