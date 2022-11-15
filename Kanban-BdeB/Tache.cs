using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using System.Text;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Kanban_BdeB
{
    internal class Tache : IXMLSerializable
    {
        public DateOnly? DateCreation
        {
            get;
            set;
        }
        public DateOnly? DateDebut
        {
            get;
            set;
        }
        public DateOnly? DateFin
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
            DateCreation = DateOnly.FromDateTime(DateTime.Now);
            DateDebut = null;
            DateFin = null;
            Description = "";
            Etapes = new ObservableCollection<Etape>();
        }
        public Tache(XmlElement xmlElement)
        {
            FromXml(xmlElement);
        }

        /// <summary>
        /// Recuperation de l'element xml et classer ses données dans les attributs de la classe Tache
        /// </summary>
        public void FromXml(XmlElement xmlElementTache)
        {
            string dateFormat = "yyyy-MM-dd";

            if (DateOnly.TryParseExact(xmlElementTache.GetAttribute("creation"), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly rdc))
            {
                DateCreation = rdc;
            }
            if (DateOnly.TryParseExact(xmlElementTache.GetAttribute("debut"), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly rdd))
            {
                DateDebut = rdd;
            }
            if (DateOnly.TryParseExact(xmlElementTache.GetAttribute("fin"), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly rdf))
            {
                DateFin = rdf;
            }

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
        /// <summary>
        /// Recuperation du document xml et retourner un element xml representant un objet (tache)   
        /// </summary>
        public XmlElement ToXml(XmlDocument xmlDocument)
        {
            XmlElement elementTache = xmlDocument.CreateElement("tache");
            elementTache.SetAttribute("creation", DateCreation.ToString());
            elementTache.SetAttribute("debut", DateDebut.ToString());
            elementTache.SetAttribute("fin", DateFin.ToString());

            XmlElement elementDescription = xmlDocument.CreateElement("description");
            elementDescription.InnerText = Description.Trim();
            elementTache.AppendChild(elementDescription);

            XmlElement racineEtape = xmlDocument.CreateElement("etapes");

            foreach (Etape etape in Etapes)
            {
                XmlElement elementEtape = etape.ToXml(xmlDocument);
               racineEtape.AppendChild(elementEtape);
            }
            
            elementTache.AppendChild(racineEtape);

            return elementTache;
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
