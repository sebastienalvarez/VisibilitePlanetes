/****************************************************************************************************************************
 * Classe J2000
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe J2000 permet de représenter les objets du ciel profond.
 * Cette classe intègre les algorithmes 16, 18P, 19, 21, 23 et 39.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class J2000 : CorpsCeleste
    {
        // FIELDS PRIVES
        private CielProfond objetCielProfond; // Objet du ciel profond définissant ses coordonnées équatoriales rapportées à l'équinoxe J2000
        private double deltaAlpha1; // Correction de l'ascension droite géocentrique géométrique rapportée à l'équinoxe J2000 due au mouvement propre
        private double deltaDelta1; // Correction de la déclinaison géocentrique géométrique rapportée à l'équinoxe J2000 due au mouvement propre
        private double deltaAlpha2; // Correction de l'ascension droite géocentrique géométrique rapportée à l'équinoxe J2000 due à la précession
        private double deltaDelta2; // Correction de la déclinaison géocentrique géométrique rapportée à l'équinoxe J2000 due au mouvement propre
        private double deltaAlpha3; // Correction de l'ascension droite géocentrique géométrique due à la nutation
        private double deltaDelta3; // Correction de la déclinaison géocentrique géométrique due à la nutation
        private double deltaAlpha4; // Correction de l'ascension droite géocentrique géométrique due à l'aberration de la lumière
        private double deltaDelta4; // Correction de la déclinaison géocentrique géométrique due à l'aberration de la lumière
        private double deltaMagnitude; // Variation de magnitude due à l'extinction atmosphérique

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Objet du ciel profond définissant ses coordonnées équatoriales rapportées à l'équinoxe J2000.
        /// </summary>
        public CielProfond ObjetCielProfond
        {
            get { return objetCielProfond; }
        }
        /// <summary>
        /// Correction de l'ascension droite géocentrique géométrique rapportée à l'équinoxe J2000 due au mouvement propre.
        /// </summary>
        public double DeltaAlpha1
        {
            get { return deltaAlpha1; }
        }
        /// <summary>
        /// Correction de la déclinaison géocentrique géométrique rapportée à l'équinoxe J2000 due au mouvement propre.
        /// </summary>
        public double DeltaDelta1
        {
            get { return deltaDelta1; }
        }
        /// <summary>
        /// Correction de l'ascension droite géocentrique géométrique rapportée à l'équinoxe J2000 due à la précession.
        /// </summary>
        public double DeltaAlpha2
        {
            get { return deltaAlpha2; }
        }
        /// <summary>
        /// Correction de la déclinaison géocentrique géométrique rapportée à l'équinoxe J2000 due au mouvement propre.
        /// </summary>
        public double DeltaDelta2
        {
            get { return deltaDelta2; }
        }
        /// <summary>
        /// Correction de l'ascension droite géocentrique géométrique due à la nutation.
        /// </summary>
        public double DeltaAlpha3
        {
            get { return deltaAlpha3; }
        }
        /// <summary>
        /// Correction de la déclinaison géocentrique géométrique due à la nutation.
        /// </summary>
        public double DeltaDelta3
        {
            get { return deltaDelta3; }
        }
        /// <summary>
        /// Correction de l'ascension droite géocentrique géométrique due à l'aberration de la lumière.
        /// </summary>
        public double DeltaAlpha4
        {
            get { return deltaAlpha4; }
        }
        /// <summary>
        /// Correction de la déclinaison géocentrique géométrique due à l'aberration de la lumière.
        /// </summary>
        public double DeltaDelta4
        {
            get { return deltaDelta4; }
        }
        /// <summary>
        /// Variation de magnitude due à l'extinction atmosphérique.
        /// </summary>
        public double DeltaMagnitude
        {
            get { return deltaMagnitude; }
        }

        // CONSTRUCTEUR
        public J2000(CielProfond a_objetCielProfond) : base(a_objetCielProfond.NomCommun, TypeCorpsCeleste.J2000)
        {
            objetCielProfond = a_objetCielProfond;
        }

        // METHODES PUBLIQUES
        /// Calcule les paramètres d'un objet du ciel profond pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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
        /// Calcule les paramètres d'un objet du ciel profond pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            CalculerNonIteratif(a_lieuEtDateCalcul, a_terreSoleil, 0);
        }

        /// <summary>
        /// Calcule les paramètres d'un objet du ciel profond pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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
        /// Calcule les paramètres d'un objet du ciel profond pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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

            // Calcul initial des paramètres de l'objet du ciel profond
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

            // Calcul itératif pour l'heure de lever de l'objet du ciel profond
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

                // Calcul des paramètres de l'objet du ciel profond à l'instant différé
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

            // Calcul itératif pour l'heure de coucher de l'objet du ciel profond
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
        /// Calcule les paramètres d'un objet du cie profond pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        private void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil, int a_essai)
        {
            CalculerMouvementPropre(a_lieuEtDateCalcul); // Algorithme 16
            CalculerCorrectionCoordonneesEquatorialesPrecession(a_lieuEtDateCalcul); // Algorithme 18P
            CalculerCorrectionCoordonneesEquatorialesNutation(a_lieuEtDateCalcul); // Algorithme 19
            CalculerCorrectionCoordonneesEquatorialesAberration(a_lieuEtDateCalcul, a_terreSoleil); // Algorithme 21
            CalculerCoordonneesEquatorialesGeocentriques(); // Algorithme 23
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, true); // Algorithme 13
            CalculerRefractionAtmospherique(a_lieuEtDateCalcul); // Algorithme 24
            ConvertirCoordonneesHorizontalesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 14
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, false); // Algorithme 13
            CalculerMagnitudeApparente(); // Algorithme 39
            CalculerHeuresSideralesEtAzimutsLeverCoucher(a_lieuEtDateCalcul); // Algorithme 28
            CalculerHeuresSideralesVraiesAGreenwichLeverCoucher(a_lieuEtDateCalcul); // Algorithme 7
            CalculerHeuresTULeverCoucher(a_lieuEtDateCalcul, a_essai); // Algorithme 5
            CalculerHeuresLocalesLeverCoucher(a_lieuEtDateCalcul); // Algorithme 2
        }

        // ***** ALGORITHME 16 *****
        /// <summary>
        /// Calcule les corrections dues au mouvement propre d’un corps céleste à apporter sur les coordonnées équatoriales géocentriques géométriques à l’époque J2000 d’un corps céleste
        /// à partir du Jour Julien des Ephémérides correspondant à la date et l’heure considérées, des mouvements propres annuels du corps céleste en ascension droite et déclinaison et
        /// de la déclinaison géocentrique géométrique du corps céleste à l’époque J2000.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        private void CalculerMouvementPropre(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double T; // variable de calcul

            // Calcul du nombre d'années écoulées depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 2451545.0) / 365.25;

            // Calcul de la correction à apporter sur l'ascension droite géocentrique géométrique à l'époque J2000
            deltaAlpha1 = T * (objetCielProfond.MouvementPropreAlpha / (1000.0 * Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale)))); // correction en s
            deltaAlpha1 = deltaAlpha1 / 3600.0; // correction en h

            // Calcul de la correction à apporter sur la déclinaison géocentrique géométrique à l'époque J2000
            deltaDelta1 = T * (objetCielProfond.MouvementPropreDelta / 1000.0); // correction en s
            deltaDelta1 = deltaDelta1 / 3600.0; // correction en degrés
        }

        // ***** ALGORITHME 18P *****
        /// <summary>
        /// Calcule les corrections dues à la précession à apporter sur les coordonnées équatoriales géocentriques géométriques d’un corps céleste rapportées à l’équinoxe J2000
        /// à partir du Jour Julien des Ephémérides correspondant à la date et l’heure considérées, des coordonnées équatoriales géocentriques géométriques du corps céleste à
        /// l’époque J2000 et des corrections dues au mouvement propre.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        private void CalculerCorrectionCoordonneesEquatorialesPrecession(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthodes
            double T, dzeta, z, teta, A, B, C; // variables de calcul

            // Calcul du nombre de siècles depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de l'angle eta
            dzeta = 2306.2181 * T + 0.30188 * T * T + 0.017998 * T * T * T; // angle en secondes d'arc
            dzeta = dzeta / 3600.0; // angle en degrés

            // Calcul de l'angle pi
            z = 2306.2181 * T + 1.09468 * T * T + 0.018203 * T * T * T; // angle en secondes d'arc
            z = z / 3600.0; // angle en degrés

            // Calcul de l'angle p
            teta = 2004.3109 * T - 0.42665 * T * T - 0.041833 * T * T * T; // angle en secondes d'arc
            teta = teta / 3600.0; // angle en degrés

            // Calcul de l'angle A
            A = Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1)) * Math.Sin(Maths.DegToRad(15.0 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1) + dzeta));

            // Calcul de l'angle B
            B = Math.Cos(Maths.DegToRad(teta)) * Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1)) * Math.Cos(Maths.DegToRad(15.0 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1) + dzeta)) - Math.Sin(Maths.DegToRad(teta)) * Math.Sin(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1));

            // Calcul de l'angle C
            C = Math.Sin(Maths.DegToRad(teta)) * Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1)) * Math.Cos(Maths.DegToRad(15.0 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1) + dzeta)) + Math.Cos(Maths.DegToRad(teta)) * Math.Sin(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1));

            // Calcul de la correction à apporter sur l'ascension droite géocentrique géométrique rapportée à l'équinoxe J2000
            deltaAlpha2 = (Maths.Modulo(Maths.RadToDeg(Math.Atan2(A, B)) + z, TypeAngle.ANGLE_DEGRES_360) / 15.0) - deltaAlpha1 - objetCielProfond.AlphaJ2000.Decimale;

            // Calcul de la correction à apporter sur la déclinaison géocentrique géométrique rapportée à l'équinoxe J2000
            deltaDelta2 = Maths.RadToDeg(Math.Asin(C)) - deltaDelta1 - objetCielProfond.DeltaJ2000.Decimale;
        }

        // ***** ALGORITHME 19 *****
        /// <summary>
        /// Calcule les corrections dues à la nutation à apporter sur les coordonnées équatoriales géocentriques géométriques d’un corps céleste à partir de l’obliquité moyenne,
        /// de la nutation en longitude écliptique, de la nutation en obliquité, des coordonnées équatoriales géocentriques géométriques du corps céleste à l’époque J2000,
        /// des corrections dues au mouvement propre et des corrections dues à la précession.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        private void CalculerCorrectionCoordonneesEquatorialesNutation(PositionTemps a_lieuEtDateCalcul)
        {
            // Calcul de la correction à apporter sur l'ascension droite géométrique
            deltaAlpha3 = (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne)) + Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne)) * Math.Sin(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))
                          * Math.Tan(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2))) * (a_lieuEtDateCalcul.NutationLongitude * 3600.0)
                          - (Math.Cos(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2))) * Math.Tan(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2))) * (a_lieuEtDateCalcul.NutationObliquite * 3600.0); // correction en seconde d'arc de degrés

            deltaAlpha3 = deltaAlpha3 / 54000.0; // correction en heure

            // Calcul de la correction à apporter sur la déclinaison géométrique
            deltaDelta3 = (Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne)) * Math.Cos(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))) * (a_lieuEtDateCalcul.NutationLongitude * 3600.0)
                            + Math.Sin(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2))) * (a_lieuEtDateCalcul.NutationObliquite * 3600.0); // correction en secondes d'arc

            deltaDelta3 = deltaDelta3 / 3600.0; // correction en degrés
        }

        // ***** ALGORITHME 21 *****
        /// <summary>
        /// Calcule les corrections dues à l’aberration de la lumière à apporter sur les coordonnées équatoriales géocentriques géométriques d’un corps céleste à partir du Jour Julien des Ephémérides correspondant
        /// à la date et l’heure considérées, de la longitude écliptique géocentrique géométrique du Soleil, de l’obliquité moyenne, de la nutation en obliquité, des coordonnées équatoriales géocentriques
        /// géométriques du corps céleste à l’époque J2000, des corrections dues au mouvement propre et des corrections dues à la précession.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        private void CalculerCorrectionCoordonneesEquatorialesAberration(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            // Déclaration des variables de la méthode
            double T, et, pit; // variables de calcul

            // Calcul du nombre de siècles depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de l'excentricité de l'orbite de la Terre
            et = 0.016708617 - 0.000042037 * T - 0.0000001236 * T * T + 0.00000000004 * T * T * T;

            // Calcul de la longitude du perihélie de la Terre
            pit = 102.937348 + 1.7195269 * T + 0.00045962 * T * T + 0.000000499 * T * T * T;

            // Calcul de la correction à apporter sur la longitude écliptique géocentrique géométrique
            deltaAlpha4 = -20.49552 * ((Math.Cos(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2))) * Math.Cos(Maths.DegToRad(a_terreSoleil.LambdaGeometrique))
                            * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)) + Math.Sin(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))
                            * Math.Sin(Maths.DegToRad(a_terreSoleil.LambdaGeometrique))) / Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2)))
                            + 20.49552 * et * ((Math.Cos(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2))) * Math.Cos(Maths.DegToRad(pit))
                            * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)) + Math.Sin(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2))) * Math.Sin(Maths.DegToRad(pit)))
                            / Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2)));

            deltaAlpha4 = deltaAlpha4 / 54000.0; // corrections en heure

            // Calcul de la correction à apporter sur la déclinaison géocentrique géométrique
            deltaDelta4 = -20.49552 * (Math.Cos(Maths.DegToRad(a_terreSoleil.LambdaGeometrique)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)) * (Math.Tan(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite))
                            * Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2)) - Math.Sin(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))
                            * Math.Sin(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2))) + Math.Cos(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))
                            * Math.Sin(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2)) * Math.Sin(Maths.DegToRad(a_terreSoleil.LambdaGeometrique)))
                            + 20.49552 * et * (Math.Cos(Maths.DegToRad(pit)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite)) * (Math.Tan(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite))
                            * Math.Cos(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2)) - Math.Sin(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))
                            * Math.Sin(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2))) + Math.Cos(Maths.DegToRad(15 * (objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2)))
                            * Math.Sin(Maths.DegToRad(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2)) * Math.Sin(Maths.DegToRad(pit))); // correction en secondes d'arc

            deltaDelta4 = deltaDelta4 / 3600.0; // corrections en degrés
        }

        // ***** ALGORITHME 23 *****
        /// <summary>
        /// Calcule les coordonnées équatoriales géocentriques d’un corps céleste à partir des différentes corrections (mouvement propre, précession, nutation et aberration de la lumière) et des coordonnées équatoriales
        /// géocentriques géométriques du corps céleste à l’époque J2000.
        /// </summary>
        private void CalculerCoordonneesEquatorialesGeocentriques()
        {
            // Calcul de l'ascension droite géocentrique
            ascensionDroiteGeocentrique = new Angle(objetCielProfond.AlphaJ2000.Decimale + deltaAlpha1 + deltaAlpha2 + deltaAlpha3 + deltaAlpha4, TypeAngle.ANGLE_HEURES_24);

            // Calcul de la déclinaison géocentrique
            declinaisonGeocentrique = new Angle(objetCielProfond.DeltaJ2000.Decimale + deltaDelta1 + deltaDelta2 + deltaDelta3 + deltaDelta4, TypeAngle.ANGLE_DEGRES_90);
        }

        // ***** ALGORITHME 39 *****
        /// <summary>
        /// Calcule la magnitude apparente du corps céleste à partir de la magnitude apparente de référence et de l’altitude topocentrique du corps céleste.
        /// </summary>
        private void CalculerMagnitudeApparente()
        {
            // Calcul de la correction de la magnitude apparente due à l'extinction atmosphérique
            if (altitudeTopocentrique.Decimale > 0.0)
                deltaMagnitude = 0.2 / Math.Cos(Maths.DegToRad(90.0 - altitudeTopocentrique.Decimale)); // Calcul seulement si l'altitude est positive
            else
                deltaMagnitude = 0.0;
        }
    }
}