using System.Collections.ObjectModel;
using System.Xml;
using Taches;
using Etapes;
using System.Text;

namespace Tests
{
    public class TestTache
    {
        private string? strTacheP;
        private string? strTacheEC;
        private string? strTacheT;
        private Tache? objTache;

        [SetUp]
        public void Setup()
        {
            strTacheP = intialiserTachePl(new DateOnly(2022, 10, 31), null, null, "Préparation du TP #3 de 420-3GP-BB (A2022)",
                false, 1, "Décider sujet TP");

            strTacheEC = intialiserTacheEC(new DateOnly(2022, 10, 31), new DateOnly(2022, 11, 1), null,
                "test2", true, false, 0, "test3", "test4");

            strTacheT = intialiserTacheT(new DateOnly(2022, 10, 31), new DateOnly(2022, 11, 1), new DateOnly(2022, 11, 2),
                "test5", true, true, 0, "test6", "test7");

        }

        public string intialiserTacheT(DateOnly? dateCreation, DateOnly? dateDebut, DateOnly? dateFin,
           string Description, bool eT1, bool eT2, int NumeroEtape, string descE1, string descE2)
        {
            string strTache = @$"<tache creation=""{dateCreation}"" debut=""{dateDebut}"" fin=""{dateFin}"">
                             <description>{Description}</description>
                             <etapes>
                                <etape termine=""{eT1}"" no=""{NumeroEtape += 1}"">""{descE1}""</etape>
                                <etape termine=""{eT2}"" no=""{NumeroEtape += 1}"">""{descE2}""</etape>
                             </etapes>
                          </tache>";

            return strTache;
        }

        public string intialiserTacheEC(DateOnly? dateCreation, DateOnly? dateDebut, DateOnly? dateFin,
           string Description, bool eT1, bool eT2, int NumeroEtape, string descE1, string descE2)
        {
            string strTache = @$"<tache creation=""{dateCreation}"" debut=""{dateDebut}"" fin=""{dateFin}"">
                             <description>{Description}</description>
                             <etapes>
                                <etape termine=""{eT1}"" no=""{NumeroEtape += 1}"">""{descE1}""</etape>
                                <etape termine=""{eT2}"" no=""{NumeroEtape += 1}"">""{descE2}""</etape>
                             </etapes>
                          </tache>";

            return strTache;
        }

        public string intialiserTachePl(DateOnly? dateCreation, DateOnly? dateDebut, DateOnly? dateFin,
            string Description, bool EtapeTerminer, int NumeroEtape, string DescriptionEtape)
        {
            string strTache = @$"<tache creation=""{dateCreation}"" debut=""{dateDebut}"" fin=""{dateFin}"">
                             <description>{Description}</description>
                             <etapes>
                                <etape termine=""{EtapeTerminer}"" no=""{NumeroEtape}"">""{DescriptionEtape}""</etape>
                             </etapes>
                          </tache>";

            return strTache;
        }

        [Test]
        public void TestStatutTermine()
        {
            bool checkStatutTermine = false;
            ChargerTacheTermineeXML();
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

        [Test]
        public void TestStatutEncours()
        {
            int countDone = 0;
            bool checkStatutEnCours = false;
            ChargerTacheEnCoursXML();
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

        [Test]
        public void TestStatutPlanifie()
        {
            bool checkStatutPlanifiee = false;
            ChargerTachePlanifieeXML();
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

        void ChargerTacheTermineeXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strTacheT);
            objTache = new Tache(doc.DocumentElement);
        }

        void ChargerTacheEnCoursXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strTacheEC);
            objTache = new Tache(doc.DocumentElement);
        }

        void ChargerTachePlanifieeXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strTacheP);
            objTache = new Tache(doc.DocumentElement);
        }
    }
}
