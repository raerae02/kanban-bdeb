using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using System.Text;
using System.Collections.ObjectModel;

namespace Kanban_BdeB
{
    internal class Tache : IXMLSerializable
    {
        public DateOnly DateCreation
        {
            get;
            set;
        }
        public DateOnly DateDebut
        {
            get;
            set;
        }
        public DateOnly DateFin
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public ObservableCollection<Etape> Etapes
        {
            get;
            set;
        }

        public Tache()
        {
            DateCreation = new DateOnly();
            DateDebut = new DateOnly();
            DateFin = new DateOnly();
            Description = "";
            Etapes = new ObservableCollection<Etape>();
        }
        public Tache(XmlElement xmlElement)
        {
            FromXml(xmlElement);
        }

        //Recuperation de l'element xml et classer ses données dans les attributs de la classe Tache

        public void FromXml(XmlElement xmlElementTache)
        {
            DateCreation = DateOnly.Parse(xmlElementTache.GetAttribute("creation"));
            DateDebut = DateOnly.Parse(xmlElementTache.GetAttribute("debut"));
            DateFin = DateOnly.Parse(xmlElementTache.GetAttribute("fin"));

            XmlElement description = xmlElementTache["description"];
            Description = description.InnerText.Trim();

            XmlElement noeudEtape = xmlElementTache["etapes"];
            XmlNodeList noudeListEtapes = noeudEtape.GetElementsByTagName("etape");

            Etapes = new ObservableCollection<Etape>();

            foreach(XmlElement elementEtapeXml in noudeListEtapes)
            {
                Etape etape = new Etape(elementEtapeXml);
                Etapes.Add(etape);
            }
        }
        //Recuperation du document xml et retourner un element xml representant un objet
        public XmlElement ToXml(XmlDocument xmlDocument)
        {
            XmlElement elementTache = xmlDocument.CreateElement("tache");
            elementTache.SetAttribute("creation", DateCreation.ToString());
            elementTache.SetAttribute("debut", DateDebut.ToString());
            elementTache.SetAttribute("fin", DateFin.ToString());

            XmlElement elementDescription = xmlDocument.CreateElement("description");
            elementDescription.SetAttribute("description", Description);
            
            return elementTache;
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
