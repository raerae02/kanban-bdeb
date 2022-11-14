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
            lesTachesPlanifiees = new ObservableCollection<Tache>();
            lesTachesEnCours = new ObservableCollection<Tache>();
            lesTachesTerminees = new ObservableCollection<Tache>();

            InitializeComponent();

            listBoxes.Add(listBoxTachesPlanifiees);
            listBoxes.Add(listBoxTachesEnCours);
            listBoxes.Add(listBoxTachesTerminees);

            listBoxes.Add(listBoxEtapes);
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


        private void SupprimerTacheCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach(ListBox listBox in listBoxes)
            {
                taches.Remove(listBox.SelectedItem as Tache);
            }
            
        }
        private void SupprimerTacheCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach(ListBox listBox in listBoxes)
            {
                e.CanExecute = listBox.SelectedItem != null;
            }
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
        /// </summary>
        private void TerminerEtape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string strTerminer = " (terminée)";
            currentEtape = listBoxEtapes.SelectedItem as Etape;

            if (currentEtape.EtapeTerminer == false)
            {
                currentEtape.EtapeTerminer = true;
                currentEtape.DescriptionEtape += strTerminer;
            }
        }
        private void TerminerEtape_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(listBoxEtapes.SelectedItem != null)
            {
                currentEtape = listBoxEtapes.SelectedItem as Etape;
                if(currentEtape.EtapeTerminer == false)
                {
                    e.CanExecute = true;
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
            currentEtape = listBoxEtapes.SelectedItem as Etape;
            if (currentTache != null && currentEtape.EtapeTerminer == false)
            {
                e.CanExecute = true;
            }
        }
    }
}
