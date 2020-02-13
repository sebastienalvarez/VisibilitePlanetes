/****************************************************************************************************************************
 * Classe CorpsSystemeSolaire
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe abstraite CorpsSystemeSolaire définit les propriétés additionnelles communes à tous les corps du système
 * solaire.
 * Cette classe intègre les algorithmes 8P, 12, 15, 22, 25, 26, 27, 33 et 34. 
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;
using AlgorithmesAstronomiques.DonneesTheories;

namespace AlgorithmesAstronomiques
{
    public abstract class CorpsSystemeSolaire : CorpsCeleste
    {
        // FIELDS PRIVES
        protected double l; // Longitude écliptique héliocentrique
        protected double b; // Latitude écliptique héliocentrique
        protected double rSoleil; // Distance au Soleil
        protected double lambdaGeometrique; // Longitude écliptique géocentrique géométrique
        protected double betaGeometrique; // Latitude écliptique géocentrique géométrique
        protected Angle lambda; // Longitude écliptique géocentrique
        protected Angle beta; // Latitude écliptique géocentrique
        protected double deltaAlpha5; // Correction de l'ascension droite géocentrique due à la réfraction atmosphérique
        protected double deltaDelta5; // Correction de la déclinaison géocentrique due à la réfraction atmosphérique
        protected double deltaAlpha6; // Correction de l'ascension droite géocentrique due à la parallaxe
        protected double deltaDelta6; // Correction de la déclinaison géocentrique due à la parallaxe
        protected double r; // Distance à la Terre

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Longitude écliptique héliocentrique du corps céleste.
        /// </summary>
        public double L
        {
            get { return l; }
        }
        /// <summary>
        /// Latitude écliptique héliocentrique du corps céleste.
        /// </summary>
        public double B
        {
            get { return b; }
        }
        /// <summary>
        /// Distance au Soleil du corps céleste.
        /// </summary>
        public double RSoleil
        {
            get { return rSoleil; }
        }
        /// <summary>
        /// Longitude écliptique géocentrique géométrique du corps céleste.
        /// </summary>
        public double LambdaGeometrique
        {
            get { return lambdaGeometrique; }
        }
        /// <summary>
        /// Latitude écliptique géocentrique géométrique du corps céleste.
        /// </summary>
        public double BetaGeometrique
        {
            get { return betaGeometrique; }
        }
        /// <summary>
        /// Longitude écliptique géocentrique du corps céleste.
        /// </summary>
        public Angle Lambda
        {
            get { return lambda; }
        }
        /// <summary>
        /// Latitude écliptique géocentrique du corps céleste.
        /// </summary>
        public Angle Beta
        {
            get { return beta; }
        }
        /// <summary>
        /// Correction de l'ascension droite géocentrique due à la réfraction atmosphérique du corps céleste.
        /// </summary>
        public double DeltaAlpha5
        {
            get { return deltaAlpha5; }
        }
        /// <summary>
        /// Correction de la déclinaison géocentrique due à la réfraction atmosphérique du corps céleste.
        /// </summary>
        public double DeltaDelta5
        {
            get { return deltaDelta5; }
        }
        /// <summary>
        /// Correction de l'ascension droite géocentrique due à la parallaxe du corps céleste.
        /// </summary>
        public double DeltaAlpha6
        {
            get { return deltaAlpha6; }
        }
        /// <summary>
        /// Correction de la déclinaison géocentrique due à la parallaxe du corps céleste.
        /// </summary>
        public double DeltaDelta6
        {
            get { return deltaDelta6; }
        }
        /// <summary>
        /// Distance à la Terre du corps céleste.
        /// </summary>
        public double R
        {
            get { return r; }
        }

        // CONSTRUCTEURS
        public CorpsSystemeSolaire(string a_nom, TypeCorpsCeleste a_type) : base(a_nom, a_type)
        {
        }

        // METHODES PUBLIQUES
        //Aucune

        // METHODES PRIVEES

        // Algorithme 8P et 33
        /// <summary>
        /// Calcule la longitude et la latitude écliptiques héliocentriques d'une planète ainsi que le rayon vecteur de la planète
        /// à partir du Jour Julien des Ephémérides et de la distance de la planète à la Terre. Lorsqu'il s'agit de la Terre, les coordonnées
        /// écliptiques géocentriques du Soleil sont également calculées.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_planete">Type de planète pour le calcul.</param>
        /// <param name="a_distance">Distance de la planète pour la prise en compte du temps de trajet de la lumière. Ce paramètre est optionnel, par défaut la distance est prise à 0 (pas de prise en compte du temps de trajet de la lumière).</param>
        protected void CalculerCoordonneesEcliptiquesEtRayonVecteur(PositionTemps a_lieuEtDateCalcul, TypeCorpsCeleste a_planete, double a_distance = 0)
        {
            // Déclaration des variables de calcul de la méthode
            double T;
            int indexMaxCoefficientL0 = 0;
            int indexMaxCoefficientL1 = 0;
            int indexMaxCoefficientL2 = 0;
            int indexMaxCoefficientL3 = 0;
            int indexMaxCoefficientL4 = 0;
            int indexMaxCoefficientL5 = 0;
            int indexMaxCoefficientB0 = 0;
            int indexMaxCoefficientB1 = 0;
            int indexMaxCoefficientB2 = 0;
            int indexMaxCoefficientB3 = 0;
            int indexMaxCoefficientB4 = 0;
            int indexMaxCoefficientB5 = 0;
            int indexMaxCoefficientR0 = 0;
            int indexMaxCoefficientR1 = 0;
            int indexMaxCoefficientR2 = 0;
            int indexMaxCoefficientR3 = 0;
            int indexMaxCoefficientR4 = 0;
            int indexMaxCoefficientR5 = 0;
            double[,] coefficientL0 = null;
            double[,] coefficientL1 = null;
            double[,] coefficientL2 = null;
            double[,] coefficientL3 = null;
            double[,] coefficientL4 = null;
            double[,] coefficientL5 = null;
            double[,] coefficientB0 = null;
            double[,] coefficientB1 = null;
            double[,] coefficientB2 = null;
            double[,] coefficientB3 = null;
            double[,] coefficientB4 = null;
            double[,] coefficientB5 = null;
            double[,] coefficientR0 = null;
            double[,] coefficientR1 = null;
            double[,] coefficientR2 = null;
            double[,] coefficientR3 = null;
            double[,] coefficientR4 = null;
            double[,] coefficientR5 = null;
            int i;
            double L0 = 0.0, L1 = 0.0, L2 = 0.0, L3 = 0.0, L4 = 0.0, L5 = 0.0;
            double B0 = 0.0, B1 = 0.0, B2 = 0.0, B3 = 0.0, B4 = 0.0, B5 = 0.0;
            double R0 = 0.0, R1 = 0.0, R2 = 0.0, R3 = 0.0, R4 = 0.0, R5 = 0.0;
            double Lprime, deltaL, deltaB;

            // Calcul du nombre de millénaires écoulés depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 0.0057755183 * a_distance - 2451545.0) / 365250.0;

            // Calcul des séries pour la longitude écliptique héliocentrique de la planète
            switch (a_planete)
            {
                case TypeCorpsCeleste.TERRE_SOLEIL:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_TERRE;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_TERRE;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_TERRE;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_TERRE;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_TERRE;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_TERRE;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_TERRE;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_TERRE;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_TERRE;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_TERRE;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_TERRE;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_TERRE;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_TERRE;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_TERRE;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_TERRE;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_TERRE;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_TERRE;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_TERRE;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_TERRE;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_TERRE;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_TERRE;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_TERRE;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_TERRE;
                    coefficientL5 = VSOP87BP.COEFFICIENT_L5_TERRE;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_TERRE;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_TERRE;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_TERRE;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_TERRE;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_TERRE;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_TERRE;
                    coefficientR4 = VSOP87BP.COEFFICIENT_R4_TERRE;
                    break;
                case TypeCorpsCeleste.MERCURE:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_MERCURE;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_MERCURE;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_MERCURE;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_MERCURE;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_MERCURE;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_MERCURE;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_MERCURE;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_MERCURE;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_MERCURE;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_MERCURE;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_MERCURE;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_MERCURE;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_MERCURE;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_MERCURE;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_MERCURE;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_MERCURE;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_MERCURE;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_MERCURE;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_MERCURE;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_MERCURE;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_MERCURE;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_MERCURE;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_MERCURE;
                    coefficientL5 = VSOP87BP.COEFFICIENT_L5_MERCURE;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_MERCURE;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_MERCURE;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_MERCURE;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_MERCURE;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_MERCURE;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_MERCURE;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_MERCURE;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_MERCURE;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_MERCURE;
                    break;
                case TypeCorpsCeleste.VENUS:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_VENUS;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_VENUS;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_VENUS;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_VENUS;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_VENUS;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_VENUS;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_VENUS;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_VENUS;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_VENUS;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_VENUS;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_VENUS;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_VENUS;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_VENUS;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_VENUS;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_VENUS;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_VENUS;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_VENUS;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_VENUS;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_VENUS;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_VENUS;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_VENUS;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_VENUS;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_VENUS;
                    coefficientL5 = VSOP87BP.COEFFICIENT_L5_VENUS;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_VENUS;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_VENUS;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_VENUS;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_VENUS;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_VENUS;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_VENUS;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_VENUS;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_VENUS;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_VENUS;
                    coefficientR4 = VSOP87BP.COEFFICIENT_R4_VENUS;
                    break;
                case TypeCorpsCeleste.MARS:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_MARS;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_MARS;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_MARS;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_MARS;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_MARS;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_MARS;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_MARS;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_MARS;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_MARS;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_MARS;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_MARS;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_MARS;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_MARS;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_MARS;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_MARS;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_MARS;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_MARS;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_MARS;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_MARS;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_MARS;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_MARS;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_MARS;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_MARS;
                    coefficientL5 = VSOP87BP.COEFFICIENT_L5_MARS;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_MARS;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_MARS;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_MARS;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_MARS;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_MARS;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_MARS;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_MARS;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_MARS;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_MARS;
                    coefficientR4 = VSOP87BP.COEFFICIENT_R4_MARS;
                    break;
                case TypeCorpsCeleste.JUPITER:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_JUPITER;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_JUPITER;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_JUPITER;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_JUPITER;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_JUPITER;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_JUPITER;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_JUPITER;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_JUPITER;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_JUPITER;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_JUPITER;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_JUPITER;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_JUPITER;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_JUPITER;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_JUPITER;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_JUPITER;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_JUPITER;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_JUPITER;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_JUPITER;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_JUPITER;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_JUPITER;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_JUPITER;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_JUPITER;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_JUPITER;
                    coefficientL5 = VSOP87BP.COEFFICIENT_L5_JUPITER;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_JUPITER;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_JUPITER;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_JUPITER;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_JUPITER;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_JUPITER;
                    coefficientB5 = VSOP87BP.COEFFICIENT_B5_JUPITER;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_JUPITER;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_JUPITER;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_JUPITER;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_JUPITER;
                    coefficientR4 = VSOP87BP.COEFFICIENT_R4_JUPITER;
                    coefficientR5 = VSOP87BP.COEFFICIENT_R5_JUPITER;
                    break;
                case TypeCorpsCeleste.SATURNE:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_SATURNE;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_SATURNE;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_SATURNE;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_SATURNE;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_SATURNE;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_SATURNE;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_SATURNE;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_SATURNE;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_SATURNE;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_SATURNE;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_SATURNE;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_SATURNE;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_SATURNE;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_SATURNE;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_SATURNE;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_SATURNE;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_SATURNE;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_SATURNE;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_SATURNE;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_SATURNE;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_SATURNE;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_SATURNE;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_SATURNE;
                    coefficientL5 = VSOP87BP.COEFFICIENT_L5_SATURNE;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_SATURNE;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_SATURNE;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_SATURNE;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_SATURNE;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_SATURNE;
                    coefficientB5 = VSOP87BP.COEFFICIENT_B5_SATURNE;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_SATURNE;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_SATURNE;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_SATURNE;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_SATURNE;
                    coefficientR4 = VSOP87BP.COEFFICIENT_R4_SATURNE;
                    coefficientR5 = VSOP87BP.COEFFICIENT_R5_SATURNE;
                    break;
                case TypeCorpsCeleste.URANUS:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_URANUS;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_URANUS;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_URANUS;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_URANUS;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_URANUS;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_URANUS;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_URANUS;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_URANUS;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_URANUS;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_URANUS;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_URANUS;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_URANUS;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_URANUS;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_URANUS;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_URANUS;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_URANUS;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_URANUS;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_URANUS;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_URANUS;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_URANUS;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_URANUS;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_URANUS;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_URANUS;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_URANUS;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_URANUS;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_URANUS;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_URANUS;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_URANUS;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_URANUS;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_URANUS;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_URANUS;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_URANUS;
                    coefficientR4 = VSOP87BP.COEFFICIENT_R4_URANUS;
                    break;
                case TypeCorpsCeleste.NEPTUNE:
                    indexMaxCoefficientL0 = VSOP87BP.INDEX_MAX_COEFFICIENT_L0_NEPTUNE;
                    indexMaxCoefficientL1 = VSOP87BP.INDEX_MAX_COEFFICIENT_L1_NEPTUNE;
                    indexMaxCoefficientL2 = VSOP87BP.INDEX_MAX_COEFFICIENT_L2_NEPTUNE;
                    indexMaxCoefficientL3 = VSOP87BP.INDEX_MAX_COEFFICIENT_L3_NEPTUNE;
                    indexMaxCoefficientL4 = VSOP87BP.INDEX_MAX_COEFFICIENT_L4_NEPTUNE;
                    indexMaxCoefficientL5 = VSOP87BP.INDEX_MAX_COEFFICIENT_L5_NEPTUNE;
                    indexMaxCoefficientB0 = VSOP87BP.INDEX_MAX_COEFFICIENT_B0_NEPTUNE;
                    indexMaxCoefficientB1 = VSOP87BP.INDEX_MAX_COEFFICIENT_B1_NEPTUNE;
                    indexMaxCoefficientB2 = VSOP87BP.INDEX_MAX_COEFFICIENT_B2_NEPTUNE;
                    indexMaxCoefficientB3 = VSOP87BP.INDEX_MAX_COEFFICIENT_B3_NEPTUNE;
                    indexMaxCoefficientB4 = VSOP87BP.INDEX_MAX_COEFFICIENT_B4_NEPTUNE;
                    indexMaxCoefficientB5 = VSOP87BP.INDEX_MAX_COEFFICIENT_B5_NEPTUNE;
                    indexMaxCoefficientR0 = VSOP87BP.INDEX_MAX_COEFFICIENT_R0_NEPTUNE;
                    indexMaxCoefficientR1 = VSOP87BP.INDEX_MAX_COEFFICIENT_R1_NEPTUNE;
                    indexMaxCoefficientR2 = VSOP87BP.INDEX_MAX_COEFFICIENT_R2_NEPTUNE;
                    indexMaxCoefficientR3 = VSOP87BP.INDEX_MAX_COEFFICIENT_R3_NEPTUNE;
                    indexMaxCoefficientR4 = VSOP87BP.INDEX_MAX_COEFFICIENT_R4_NEPTUNE;
                    indexMaxCoefficientR5 = VSOP87BP.INDEX_MAX_COEFFICIENT_R5_NEPTUNE;
                    coefficientL0 = VSOP87BP.COEFFICIENT_L0_NEPTUNE;
                    coefficientL1 = VSOP87BP.COEFFICIENT_L1_NEPTUNE;
                    coefficientL2 = VSOP87BP.COEFFICIENT_L2_NEPTUNE;
                    coefficientL3 = VSOP87BP.COEFFICIENT_L3_NEPTUNE;
                    coefficientL4 = VSOP87BP.COEFFICIENT_L4_NEPTUNE;
                    coefficientB0 = VSOP87BP.COEFFICIENT_B0_NEPTUNE;
                    coefficientB1 = VSOP87BP.COEFFICIENT_B1_NEPTUNE;
                    coefficientB2 = VSOP87BP.COEFFICIENT_B2_NEPTUNE;
                    coefficientB3 = VSOP87BP.COEFFICIENT_B3_NEPTUNE;
                    coefficientB4 = VSOP87BP.COEFFICIENT_B4_NEPTUNE;
                    coefficientR0 = VSOP87BP.COEFFICIENT_R0_NEPTUNE;
                    coefficientR1 = VSOP87BP.COEFFICIENT_R1_NEPTUNE;
                    coefficientR2 = VSOP87BP.COEFFICIENT_R2_NEPTUNE;
                    coefficientR3 = VSOP87BP.COEFFICIENT_R3_NEPTUNE;
                    break;
            }

            // Calcul des séries pour la longitude écliptique héliocentrique de la planète
            for (i = 0; i < indexMaxCoefficientL0; i++)
            {
                L0 += coefficientL0[i, 0] * Math.Cos(coefficientL0[i, 1] + coefficientL0[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientL1; i++)
            {
                L1 += coefficientL1[i, 0] * Math.Cos(coefficientL1[i, 1] + coefficientL1[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientL2; i++)
            {
                L2 += coefficientL2[i, 0] * Math.Cos(coefficientL2[i, 1] + coefficientL2[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientL3; i++)
            {
                L3 += coefficientL3[i, 0] * Math.Cos(coefficientL3[i, 1] + coefficientL3[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientL4; i++)
            {
                L4 += coefficientL4[i, 0] * Math.Cos(coefficientL4[i, 1] + coefficientL4[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientL5; i++)
            {
                L5 += coefficientL5[i, 0] * Math.Cos(coefficientL5[i, 1] + coefficientL5[i, 2] * T);
            }

            // Calcul des séries pour la latitude écliptique héliocentrique de la planète
            for (i = 0; i < indexMaxCoefficientB0; i++)
            {
                B0 += coefficientB0[i, 0] * Math.Cos(coefficientB0[i, 1] + coefficientB0[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientB1; i++)
            {
                B1 += coefficientB1[i, 0] * Math.Cos(coefficientB1[i, 1] + coefficientB1[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientB2; i++)
            {
                B2 += coefficientB2[i, 0] * Math.Cos(coefficientB2[i, 1] + coefficientB2[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientB3; i++)
            {
                B3 += coefficientB3[i, 0] * Math.Cos(coefficientB3[i, 1] + coefficientB3[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientB4; i++)
            {
                B4 += coefficientB4[i, 0] * Math.Cos(coefficientB4[i, 1] + coefficientB4[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientB5; i++)
            {
                B5 += coefficientB5[i, 0] * Math.Cos(coefficientB5[i, 1] + coefficientB5[i, 2] * T);
            }

            // Calcul des séries pour le rayon vecteur de la planète
            for (i = 0; i < indexMaxCoefficientR0; i++)
            {
                R0 += coefficientR0[i, 0] * Math.Cos(coefficientR0[i, 1] + coefficientR0[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientR1; i++)
            {
                R1 += coefficientR1[i, 0] * Math.Cos(coefficientR1[i, 1] + coefficientR1[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientR2; i++)
            {
                R2 += coefficientR2[i, 0] * Math.Cos(coefficientR2[i, 1] + coefficientR2[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientR3; i++)
            {
                R3 += coefficientR3[i, 0] * Math.Cos(coefficientR3[i, 1] + coefficientR3[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientR4; i++)
            {
                R4 += coefficientR4[i, 0] * Math.Cos(coefficientR4[i, 1] + coefficientR4[i, 2] * T);
            }
            for (i = 0; i < indexMaxCoefficientR5; i++)
            {
                R5 += coefficientR5[i, 0] * Math.Cos(coefficientR5[i, 1] + coefficientR5[i, 2] * T);
            }

            // Calcul de la longitude écliptique héliocentrique de la planète à l'équinoxe dynamique de la théorie VSOP87
            l = (L0 + L1 * T + L2 * T * T + L3 * T * T * T + L4 * T * T * T * T + L5 * T * T * T * T * T) / 100000000;
            l = Maths.RadToDeg(l);
            l = Maths.Modulo(l, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de la latitude écliptique héliocentrique de la planète à l'équinoxe dynamique de la théorie VSOP87
            b = (B0 + B1 * T + B2 * T * T + B3 * T * T * T + B4 * T * T * T * T + B5 * T * T * T * T * T) / 100000000;
            b = Maths.RadToDeg(b);

            // Calcul du rayon vecteur de la planète
            rSoleil = (R0 + R1 * T + R2 * T * T + R3 * T * T * T + R4 * T * T * T * T + R5 * T * T * T * T * T) / 100000000;

            // Calcul des corrections de l'équinoxe
            Lprime = l - 13.97 * T - 0.031 * T * T;
            deltaL = -0.09033 + 0.03916 * (Math.Cos(Maths.DegToRad(Lprime)) + Math.Sin(Maths.DegToRad(Lprime))) * Math.Tan(Maths.DegToRad(b));
            deltaB = 0.03916 * (Math.Cos(Maths.DegToRad(Lprime)) - Math.Sin(Maths.DegToRad(Lprime)));

            // Calcul de la longitude écliptique héliocentrique de la planète à l'équinoxe moyen
            l += deltaL / 3600.0;
            l = Maths.Modulo(l, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de la latitude écliptique héliocentrique de la planète à l'équinoxe moyen
            b += deltaB / 3600.0;

            // Suite de l'algorithme 8 pour la Terre (Soleil)
            if (a_planete == TypeCorpsCeleste.TERRE_SOLEIL)
            {
                // Calcul de la longitude écliptique géocentrique géométrique du Soleil
                lambdaGeometrique = l + 180.0;
                lambdaGeometrique = Maths.Modulo(LambdaGeometrique, TypeAngle.ANGLE_DEGRES_360);

                // Calcul de la longitude écliptique géocentrique du Soleil (corrigée de l'aberration de la lumière et de la nutation)
                r = rSoleil;
                lambda = new Angle(lambdaGeometrique + a_lieuEtDateCalcul.NutationLongitude - (20.4898 / (3600.0 * r)));

                // Calcul de la latitude écliptique héliocentrique et géocentrique
                betaGeometrique = -b;
                beta = new Angle(-b, TypeAngle.ANGLE_DEGRES_90);
            }
        }

        // Algorithme 12
        /// <summary>
        /// Calcule les coordonnées écliptiques géocentriques d’un corps céleste en coordonnées équatoriales géocentriques à partir de l’obliquité moyenne et de la nutation en obliquité.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        protected void ConvertirCoordonneesEcliptiquesVersEquatoriales(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double x, y;    // variables de calcul

            // Calcul de la déclinaison géocentrique
            double declGeo = Math.Asin((Math.Sin(Maths.DegToRad(beta.Decimale)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)))
                                      + (Math.Cos(Maths.DegToRad(beta.Decimale)) * Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)) * Math.Sin(Maths.DegToRad(lambda.Decimale))));
            declGeo = Maths.RadToDeg(declGeo);
            declinaisonGeocentrique = new Angle(declGeo, TypeAngle.ANGLE_DEGRES_90);

            // Calcul de l'ascension droite géocentrique
            y = (Math.Sin(Maths.DegToRad(lambda.Decimale)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite))) - (Math.Tan(Maths.DegToRad(beta.Decimale)) * Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)));
            x = Math.Cos(Maths.DegToRad(lambda.Decimale));
            double alphaGeo = Math.Atan2(y, x);
            alphaGeo = Maths.RadToDeg(alphaGeo);
            alphaGeo = Maths.Modulo(alphaGeo, TypeAngle.ANGLE_DEGRES_360) / 15.0;
            ascensionDroiteGeocentrique = new Angle(alphaGeo, TypeAngle.ANGLE_HEURES_24);
        }

        // Algorithme 15
        /// <summary>
        /// Calcule les coordonnées écliptiques géocentriques géométriques (non corrigées de l’aberration de la lumière et de la
        /// nutation) à partir de la longitude écliptique héliocentrique et du rayon vecteur de la Terre.
        /// </summary>
        /// <param name="a_terreSoleil">Objet représentant la Terre/le Soleil.</param>
        protected void CalculerCoordonneesEcliptiquesGeocentriquesGeometriques(CorpsSystemeSolaire a_terreSoleil)
        {
            // Déclaration des variables de la méthode
            double x, y; // variables de calcul

            // Calcul de la longitude écliptique géocentrique géométrique
            y = rSoleil * Math.Cos(Maths.DegToRad(b)) * Math.Sin(Maths.DegToRad(a_terreSoleil.L - l));
            x = a_terreSoleil.R - rSoleil * Math.Cos(Maths.DegToRad(b)) * Math.Cos(Maths.DegToRad(a_terreSoleil.L - l));
            lambdaGeometrique = 180.0 + a_terreSoleil.L + Maths.RadToDeg(Math.Atan2(y, x));
            lambdaGeometrique = Maths.Modulo(lambdaGeometrique, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de la latitude écliptique géocentrique géométrique
            betaGeometrique = Maths.RadToDeg(Math.Atan((rSoleil * Math.Cos(Maths.DegToRad(b)) * Math.Tan(Maths.DegToRad(b)) * Math.Sin((Maths.DegToRad(lambdaGeometrique - l))) / (a_terreSoleil.R * Math.Sin(Maths.DegToRad(l - a_terreSoleil.L))))));
        }

        // Algorithme 22
        /// <summary>
        /// Calcule les coordonnées écliptiques géocentriques d'une planète ou d'une comète à partir des différentes corrections (nutation et aberration de la lumière)
        /// et des coordonnées écliptiques géocentriques géométriques de la planète ou de la comète.
        /// Pour une planète utiliser 0 pour les 2 derniers arguments ou utiliser une version surchargée de la méthode.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_deltaLambda4">Correction de la longitude écliptique géométrique due à l'aberration de la lumière.</param>
        /// <param name="a_deltaBeta4">Correction de la latitude écliptique géométrique due à l'aberration de la lumière.</param>
        /// <param name="a_deltaLambda2">Correction de la longitude écliptique due à la précession.</param>
        /// <param name="a_deltaBeta2">Correction de la latitude écliptique due à la précession.</param>
        protected void CalculerCoordonneesEcliptiquesGeocentriques(PositionTemps a_lieuEtDateCalcul, double a_deltaLambda4, double a_deltaBeta4, double a_deltaLambda2, double a_deltaBeta2)
        {
            lambda = new Angle(lambdaGeometrique + a_lieuEtDateCalcul.NutationLongitude + a_deltaLambda2 + a_deltaLambda4);
            beta = new Angle(betaGeometrique + a_deltaBeta2 + a_deltaBeta4, TypeAngle.ANGLE_DEGRES_90);
        }
        /// <summary>
        /// Calcule les coordonnées écliptiques géocentriques d’une planète à partir des corrections dues à l'aberration de la lumière
        /// et des coordonnées écliptiques géocentriques géométriques de la planète.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_deltaLambda4">Correction de la longitude écliptique géométrique due à l'aberration de la lumière.</param>
        /// <param name="a_deltaBeta4">Correction de la latitude écliptique géométrique due à l'aberration de la lumière.</param>
        protected void CalculerCoordonneesEcliptiquesGeocentriques(PositionTemps a_lieuEtDateCalcul, double a_deltaLambda4, double a_deltaBeta4)
        {
            CalculerCoordonneesEcliptiquesGeocentriques(a_lieuEtDateCalcul, a_deltaLambda4, a_deltaBeta4, 0, 0);
        }

        // Algorithme 25
        /// <summary>
        /// Calcule les corrections dues à la réfraction atmosphérique à apporter sur les coordonnées équatoriales géocentriques d’un corps céleste
        /// à partir des coordonnées équatoriales géocentriques et des coordonnées équatoriales topocentriques corrigées de la réfraction atmosphérique.
        /// </summary>
        protected void CalculerCorrectionCoordonneesEquatorialesRefraction()
        {
            // Détermination des corrections
            deltaAlpha5 = ascensionDroiteTopocentrique.Decimale - ascensionDroiteGeocentrique.Decimale;
            deltaDelta5 = declinaisonTopocentrique.Decimale - declinaisonGeocentrique.Decimale;
        }

        // Algorithme 26
        /// <summary>
        /// Calcule les corrections dues à la parallaxe à apporter sur les coordonnées équatoriales géocentriques d’un corps céleste à partir des coordonnées rectangulaires géocentriques du lieu d’observation,
        /// des coordonnées équatoriales géocentriques du corps céleste, de l’heure sidérale locale et de la distance du corps céleste à la Terre
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        protected void CalculerCorrectionCoordonneesEquatorialesParallaxe(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double sinpi = 0, H;    // variables de calcul
            double x, y;        // variables de calcul

            // Calcul de la parallaxe
            if (type == TypeCorpsCeleste.LUNE)
            {
                sinpi = (6378.14 / r);
            }
            if (type != TypeCorpsCeleste.LUNE)
            {
                sinpi = (Math.Sin(Maths.DegToRad(8.794 / 3600.0)) / r);
            }

            // Calcul de l'angle horaire
            H = Maths.CalculerHeureDecimale(a_lieuEtDateCalcul.HeureSideraleLocale) - ascensionDroiteGeocentrique.Decimale;
            H = Maths.Modulo(H, TypeAngle.ANGLE_HEURES_24);
            H = 15.0 * H; // H en degrés

            // Calcul des corrections à apporter sur les coordonnées équatoriales géocentriques
            y = -a_lieuEtDateCalcul.LieuObservation.CosPhi * sinpi * Math.Sin(Maths.DegToRad(H));
            x = Math.Cos(Maths.DegToRad(declinaisonGeocentrique.Decimale)) - a_lieuEtDateCalcul.LieuObservation.CosPhi * sinpi * Math.Cos(Maths.DegToRad(H));
            deltaAlpha6 = Math.Atan2(y, x);
            declinaisonTopocentrique = new Angle(Maths.RadToDeg(Math.Atan(((Math.Sin(Maths.DegToRad(declinaisonGeocentrique.Decimale)) - a_lieuEtDateCalcul.LieuObservation.SinPhi * sinpi) * Math.Cos(deltaAlpha6))
                                                 / (Math.Cos(Maths.DegToRad(declinaisonGeocentrique.Decimale)) - a_lieuEtDateCalcul.LieuObservation.CosPhi * sinpi * Math.Cos(Maths.DegToRad(H))))), TypeAngle.ANGLE_DEGRES_90);
            deltaDelta6 = declinaisonTopocentrique.Decimale - declinaisonGeocentrique.Decimale;
            deltaAlpha6 = Maths.RadToDeg(deltaAlpha6) / 15.0;
        }

        // Algorithme 27
        /// <summary>
        /// Calcule les coordonnées équatoriales topocentriques d’un corps céleste à partir des différentes corrections liées au lieu d’observation et des coordonnées équatoriales géocentriques du corps céleste.
        /// </summary>
        protected void CalculerCoordonneesEquatorialesTopocentriques()
        {
            // Détermination des coordonnées équatoriales topocentriques
            ascensionDroiteTopocentrique = new Angle(ascensionDroiteGeocentrique.Decimale + deltaAlpha5 + deltaAlpha6, TypeAngle.ANGLE_HEURES_24);
            declinaisonTopocentrique = new Angle(declinaisonGeocentrique.Decimale + deltaDelta5 + deltaDelta6, TypeAngle.ANGLE_DEGRES_90);
        }

        // Algorithme 34
        /// <summary>
        /// Calcule la distance à la Terre des planètes et des comètes à partir de la longitude et de la latitude écliptiques
        /// héliocentriques et du rayon vecteur des planètes et des comètes, ainsi que la longitude écliptique héliocentrique
        /// et le rayon vecteur de la Terre.
        /// </summary>
        /// <param name="a_terreSoleil">Objet représentant la Terre/le Soleil.</param>
        protected void CalculerDistanceALaTerre(CorpsSystemeSolaire a_terreSoleil)
        {
            r = Math.Sqrt(a_terreSoleil.R * a_terreSoleil.R + rSoleil * rSoleil - 2 * a_terreSoleil.R * rSoleil * Math.Cos(Maths.DegToRad(l - a_terreSoleil.L)) * Math.Cos(Maths.DegToRad(b)));
        }
    }
}