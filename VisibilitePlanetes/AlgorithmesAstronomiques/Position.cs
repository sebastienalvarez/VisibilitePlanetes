/****************************************************************************************************************************
 * Classe Position
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe Position permet de définir un lieu d'observation à la surface de la Terre. 
 * Cette classe intègre l'algorithme 11.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class Position
    {
        // FIELDS PRIVES
        private readonly string nomLieuObservation; // Nom du lieu d'observation (paramètre saisi par l'utilisateur)
        private readonly Angle longitude; // Longitude en degrés du lieu d'observation (paramètre saisi)
        private readonly Angle latitude; // Latitude en degrés du lieu d'observation (paramètre saisi)
        private readonly float altitude; // Altitude en m du lieu d'observation (paramètre saisi par l'utilisateur)
        private float pression; // Pression en mbar du lieu d'observation (paramètre saisie par l'utilisateur ou valeur par défaut)
        private float temperature; // Température en °C du lieu d'observation (paramètre saisi par l'utilisateur ou valeur par défaut)
        private double cosphi; // Abscisse rectangulaire géocentrique du lieu d'observation (paramètre calculé) 
        private double sinphi; // Ordonnée rectangulaire géocentrique du lieu d'observation (paramètre calculé)

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Nom du lieu d'observation.
        /// </summary>
        public string NomLieuObservation
        {
            get { return nomLieuObservation; }
        }
        /// <summary>
        /// Longitude du lieu d'observation.
        /// </summary>
        public Angle Longitude
        {
            get { return longitude; }
        }
        /// <summary>
        /// Latitude du lieu d'observation.
        /// </summary>
        public Angle Latitude
        {
            get { return latitude; }
        }
        /// <summary>
        /// Altitude du lieu d'observation en m.
        /// </summary>
        public float Altitude
        {
            get { return altitude; }
        }
        /// <summary>
        /// Pression au lieu d'observation en mbar (par défaut 1013 mbar).
        /// </summary>
        public float Pression
        {
            get { return pression; }
            set
            {
                // Validation de la pression : la pression doit être comprise entre 700 et 1300mbar
                if (value >= 700f && value <= 1300f)
                {
                    pression = value;
                }
                else if (value < 700f)
                {
                    pression = 700f;
                }
                else if (value > 1300f)
                {
                    pression = 1300f;
                }
            }
        }
        /// <summary>
        /// Température en °C au lieu d'observation (par défaut 10°C).
        /// </summary>
        public float Temperature
        {
            get { return temperature; }
            set
            {
                // Validation de la température : la température doit être comprise entre -60°C et 60°C
                if (value >= -60f && value < 60f)
                {
                    temperature = value;
                }
                else if (value < -60f)
                {
                    temperature = -60f;
                }
                else if (value > 60f)
                {
                    temperature = 60f;
                }
            }
        }
        /// <summary>
        /// Abscisse rectangulaire géocentrique du lieu d'observation.
        /// </summary>
        public double CosPhi
        {
            get { return cosphi; }
        }
        /// <summary>
        /// Ordonnée rectangulaire géocentrique du lieu d'observation.
        /// </summary>
        public double SinPhi
        {
            get { return sinphi; }
        }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur d'une instance de Position.
        /// </summary>
        /// <param name="a_nomLieu">Nom du lieu d'observation.</param>
        /// <param name="a_longitudeDegres">Nombre de degrés de la longitude (compris entre -179° et 180°).</param>
        /// <param name="a_longitudeMinutes">Nombre de minutes d'arc de la longitude (compris entre 0' et 59').</param>
        /// <param name="a_longitudeSecondes">Nombre de secondes d'arc de la longitude (compris entre 0" et 59").</param>
        /// <param name="a_latitudeDegres">Nombre de degrés de la latitude (compris entre -90° et 90°).</param>
        /// <param name="a_latitudeMinutes">Nombre de minutes d'arc de la latitude (compris entre 0' et 59').</param>
        /// <param name="a_latitudeSecondes">Nombre de secondes d'arc de la latitude (compris entre 0" et 59").</param>
        /// <param name="a_altitude">Altitude en m (compris entre 0m et 8848m). Ce paramètre est optionnel, par défaut c'est 0m.</param>
        /// <param name="a_pression">Pression en mbar (compris entre 700mbar et 1300mbar). Ce paramètre est optionnel, par défaut c'est 1013mbar.</param>
        /// <param name="a_temperature">Température en °C (compris entre -60°C et 60°C). Ce paramètre est optionnel, par défaut c'est 10°C.</param>
        public Position(string a_nomLieu, int a_longitudeDegres, int a_longitudeMinutes, float a_longitudeSecondes, int a_latitudeDegres, int a_latitudeMinutes, float a_latitudeSecondes, float a_altitude = 0f, float a_pression = 1013f, float a_temperature = 10f)
        {
            nomLieuObservation = a_nomLieu;
            longitude = new Angle(a_longitudeDegres, a_longitudeMinutes, a_longitudeSecondes, TypeAngle.ANGLE_DEGRES_180);
            latitude = new Angle(a_latitudeDegres, a_latitudeMinutes, a_latitudeSecondes, TypeAngle.ANGLE_DEGRES_180);
            // Validation de l'argument a_altitude : l'altitude doit être comprise entre 0 et 8848m
            if (a_altitude >= 0f && a_altitude < 8848f)
            {
                altitude = a_altitude;
            }
            else if (a_altitude < 0f)
            {
                altitude = 0f;
            }
            else if(a_altitude > 8848f)
            {
                altitude = 8848f;
            }
            Pression = a_pression;
            Temperature = a_temperature;
            // Calcul des coordonnées rectangulaires géocentriques
            CalculerCoordonneesRectangulaires();
        }

        // METHODES PUBLIQUES
        //Aucune

        // METHODES PRIVEES
        // ALGORITHME 11
        /// <summary>
        /// Calcule l'abscisse et l'ordonnée des coordonnées rectangulaires géocentriques du lieu d'observation.
        /// </summary>
        private void CalculerCoordonneesRectangulaires()
        {
            double u; // variable de calcul

            u = Math.Atan(0.99664719 * System.Math.Tan(Maths.DegToRad(latitude.Decimale)));

            // Calcul des coordonnées rectangulaires de l'observateur
            cosphi = System.Math.Cos(u) + ((double)altitude / 6378140.0) * System.Math.Cos(Maths.DegToRad(latitude.Decimale));
            sinphi = 0.99664719 * System.Math.Sin(u) + ((double)altitude / 6378140.0) * System.Math.Sin(Maths.DegToRad(latitude.Decimale));
        }
    }
}