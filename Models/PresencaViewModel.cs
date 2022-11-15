using System;

namespace TrabalhoInterdisciplinar.Models
{
    public class PresencaViewModel:PadraoViewModel
    {
        public int CodAluno { get; set; }
        public int CodAula { get; set; }
        public string Presente { get; set; }
        public DateTime DataHoraPresenca { get; set; }
        public string AlunoNome { get; set; }
        public string AulaDescricao { get; set; }
        public string MateriaDescricao { get; set; }
    }
}
