/****************************************************************************************************************************************
 * 
 * Classe MoonAndPlanetInfoViewModel
 * Auteur : S. ALVAREZ
 * Date : 29-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter les données affichées pour les informations de visibilité des corps célestes (Lune et planètes)
 *         dans la page.
 * 
 ****************************************************************************************************************************************/

using AlgorithmesAstronomiques;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VisibilitePlanetes.ViewModel
{
    public class MoonAndPlanetInfoViewModel : INotifyPropertyChanged
    {
        // PROPRIETES
        private ObservableCollection<MoonAndPlanet> moonAndPlanetsList;
        /// <summary>
        /// Collection d'objets MoonAndPlanet intégrant les données astronomiques calculées et le choix de sélection de l'utilisateur
        /// </summary>
        public ObservableCollection<MoonAndPlanet> MoonAndPlanetsList
        {
            get { return moonAndPlanetsList; }
            set
            {
                moonAndPlanetsList = value;
                OnPropertyChanged("MoonAndPlanetsList");
            }
        }

        private bool computationPossible;
        /// <summary>
        /// Flag indiquant si le calcul est possible et permettant d'afficher une sélection de contrôles dont la propriété IsVisible est liée par Data Binding
        /// </summary>
        public bool ComputationPossible
        {
            get { return computationPossible; }
            set
            {
                computationPossible = value;
                OnPropertyChanged("ComputationPossible");
            }
        }

        private bool computationImpossible;
        /// <summary>
        /// Flag indiquant si le calcul est impossible et permettant d'afficher une sélection de contrôles dont la propriété IsVisible est liée par Data Binding
        /// </summary>
        public bool ComputationImpossible
        {
            get { return computationImpossible; }
            set
            {
                computationImpossible = value;
                OnPropertyChanged("ComputationImpossible");
            }
        }

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public MoonAndPlanetInfoViewModel()
        {
            // Calcul possible
            if (App.ObservationPointsViewModel.LieuObservationSelectionne != null && App.PlanetSelectionViewModel.SelectedPlanets != null && App.PlanetSelectionViewModel.SelectedPlanets.Count == 8)
            {
                MoonAndPlanetsList = new ObservableCollection<MoonAndPlanet>();
                ComputationPossible = true;
                ComputationImpossible = false;
                Compute();
            }
            // Calcul impossible
            else
            {
                ComputationPossible = false;
                ComputationImpossible = true;
            }
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

        // Methode permettant de calculer les corps célestes
        public void Compute()
        {
            //PositionTemps positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, DateTime.Now.AddHours(1 + ComputeHourChange(DateTime.Now)), 1, ComputeHourChange(DateTime.Now));
            PositionTemps positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, DateTime.Now, 1, ComputeHourChange(DateTime.Now));
            for (int i = 0; i < App.PlanetSelectionViewModel.SelectedPlanets.Count; i++)
            {
                CorpsSystemeSolaire c = null;
                double phase = -100;
                double magnitude = -100;
                switch (App.PlanetSelectionViewModel.SelectedPlanets[i].Name)
                {
                    case "La Lune":
                        c = new Lune();
                        ((Lune)c).CalculerIteratif(positionTemps);
                        phase = ((Lune)c).Phase;
                        break;
                    case "Mercure":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.MERCURE);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        phase = ((Planete)c).Phase;
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                    case "Venus":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.VENUS);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        phase = ((Planete)c).Phase;
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                    case "Mars":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.MARS);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                    case "Jupiter":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.JUPITER);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                    case "Saturne":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.SATURNE);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                    case "Uranus":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.URANUS);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                    case "Neptune":
                        c = new Planete(App.PlanetSelectionViewModel.SelectedPlanets[i].Name, TypeCorpsCeleste.NEPTUNE);
                        ((Planete)c).CalculerIteratif(positionTemps);
                        magnitude = c.AltitudeTopocentrique.Decimale < 0 ? ((Planete)c).Magnitude : ((Planete)c).Magnitude + ((Planete)c).DeltaMagnitude;
                        break;
                }
                MoonAndPlanet moonAndPlanet = new MoonAndPlanet(c, App.PlanetSelectionViewModel.SelectedPlanets[i], phase, magnitude);
                if (App.PlanetSelectionViewModel.SelectedPlanets[i].IsSelected)
                {
                    MoonAndPlanetsList.Add(moonAndPlanet);
                }
            }
        }

        // Méthode permettant de calculer le changement d'heure en France (0 ou 1)
        private short ComputeHourChange(DateTime a_date)
        {
            short result = 0; // Cas des mois 1, 2, 11 et 12
            if (a_date.Month > 3 && a_date.Month < 10) // Case des mois 4, 5, 6, 7, 8, 9
            {
                result = 1;
            }

            if (a_date.Month == 3) // Cas du mois particulier 3 (Mars)
            {
                int hourChangedDayIndex = SearchDayNumberOfHourChange(a_date);
                if (a_date.Day >= hourChangedDayIndex)
                {
                    result = 1;
                }
            }

            if (a_date.Month == 10) // Cas du mois particulier 10 (Octobre)
            {
                int hourChangedDayIndex = SearchDayNumberOfHourChange(a_date);
                if (a_date.Day < hourChangedDayIndex)
                {
                    result = 1;
                }
            }

            return result;
        }

        // Méthode permettant d'identifier le jour du changement d'heure
        private int SearchDayNumberOfHourChange(DateTime a_date)
        {
            int hourChangedDayIndex = 0;
            for (int day = 25; day < 32; day++)
            {
                DateTime testDate = new DateTime(a_date.Year, a_date.Month, day);
                if (testDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    hourChangedDayIndex = day;
                    break;
                }
            }
            return hourChangedDayIndex;
        }
    }
}