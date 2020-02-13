/****************************************************************************************************************************************
 * 
 * Classe PlanetSelectionViewMode
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter les données affichées pour la sélection des corps célestes dans la page.
 * 
 ****************************************************************************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using VisibilitePlanetes.Model;

namespace VisibilitePlanetes.ViewModel
{
    public class PlanetSelectionViewModel : INotifyPropertyChanged
    {
        // PROPRIETES
        private ObservableCollection<PlanetSelection> selectedPlanets;
        /// <summary>
        /// Collection d'objets PlanetSelection pour représenter les différents corps célestes sélectionnables 
        /// </summary>
        public ObservableCollection<PlanetSelection> SelectedPlanets
        {
            get { return selectedPlanets; }
            set
            {
                selectedPlanets = value;
                OnPropertyChanged("SelectedPlanets");
            }
        }

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        /// <summary>
        /// Instancie l'objet ViewModel avec les données enregistrées saisies précédemment par l'utilisateur
        /// </summary>
        /// <param name="a_selectedPlanets">Données enregistrées saisies précédemment par l'utilisateur</param>
        public PlanetSelectionViewModel(List<PlanetSelectionModel> a_selectedPlanets)
        {
            // Création de la collection SelectedPlanets
            ObservableCollection<PlanetSelection> liste = new ObservableCollection<PlanetSelection>();
            bool isSelected;
            // Lune
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Lune");
            liste.Add(new PlanetSelection("LuneSelectionnee.png", "LuneNonSelectionnee.png", 1, "La Lune", "La plus spectaculaire...", isSelected));
            // Mercure
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Mercure");
            liste.Add(new PlanetSelection("MercureSelectionee.png", "MercureNonSelectionnee.png", 2, "Mercure", "La plus petite...", isSelected));
            // Vénus
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Venus");
            liste.Add(new PlanetSelection("VenusSelectionnee.png", "VenusNonSelectionnee.png", 3, "Venus", "La fournaise...", isSelected));
            // Mars
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Mars");
            liste.Add(new PlanetSelection("MarsSelectionnee.png", "MarsNonSelectionnee.png", 4, "Mars", "La planète rouge...", isSelected));
            // Jupiter
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Jupiter");
            liste.Add(new PlanetSelection("JupiterSelectionnee.png", "JupiterNonSelectionnee.png", 5, "Jupiter", "La plus massive...", isSelected));
            // Saturne
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Saturne");
            liste.Add(new PlanetSelection("SaturneSelectionnee.png", "SaturneNonSelectionnee.png", 6, "Saturne", "Le joyau du système solaire...", isSelected));
            // Uranus
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Uranus");
            liste.Add(new PlanetSelection("UranusSelectionnee.png", "UranusNonSelectionnee.png", 7, "Uranus", "La moins connue...", isSelected));
            // Neptune
            isSelected = GetUserSelectedValue(a_selectedPlanets, "Neptune");
            liste.Add(new PlanetSelection("NeptuneSelectionnee.png", "NeptuneNonSelectionnee.png", 8, "Neptune", "La plus lointaine...", isSelected));
            SelectedPlanets = liste;
        }

        // METHODES
        // Méthode permettant l'envoi de notification de changement de valeur pour le Data Binding
        private void OnPropertyChanged(string a_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(a_propertyName));
            }
        }

        // Méthode permettant de récupérer la sélection de l'utilisateur pour le corps céleste concerné
        private bool GetUserSelectedValue(List<PlanetSelectionModel> a_selectedPlanets, string a_name)
        {
            bool isSelected = true;
            if(a_selectedPlanets != null)
            {
                PlanetSelectionModel planet = a_selectedPlanets.Where(p => p.Name == a_name).FirstOrDefault();
                if(planet != null)
                {
                    isSelected = planet.IsSelected;
                }
            }
            return isSelected;
        }
    }
}