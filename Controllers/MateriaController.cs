using Microsoft.AspNetCore.Mvc;
using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class MateriaController : PadraoController<MateriaViewModel>
    {
        public MateriaController()
        {
            DAO = new MateriaDAO();
        }
    }
}
