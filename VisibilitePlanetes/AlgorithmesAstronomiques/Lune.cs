/****************************************************************************************************************************
 * Classe Lune
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe Lune permet de représenter la Lune.
 * Cette classe intègre les algorithmes 40P et 41.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;
using AlgorithmesAstronomiques.DonneesTheories;

namespace AlgorithmesAstronomiques
{
    public class Lune : CorpsSystemeSolaire
    {
        // FIELDS PRIVES
        private double phase; // Phase
        private Angle taille; // Taille apparente

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Phase de la Lune.
        /// </summary>
        public double Phase
        {
            get { return phase; }
        }
        /// <summary>
        /// Taille apparente de la Lune.
        /// </summary>
        public Angle Taille
        {
            get { return taille; }
        }

        // CONSTRUCTEUR
        public Lune() : base("Lune", TypeCorpsCeleste.LUNE)
        {   
        }

        // METHODES PUBLIQUES
        /// <summary>
        /// Calcule les paramètres de la Lune pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher. Les calculs nécessitent un objet Soleil qui est automatiquement
        /// créé par la méthode.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul"></param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul)
        {
            Soleil soleil = new Soleil();
            soleil.CalculerNonIteratif(a_lieuEtDateCalcul);
            CalculerNonIteratif(a_lieuEtDateCalcul, soleil, 0);
        }

        /// <summary>
        /// Calcule les paramètres de la Lune pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du caluul.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil)
        {
            CalculerNonIteratif(a_lieuEtDateCalcul, a_terreSoleil, 0);
        }

        /// <summary>
        /// Calcule les paramètres de la Lune pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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
        /// Calcule les paramètres de la Lune pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
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
        /// Calcule les paramètres de la Lune pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_terreSoleil">Objet Soleil calculé pour le lieu d'observation et la date du calcul.</param>
        /// <param name="a_essai">Incrémentation de l'heure TU de 23h56min04s lorsque cet argument prend une valeur de 1 pour forcer la 2ème solution possible si l'heure TU est proche de 0h.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul, CorpsSystemeSolaire a_terreSoleil, int a_essai)
        {
            CalculerCoordoonneesEcliptiquesEtRayonVecteur(a_lieuEtDateCalcul); // Algorithme 40
            ConvertirCoordonneesEcliptiquesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 12
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, true); // Algorithme 13
            CalculerRefractionAtmospherique(a_lieuEtDateCalcul); // Algorithme 24
            ConvertirCoordonneesHorizontalesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 14
            CalculerCorrectionCoordonneesEquatorialesRefraction(); // Algorithme 25
            CalculerCorrectionCoordonneesEquatorialesParallaxe(a_lieuEtDateCalcul); // Algorithme 26
            CalculerCoordonneesEquatorialesTopocentriques(); // Algorithme 27
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, false); // Algorithme 13
            CalculerPhaseEtTailleApparenteLune(a_terreSoleil); // Algorithme 41
            CalculerHeuresSideralesEtAzimutsLeverCoucher(a_lieuEtDateCalcul, r, taille.Decimale); // Algorithme 28
            CalculerHeuresSideralesVraiesAGreenwichLeverCoucher(a_lieuEtDateCalcul); // Algorithme 7
            CalculerHeuresTULeverCoucher(a_lieuEtDateCalcul, 0); // Algorithme 5
            CalculerHeuresLocalesLeverCoucher(a_lieuEtDateCalcul); // Algorithme 2            
        }

        // ***** ALGORITHME 40P *****
        private void CalculerCoordoonneesEcliptiquesEtRayonVecteur(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double T, ll, Dl, Ms, Ml, Fl, A1, A2, A3, e;    // variable de calcul
            int i;  // variable de comptage pour les boucles de calcul
            double argument, somme_l = 0.0, somme_b = 0.0, somme_r = 0.0;   // variable de calcul

            // Calcul du nombre de siècles écoulés depuis le 1,5 janvier 2000
            T = (a_lieuEtDateCalcul.JourJulienEphemerides - 2451545.0) / 36525.0;

            // Calcul de la longitude moyenne de la Lune ll
            ll = 218.316654361 + 481267.88134240 * T - 0.00132675 * T * T + (1 / 538841) * T * T * T - (1 / 65193770) * T * T * T * T;
            ll = Maths.Modulo(ll, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'élongation moyenne de la Lune Dl
            Dl = 297.85020420 + 445267.11151675 * T - 0.001630028 * T * T + (1 / 545868) * T * T * T - (1 / 113065327) * T * T * T * T;
            Dl = Maths.Modulo(Dl, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'anomalie moyenne du Soleil Ms
            Ms = 357.52910918 + 35999.05029094 * T - 0.000153583 * T * T + (1 / 24489796) * T * T * T;
            Ms = Maths.Modulo(Ms, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'anomalie moyenne de la Lune Ml
            Ml = 134.96341138 + 477198.86763133 * T + 0.008997028 * T * T + (1 / 69699) * T * T * T - (1 / 14711892) * T * T * T * T;
            Ml = Maths.Modulo(Ml, TypeAngle.ANGLE_DEGRES_360);

            // Calcul de l'argument de la latitude moyenne de la Lune Fl
            Fl = 93.27209932 + 483202.01752731 * T - 0.003402917 * T * T - (1 / 3525955) * T * T * T + (1 / 863309353) * T * T * T * T;
            Fl = Maths.Modulo(Fl, TypeAngle.ANGLE_DEGRES_360);

            // Calcul du paramètre correcteur A1 dû à Vénus
            A1 = 119.75 + 131.849 * T;
            A1 = Maths.Modulo(A1, TypeAngle.ANGLE_DEGRES_360);

            // Calcul du paramètre correcteur A2 dû à Jupiter
            A2 = 53.09 + 479264.290 * T;
            A2 = Maths.Modulo(A2, TypeAngle.ANGLE_DEGRES_360);

            // Calcul du paramètre correcteur additionnel A3
            A3 = 313.45 + 481266.484 * T;
            A3 = Maths.Modulo(A3, TypeAngle.ANGLE_DEGRES_360);

            // Calcul du paramètre correcteur e dû à l'excentricité de la Terre
            e = 1 - 0.002516 * T - 0.0000074 * T * T;

            // Calcul de la longitude écliptique géométrique et géocentrique de la Lune
            for (i = 0; i < ELP2000_82B_BP.INDEX_MAX_COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP; i++)
            {
                argument = Maths.DegToRad(ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,0] * Dl + ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] * Ms + ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,2] * Ml + ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,3] * Fl);
                if (ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == 0)
                {
                    somme_l += ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,4] * Math.Sin(argument);
                }
                if (ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == -1 || ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == 1)
                {
                    somme_l += ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,4] * e * Math.Sin(argument);
                }
                if (ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == -2 || ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == 2)
                {
                    somme_l += ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i, 4] * e * e * Math.Sin(argument);
                }
            }
            somme_l += 3958 * Math.Sin(Maths.DegToRad(A1)) + 1962 * Math.Sin(Maths.DegToRad(ll - Fl)) + 318 * Math.Sin(Maths.DegToRad(A2));
            lambdaGeometrique = ll + (somme_l / 1000000.0);
            lambdaGeometrique = Maths.Modulo(lambdaGeometrique, TypeAngle.ANGLE_DEGRES_360);
            lambda = new Angle((lambdaGeometrique + a_lieuEtDateCalcul.NutationLongitude), TypeAngle.ANGLE_DEGRES_360);

            // Calcul de la latitude écliptique géométrique et géocentrique de la Lune
            for (i = 0; i < ELP2000_82B_BP.INDEX_MAX_COEFFICIENT_LATITUDE_LUNE_BP; i++)
            {
                argument = Maths.DegToRad(ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,0] * Dl + ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,1] * Ms + ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,2] * Ml + ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,3] * Fl);
                if (ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,1] == 0)
                {
                    somme_b += ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,4] * Math.Sin(argument);
                }
                if (ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,1] == -1 || ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,1] == 1)
                {
                    somme_b += ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,4] * e * Math.Sin(argument);
                }
                if (ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,1] == -2 || ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,1] == 2)
                {
                    somme_b += ELP2000_82B_BP.COEFFICIENT_LATITUDE_LUNE_BP[i,4] * e * e * Math.Sin(argument);
                }
            }
            somme_b += -2235 * Math.Sin(Maths.DegToRad(ll)) + 382 * Math.Sin(Maths.DegToRad(A3)) + 175 * Math.Sin(Maths.DegToRad(A1 - Fl)) + 127 * Math.Sin(Maths.DegToRad(ll - Ml)) - 115 * Math.Sin(Maths.DegToRad(ll + Ml));
            betaGeometrique = (somme_b / 1000000.0);
            beta = new Angle(betaGeometrique, TypeAngle.ANGLE_DEGRES_90);

            // Calcul du rayon vecteur de la Lune
            for (i = 0; i < ELP2000_82B_BP.INDEX_MAX_COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP; i++)
            {
                argument = Maths.DegToRad(ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,0] * Dl + ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] * Ms + ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,2] * Ml + ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,3] * Fl);
                if (ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == 0)
                {
                    somme_r += ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,5] * Math.Cos(argument);
                }
                if (ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == -1 || ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == 1)
                {
                    somme_r += ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,5] * e * Math.Cos(argument);
                }
                if (ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == -2 || ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,1] == 2)
                {
                    somme_r += ELP2000_82B_BP.COEFFICIENT_LONGITUDE_RAYON_VECTEUR_LUNE_BP[i,5] * e * e * Math.Cos(argument);
                }
            }
            r = 385000.56 + (somme_r / 1000.0);
        }

        // ***** ALGORITHME 41 *****
        private void CalculerPhaseEtTailleApparenteLune(CorpsSystemeSolaire a_terreSoleil)
        {
            // Déclaration des variables de la méthode
            double elongation, x, y;    // variable de calcul
            double angle_phase, taille_centre_lune; // variable de calcul

            // Calcul de l'élongation de la Lune au Soleil
            elongation = Maths.RadToDeg(Math.Acos(Math.Cos(Maths.DegToRad(beta.Decimale)) * Math.Cos(Maths.DegToRad(lambdaGeometrique - a_terreSoleil.LambdaGeometrique))));

            // Calcul de l'angle de phase
            y = 149598500 * a_terreSoleil.R * Math.Sin(Maths.DegToRad(elongation));
            x = r - 149598500 * a_terreSoleil.R * Math.Cos(Maths.DegToRad(elongation));
            angle_phase = Maths.RadToDeg(Math.Atan2(y, x));

            // Calcul de la phase
            phase = (1.0 + Math.Cos(Maths.DegToRad(angle_phase))) / 2.0;

            // Calcul de la taille apparente de la Lune vu du centre de la Terre
            taille_centre_lune = 2 * Maths.RadToDeg(Math.Asin(1737.92196534 / r));

            // Calcul de la taille apparente de la Lune vu du lieu d'observation
            taille = new Angle(taille_centre_lune * (1 + Math.Sin(Maths.DegToRad(altitudeTopocentrique.Decimale)) * (6378.14 / r)), TypeAngle.ANGLE_DEGRES_360);
        }
    }
}