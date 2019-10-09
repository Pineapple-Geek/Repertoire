using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SyntheseMVVM.Model
{
  class Serialisation
  {
    private static BinaryFormatter formatter = new BinaryFormatter();
    private static Stream stream;



    public static void Serialiser(Object objet, string chemin)
    {
      stream = new FileStream(@chemin, FileMode.Create);
      formatter.Serialize(stream, objet);
      stream.Close();
      stream.Dispose();
    }

    public static Object Deserialiser(string chemin)
    {
      object objet = null;
      try
      {
        stream = new FileStream(@chemin, FileMode.Open);
      }
      catch (FileNotFoundException)
      {
        String erreur = "";
        return erreur;
      }
      objet = (object)formatter.Deserialize(stream);
      stream.Close();
      stream.Dispose();
      return objet;
    }
  }
}
