/****************************************************************************************************************************
 * Classe Comete
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       En cours
 * 
 * La classe Comete permet de représenter les comètes élliptiques et les comètes paraboliques.
 * Cette classe intègre les algorithmes 3, 17, 20, 31, 32, 36, 37 et 38.
 * 
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class Comete : CorpsSystemeSolaire
    {
        // FIELDS PRIVES
        private ParametresOrbitaux parametresOrbitaux; // Paramètres orbitaux de la comète
        private double demiGrandAxe; // Demi grand axe de la comète en UA
        private Angle anomalieMoyenne; // Anomalie moyenne de la comète en degré (uniquement pour une comète périodique e < 1.0)
        private Angle anomalieVraie; // Anomalie vraie de la comète en degré
        private double periodeOrbitale; // Période Orbitale de la comète en année (uniquement pour une comète périodique e < 1.0)
        private double jourJulienEphemeridePP; // Jour Julien des Ephémérides de la date de passage au périhélie de la comète
        private double deltaLambda2; // Correction de la longitude écliptique géocentrique géométrique rapportée à l'équinoxe due à la précession
        private double deltaBeta2; // Correction de la latitude écliptique géocentrique géométrique rapportée à l'équinoxe due à la précession
        private double deltaLambda4; // Correction de la longitude écliptique géocentrique géométrique rapportée à l'aberration de la lumière
        private double deltaBeta4; // Correction de la latitude écliptique géocentrique géométrique rapportée à l'aberration de la lumière

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Paramètres orbitaux de la comète.
        /// </summary>
        public ParametresOrbitaux ParametresOrbitaux
        {
            get { return parametresOrbitaux; }
        }
        /// <summary>
        /// Demi grand axe de la comète en UA.
        /// </summary>
        public double DemiGrandAxe
        {
            get { return demiGrandAxe; }
        }
        /// <summary>
        /// Anomalie moyenne de la comète en degré (uniquement pour une comète périodique e inférieur à 1.0).
        /// </summary>
        public Angle AnomalieMoyenne
        {
            get { return anomalieMoyenne; }
        }
        /// <summary>
        /// Anomalie vraie de la comète en degré.
        /// </summary>
        public Angle AnomalieVraie
        {
            get { return anomalieVraie; }
        }
        /// <summary>
        /// Période Orbitale de la comète en année (uniquement pour une comète périodique e inférieur à 1.0).
        /// </summary>
        public double PeriodeOrbitale
        {
            get { return periodeOrbitale; }
        }
        /// <summary>
        /// Jour Julien des Ephémérides de la date de passage au périhélie de la comète.
        /// </summary>
        public double JourJulienEphemeridePP
        {
            get { return jourJulienEphemeridePP; }
        }
        /// <summary>
        /// Correction de la longitude écliptique géocentrique géométrique rapportée à l'équinoxe due à la précession.
        /// </summary>
        public double DeltaLambda2
        {
            get { return deltaLambda2; }
        }
        /// <summary>
        /// Correction de la latitude écliptique géocentrique géométrique rapportée à l'équinoxe due à la précession.
        /// </summary>
        public double DeltaBeta2
        {
            get { return deltaBeta2; }
        }
        /// <summary>
        /// Correction de la longitude écliptique géocentrique géométrique rapportée à l'aberration de la lumière.
        /// </summary>
        public double DeltaLambda4
        {
            get { return deltaLambda4; }
        }
        /// <summary>
        /// Correction de la latitude écliptique géocentrique géométrique rapportée à l'aberration de la lumière.
        /// </summary>
        public double DeltaBeta4
        {
            get { return deltaBeta4; }
        }

        // CONSTRUCTEUR
        public Comete(ParametresOrbitaux a_parametresOrbitaux) : base(a_parametresOrbitaux.Nom, (a_parametresOrbitaux.Exentricite < 1) ? TypeCorpsCeleste.COMETE_PERIODIQUE : TypeCorpsCeleste.COMETE_PARABOLIQUE)
        {
            parametresOrbitaux = a_parametresOrbitaux;
            CalculerJourJulienEphemeridesPP();
            if(type == TypeCorpsCeleste.COMETE_PERIODIQUE)
            {
                CalculerPeriodeOrbitale();
            }
        }

        // METHODES PUBLIQUES
        /// <summary>
        /// Calcule les paramètres d'une comète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher. Les calculs nécessitent un objet Soleil qui est automatiquement
        /// créé par la méthode.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul)
        {
            Soleil soleil = new Soleil();
            soleil.CalculerNonIteratif(a_lieuEtDateCalcul);
            CalculerNonIteratif(a_lieuEtDateCalcul, soleil);
        }

        /// <summary>
        /// Calcule les paramètres d'une comète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            CalculerNonIteratif(a_lieuEtDateCalcul, a_terreSoleil, 0);
        }

        /// <summary>
        /// Calcule les paramètres d'une comète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// de façon itérative jusqu'à convergence des heures de lever et de coucher.
        /// Les calculs nécessitent un objet Soleil qui est automatiquement créé par la méthode.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <returns>Résultat du succès (true) ou de l'échec (false) du calcul de convergence des heures de lever et de coucher.</returns>
        public bool CalculerIteratif(PositionTemps a_lieuEtDateCalcul)
        {
            Soleil soleil = new Soleil();
            soleil.CalculerNonIteratif(a_lieuEtDateCalcul);
            return CalculerIteratif(a_lieuEtDateCalcul, soleil);
        }

        /// <summary>
        /// Calcule les paramètres d'une comète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// de façon itérative jusqu'à convergence des heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du caluul.</param>
        /// <returns>Résultat du succès (true) ou de l'échec (false) du calcul de convergence des heures de lever et de coucher.</returns>
        public bool CalculerIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            // Déclaration des variables de la méthode
            double heureSideraleVraieLocaleLever; // Heure sidérale vraie locale de lever
            double azimutLever; // Azimut de lever
            double heureSideraleVraieLocaleCoucher; // Heure sidérale vraie locale de coucher
            double azimutCoucher; // Azimut de coucher
            double heureSideraleVraieAGreenwichLever; // Heure sidérale vraie à Greenwich de lever
            double heureSideraleVraieAGreenwichCoucher; // Heure sidérale vraie à Greenwich de coucher
            double heureTULever; // Heure TU de lever
            double heureTUCoucher; // Heure TU de coucher  
            DateTime heureLocaleLever = new DateTime(); // Heure locale de lever
            DateTime heureLocaleCoucher = new DateTime(); // Heure locale de coucher
            TimeSpan precision = new TimeSpan(); // variable de contrôle de la convergence
            PositionTemps dateTemporaireCalcul = null; // Objet PositionTemps pour le calcul en temps différé
            Soleil terreSoleil = null; // Objet Soleil pour le calcul en temps différé des paramètres du Soleil
            int compteur; // Variable de contrôle de la convergence du calcul
            bool succes = true;

            // Calcul initial des paramètres de la comète
            CalculerNonIteratif(a_lieuEtDateCalcul, a_terreSoleil);

            // Sauvegarde des heures de lever et de coucher
            heureSideraleVraieLocaleLever = this.heureSideraleVraieLocaleLever;
            azimutLever = this.azimutLever;
            heureSideraleVraieLocaleCoucher = this.heureSideraleVraieLocaleCoucher;
            azimutCoucher = this.azimutCoucher;
            heureSideraleVraieAGreenwichLever = this.heureSideraleVraieAGreenwichLever;
            heureSideraleVraieAGreenwichCoucher = this.heureSideraleVraieAGreenwichCoucher;
            heureTULever = this.heureTULever;
            heureTUCoucher = this.heureTUCoucher;
            heureLocaleLever = this.heureLocaleLever;
            heureLocaleCoucher = this.heureLocaleCoucher;

            // Création et initialisation d'un nouvel objet PositionTemps pour calculer les heures de lever et de coucher en temps différé
            dateTemporaireCalcul = new PositionTemps(a_lieuEtDateCalcul.LieuObservation, a_lieuEtDateCalcul.HeureLocale.Year, a_lieuEtDateCalcul.HeureLocale.Month, a_lieuEtDateCalcul.HeureLocale.Day, heureLocaleLever.Hour, heureLocaleLever.Minute, 0, a_lieuEtDateCalcul.ZoneHoraire, a_lieuEtDateCalcul.ChangementHeure, a_lieuEtDateCalcul.DeltaT);

            // Calcul itératif pour l'heure de lever de la comète
            compteur = 0;
            do
            {
                // Paramétrage de l'objet dateTemporaireCalcul avec l'heure de lever précédemment calculée
                dateTemporaireCalcul.HeureLocale = heureLocaleLever;
                // Calcul de dateTemporaireCalcul avec les nouveaux réglages
                dateTemporaireCalcul.CalculerParametres();

                // Création et initialisation d'un nouvel objet Soleil pour calculer les heures de lever et de coucher en temps différé
                terreSoleil = new Soleil();
                terreSoleil.CalculerNonIteratif(dateTemporaireCalcul);

                // Calcul des paramètres de la planète à l'instant différé
                CalculerNonIteratif(dateTemporaireCalcul, terreSoleil);

                // Au-delà de 6 itérations si le calcul n'a pas convergé, la méthode force la 2ème solution TU en ajoutant +23h56min04s si l'heure TU est compris entre 0h TU et 03h3min56s
                if (compteur >= 6)
                {
                    CalculerNonIteratif(dateTemporaireCalcul, terreSoleil, 1);
                }

                // Calcul de la précision atteinte                
                precision = this.heureLocaleLever - heureLocaleLever;

                // Détection d'une absence de solution (cas d'un lever du corps céleste proche de minuit et oscillant autour de minuit)
                if (precision > TimeSpan.FromHours(20))
                {
                    this.azimutLever = TOUJOURS_INVISIBLE;
                    this.heureSideraleVraieLocaleLever = TOUJOURS_INVISIBLE;
                    this.heureSideraleVraieAGreenwichLever = TOUJOURS_INVISIBLE;
                    this.heureTULever = TOUJOURS_INVISIBLE;
                    this.heureLocaleLever = new DateTime();
                    precision = TimeSpan.FromSeconds(1);
                }

                // Sauvegarde de la nouvelle heure de lever
                heureSideraleVraieLocaleLever = this.heureSideraleVraieLocaleLever;
                azimutLever = this.azimutLever;
                heureSideraleVraieAGreenwichLever = this.heureSideraleVraieAGreenwichLever;
                heureTULever = this.heureTULever;
                heureLocaleLever = this.heureLocaleLever;

                // Incrémentation du compteur
                compteur++;

                // Au-delà de 10 itérations si le calcul n'a pas convergé, la méthode la boucle est stoppée
                if (compteur == 10)
                {
                    succes = false;
                    precision = TimeSpan.FromSeconds(1);
                }
            } while (precision > TimeSpan.FromSeconds(59.0)); // Précision de 1 minute

            // Calcul itératif pour l'heure de coucher de la comète
            compteur = 0;
            do
            {
                // Paramétrage de l'objet dateTemporaireCalcul avec l'heure de coucher précédemment calculée
                dateTemporaireCalcul.HeureLocale = heureLocaleCoucher;
                // Calcul de dateTemporaireCalcul avec les nouveaux réglages
                dateTemporaireCalcul.CalculerParametres();

                // Création et initialisation d'un nouvel objet Soleil pour calculer les heures de lever et de coucher en temps différé
                terreSoleil = new Soleil();
                terreSoleil.CalculerNonIteratif(dateTemporaireCalcul);

                // Calcul des paramètres de la planète à l'instant différé
                CalculerNonIteratif(dateTemporaireCalcul, terreSoleil);

                // Au-delà de 6 itérations si le calcul n'a pas convergé, la méthode force la 2ème solution TU en ajoutant +23h56min04s si l'heure TU est compris entre 0h TU et 03h3min56s
                if (compteur >= 6)
                {
                    CalculerNonIteratif(dateTemporaireCalcul, terreSoleil, 1);
                }

                // Calcul de la précision atteinte
                precision = this.heureLocaleCoucher - heureLocaleCoucher;

                // Détection d'une absence de solution (cas d'un coucher du corps céleste proche de minuit et oscillant autour de minuit)
                if (precision > TimeSpan.FromHours(20))
                {
                    this.azimutCoucher = TOUJOURS_INVISIBLE;
                    this.heureSideraleVraieLocaleCoucher = TOUJOURS_INVISIBLE;
                    this.heureSideraleVraieAGreenwichCoucher = TOUJOURS_INVISIBLE;
                    this.heureTUCoucher = TOUJOURS_INVISIBLE;
                    this.heureLocaleCoucher = new DateTime();
                    precision = TimeSpan.FromSeconds(1);
                }

                // Sauvegarde de la nouvelle heure de coucher
                heureSideraleVraieLocaleCoucher = this.heureSideraleVraieLocaleCoucher;
                azimutCoucher = this.azimutCoucher;
                heureSideraleVraieAGreenwichCoucher = this.heureSideraleVraieAGreenwichCoucher;
                heureTUCoucher = this.heureTUCoucher;
                heureLocaleCoucher = this.heureLocaleCoucher;

                // Incrémentation du compteur
                compteur++;

                // Au-delà de 10 itérations si le calcul n'a pas convergé, la méthode la boucle est stoppée
                if (compteur == 10)
                {
                    succes = false;
                    precision = TimeSpan.FromSeconds(1);
                }
            } while (precision > TimeSpan.FromSeconds(59.0)); // Précision de 1 minute

            // Calcul des paramètres du Soleil pour la date et l'heure considérée
            CalculerNonIteratif(a_lieuEtDateCalcul, a_terreSoleil);

            // Affectation des heures de lever et de coucher calculées
            this.heureSideraleVraieLocaleLever = heureSideraleVraieLocaleLever;
            this.azimutLever = azimutLever;
            this.heureSideraleVraieLocaleCoucher = heureSideraleVraieLocaleCoucher;
            this.azimutCoucher = azimutCoucher;
            this.heureSideraleVraieAGreenwichLever = heureSideraleVraieAGreenwichLever;
            this.heureSideraleVraieAGreenwichCoucher = heureSideraleVraieAGreenwichCoucher;
            this.heureTULever = heureTULever;
            this.heureTUCoucher = heureTUCoucher;
            this.heureLocaleLever = heureLocaleLever;
            this.heureLocaleCoucher = heureLocaleCoucher;

            return succes;
        }

        // METHODES PRIVEES    
        /// <summary>
        /// Calcule les paramètres d'une planète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        /// <param name="a_essai">Incrémentation de l'heure TU de 23h56min04s lorsque cet argument prend une valeur de 1 pour forcer la 2ème solution possible si l'heure TU est proche de 0h.</param>
        private void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil, int a_essai)
        {
            // Déclaration des variables locales de la méthode
            double distanceCalculPrecedent; // variable de calcul
            double precision;               // variable de calcul

            if (type == TypeCorpsCeleste.COMETE_PERIODIQUE) // Cas d'une comète périodique
            {

                do
                {
                    // Affectation de la distance précédemment calculée à la variable distance_calcul_precedent
                    distanceCalculPrecedent = r;
                    CalculerAnomalieMoyenne(a_lieuEtDateCalcul); // Algorithme 37
                    CalculerAnomalieVraieEtRayonVecteur(); // Algorithme 31
                    CalculerCoordonneesEcliptiqueHeliocentriques(); // Algorithme 32
                    CalculerDistanceALaTerre(a_terreSoleil); // Algorithme 34
                    // Calcul de la précision sur la distance de la comète
                    precision = r - distanceCalculPrecedent;
                }
                while (precision > 0.0000000001 || precision < -0.0000000001);
            }
            else // Cas d'une comète périodique
            {
                do
                {
                    // Affectation de la distance précédemment calculée à la variable distance_calcul_precedent
                    distanceCalculPrecedent = r;
                    CalculerAnomalieVraieEtRayonVecteurParabolique(a_lieuEtDateCalcul); // Algorithme 38
                    CalculerCoordonneesEcliptiqueHeliocentriques(); // Algorithme 32
                    CalculerDistanceALaTerre(a_terreSoleil); // Algorithme 34
                    // Calcul de la précision sur la distance de la comète
                    precision = r - distanceCalculPrecedent;
                }
                while (precision > 0.0000000001 || precision < -0.0000000001);
            }

            CalculerCoordonneesEcliptiquesGeocentriquesGeometriques(a_terreSoleil); // Algorithme 15
            CalculerPrecessionEcliptiques(a_lieuEtDateCalcul); // Algorithme 17
            CalculerCorrectionsCoordonneesEcliptiquesAberration(a_lieuEtDateCalcul, a_terreSoleil); // Algorithme 20
            CalculerCoordonneesEcliptiquesGeocentriques(a_lieuEtDateCalcul, deltaLambda4, deltaBeta4, deltaLambda2, deltaBeta2); // Algorithme 22
            ConvertirCoordonneesEcliptiquesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 12
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, true); // Algorithme 13
            CalculerRefractionAtmospherique(a_lieuEtDateCalcul); // Algorithme 24
            ConvertirCoordonneesHorizontalesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 14
            CalculerCorrectionCoordonneesEquatorialesRefraction(); // Algorithme 25
            CalculerCorrectionCoordonneesEquatorialesParallaxe(a_lieuEtDateCalcul); // Algorithme 26
            CalculerCoordonneesEquatorialesTopocentriques(); // Algorithme 27
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, false); // Algorithme 13
            CalculerHeuresSideralesEtAzimutsLeverCoucher(a_lieuEtDateCalcul, r, 0); // Algorithme 28
            CalculerHeuresSideralesVraiesAGreenwichLeverCoucher(a_lieuEtDateCalcul); // Algorithme 7
            CalculerHeuresTULeverCoucher(a_lieuEtDateCalcul, a_essai); // Algorithme 5
            CalculerHeuresLocalesLeverCoucher(a_lieuEtDateCalcul); // Algorithme 2
        }

        // ***** ALGORITHME 3 *****
        /// <summary>
        /// Calcule le Jour Julien des Ephémérides pour une date et une heure à Greenwich correspondant à l'instant de passage d'une comète périodique au périhélie
        /// </summary>
        private void CalculerJourJulienEphemeridesPP()
        {
            // Déclaration des variables de la méthode
            int a;  // variable pour l'année
            int m;  // variable pour le mois
            double j;   // variable pour le jour en décimale
            int A, B;   // variables pour tenir compte du calendrier Grégorien
            long C, D;  // variables de calcul

            // Détermination des variables j, a, m et dt
            j = (double)parametresOrbitaux.DatePassagePerihelie.Day + ((double)parametresOrbitaux.DatePassagePerihelie.Hour + (double)parametresOrbitaux.DatePassagePerihelie.Minute / 60 + ((double)parametresOrbitaux.DatePassagePerihelie.Second + (double)parametresOrbitaux.DatePassagePerihelie.Millisecond / 1000) / 3600) / 24;
            a = parametresOrbitaux.DatePassagePerihelie.Year;
            m = parametresOrbitaux.DatePassagePerihelie.Month;
            if (m < 3)
            {
                a = a - 1;
                m = m + 12;
            }

            // Calcul des paramètres A, B, C et D
            A = a / 100; // partie entière de a/100 par typage des données
            B = 0;
            if ((parametresOrbitaux.DatePassagePerihelie.Year > 1582) || ((parametresOrbitaux.DatePassagePerihelie.Year == 1582)
                && (parametresOrbitaux.DatePassagePerihelie.Month > 10)) || (parametresOrbitaux.DatePassagePerihelie.Year == 1582)
                && (parametresOrbitaux.DatePassagePerihelie.Month == 10) && (parametresOrbitaux.DatePassagePerihelie.Day >= 15))
            {
                B = 2 - A + (A / 4); // partie entière de A/4 par typage des données
            }
            C = (long)(365.25 * a); // partie entière de 365.25*a par typage des données
            if (a < 0)
            {
                C = (long)(365.25 * a - 0.75); // partie entière de 365.25*a-0.75 par typage des données
            }
            D = (long)(30.6001 * (m + 1)); // partie entière de 30.6001*(m+1) par typage des données

            // Calcul du Jour Julien des Ephémérides de la date du passage au périhélie de la comète
            jourJulienEphemeridePP = 1720994.5 + j + (double)(B) + (double)(C) + (double)(D) + ((double)parametresOrbitaux.DeltaT / 86400.0);
        }

        // ***** ALGORITHME 17 *****
        /// <summary>
        /// Calcule les corrections dues à la précession à apporter sur les coordonnées écliptiques géocentriques géométriques d’un corps céleste rapportées
        /// à l’équinoxe J2000 à partir du Jour Julien des Ephémérides correspondant à la sate et l’heure considérées et des coordonnées écliptiques géocentriques
        /// géométriques du corps céleste rapportées à l’équinoxe J2000.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        private void CalculerPrecessionEcliptiques(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double T, eta, pi, p, A, B, C;      // variables de calcul

            // Calcul du nombre de siècles depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de l'angle eta
            eta = 47.0029 * T - 0.03302 * T * T + 0.000060 * T * T * T; // angle en secondes d'arc
            eta = eta / 3600.0; // angle en degrés

            // Calcul de l'angle pi
            pi = 174.876384 - 869.8089 * T + 0.03536 * T * T; // angle en secondes d'arc
            pi = pi / 3600.0; // angle en degrés

            // Calcul de l'angle p
            p = 5029.0966 * T + 1.11113 * T * T - 0.000006 * T * T * T; // angle en secondes d'arc
            p = p / 3600.0; // angle en degrés

            // Calcul de l'angle A
            A = Math.Cos(Maths.DegToRad(eta)) * Math.Cos(Maths.DegToRad(betaGeometrique)) * Math.Sin(Maths.DegToRad(pi - lambdaGeometrique)) - Math.Sin(Maths.DegToRad(eta)) * Math.Sin(Maths.DegToRad(betaGeometrique));

            // Calcul de l'angle B
            B = Math.Cos(Maths.DegToRad(betaGeometrique)) * Math.Cos(Maths.DegToRad(pi - lambdaGeometrique));

            // Calcul de l'angle C
            C = Math.Cos(Maths.DegToRad(eta)) * Math.Sin(Maths.DegToRad(betaGeometrique)) + Math.Sin(Maths.DegToRad(eta)) * Math.Cos(Maths.DegToRad(BetaGeometrique)) * Math.Sin(Maths.DegToRad(pi - lambdaGeometrique));

            // Calcul de la correction à apporter sur la longitude écliptique géocentrique géométrique rapportée à l'équinoxe J2000
            deltaLambda2 = Maths.Modulo(p + pi - Maths.RadToDeg(Math.Atan2(A, B)), TypeAngle.ANGLE_DEGRES_360) - lambdaGeometrique;

            // Calcul de la correction à apporter sur la latitude écliptique géocentrique géométrique rapportée à l'équinoxe J2000
            deltaBeta2 = Maths.RadToDeg(Math.Asin(C)) - betaGeometrique;
        }

        // ***** ALGORITHME 20 *****
        /// <summary>
        /// Calcule les corrections dues à l’aberration de la lumière à apporter sur les coordonnées écliptiques géocentriques géométriques d’une comète à partir du Jour Julien
        /// des Ephémérides correspondant à la date et l’heure considérées, de la longitude écliptique géocentrique géométrique du Soleil et des coordonnées écliptiques géocentriques
        /// géométriques de la comète. 
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet représentant la Terre/le Soleil.</param>
        private void CalculerCorrectionsCoordonneesEcliptiquesAberration(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            // Déclaration des variables de la méthode
            double T, et, pit; // variables de calcul

            // Calcul du nombre de siècles depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de l'excentricité de l'orbite de la Terre
            et = 0.016708617 - 0.000042037 * T - 0.0000001236 * T * T + 0.00000000004 * T * T * T;

            // Calcul de la longitude du perihélie de la Terre
            pit = 102.937348 + 1.7195269 * T + 0.00045962 * T * T + 0.000000499 * T * T * T;

            // Calcul de la correction à apporter sur la longitude écliptique géocentrique géométrique de la comète
            deltaLambda4 = (-20.49552 * Math.Cos(Maths.DegToRad(a_terreSoleil.LambdaGeometrique - (lambdaGeometrique + deltaLambda2 + a_lieuEtDateCalcul.NutationLongitude))) + 20.49552 * et * Math.Cos(Maths.DegToRad(pit - (lambdaGeometrique + deltaLambda2 + a_lieuEtDateCalcul.NutationLongitude)))) / Math.Cos(Maths.DegToRad(betaGeometrique + deltaBeta2));
            deltaLambda4 = deltaLambda4 / 3600.0;

            // Calcul de la correction à apporter sur la latitude écliptique géocentrique géométrique de la comète
            deltaBeta4 = -20.49552 * Math.Sin(Maths.DegToRad(betaGeometrique + deltaBeta2)) * (Math.Sin(Maths.DegToRad(a_terreSoleil.LambdaGeometrique - (lambdaGeometrique + deltaLambda2 + a_lieuEtDateCalcul.NutationLongitude))) - et * Math.Sin(Maths.DegToRad(pit - (lambdaGeometrique + deltaLambda2 + a_lieuEtDateCalcul.NutationLongitude))));
            deltaBeta4 = deltaBeta4 / 3600.0;
        }

        // ***** ALGORITHME 31 *****
        /// <summary>
        /// Calcule l’anomalie vraie et le rayon vecteur des comètes périodiques rapportés à l’équinoxe J2000 à partir de l’anomalie moyenne
        /// des comètes, ainsi que de l’excentricité et du périhélie des orbites des comètes.
        /// </summary>
        private void CalculerAnomalieVraieEtRayonVecteur()
        {
            // Déclaration des variables de la méthode
            double E; // variable de calcul

            // Calcul de l'anomalie excentrique en radians
            E = Maths.CalculerAnomalieExcentriqueAvecKelper(anomalieMoyenne.Decimale, parametresOrbitaux.Exentricite);

            // Calcul de l'anomalie vraie
            anomalieVraie = new Angle(2 * Maths.RadToDeg(Math.Atan(Math.Sqrt((1 + parametresOrbitaux.Exentricite) / (1 - parametresOrbitaux.Exentricite)) * Math.Tan(E / 2))), TypeAngle.ANGLE_DEGRES_360);

            // Calcul du rayon vecteur
            rSoleil = (parametresOrbitaux.Perihelie / (1 - parametresOrbitaux.Exentricite)) * ((1 - parametresOrbitaux.Exentricite
                      * parametresOrbitaux.Exentricite) / (1 + parametresOrbitaux.Exentricite * Math.Cos(Maths.DegToRad(anomalieVraie.Decimale))));
        }

        // ***** ALGORITHME 32 *****
        /// <summary>
        /// Calcule les coordonnées écliptiques héliocentriques d’une comète rapportées à l’équinoxe J2000 à partir des paramètres orbitaux de la comète.
        /// </summary>
        private void CalculerCoordonneesEcliptiqueHeliocentriques()
        {
            // Déclaration des variables de la méthode
            double longitudeVraie, x, y; // variable de calcul

            // Calcul due la longitude vraie
            longitudeVraie = anomalieVraie.Decimale + parametresOrbitaux.ArgumentPerihelie.Decimale + parametresOrbitaux.LongitudeNoeudAscendant.Decimale;

            // Calcul de la latitude écliptique à l'équinoxe J2000
            b = Maths.RadToDeg(Math.Asin(Math.Sin(Maths.DegToRad(longitudeVraie - parametresOrbitaux.LongitudeNoeudAscendant.Decimale)) * Math.Sin(Maths.DegToRad(parametresOrbitaux.Inclinaison.Decimale))));

            // Calcul de la longitude écliptique à l'équinoxe J2000
            y = Math.Sin(Maths.DegToRad(longitudeVraie - parametresOrbitaux.LongitudeNoeudAscendant.Decimale)) * Math.Cos(Maths.DegToRad(parametresOrbitaux.Inclinaison.Decimale));
            x = Math.Cos(Maths.DegToRad(longitudeVraie - parametresOrbitaux.LongitudeNoeudAscendant.Decimale));
            l = Maths.RadToDeg(Math.Atan2(y, x)) + parametresOrbitaux.LongitudeNoeudAscendant.Decimale;
            l = Maths.Modulo(l, TypeAngle.ANGLE_DEGRES_360);
        }

        // ***** ALGORITHME 36 *****
        /// <summary>
        /// Calcule la période orbitale d'une comète périodique à partir du périhélie et de l’excentricité de l’orbite de la comète
        /// </summary>
        private void CalculerPeriodeOrbitale()
        {
            // Calcul du demi-grand axe de la comète périodique
            demiGrandAxe = (149598500.0 * parametresOrbitaux.Perihelie) / (1 - parametresOrbitaux.Exentricite);

            // Calcul de la période orbitale
            periodeOrbitale = 2 * Math.PI * Math.Pow(demiGrandAxe, 1.5) / Math.Sqrt(1.327e11); // période orbitale en s
            periodeOrbitale = periodeOrbitale / 31556925.3; // période orbitale en années
        }

        // ***** ALGORITHME 37 *****
        /// <summary>
        /// Calcule l’anomalie moyenne de la comète périodique à partir du Jour Julien des Ephémérides correspondant à la date et l’heure considérées,
        /// de la distance de la comète à la Terre, du Jour Julien des Ephémérides du passage de la comète au périhélie et de la période orbitale de la comète.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        private void CalculerAnomalieMoyenne(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double D; // variable de calcul

            // Calcul du nombre de jours écoulés depuis le passage de la comète au périhélie
            D = (a_lieuEtDateCalcul.JourJulienEphemerides - 0.0057755183 * r) - jourJulienEphemeridePP;

            // Calcul de l'anomalie moyenne de la comète
            anomalieMoyenne =  new Angle((360.0 / 365.242191) * (D / periodeOrbitale), TypeAngle.ANGLE_DEGRES_360);
        }

        // ***** ALGORITHME 38 *****
        /// <summary>
        /// Calcule l’anomalie vraie et le rayon vecteur d’une comète non périodique rapportés à l’équinoxe J2000 à partir du Jour Julien des Ephémérides
        /// correspondant à la date et l’heure considérées, de la distance de la comète à la Terre, du Jour Julien des Ephémérides du passage de la
        /// comète au périhélie et du périhélie de la comète.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        private void CalculerAnomalieVraieEtRayonVecteurParabolique(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double D, W, s; // variables de calcul
            double precision; // variable de calcul

            // Calcul du nombre de jours écoulés depuis le passage de la comète au périhélie
            D = (a_lieuEtDateCalcul.JourJulienEphemerides - 0.0057755183 * r) - jourJulienEphemeridePP;

            // Calcul du paramètre W
            W = (0.03649116245 * D) / (parametresOrbitaux.Perihelie * Math.Sqrt(parametresOrbitaux.Perihelie)); // W est en radians

            // Calcul du paramètre s (s*s*s +3s -W = 0)
            s = W / 3.0;
            do
            {
                s = (2 * s * s * s + W) / (3 * (s * s + 1));
                precision = s * s * s + 3 * s - W;
            }
            while (precision > 0.0000000001 || precision < -0.0000000001);

            // Calcul de l'anomalie vraie de la comète rapportée à l'équinoxe J2000
            anomalieVraie = new Angle(Maths.RadToDeg(2 * Math.Atan(s)), TypeAngle.ANGLE_DEGRES_360);

            // Calcul du rayon vecteur de la comète
            rSoleil = parametresOrbitaux.Perihelie * (1 + s * s);
        }
    }
}