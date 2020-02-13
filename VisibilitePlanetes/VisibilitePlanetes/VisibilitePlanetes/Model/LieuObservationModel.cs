/****************************************************************************************************************************************
 * 
 * Classe LieuObservationModel
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter un lieu d'observation.
 *         Cette classe permet la persistance des données dans le téléphone (fichier de base de données SQLite).
 * 
 ****************************************************************************************************************************************/

using System.ComponentModel;
using AlgorithmesAstronomiques;
using SQLite;

namespace VisibilitePlanetes.Model
{
    public class LieuObservationModel : INotifyPropertyChanged
    {
        // PROPRIETES
        /// <summary>
        /// ID du lieu d'observation (pour clé primaire d'une base de données)
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private Position lieuObservation;
        /// <summary>
        /// Objet Position représentant un lieu d'observation (issu de la bibliothèque AlgorithmesAstronomiques)
        /// </summary>
        [Ignore]
        public Position LieuObservation
        {
            get { return lieuObservation; }
            set
            {
                lieuObservation = value;
                // Copie des données dans les propriétés wrappées
                nomLieuObservation = LieuObservation.NomLieuObservation;
                longitude = LieuObservation.Longitude.Decimale;
                latitude = LieuObservation.Latitude.Decimale;
                altitude = LieuObservation.Altitude;
                OnPropertyChanged("LieuObservation");
            }
        }

        public bool LieuSelectionne { get; set; } = false;

        // La propriété LieuObservation étant un type complexe, pour la persistence des données avec SQLite, il faut une table pour les objets Angle de LieuObservation et une table pour l'objet LieuObservation.
        // Deux solutions peuvent être envisagées pour la persistence des données :
        // 1 - Création des tables et modifications nécessaires des classes Position et Angles de la bibliothèque AlgorithmesAstronomiques
        // 2 - Solution de contournement pour ne pas modifier la la bibliothèque AlgorithmesAstronomiques : création de propriétés supplémentaires wrappant les données essentielles à sauvegarder 
        // => c'est la solution 2 qui est implémentée
        private string nomLieuObservation;
        /// <summary>
        /// Nom du lieu d'observation (pour la persistence des données avec SQLite) 
        /// </summary>
        public string NomLieuObservation
        {
            get
            {
                //nomLieuObservation = LieuObservation.NomLieuObservation;
                return nomLieuObservation;
            }
            set { nomLieuObservation = value; }
        }

        private double longitude;
        /// <summary>
        /// Longitude du lieu d'observation (pour la persistence des données avec SQLite)
        /// </summary>
        public double Longitude
        {
            get
            {
                //longitude = LieuObservation.Longitude.Decimale;
                return longitude;
            }
            set { longitude = value; }
        }

        private double latitude;
        /// <summary>
        /// Latitude du lieu d'observation (pour la persistence des données avec SQLite)
        /// </summary>
        public double Latitude
        {
            get
            {
                //latitude = LieuObservation.Latitude.Decimale;
                return latitude;
            }
            set { latitude = value; }
        }

        private float altitude;
        /// <summary>
        /// Altitude du lieu d'observation (pour la persistence des données avec SQLite)
        /// </summary>
        public float Altitude
        {
            get
            {
                //altitude = LieuObservation.Altitude;
                return altitude;
            }
            set { altitude = value; }
        }

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public LieuObservationModel()
        {
        }
        public LieuObservationModel(string a_nomLieu, int a_longitudeDegres, int a_longitudeMinutes, float a_longitudeSecondes, int a_latitudeDegres, int a_latitudeMinutes, float a_latitudeSecondes, float a_altitude)
        {
            LieuObservation = new Position(a_nomLieu, a_longitudeDegres, a_longitudeMinutes, a_longitudeSecondes, a_latitudeDegres, a_latitudeMinutes, a_latitudeSecondes, a_altitude);
        }

        // METHODES
        // Méthode permettant l'envoi de notification de changement de valeur pour le Data Binding
        protected void OnPropertyChanged(string a_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(a_propertyName));
            }
        }

    }
}