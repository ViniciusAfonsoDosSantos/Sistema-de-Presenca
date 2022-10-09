using TrabalhoInterdisciplinar.DAO;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.Controllers
{
    public class AulaController: PadraoController<AulaViewModel>
    {
        public AulaController()
        {
            DAO = new AulaDAO();
        }
    }
}
