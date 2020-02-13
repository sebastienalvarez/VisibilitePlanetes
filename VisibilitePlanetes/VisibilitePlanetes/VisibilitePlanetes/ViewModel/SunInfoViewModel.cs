/****************************************************************************************************************************************
 * 
 * Classe SunInfoViewModel
 * Auteur : S. ALVAREZ
 * Date : 28-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter les données affichées pour les informations de visibilité des corps célestes dans la page.
 * 
 ****************************************************************************************************************************************/

using AlgorithmesAstronomiques;
using System;
using System.ComponentModel;

namespace VisibilitePlanetes.ViewModel
{
    public class SunInfoViewModel : INotifyPropertyChanged
    {
        // PROPRIETES
        private Soleil soleil;
        /// <summary>
        /// Objet Soleil (bibliothèque AlgorithmesAstronomiques) avec toutes les données calculées sur le Soleil
        /// </summary>
        public Soleil Soleil
        {
            get { return soleil; }
            set
            {
                soleil = value;
                OnPropertyChanged("Soleil");
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

        private TimeSpan dureeJour;
        /// <summary>
        /// Durée du jour calculé à partir des données de l'objet Soleil
        /// </summary>
        public TimeSpan DureeJour
        {
            get { return dureeJour; }
            set
            {
                dureeJour = value;
                OnPropertyChanged("DureeJour");
            }
        }

        private DateTime heureLeverAstronomique;
        /// <summary>
        /// Heure de lever astronomique du Soleil (c'est-à-dire quand le Soleil est à une altitude de -12°)
        /// </summary>
        public DateTime HeureLeverAstronomique
        {
            get { return heureLeverAstronomique; }
            set
            {
                heureLeverAstronomique = value;
                OnPropertyChanged("HeureLeverAstronomique");
            }
        }

        private DateTime heureCoucherAstronomique;
        /// <summary>
        /// Heure de coucher astronomique du Soleil (c'est-à-dire quand le Soleil est à une altitude de -18°)
        /// </summary>
        public DateTime HeureCoucherAstronomique
        {
            get { return heureCoucherAstronomique; }
            set
            {
                heureCoucherAstronomique = value;
                OnPropertyChanged("HeureCoucherAstronomique");
            }
        }

        private TimeSpan dureeNuitAstronomique;
        /// <summary>
        /// Durée de la nuit astronomique (propice aux observations) calculée à partir des heures de lever et de coucher astronomiques du Soleil
        /// </summary>
        public TimeSpan DureeNuitAstronomique
        {
            get { return dureeNuitAstronomique; }
            set
            {
                dureeNuitAstronomique = value;
                OnPropertyChanged("DureeNuitAstronomique");
            }
        }

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public SunInfoViewModel()
        {
            // Calcul possible
            if (App.ObservationPointsViewModel.LieuObservationSelectionne != null && App.PlanetSelectionViewModel.SelectedPlanets != null && App.PlanetSelectionViewModel.SelectedPlanets.Count == 8)
            {
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

        // Methode permettant de calculer le Soleil
        public void Compute()
        {
            //PositionTemps positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, DateTime.Now.AddHours(1 + ComputeHourChange(DateTime.Now)), 1, ComputeHourChange(DateTime.Now));
            PositionTemps positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, DateTime.Now, 1, ComputeHourChange(DateTime.Now));
            Soleil s = new Soleil();
            s.CalculerIteratif(positionTemps);
            Soleil = s;
            DureeJour = Soleil.HeureLocaleCoucher - Soleil.HeureLocaleLever;
            ComputeEndNight();
            ComputeStartNight();
            DureeNuitAstronomique = new TimeSpan(24,0,0) - (HeureCoucherAstronomique - HeureLeverAstronomique);
        }

        // Méthode permettant de calculer la fin de nuit : le Soleil est à une altitude de -12° (avant le coucher du Soleil donc)
        private void ComputeEndNight()
        {
            DateTime time = Soleil.HeureLocaleLever.AddMinutes(-30);
            PositionTemps positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, time, 1, ComputeHourChange(time));
            Soleil sun = new Soleil();
            sun.CalculerNonIteratif(positionTemps);
            while (sun.AltitudeTopocentrique.Decimale > new AlgorithmesAstronomiques.Utilitaires.Angle(-12, 0, 0f, AlgorithmesAstronomiques.Utilitaires.TypeAngle.ANGLE_DEGRES_90).Decimale)
            {
                time = time.AddMinutes(-1);
                positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, time, 1, ComputeHourChange(time));
                sun.CalculerNonIteratif(positionTemps);
            }
            HeureLeverAstronomique = time;
        }

        // Méthode permettant de calculer le début de nuit : le Soleil est à une altitude de -18° (après le coucher du Soleil donc)
        private void ComputeStartNight()
        {
            DateTime time = Soleil.HeureLocaleCoucher.AddMinutes(30);
            PositionTemps positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, time, 1, ComputeHourChange(time));
            Soleil sun = new Soleil();
            sun.CalculerNonIteratif(positionTemps);
            while (sun.AltitudeTopocentrique.Decimale > new AlgorithmesAstronomiques.Utilitaires.Angle(-18, 0, 0f, AlgorithmesAstronomiques.Utilitaires.TypeAngle.ANGLE_DEGRES_90).Decimale)
            {
                time = time.AddMinutes(1);
                positionTemps = new PositionTemps(App.ObservationPointsViewModel.LieuObservationSelectionne.LieuObservation, time, 1, ComputeHourChange(time));
                sun.CalculerNonIteratif(positionTemps);
            }
            HeureCoucherAstronomique = time;
        }


        // Méthode permettant de calculer le changement d'heure en France (0 ou 1)
        private short ComputeHourChange(DateTime a_date)
        {
            short result = 0; // Cas des mois 1, 2, 11 et 12
            if (a_date.Month > 3 && a_date.Month < 10) // Case des mois 4, 5, 6, 7, 8, 9
            {
                result = 1;
            }

            if(a_date.Month == 3) // Cas du mois particulier 3 (Mars)
            {
                int hourChangedDayIndex = SearchDayNumberOfHourChange(a_date);
                if(a_date.Day >= hourChangedDayIndex)
                {
                    result = 1;
                }
            }

            if(a_date.Month == 10) // Cas du mois particulier 10 (Octobre)
            {
                int hourChangedDayIndex = SearchDayNumberOfHourChange(a_date);
                if (a_date.Day < hourChangedDayIndex)
                {
                    result = 1;
                }
            }

            return result;
        }
        
        // Méthode permettant d'identifier lz jour du changement d'heure
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