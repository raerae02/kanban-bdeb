using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace Kanban_BdeB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Commandes pour le menu
        public static RoutedCommand FichierMenuCmd = new RoutedCommand();
        public static RoutedCommand EditionMenuCmd = new RoutedCommand();
        public static RoutedCommand AideMenuCmd = new RoutedCommand();



        //Commande pour le menu Aide
        public static RoutedCommand AProposCmd = new RoutedCommand();

        //Commande pour le menu Fichier
        public static RoutedCommand OuvrirFichierCmd = new RoutedCommand();
        public static RoutedCommand EnregistrerFichierCmd = new RoutedCommand();
        public static RoutedCommand EnregistrerSousFichierCmd = new RoutedCommand();

        public static RoutedCommand SupprimerTacheCmd = new RoutedCommand();

        //Commandes pour les boutons des Étapes
        public static RoutedCommand TerminerEtapeCmd = new RoutedCommand();
        public static RoutedCommand SupprimerEtapeCmd = new RoutedCommand();

        private Tache currentTache;
        private Etape currentEtape;

        //Utilaires pour XML
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private string dossierBase;
        private string pathFichier;

        //Listes
        private List<Tache> taches;
        private List<ObservableCollection<Tache>> listObservableCollections;

        private List<ListBox> listBoxes;
        private ObservableCollection<Tache> lesTachesPlanifiees;
        private ObservableCollection<Tache> lesTachesEnCours;
        private ObservableCollection<Tache> lesTachesTerminees;

        public MainWindow()
        {
            dossierBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{DIR_SEPARATOR}" + "Fichiers-3GP";
            pathFichier = dossierBase + DIR_SEPARATOR + "taches.xml";

            currentTache = null;
            currentEtape = null;

            taches = new List<Tache>();
            listBoxes = new List<ListBox>();
            listObservableCollections = new List<ObservableCollection<Tache>>();
            lesTachesPlanifiees = new ObservableCollection<Tache>();
            lesTachesEnCours = new ObservableCollection<Tache>();
            lesTachesTerminees = new ObservableCollection<Tache>();

            InitializeComponent();

            listBoxes.Add(listBoxTachesPlanifiees);
            listBoxes.Add(listBoxTachesEnCours);
            listBoxes.Add(listBoxTachesTerminees);
            listBoxes.Add(listBoxEtapes);

            listObservableCollections.Add(lesTachesPlanifiees);
            listObservableCollections.Add(lesTachesEnCours);
            listObservableCollections.Add(lesTachesTerminees);
        }

        //Methodes pour le bouton À propos
        private void APropos_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(
                "Kanban BdeB\n" +
                "Version 0.1\n\n" +
                "Auteur: Mohammad Raed Alkhatib"
                );
        }
        private void APropos_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        //Methodes pour le bouton Ouvrir Fichier
        private void OuvrirFichier_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OuvrirFichier();
        }
        private void OuvrirFichier_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OuvrirFichier()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = dossierBase;
            bool? resultat = openFileDialog.ShowDialog();

            if (resultat.HasValue && resultat.Value)
            {
                pathFichier = openFileDialog.FileName;
                ChargerTaches(pathFichier);
            }
        }
        private void ChargerTaches(string pathFichier)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(pathFichier);
            XmlElement racine = xmlDocument.DocumentElement;

            //Chargement des taches
            XmlNodeList nouedsTaches = racine.GetElementsByTagName("tache");
            foreach(XmlElement xmlElementTache in nouedsTaches)
            {
                Tache tache = new Tache(xmlElementTache);
                taches.Add(tache);
            }
            ChargerListBoxTaches();
        }
        private void ChargerListBoxTaches()
        {
            if(taches.Count > 0)
            {
                foreach(Tache tache in taches)
                {
                    if (tache.DateCreation != null && tache.DateDebut == null && tache.DateFin == null)
                    {
                        lesTachesPlanifiees.Add(tache);
                    }
                    if (tache.DateCreation != null && tache.DateDebut != null && tache.DateFin == null)
                    {
                        lesTachesEnCours.Add(tache);
                    }
                    if (tache.DateCreation != null && tache.DateDebut != null && tache.DateFin != null)
                    {
                        lesTachesTerminees.Add(tache);
                    }
                }
                listBoxTachesPlanifiees.ItemsSource = lesTachesPlanifiees;
                listBoxTachesEnCours.ItemsSource = lesTachesEnCours;
                listBoxTachesTerminees.ItemsSource = lesTachesTerminees;
            }
        }

        //Methodes pour le bouton Enregister Fichier
        private void EnregistrerFichier_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SauvegarderTache(pathFichier);
        }
        private void EnregistrerFichier_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = taches.Count > 0;
        }
        private void SauvegarderTache(string nomFichier)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement racine = xmlDocument.CreateElement("taches");
            xmlDocument.AppendChild(racine);

            foreach(Tache tache in taches)
            {
                racine.AppendChild(tache.ToXml(xmlDocument));
            }
            xmlDocument.Save(nomFichier);
        }

        //Methodes pour le bouton Enregistrer le fichier sous... 
        private void EnregistrerSousFichier_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog.InitialDirectory = dossierBase;
            bool? resultat = saveFileDialog.ShowDialog();

            if (resultat.HasValue && resultat.Value)
            {
                pathFichier = saveFileDialog.FileName;
                SauvegarderTache(pathFichier);
            }
        }
        private void EnregistrerSousFichier_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = taches.Count > 0;
        }

        private void selectionChangeAction(ListBox listBox)
        {
            currentTache = listBox.SelectedItem as Tache;
           // currentEtape = listBoxEtapes.SelectedItem as Etape;

            if (currentTache != null)
            {
                listBoxEtapes.ItemsSource = currentTache.Etapes;
                DataContext = currentTache;

                //foreach(Etape etape in listBoxEtapes.Items)
                //{
                //    listBoxEtapes.SelectedItem = currentEtape.EtapeTerminer == false;
                //    //Do i need this
                //    DataContext = currentEtape;
                //}
            }
            else listBox.SelectedItem = null;
            
        }
        private void listBoxTachesPlanifiees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChangeAction(listBoxTachesPlanifiees);
        }

        private void listBoxTachesEnCours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChangeAction(listBoxTachesEnCours);
        }
        private void listBoxTachesTerminees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChangeAction(listBoxTachesTerminees);
        }

        /// <summary>
        /// Methodes pour le bouton SupprimerTache
        /// Ce bouton permet de supprimer une tache selectionnée
        /// Elle est active seulement quand une tache est selectionnée
        /// </summary>
        private void SupprimerTacheCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            taches.Remove(currentTache);
            ObservableCollection<Tache> currentObservableCollection = null;
            foreach(ObservableCollection<Tache> observableCollection in listObservableCollections)
            {
                if (observableCollection.Contains(currentTache))
                {
                    currentObservableCollection = observableCollection;
                    break;
                }
            }
            if(currentObservableCollection != null)
            {
                currentObservableCollection.Remove(currentTache);
            }
        }
        private void SupprimerTacheCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = currentTache !=null;
        }

        private void FichierMenuCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OuvrirMenu(MenuFichier);
        }
        private void FichierMenuCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void EditionMenuCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OuvrirMenu(MenuEdition);
        }
        private void EditionMenuCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AideMenuCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OuvrirMenu(MenuAPropos);
        }
        private void AideMenuCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OuvrirMenu(MenuItem menuItem)
        {
            menuItem.IsSubmenuOpen = true;
        }

        /// <summary>
        /// Methodes pour le bouton TerminerEtape
        /// Ce bouton permet de terminer une etape selectionnée
        /// Doit etre la premiere etape non terminée sinon le bouton est desactivé
        /// Peut deplacer une tache entre les listbox si toutes ses etapes sont terminées
        /// </summary>
        private void TerminerEtape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentEtape.EtapeTerminer = true;
            if (currentTache.DateDebut == null)
            {
                currentTache.DateDebut = DateOnly.FromDateTime(DateTime.Now);
            }
            if (lesTachesPlanifiees.Contains(currentTache))
            {
                lesTachesPlanifiees.Remove(currentTache);
                lesTachesEnCours.Add(currentTache);
            }
            AllTachesTermineesAction();
        }
        private void TerminerEtape_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (currentEtape != null)
            {
                int indexEtape = -1;
                foreach (Etape etape in currentTache.Etapes)
                {
                    if(etape.EtapeTerminer == false)
                    {
                        indexEtape = currentTache.Etapes.IndexOf(etape);
                        break;
                    }
                }
                if (indexEtape > -1)
                {
                    e.CanExecute = indexEtape == currentTache.Etapes.IndexOf(currentEtape);
                }
            }
        }
        /// <summary>
        /// Cette methode sert a verifier que si toutes les etapes sont terminées. Si cela est le cas, la tache passera du listbox TachesEnCours au listbox TachesTerminée
        /// </summary>
        private void AllTachesTermineesAction()
        {
            int doneCount = 0;
            int allCount = currentTache.Etapes.Count;

            foreach (Etape etape in currentTache.Etapes)
            {
                if (etape.EtapeTerminer == true)
                {
                    doneCount += 1;
                }
            }
            if(doneCount == allCount)
            {
                if (currentTache.DateFin == null)
                {
                    currentTache.DateFin = DateOnly.FromDateTime(DateTime.Now);
                }
                if (lesTachesEnCours.Contains(currentTache))
                {
                    lesTachesEnCours.Remove(currentTache);
                    lesTachesTerminees.Add(currentTache);
                }
            }
        }

        /// <summary>
        /// Methodes pour le bouton SupprimerEtape
        /// Ce bouton permet de supprimer une etape selectionée
        /// </summary>
        private void SupprimerEtape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentEtape = listBoxEtapes.SelectedItem as Etape;
            currentTache.Etapes.Remove(currentEtape);
        }
        private void SupprimerEtape_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //currentEtape = listBoxEtapes.SelectedItem as Etape;
            //if (currentTache != null && currentEtape.EtapeTerminer == false)
            //{
                e.CanExecute = true;
            //}
        }
    }
}
