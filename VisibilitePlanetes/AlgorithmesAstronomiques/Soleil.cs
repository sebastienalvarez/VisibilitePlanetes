/****************************************************************************************************************************
 * Classe Soleil
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe Soleil permet de représenter le Soleil.
 * Cette classe intègre l'algorithme 29.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class Soleil : CorpsSystemeSolaire
    {
        // FIELDS PRIVES
        private Angle taille; // Taille apparente

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Taille apparente du Soleil.
        /// </summary>
        public Angle Taille
        {
            get { return taille; }
        }

        // CONSTRUCTEUR
        public Soleil() : base("Soleil", TypeCorpsCeleste.TERRE_SOLEIL)
        {
        }

        // METHODES PUBLIQUES
        /// <summary>
        /// Calcule les paramètres de la Terre-Soleil pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// une seule fois sans calcul itératif de convergence pour les heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        public void CalculerNonIteratif(PositionTemps a_lieuEtDateCalcul)
        {
            CalculerCoordonneesEcliptiquesEtRayonVecteur(a_lieuEtDateCalcul, TypeCorpsCeleste.TERRE_SOLEIL); // Algorithme 8
            ConvertirCoordonneesEcliptiquesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 12
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, true); // Algorithme 13
            CalculerRefractionAtmospherique(a_lieuEtDateCalcul); // Algorithme 24
            ConvertirCoordonneesHorizontalesVersEquatoriales(a_lieuEtDateCalcul); // Algorithme 14
            CalculerCorrectionCoordonneesEquatorialesRefraction(); // Algorithme 25
            CalculerCorrectionCoordonneesEquatorialesParallaxe(a_lieuEtDateCalcul); // Algorithme 26
            CalculerCoordonneesEquatorialesTopocentriques(); // Algorithme 27
            ConvertirCoordonneesEquatorialesVersHorizontales(a_lieuEtDateCalcul, false); // Algorithme 13
            CalculerTailleApparente(); // Algorithme 29
            CalculerHeuresSideralesEtAzimutsLeverCoucher(a_lieuEtDateCalcul, r, taille.Decimale); // Algorithme 28
            CalculerHeuresSideralesVraiesAGreenwichLeverCoucher(a_lieuEtDateCalcul); // Algorithme 7
            CalculerHeuresTULeverCoucher(a_lieuEtDateCalcul, 0); // Algorithme 5
            CalculerHeuresLocalesLeverCoucher(a_lieuEtDateCalcul); // Algorithme 2
        }

        /// <summary>
        /// Calcule les paramètres de la Terre-Soleil pour le lieu d'observation et la date de calcul spécifié. La méthode effectue tous les calculs du synoptique
        /// de façon itérative jusqu'à convergence des heures de lever et de coucher.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        public void CalculerIteratif(PositionTemps a_lieuEtDateCalcul)
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

            // Calcul initial des paramètres du Soleil
            CalculerNonIteratif(a_lieuEtDateCalcul);

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

            // Calcul itératif pour l'heure de lever du Soleil
            do
            {
                // Paramétrage de l'objet dateTemporaireCalcul avec l'heure de lever précédemment calculée
                dateTemporaireCalcul.HeureLocale = heureLocaleLever;
                // Calcul de dateTemporaireCalcul avec les nouveaux réglages
                dateTemporaireCalcul.CalculerParametres();

                // Calcul des paramètres du Soleil à l'instant différé
                CalculerNonIteratif(dateTemporaireCalcul);

                // Calcul de la précision atteinte                
                precision = this.heureLocaleLever - heureLocaleLever;

                // Sauvegarde de la nouvelle heure de lever
                heureSideraleVraieLocaleLever = this.heureSideraleVraieLocaleLever;
                azimutLever = this.azimutLever;
                heureSideraleVraieAGreenwichLever = this.heureSideraleVraieAGreenwichLever;
                heureTULever = this.heureTULever;
                heureLocaleLever = this.heureLocaleLever;
            } while (precision > TimeSpan.FromSeconds(59.0)); // Précision de 1 minute

            // Calcul itératif pour l'heure de coucher du Soleil
            do
            {
                // Paramétrage de l'objet dateTemporaireCalcul avec l'heure de coucher précédemment calculée
                dateTemporaireCalcul.HeureLocale = heureLocaleCoucher;
                // Calcul de dateTemporaireCalcul avec les nouveaux réglages
                dateTemporaireCalcul.CalculerParametres();

                // Calcul des paramètres du Soleil à l'instant différé
                CalculerNonIteratif(dateTemporaireCalcul);

                // Calcul de la précision atteinte
                precision = this.heureLocaleCoucher - heureLocaleCoucher;

                // Sauvegarde de la nouvelle heure de coucher
                heureSideraleVraieLocaleCoucher = this.heureSideraleVraieLocaleCoucher;
                azimutCoucher = this.azimutCoucher;
                heureSideraleVraieAGreenwichCoucher = this.heureSideraleVraieAGreenwichCoucher;
                heureTUCoucher = this.heureTUCoucher;
                heureLocaleCoucher = this.heureLocaleCoucher;
            } while (precision > TimeSpan.FromSeconds(59.0)); // Précision de 1 minute

            // Calcul des paramètres du Soleil pour la date et l'heure considérée
            CalculerNonIteratif(a_lieuEtDateCalcul);
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
        }

        // METHODES PRIVEES
        // Algorithme 29
        /// <summary>
        /// Calcule la taille apparente du Soleil à partir de la distance du Soleil à la Terre
        /// </summary>
        private void CalculerTailleApparente()
        {
            // Calcul de la taille apparente du Soleil
            taille = new Angle(1919.26 / (3600.0 * r), TypeAngle.ANGLE_DEGRES_360);
        }
    }
}