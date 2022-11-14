using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kanban_BdeB
{
    public class Etape:IXMLSerializable
    {
        public bool EtapeTerminer
        {
            get;
            set;
        }
        public int? NumeroEtape
        {
            get;
            set;
        }
        public string DescriptionEtape
        {
            get;
            set;
        }

        public Etape()
        {
            EtapeTerminer = false;
            NumeroEtape = null;
            DescriptionEtape = "";
        }
        
        public Etape(XmlElement elementEtape)
        {
            FromXml(elementEtape);
        }

        public void FromXml(XmlElement xmlElement)
        {
            EtapeTerminer = Boolean.Parse(xmlElement.GetAttribute("termine"));
            NumeroEtape = Int32.Parse(xmlElement.GetAttribute("no"));
            DescriptionEtape = xmlElement.InnerText.Trim();

        }

        public XmlElement ToXml(XmlDocument xmlDocument)
        {
            XmlElement elementEtape = xmlDocument.CreateElement("etape");
            elementEtape.SetAttribute("termine", EtapeTerminer.ToString());
            elementEtape.SetAttribute("no", NumeroEtape.ToString());
            elementEtape.InnerText = DescriptionEtape;

            return elementEtape;
        }
        public override string ToString()
        {
            if (EtapeTerminer)
            {
                return $"{DescriptionEtape} (terminée)";
            }
            else return DescriptionEtape;
        }
    }
}
