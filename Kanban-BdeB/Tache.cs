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

        public void FromXml(XmlElement xmlElement)
        {
            DateCreation = DateOnly.Parse(xmlElement.GetAttribute("creation"));
            DateDebut = DateOnly.Parse(xmlElement.GetAttribute("debut"));
            DateFin = DateOnly.Parse(xmlElement.GetAttribute("fin"));

            XmlElement description = xmlElement["description"];
            Description = description.InnerText.Trim();

            foreach(Etape etape in Etapes)
            {
                //IM NOT SURE ABOUT THIS
                etape.FromXml(xmlElement);
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
    }
}
