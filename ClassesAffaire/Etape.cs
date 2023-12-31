﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utilitaires;

namespace Etapes
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

        /// <summary>
        /// Recuperation de l'element xml et classer ses données dans les attributs de la classe Tache
        /// </summary>
        public void FromXml(XmlElement xmlElement)
        {
            EtapeTerminer = Boolean.Parse(xmlElement.GetAttribute("termine"));
            NumeroEtape = Int32.Parse(xmlElement.GetAttribute("no"));
            DescriptionEtape = xmlElement.InnerText.Trim();

        }
        /// <summary>
        /// Recuperation du document xml et retourner un element xml representant un objet (tache)    
        /// </summary>
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
            string strTerminer = "(terminée)";
            if (EtapeTerminer && !DescriptionEtape.Contains(strTerminer))
            {
                return $"{DescriptionEtape} {strTerminer}";
            }
            else return DescriptionEtape;
        }
    }
}
