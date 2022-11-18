using System.Collections.ObjectModel;
using System.Xml;
using Taches;
using Etapes;
using System.Text;

namespace Tests
{
    /// <summary>
    /// Testes pour la logique des taches
    /// </summary>
    public class TestTache
    {
        //Variable strings à tester
        private string? strTacheP;
        private string? strTacheEC;
        private string? strTacheT;

        //Objet tache 
        private Tache? objTache;

        [SetUp]
        public void Setup()
        {
            strTacheP = intialiserTache(new DateOnly(2022, 10, 31), null, null, "Préparation du TP #3 de 420-3GP-BB (A2022)", false, false, 1, 2, "Faire la grille de correction", "Récupérer travaux sur GitHub");

            strTacheEC = intialiserTache(new DateOnly(2022, 10, 24), new DateOnly(2022, 10, 24), null, "Correction du TP #1 de 420-3GP-BB (A2022)", true, false, 1 ,3 , "Faire la grille de correction", "Corriger 3 travau");

            strTacheT = intialiserTache(new DateOnly(2022, 10, 28), new DateOnly(2022, 10, 28), new DateOnly(2022, 11, 15), "Préparation du TP #2 de 420-3GP-BB (A2022)", true, true, 1, 2, "Décider sujet TP", "Préparer données de travail");

        }

        /// <summary>
        /// Methodes pour l'initialisation d'une tache.
        /// Elle sert a retourner un string avec les attributs d'une tache qui sera tester dans les tests
        /// </summary>
        public string intialiserTache(DateOnly? dateCreation, DateOnly? dateDebut, DateOnly? dateFin, string Description, bool etapeTerminer1, bool etapeTerminer2, int NumeroEtape1, int NumeroEtape2, string descriptionEtape1, string descriptionEtape2)
        {
            string strTache = @$"<tache creation=""{dateCreation}"" debut=""{dateDebut}"" fin=""{dateFin}"">
                             <description>{Description}</description>
                             <etapes>
                                <etape termine=""{etapeTerminer1}"" no=""{NumeroEtape1}"">""{descriptionEtape1}""</etape>
                                <etape termine=""{etapeTerminer2}"" no=""{NumeroEtape2}"">""{descriptionEtape2}""</etape>
                             </etapes>
                          </tache>";

            return strTache;
        }

        /// <summary>
        /// Teste qui vérifie que lorsque toutes les étapes d’une tâche sont complétées, son statut est : Terminée.
        /// </summary>
                [Test]
        public void TestStatutTermine()
        {
            bool checkStatutTermine = false;
            ChargerTacheXML(strTacheT);
            int countDone = 0;
            if (objTache.DateCreation != null && objTache.DateDebut != null && objTache.DateFin != null)
            {
                foreach (Etape etape in objTache.Etapes)
                {
                    if (etape.EtapeTerminer == true)
                    {
                        countDone += 1;
                    }
                }
            }

            if (countDone == objTache.Etapes.Count)
            {
                checkStatutTermine = true;
            }

            Assert.AreEqual(true, checkStatutTermine);
        }

        /// <summary>
        /// Teste qui vérifie que lorsque la première étape est complétée, son statut est : En cours.
        /// </summary>
        [Test]
        public void TestStatutEncours()
        {
            int countDone = 0;
            bool checkStatutEnCours = false;
            ChargerTacheXML(strTacheEC);
            if (objTache.DateCreation != null && objTache.DateDebut != null && objTache.DateFin == null)
            {
                foreach (Etape etape in objTache.Etapes)
                {
                    if (etape.EtapeTerminer == true)
                    {
                        countDone += 1;
                    }
                }
            }

            if (countDone > 0)
            {
                checkStatutEnCours = true;
            }

            Assert.AreEqual(true, checkStatutEnCours);
        }

        /// <summary>
        /// Teste qui vérifie que lorsqu'une tache est crée, son statut est : Planifié.
        /// </summary>
        [Test]
        public void TestStatutPlanifie()
        {
            bool checkStatutPlanifiee = false;
            ChargerTacheXML(strTacheP);
            int countDone = objTache.Etapes.Count;

            if (objTache.DateCreation != null && objTache.DateDebut == null && objTache.DateFin == null)
            {
                foreach (Etape etape in objTache.Etapes)
                {
                    if (etape.EtapeTerminer == false)
                    {
                        countDone -= 1;
                    }
                }
            }

            if (countDone == 0)
            {
                checkStatutPlanifiee = true;
            }

            Assert.AreEqual(true, checkStatutPlanifiee);

        }
        void ChargerTacheXML(string strTache)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strTache);
            objTache = new Tache(doc.DocumentElement);
        }
    }
}
