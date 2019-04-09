using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketBall.AI.Environment;
using System.Windows.Forms;

namespace BasketBall.AI
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BasketballGui());
        }
    }
}
