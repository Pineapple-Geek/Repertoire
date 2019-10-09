using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntheseMVVM.Model
{
  [Serializable]
  public class Contact
  {
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Numtel { get; set; }
    public string Mail { get; set; }
  }
}
