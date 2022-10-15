using System;

namespace TrabalhoInterdisciplinar.Models
{
    public class AulaViewModel: PadraoViewModel
    {
        public string Conteudo { get; set; }
        public DateTime DataHoraAula { get; set; }
        public int CodMateria { get; set; }
    }

}
