/****************************************************************************************************************************
 * Classe PositionTemps
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe PositionTemps permet de définir un lieu d'observation à la surface de la Terre.
 * Cette classe intègre les algorithmes 1, 3, 4, 6, 9 et 10P.
 *    
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;
using AlgorithmesAstronomiques.DonneesTheories;

namespace AlgorithmesAstronomiques
{
    public class PositionTemps
    {
        
        // FIELDS PRIVES
        private readonly Position lieuObservation; // Lieu d'observation
        private DateTime heureLocale; // Date et heure locales (paramètre saisi par l'utilisateur)
        private readonly short zoneHoraire; // Zone horaire en h (paramètre saisi par l'utilisateur)
        private short changementHeure; // Changement d'heure en h pour les périodes été/hiver (paramètre saisi)
        private float deltaT; // Delta T en s lié au Temps Terrestre (paramètre saisi ou estimé) 
        private DateTime heureGreenwich; // Date et heure à Greenwich (paramètre calculé)
        private double jourJulienEphemerides; // Jour Julien des Ephémérides de la date considéré à Greenwich (paramètre calculé)
        private double jourJulien0h; // Jour Julien des Ephémérides de la date considéré à Greenwich à 0h TU (paramètre calculé)
        private double obliquiteMoyenne; // Obliquité moyenne en degrés (paramètre calculé)
        private double nutationLongitude; // Nutation en longitude en degrés (paramètre calculé)
        private double nutationObliquite; // Nutation en obliquité en degrés (paramètre calculé)
        private DateTime heureSideraleGreenwich; // Heure sidérale à Greenwich (paramètre calculé)
        private DateTime heureSideraleLocale; // Heure sidérale locale (paramètre calculé)

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Lieu d'observation (objet de type Position).
        /// </summary>
        public Position LieuObservation
        {
            get { return lieuObservation; }
        }
        /// <summary>
        /// Date et heure locales.
        /// </summary>
        public DateTime HeureLocale
        {
            get { return heureLocale; }
            set
            {
                // Validation de l'année saisie : l'année doit être comprise entre 0 et 3000
                if (value.Year <= 3000)
                {
                    heureLocale = value;
                }
                else
                {
                    heureLocale = new DateTime(3000, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
                }  
            }
        }
        /// <summary>
        /// Zone horaire (décalage horaire par rapport à Greenwich dû au fuseau horaire).
        /// </summary>
        public short ZoneHoraire
        {
            get { return zoneHoraire; }
        }
        /// <summary>
        /// Changement d'heure été / hiver.
        /// </summary>
        public short ChangementHeure
        {
            get { return changementHeure; }
            set
            // Validation du changement d'heure saisi : le changement d'heure doit être égal à 0 ou 1
            {
                if (value == 0 || value == 1)
                {
                    changementHeure = value;
                }
                else if (value < 0)
                {
                    changementHeure = 0;
                }
                else if (value > 1)
                {
                    changementHeure = 1;
                }

            }
        }
        /// <summary>
        /// Delta T en s lié au Temps Terrestre (différence en secondes entre le Temps Terrestre et le temps TU).
        /// </summary>
        public float DeltaT
        {
            get { return deltaT; }
            set
            // ajouter un calcul automatique si hors tolérance
            {
                if ((heureLocale.Year >= 1950 && heureLocale.Year <= 2100) && (value >= 0f && value <= 100f))
                {
                    deltaT = value;
                }
                else if (heureLocale.Year >= 1950 && heureLocale.Year <= 2100)
                {
                    deltaT = Maths.EstimerDeltaT(heureLocale.Year);
                }
                // Dates éloignées dans le temps, le paramètre deltaT n'est plus significatif et est forcé à 0
                else
                {
                    deltaT = 0;
                }
            }
        }
        /// <summary>
        /// Date et heure à Greenwich.
        /// </summary>
        public DateTime HeureGreenwich
        {
            get { return heureGreenwich; }
        }
        /// <summary>
        /// Jour Julien des Ephémérides tenant compte de Delta T.
        /// </summary>
        public double JourJulienEphemerides
        {
            get { return jourJulienEphemerides; }
        }
        /// <summary>
        /// Jour Julien à 0h TU.
        /// </summary>
        public double JourJulien0h
        {
            get { return jourJulien0h; }
        }
        /// <summary>
        /// Obliquité moyenne.
        /// </summary>
        public double ObliquiteMoyenne
        {
            get { return obliquiteMoyenne; }
        }
        /// <summary>
        /// Nutation en longitude (écliptique).
        /// </summary>
        public double NutationLongitude
        {
            get { return nutationLongitude; }
        }
        /// <summary>
        /// Nutation en obliquité.
        /// </summary>
        public double NutationObliquite
        {
            get { return nutationObliquite; }
        }
        /// <summary>
        /// Heure sidérale vraie à Greenwich.
        /// </summary>
        public DateTime HeureSideraleGreenwich
        {
            get { return heureSideraleGreenwich; }
        }
        /// <summary>
        /// Heure sidérale vraie locale.
        /// </summary>
        public DateTime HeureSideraleLocale
        {
            get { return heureSideraleLocale; }
        }

        // CONSTRUCTEURS
        /// <summary>
        /// Constructeur d'une instance de PositionTemps. La date et l'heure sont données sous forme de composantes.
        /// </summary>
        /// <param name="a_lieuObservation">Objet Position correspondant à la position géographique du lieu d'observation.</param>
        /// <param name="a_annee">Année locale (compris entre 0 et 3000).</param>
        /// <param name="a_mois">Mois local (compris entre 1 et 12).</param>
        /// <param name="a_jour">Jour local (compris entre 1 et 31).</param>
        /// <param name="a_heure">Nombre d'heure local (compris entre 0 et 24).</param>
        /// <param name="a_minute">Nombre de minute local (compris entre 0 et 59).</param>
        /// <param name="a_seconde">Nombre de seconde local (compris entre [0f et 60f[).</param>
        /// <param name="a_zoneHoraire">Zone horaire (fuseau horaire compris entre -12 et 12), en France : +1. Ce paramètre est optionnel, par défaut c'est 1 (pour la France).</param>
        /// <param name="a_changementHeure">Décalage horaire lié à la politique de changement d'heure (0h ou 1h) en France 0 en hiver, +1 en été. Ce paramètre est optionnel, par défaut c'est 0 (heure d'hiver).</param>
        /// <param name="a_deltaT">Delta T entre le Temps Terrestre et le Temps Universel. Cette donnée est mise à jour tous les 6 mois et peut être obtenue en consultant le bulletin A de l'IERS. Une estimation de Delta T peut être calculé avec l'équation suivante Delta T = 62.92 + 0.32217 x (année - 2000) + 0.005589 x (année - 2000)². Ce paramètre est optionnel, par défaut la valeur estimée calculée par l'équation précédente est utilisée.</param>
        public PositionTemps(Position a_lieuObservation, int a_annee, int a_mois, int a_jour, int a_heure, int a_minute, float a_seconde, short a_zoneHoraire = 1, short a_changementHeure = 0, float a_deltaT = -1)
        {
            lieuObservation = a_lieuObservation;
            int millisecond = (int)(1000 * (Math.Round(a_seconde, 3) - (int)a_seconde));
            if (millisecond < 0)
            {
                millisecond = 0;
            }
            else if (millisecond > 999)
            {
                millisecond = 999;
            }
            HeureLocale = new DateTime(a_annee, a_mois, a_jour, a_heure, a_minute, (int)a_seconde, millisecond);
            // Validation de la zone horaire
            if (a_zoneHoraire >= -12 && a_zoneHoraire <= 12)
            {
                zoneHoraire = a_zoneHoraire;
            }
            else if (a_zoneHoraire < -12)
            {
                zoneHoraire = -12;
            }
            else if (a_zoneHoraire > 12)
            {
                zoneHoraire = 12;
            }           
            ChangementHeure = a_changementHeure;
            DeltaT = a_deltaT;

            // Calcul des autres paramètres
            CalculerParametres();
        }
        /// <summary>
        /// Constructeur d'une instance de PositionTemps. La date et l'heure sont données sous forme d'un objet DateTime.
        /// </summary>
        /// <param name="a_lieuObservation">Objet Position correspondant à la position géographique du lieu d'observation.</param>
        /// <param name="a_dateHeure">Date et heure locales (année comprise entre 0 et 3000).</param>
        /// <param name="a_zoneHoraire">Zone horaire (fuseau horaire compris entre -12 et 12), en France : +1. Ce paramètre est optionnel, par défaut c'est 1 (pour la France).</param>
        /// <param name="a_changementHeure">Décalage horaire lié à la politique de changement d'heure (0h ou 1h) en France 0 en hiver, +1 en été. Ce paramètre est optionnel, par défaut c'est 0 (heure d'hiver).</param>
        /// <param name="a_deltaT">Delta T entre le Temps Terrestre et le Temps Universel. Cette donnée est mise à jour tous les 6 mois et peut être obtenue en consultant le bulletin A de l'IERS. Une estimation de Delta T peut être calculé avec l'équation suivante Delta T = 62.92 + 0.32217 x (année - 2000) + 0.005589 x (année - 2000)². Ce paramètre est optionnel, par défaut la valeur estimée calculée par l'équation précédente est utilisée.</param>
        public PositionTemps(Position a_lieuObservation, DateTime a_dateHeure, short a_zoneHoraire = 1, short a_changementHeure = 0, float a_deltaT = -1)
        {
            lieuObservation = a_lieuObservation;
            HeureLocale = a_dateHeure;
            // Validation de la zone horaire
            if (a_zoneHoraire >= -12 && a_zoneHoraire <= 12)
            {
                zoneHoraire = a_zoneHoraire;
            }
            else if (a_zoneHoraire < -12)
            {
                zoneHoraire = -12;
            }
            else if (a_zoneHoraire > 12)
            {
                zoneHoraire = 12;
            }
            ChangementHeure = a_changementHeure;
            DeltaT = a_deltaT;

            // Calcul des autres paramètres
            CalculerParametres();
        }

        // METHODES PUBLIQUES
        /// <summary>
        /// Initialise les fields de l'instance de l'objet PositionTemps par calcul. Cette méthode est appelée par le constructeur.
        /// </summary>
        public void CalculerParametres()
        {
            CalculerDateHeureAGreenwich(); // Algorithme 1
            CalculerJourJulien(); // Algorithme 3
            CalculerObliquiteMoyenne(); // Algorithme 9
            CalculerNutation(); // Algorithme 10P
            CalculerHeureSideraleVraieAGreenwich(); // Algorithme 4
            CalculerHeureSideraleVraieLocale(); // Algorithme 6
        }

        // METHODES PRIVEES
        // ALGORITHME 1
        /// <summary>
        /// Calcule la date et l'heure à Greenwich, c’est-à-dire la date et l’heure TU, à partir de la date et de l’heure locales du point d’observation et du décalage horaire dû au fuseau horaire (en France +1h) et aux économies d’énergie liées à l’heure d’été et à l’heure d’hiver (en France +1h en été et 0h en hiver). L’algorithme tient compte du caractère bissextile de l’année.
        /// </summary>
        private void CalculerDateHeureAGreenwich()
        {
            // Déclaration des variables de la méthode
            int AB; // variable pour tenir compte du caractère bissextile de l'année
            // Initialisation de la date et de l'heure à Greenwich avec la date locale avant calcul
            int anneeGreenwich = heureLocale.Year;
            int moisGreenwich = heureLocale.Month;
            int jourGreenwich = heureLocale.Day;
            int heureGreenwich = heureLocale.Hour - zoneHoraire - changementHeure;

            // Calcul de l'heure à Greenwich
            if (heureGreenwich > 23)
            {
                heureGreenwich -= 24;
                jourGreenwich = heureLocale.Day + 1;
            }
            if (heureGreenwich < 0)
            {
                heureGreenwich += 24;
                jourGreenwich = heureLocale.Day - 1;
            }

            // Détermination du caractère bissextile de l'année du point d'observation
            AB = 0;
            if (heureLocale.Year % 4 == 0)
            {
                AB = 1;
            }
            if (heureLocale.Year % 100 == 0)
            {
                if (heureLocale.Year / 400 != 0)
                {
                    AB = 0;
                }
            }

            // Calcul de la date à Greenwich
            // Cas du mois de janvier
            if (heureLocale.Month == 1)
            {
                if (jourGreenwich > 31)
                {
                    moisGreenwich = 2;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    anneeGreenwich = heureLocale.Year - 1;
                    moisGreenwich = 12;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de février
            if (heureLocale.Month == 2)
            {
                if (((AB == 0) && (jourGreenwich > 28)) || ((AB == 1) && (jourGreenwich > 29)))
                {
                    moisGreenwich = 3;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 1;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de mars
            if (heureLocale.Month == 3)
            {
                if (jourGreenwich > 31)
                {
                    moisGreenwich = 4;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 2;
                    if (AB == 0)
                    {
                        jourGreenwich = 28;
                    }
                    if (AB == 1)
                    {
                        moisGreenwich = 29;
                    }
                }
            }
            // Cas du mois de avril
            if (heureLocale.Month == 4)
            {
                if (jourGreenwich > 30)
                {
                    moisGreenwich = 5;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 3;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de mai
            if (heureLocale.Month == 5)
            {
                if (jourGreenwich > 31)
                {
                    moisGreenwich = 6;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 4;
                    jourGreenwich = 30;
                }
            }
            // Cas du mois de juin
            if (heureLocale.Month == 6)
            {
                if (jourGreenwich > 30)
                {
                    moisGreenwich = 7;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 5;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de juillet
            if (heureLocale.Month == 7)
            {
                if (jourGreenwich > 31)
                {
                    moisGreenwich = 8;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 6;
                    jourGreenwich = 30;
                }
            }
            // Cas du mois de août
            if (heureLocale.Month == 8)
            {
                if (jourGreenwich > 31)
                {
                    moisGreenwich = 9;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 7;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de septembre
            if (heureLocale.Month == 9)
            {
                if (jourGreenwich > 30)
                {
                    moisGreenwich = 10;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 8;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de octobre
            if (heureLocale.Month == 10)
            {
                if (jourGreenwich > 31)
                {
                    moisGreenwich = 11;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 9;
                    jourGreenwich = 30;
                }
            }
            // Cas du mois de novembre
            if (heureLocale.Month == 11)
            {
                if (jourGreenwich > 30)
                {
                    moisGreenwich = 12;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 10;
                    jourGreenwich = 31;
                }
            }
            // Cas du mois de décembre
            if (heureLocale.Month == 12)
            {
                if (jourGreenwich > 31)
                {
                    anneeGreenwich = anneeGreenwich + 1;
                    moisGreenwich = 1;
                    jourGreenwich = 1;
                }
                if (jourGreenwich < 1)
                {
                    moisGreenwich = 11;
                    jourGreenwich = 30;
                }
            }

            // Affectation de la date et de l'heure à Greenwich
            this.heureGreenwich = new DateTime(anneeGreenwich, moisGreenwich, jourGreenwich, heureGreenwich, heureLocale.Minute, heureLocale.Second, heureLocale.Millisecond);
        }

        // ALGORITHME 3
        /// <summary>
        /// Calcule le Jour Julien des Ephémérides et le Jour Julien à 0h TU pour la date et l'heure à Greenwich.
        /// </summary>
        private void CalculerJourJulien()
        {
            // Déclaration des variables de la méthode
            int a;      // variable pour l'année
            int m;      // variable pour le mois
            double j;   // variable pour le jour en décimale
            int A, B;   // variables pour tenir compte du calendrier Grégorien
            long C, D;  // variables de calcul

            // Calcul du jour incluant l’heure en décimale
            j = (double)heureGreenwich.Day + ((double)heureGreenwich.Hour + (double)heureGreenwich.Minute / 60 + ((double)heureGreenwich.Second + (double)heureGreenwich.Millisecond / 1000) / 3600) / 24;

            // Détermination des variables a, m et dt
            a = heureGreenwich.Year;
            m = heureGreenwich.Month;
            if (heureGreenwich.Month < 3)
            {
                a = heureGreenwich.Year - 1;
                m = heureGreenwich.Month + 12;
            }

            // Calcul des paramètres A, B, C et D
            A = (int)(a / 100); // partie entière de a/100 par typage des données
            B = 0;
            if ((heureGreenwich.Year > 1582) || ((heureGreenwich.Year == 1582) && (heureGreenwich.Month > 10)) || ((heureGreenwich.Year == 1582) && (heureGreenwich.Month == 10) && (heureGreenwich.Day >= 15)))
            {
                B = 2 - A + (A / 4); // partie entière de A/4 par typage des données
            }
            C = (long)(365.25 * a); // partie entière de 365.25*a par typage des données
            if (a < 0)
            {
                C = (long)(365.25 * a - 0.75); // partie entière de 365.25*a-0.75 par typage des données
            }
            D = (long)(30.6001 * (m + 1)); // partie entière de 30.6001*(m+1) par typage des données

            //Calcul du Jour Julien des Ephémérides
            jourJulienEphemerides = 1720994.5 + j + (double)(B + C + D) + (double)(deltaT) / 86400.0;
            jourJulien0h = 1720994.5 + (double)(heureGreenwich.Day + B + C + D);
        }
        
        // Algorithme 4
        /// <summary>
        /// Calcule l’heure sidérale vraie à Greenwich à partir de l’heure TU, du Jour Julien donné à 0h TU, de l’obliquité moyenne et des corrections en nutation
        /// </summary>
        private void CalculerHeureSideraleVraieAGreenwich()
        {
            // Déclaration des variables de la méthode
            double T, T0, A, GSTm;      // variables de calcul

            // Calcul du nombre de siècles depuis le 1,5 janvier 2000
            T = (jourJulien0h - 2451545.0) / 36525.0;

            // Calcul de l'heure sidérale moyenne à Greenwhich à 0h TU
            T0 = 6.6973745583 + 2400.0513369072 * T + 0.0000258622 * T * T - (1 / 580645161.0) * T * T * T;
            T0 = Maths.Modulo(T0, TypeAngle.ANGLE_HEURES_24);

            // Calcul du paramètre A
            A = 1.00273790935 * ((double)heureGreenwich.Hour + (double)heureGreenwich.Minute / 60.0 + ((double)heureGreenwich.Second + (double)heureGreenwich.Millisecond / 1000) / 3600.0);

            // Calcul de l'heure sidérale moyenne à Greenwich
            GSTm = T0 + A;
            GSTm = Maths.Modulo(GSTm, TypeAngle.ANGLE_HEURES_24);

            // Calcul de l'heure sidérale vraie à Greenwich
            double heureSiderale = GSTm + (NutationLongitude * Math.Cos(Maths.DegToRad(obliquiteMoyenne + nutationObliquite))) / 15.0;
            heureSiderale = Maths.Modulo(heureSiderale, TypeAngle.ANGLE_HEURES_24);
            int heureSideraleHeure = (int)heureSiderale;
            int heureSideraleMinute = (int)(60.0 * (heureSiderale - (double)heureSideraleHeure));
            double heureSideraleSeconde = 60.0 * ((60.0 * (heureSiderale - (double)heureSideraleHeure)) - (double)heureSideraleMinute);
            int millisecond = (int)(1000 * (Math.Round(heureSideraleSeconde, 3) - (int)heureSideraleSeconde));
            if (millisecond < 0)
            {
                millisecond = 0;
            }
            else if (millisecond > 999)
            {
                millisecond = 999;
            }
            heureSideraleGreenwich = new DateTime(heureGreenwich.Year, heureGreenwich.Month, heureGreenwich.Day, heureSideraleHeure, heureSideraleMinute, (int)heureSideraleSeconde, millisecond);
        }

        // Algorithme 6
        /// <summary>
        /// Calcule l’heure sidérale vraie locale à partir de l’heure sidérale vraie à Greenwich et de la longitude du lieu d’observation
        /// </summary>
        void CalculerHeureSideraleVraieLocale()
        {
            // Calcul de l'heure sidérale vraie locale
            double heureSiderale = Maths.CalculerHeureDecimale(heureSideraleGreenwich) + lieuObservation.Longitude.Decimale / 15.0;
            heureSiderale = Maths.Modulo(heureSiderale, TypeAngle.ANGLE_HEURES_24);
            int heureSideraleHeure = (int)heureSiderale;
            int heureSideraleMinute = (int)(60.0 * (heureSiderale - (double)heureSideraleHeure));
            double heureSideraleSeconde = 60.0 * ((60.0 * (heureSiderale - (double)heureSideraleHeure)) - (double)heureSideraleMinute);
            int millisecond = (int)(1000 * (Math.Round(heureSideraleSeconde, 3) - (int)heureSideraleSeconde));
            if (millisecond < 0)
            {
                millisecond = 0;
            }
            else if (millisecond > 999)
            {
                millisecond = 999;
            }
            heureSideraleLocale = new DateTime(heureLocale.Year, heureLocale.Month, heureLocale.Day, heureSideraleHeure, heureSideraleMinute, (int)heureSideraleSeconde, millisecond);
        }
 
        // ALGORITHME 9
        /// <summary>
        /// Calcule l’obliquité moyenne de l’écliptique à partir du Jour Julien des Ephémérides.
        /// </summary>
        private void CalculerObliquiteMoyenne()
        {
            // Déclaration des variables de la méthode
            double T, DE;   // variables de calcul

            // Calcul du nombre de siècles écoulés depuis le 1,5 janvier 2000
            T = (JourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de l'obliquité moyenne
            DE = 46.8150 * T + 0.00059 * T * T - 0.001813 * T * T * T; // DE est en secondes d'arc
            DE = DE / 3600; // DE est en degrés
            obliquiteMoyenne = 23.4392911 - DE;
        }
        
        // ALGORITHME 10P
        /// <summary>
        /// Calcule la nutation de la Terre à partir du Jour Julien des Ephémérides (JJE).
        /// </summary>
        void CalculerNutation()
        {
            // Déclaration des variables de la méthode
            double T;                       // variable de calcul
            double Dl, Ms, Ml, Fl, Ol;      // variable de calcul
            int i;                          // variable pour les boucles de calcul

            // Calcul du nombre de siècles écoulés depuis le 1,5 janvier 2000
            T = (jourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de l'élongation moyenne de la Lune au Soleil Dl
            Dl = 297.85036 + 445267.111480 * T - 0.0019142 * T * T + (1 / 189474) * T * T * T;
            Dl = Maths.Modulo(Dl, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'anomalie moyenne du Soleil Ms
            Ms = 357.52722 + 35999.050340 * T - 0.0001603 * T * T - (1 / 300000) * T * T * T;
            Ms = Maths.Modulo(Ms, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'anomalie moyenne de la Lune Ml
            Ml = 134.96298 + 477198.867398 * T + 0.0086972 * T * T - (1 / 56250) * T * T * T;
            Ml = Maths.Modulo(Ml, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'argument de la latitude de la Lune Fl
            Fl = 93.27191 + 483202.017538 * T - 0.0036825 * T * T + (1 / 327270) * T * T * T;
            Fl = Maths.Modulo(Fl, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de la longitude du noeud ascendant de l'orbite moyenne de la Lune
            Ol = 125.04452 - 1934.136261 * T + 0.0020708 * T * T + (1 / 450000) * T * T * T;
            Ol = Maths.Modulo(Ol, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de la nutation en longitude écliptique
            nutationLongitude = 0.0; // Initialisation de la nutation en longitude écliptique à 0
            for (i = 0; i < NutationAIU1980.INDEX_MAX_COEFFICIENT_BP; i++)
            {
                nutationLongitude += ((NutationAIU1980.COEFFICIENT_NUTATION[i, 5] / 10000.0) + (NutationAIU1980.COEFFICIENT_NUTATION[i, 6] * T / 10000.0))
                                       * Math.Sin(Maths.DegToRad(NutationAIU1980.COEFFICIENT_NUTATION[i, 0] * Dl + NutationAIU1980.COEFFICIENT_NUTATION[i, 1] * Ms
                                       + NutationAIU1980.COEFFICIENT_NUTATION[i, 2] * Ml + NutationAIU1980.COEFFICIENT_NUTATION[i, 3] * Fl
                                       + NutationAIU1980.COEFFICIENT_NUTATION[i, 4] * Ol));
            }
            nutationLongitude /= 3600.0;

            // Calcul de la nutation en obliquité
            nutationObliquite = 0.0; // Initialisation de la nutation en obliquité à 0
            for (i = 0; i < NutationAIU1980.INDEX_MAX_COEFFICIENT_BP; i++)
            {
                nutationObliquite += ((NutationAIU1980.COEFFICIENT_NUTATION[i, 7] / 10000.0) + (NutationAIU1980.COEFFICIENT_NUTATION[i, 8] * T / 10000.0))
                                       * Math.Cos(Maths.DegToRad(NutationAIU1980.COEFFICIENT_NUTATION[i, 0] * Dl + NutationAIU1980.COEFFICIENT_NUTATION[i, 1] * Ms
                                       + NutationAIU1980.COEFFICIENT_NUTATION[i, 2] * Ml + NutationAIU1980.COEFFICIENT_NUTATION[i, 3] * Fl
                                       + NutationAIU1980.COEFFICIENT_NUTATION[i, 4] * Ol));
            }
            nutationObliquite /= 3600.0;
        }
    }
}