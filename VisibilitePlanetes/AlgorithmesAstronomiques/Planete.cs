/****************************************************************************************************************************
 * Classe Planete
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe Planete permet de représenter une planète.
 * Cette classe intègre les algorithmes 20 et 35.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class Planete : CorpsSystemeSolaire
    {
        // FIELDS PRIVES
        private double deltaLambda4; // Correction de la longitude écliptique géométrique due à l'aberration de la lumière
        private double deltaBeta4; // Correction de la latitude écliptique géométrique due à l'aberration de la lumière
        private double phase; // Phase
        private Angle taille; // Taille apparente
        private double magnitude; // Magnitude apparente sans extinction atmosphérique
        private double deltaMagnitude; // Variation de magnitude due à l'extinction atmosphérique

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Correction de la longitude écliptique géométrique de la planète due à l'aberration de la lumière.
        /// </summary>
        public double DeltaLambda4
        {
            get { return deltaLambda4; }
        }
        /// <summary>
        /// Correction de la latitude écliptique géométrique de la planète due à l'aberration de la lumière.
        /// </summary>
        public double DeltaBeta4
        {
            get { return deltaBeta4; }
        }
        /// <summary>
        /// Phase de la planète.
        /// </summary>
        public double Phase
        {
            get { return phase; }
        }
        /// <summary>
        /// Taille apparente de la planète.
        /// </summary>
        public Angle Taille
        {
            get { return taille; }
        }
        /// <summary>
        /// Magnitude apparente de la planète sans extinction atmosphérique.
        /// </summary>
        public double Magnitude
        {
            get { return magnitude; }
        }
        /// <summary>
        /// Variation de magnitude de la planète due à l'extinction atmosphérique.
        /// </summary>
        public double DeltaMagnitude
        {
            get { return deltaMagnitude; }
        }

        // CONSTRUCTEUR
        public Planete(string a_nom, TypeCorpsCeleste a_type) : base(a_nom, a_type)
        {
        }

        // METHODES PUBLIQUES
        /// <summary>
        /// Calcule les paramètres d'une planète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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
        /// Calcule les paramètres d'une planète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            CalculerNonIteratif(a_lieuEtDateCalcul, a_terreSoleil, 0);
        }

        /// <summary>
        /// Calcule les paramètres d'une planète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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
        /// Calcule les paramètres d'une planète pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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

            // Calcul initial des paramètres de la planète
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

            // Calcul itératif pour l'heure de lever de la planète
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

            // Calcul itératif pour l'heure de coucher de la planète
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

            do
            {
                distanceCalculPrecedent = r;
                CalculerCoordonneesEcliptiquesEtRayonVecteur(a_lieuEtDateCalcul, type); // Algorithme 33
                CalculerDistanceALaTerre(a_terreSoleil); // Algorithme 34
                precision = r - distanceCalculPrecedent;
            } while (precision > 0.0000000001 || precision < -0.0000000001);


            CalculerCoordonneesEcliptiquesGeocentriquesGeometriques(a_terreSoleil); // Algorithme 15
            CalculerCorrectionsCoordonneesEcliptiquesAberration(a_lieuEtDateCalcul, a_terreSoleil); // Algorithme 20
            CalculerCoordonneesEcliptiquesGeocentriques(a_lieuEtDateCalcul, deltaLambda4, deltaBeta4); // Algorithme 22
            ConvertirCoordonneesEcliptiquesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 12
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, true); // Algorithme 13
            CalculerRefractionAtmospherique(a_lieuEtDateCalcul); // Algorithme 24
            ConvertirCoordonneesHorizontalesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 14
            CalculerCorrectionCoordonneesEquatorialesRefraction(); // Algorithme 25
            CalculerCorrectionCoordonneesEquatorialesParallaxe(a_lieuEtDateCalcul); // Algorithme 26
            CalculerCoordonneesEquatorialesTopocentriques(); // Algorithme 27
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, false); // Algorithme 13
            CalculerTaillePhaseMagnitude(a_lieuEtDateCalcul, a_terreSoleil); // Algorithme 35
            CalculerHeuresSideralesEtAzimutsLeverCoucher(a_lieuEtDateCalcul, r, taille.Decimale); // Algorithme 28
            CalculerHeuresSideralesVraiesAGreenwichLeverCoucher(a_lieuEtDateCalcul); // Algorithme 7
            CalculerHeuresTULeverCoucher(a_lieuEtDateCalcul, a_essai); // Algorithme 5
            CalculerHeuresLocalesLeverCoucher(a_lieuEtDateCalcul); // Algorithme 2
        }
        
        // Algorithme 20
        /// <summary>
        /// Calcule les corrections dues à l’aberration de la lumière à apporter sur les coordonnées écliptiques géocentriques géométriques d’une planète
        /// ou d’une comète à partir du Jour Julien des Ephémérides, de la longitude écliptique géocentrique géométrique du Soleil et des coordonnées écliptiques
        /// géocentriques géométriques de la planète ou de la comète.
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

            // Calcul de la correction à apporter sur la longitude écliptique géocentrique géométrique de la planète ou de la comète
            deltaLambda4 = (-20.49552 * Math.Cos(Maths.DegToRad(a_terreSoleil.LambdaGeometrique - lambdaGeometrique)) + 20.49552 * et * Math.Cos(Maths.DegToRad(pit - lambdaGeometrique))) / Math.Cos(Maths.DegToRad(betaGeometrique));

            // Calcul de la correction à apporter sur la latitude écliptique géocentrique géométrique de la planète ou de la comète
            deltaBeta4 = -20.49552 * Math.Sin(Maths.DegToRad(betaGeometrique)) * (Math.Sin(Maths.DegToRad(a_terreSoleil.LambdaGeometrique - lambdaGeometrique)) - et * Math.Sin(Maths.DegToRad(pit - lambdaGeometrique)));
            deltaBeta4 /= 3600.0;
        }

        // Algorithme 35
        /// <summary>
        /// Calcule la taille apparente, la phase et la magnitude apparente des planètes à partir de la distance des planètes à la Terre,
        /// du rayon vecteur des planètes, de la taille apparente des planètes à 1 UA, de l’altitude topocentrique des planètes et du
        /// rayon vecteur de la Terre.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet représentant la Terre/le Soleil.</param>
        private void CalculerTaillePhaseMagnitude(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            // Déclaration des variables de la méthode
            double i, z;                                                                 // variable de calcul
            double[] taille_1UA = { 6.72, 16.82, 9.36, 196.88, 165.40, 70.04, 67.00 };   // taille apparente des planètes à 1 UA

            // Calcul de l'angle de phase (en radians)
            i = Math.Acos((rSoleil * rSoleil + r * r - a_terreSoleil.R * a_terreSoleil.R) / (2 * rSoleil * r));

            // Calcul de la phase
            phase = (1 + Math.Cos(i)) / 2;

            // Conversion de l'angle de phase en degrés
            i = Maths.RadToDeg(i);

            // Cas de Mercure
            if (type == TypeCorpsCeleste.MERCURE)
            {
                taille = new Angle((float)(taille_1UA[0] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[0] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -0.42 + 5 * Math.Log10(rSoleil * r) + 0.0380 * i - 0.000273 * i * i + 0.000002 * i * i * i;
            }

            // Cas de Vénus
            if (type == TypeCorpsCeleste.VENUS)
            {
                taille = new Angle((float)(taille_1UA[1] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[1] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -4.40 + 5 * Math.Log10(rSoleil * r) + 0.0009 * i + 0.000239 * i * i - 0.00000065 * i * i * i;
            }

            // Cas de Mars
            if (type == TypeCorpsCeleste.MARS)
            {
                taille = new Angle((float)(taille_1UA[2] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[2] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -1.52 + 5 * Math.Log10(rSoleil * r) + 0.016 * i;
            }

            // Cas de Jupiter
            if (type == TypeCorpsCeleste.JUPITER)
            {
                taille = new Angle((float)(taille_1UA[3] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[3] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -9.40 + 5 * Math.Log10(rSoleil * r) + 0.005 * i;
            }

            // Cas de Saturne
            if (type == TypeCorpsCeleste.SATURNE)
            {
                taille = new Angle((float)(taille_1UA[4] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[4] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -8.88 + 5 * Math.Log10(rSoleil * r) + 0.044 * i;
            }

            // Cas de Uranus
            if (type == TypeCorpsCeleste.URANUS)
            {
                taille = new Angle((float)(taille_1UA[5] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[5] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -7.19 + 5 * Math.Log10(rSoleil * r);
            }

            // Cas de Neptune
            if (type == TypeCorpsCeleste.NEPTUNE)
            {
                taille = new Angle((float)(taille_1UA[6] / r)/3600, TypeAngle.ANGLE_DEGRES_360);
                //taille = new Angle(0, 0, (float)(taille_1UA[6] / r), TypeAngle.ANGLE_DEGRES_360);
                magnitude = -6.87 + 5 * Math.Log10(rSoleil * r);
            }

            // Calcul de la baisse de magnitude due à l'extinction atmosphérique
            z = 90.0 - altitudeTopocentrique.Decimale;
            deltaMagnitude = 0.2 / Math.Cos(Maths.DegToRad(z));
        }
    }
}