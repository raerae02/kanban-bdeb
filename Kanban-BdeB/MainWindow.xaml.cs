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
        //Commandes pour les buttons

        //Commande pour le menu Aide
        public static RoutedCommand AProposCmd = new RoutedCommand();

        //Commande pour le menu Fichier
        public static RoutedCommand OuvrirFichierCmd = new RoutedCommand();
        public static RoutedCommand EnregistrerFichierCmd = new RoutedCommand();
        public static RoutedCommand EnregistrerSousFichierCmd = new RoutedCommand();

        //Utilaires pour XML
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private string dossierBase;
        private string pathFichier;

        //Listes
        private ObservableCollection<Tache> lesTachesPlanifiees;

        public MainWindow()
        {
            dossierBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{DIR_SEPARATOR}" + "Fichiers-3GP";
            pathFichier = dossierBase + DIR_SEPARATOR + "taches.xml";

            lesTachesPlanifiees = new ObservableCollection<Tache>();

            InitializeComponent();
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
                lesTachesPlanifiees.Add(tache);
            }
            listBoxTachesPlanifiees.ItemsSource = lesTachesPlanifiees;
        }

        //Methodes pour le bouton Enregister Fichier
        private void EnregistrerFichier_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SauvegarderTache(pathFichier);
        }
        private void EnregistrerFichier_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //a changer
            e.CanExecute = true;
        }
        private void SauvegarderTache(string nomFichier)
        {

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
            //a changer
            e.CanExecute = true;
        }
    }
}
