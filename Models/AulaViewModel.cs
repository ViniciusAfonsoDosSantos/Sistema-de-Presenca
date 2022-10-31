using System;
using TrabalhoInterdisciplinar.Enumeradores;

namespace TrabalhoInterdisciplinar.Models
{
    public class AulaViewModel: PadraoViewModel
    {
        public string Conteudo { get; set; }
        public DateTime DataHoraAula { get; set; }
        public int CodMateria { get; set; }
        public string Materia { get; set; }
        public EnumSituacaoAula Situacao { get; set; }
    }

}
