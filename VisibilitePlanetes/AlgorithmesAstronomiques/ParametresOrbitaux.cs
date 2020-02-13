/****************************************************************************************************************************
 * Classe ParametresOrbitaux
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe ParametresOrbitaux permet de représenter les paramètres orbitaux d'une comète.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class ParametresOrbitaux
    {
        // FIELDS PRIVES
        private readonly string nom; // Nom du corps céleste
        private readonly double perihelie; // Périhélie en UA
        private readonly double excentricite; // Excentricité
        private readonly Angle inclinaison; // Inclinaison de l'orbite en degré rapportée à l'équinoxe J2000
        private readonly Angle argumentPerihelie; // Argument du périhélie en degré rapporté à l'équinoxe J2000
        private readonly Angle longitudeNoeudAscendant; // Longitude du noeud ascendant rapportée à l'équinoxe J2000
        private readonly DateTime datePassagePerihelie; // Date du dernier passage au périhélie
        private float deltaT; // Delta T Temps Terrestre à la date du dernier passage au périhélie
        private readonly int epoque; // Epoque en MJD autour de laquelle les paramètres orbitaux sont valides   

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Nom de la comète.
        /// </summary>
        public string Nom
        {
            get { return nom; }
        }
        /// <summary>
        /// Périhélie de la comète en UA.
        /// </summary>
        public double Perihelie
        {
            get { return perihelie; }
        }
        /// <summary>
        /// Exentricité de la comète.
        /// </summary>
        public double Exentricite
        {
            get { return excentricite; }
        }
        /// <summary>
        /// Inclinaison de l'orbite en degré rapportée à l'équinoxe J2000.
        /// </summary>
        public Angle Inclinaison
        {
            get { return inclinaison; }
        }
        /// <summary>
        /// Argument du périhélie en degré rapporté à l'équinoxe J2000.
        /// </summary>
        public Angle ArgumentPerihelie
        {
            get { return argumentPerihelie; }
        }
        /// <summary>
        /// Longitude du noeud ascendant rapportée à l'équinoxe J2000.
        /// </summary>
        public Angle LongitudeNoeudAscendant
        {
            get { return longitudeNoeudAscendant; }
        }
        /// <summary>
        /// Date du dernier passage au périhélie.
        /// </summary>
        public DateTime DatePassagePerihelie
        {
            get { return datePassagePerihelie; }
        }
        /// <summary>
        /// Delta T Temps Terrestre à la date du dernier passage au périhélie.
        /// </summary>
        public float DeltaT
        {
            get { return deltaT; }
            private set
            // ajouter un calcul automatique si hors tolérance
            {
                if ((datePassagePerihelie.Year >= 1950 && datePassagePerihelie.Year <= 2100) && (value >= 0f && value <= 100f))
                {
                    deltaT = value;
                }
                else if (datePassagePerihelie.Year >= 1950 && datePassagePerihelie.Year <= 2100)
                {
                    deltaT = Maths.EstimerDeltaT(datePassagePerihelie.Year);
                }
                // Dates éloignées dans le temps, le paramètre deltaT n'est plus significatif et est forcé à 0
                else
                {
                    deltaT = 0;
                }
            }
        }
        /// <summary>
        /// Epoque en MJD autour de laquelle les paramètres orbitaux sont valides.
        /// </summary>
        public int Epoque
        {
            get { return epoque; }
        }

        // CONSTRUCTEUR
        public ParametresOrbitaux(string a_nom, double a_perihelie, double a_exentricite, double a_inclinaison, double a_argumentPerihelie, double a_longitudeNoeudAscendant, int a_passagePerihelieAnnee, int a_passagePerihelieMois, double a_passagePerihelieJour, float a_deltaT = -1, int a_epoque = 365)
        {
            // Validation des paramètres saisis
            nom = a_nom;
            if(a_perihelie <= 0)
            {
                throw new ArgumentOutOfRangeException("a_perihelie", "Le paramètre a_perihelie doit être compris entre ]0 , 200] UA");
            }  
            perihelie = a_perihelie;
            if(a_exentricite < 0 || a_exentricite >= 2)
            {
                throw new ArgumentOutOfRangeException("a_exentricite", "Le paramètre a_exentricite doit être compris entre [0 , 2[");
            }
            excentricite = a_exentricite;
            if(a_inclinaison < -90 || a_inclinaison > 90)
            {
                throw new ArgumentOutOfRangeException("a_inclinaison", "Le paramètre a_inclinaison doit être compris entre [-90° , 90°]");
            }
            inclinaison = new Angle(a_inclinaison, TypeAngle.ANGLE_DEGRES_90);
            if (a_argumentPerihelie < 0 || a_argumentPerihelie >= 360)
            {
                throw new ArgumentOutOfRangeException("a_argumentPerihelie", "Le paramètre a_argumentPerihelie doit être compris entre [0° , 360°[");
            }
            argumentPerihelie = new Angle(a_argumentPerihelie, TypeAngle.ANGLE_DEGRES_360);
            if (a_longitudeNoeudAscendant < 0 || a_longitudeNoeudAscendant >= 360)
            {
                throw new ArgumentOutOfRangeException("a_longitudeNoeudAscendant", "Le paramètre a_longitudeNoeudAscendant doit être compris entre [0° , 360°[");
            }
            longitudeNoeudAscendant = new Angle(a_longitudeNoeudAscendant, TypeAngle.ANGLE_DEGRES_360);
            if (a_passagePerihelieAnnee <= 0)
            {
                throw new ArgumentOutOfRangeException("a_passagePerihelieAnnee", "Le paramètre a_passagePerihelieAnnee doit être positif (seules les dates postérieures à l'an 0 sont prises en comptes)");
            }
            if (a_passagePerihelieMois < 1 || a_passagePerihelieMois > 12)
            {
                throw new ArgumentOutOfRangeException("a_passagePerihelieMois", "Le paramètre a_passagePerihelieMois doit être compris entre 1 et 12");
            }
            if (a_passagePerihelieJour < 0 || a_passagePerihelieJour >= 32)
            {
                throw new ArgumentOutOfRangeException("a_passagePerihelieJour", "Le paramètre a_passagePerihelieJour doit être compris entre [1 , 32[");
            }
            double heure = 24.0 * (a_passagePerihelieJour - (int)a_passagePerihelieJour);
            double minute = 60 * (heure - (int)heure);
            double seconde = 60 * (minute - (int)minute);
            int milliseconde = (int)Math.Round(1000 * (seconde - (int)seconde));
            if (milliseconde < 0)
            {
                milliseconde = 0;
            }
            else if (milliseconde > 999)
            {
                milliseconde = 999;
            }
            datePassagePerihelie = new DateTime(a_passagePerihelieAnnee, a_passagePerihelieMois, (int)a_passagePerihelieJour, (int)heure, (int)minute, (int)seconde, milliseconde);
            DeltaT = a_deltaT;
            if (a_epoque < 0)
            {
                throw new ArgumentOutOfRangeException("a_epoque", "Le paramètre a_epoque doit être positif");
            }
            epoque = a_epoque;   
        }
    }
}