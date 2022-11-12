using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kanban_BdeB
{
    internal interface IXMLSerializable
    {

        //Recuperation de l'element xml et classer ses données dans les attributs de la classe Tache
        public void FromXml(XmlElement xmlElement);
        //Recuperation du document xml et retourner un element xml representant un objet
        public XmlElement ToXml(XmlDocument xmlDocument);
    }
}
