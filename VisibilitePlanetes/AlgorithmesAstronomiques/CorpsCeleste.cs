/****************************************************************************************************************************
 * Classe CorpsCeleste
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe abstraite CorpsCeleste définit les propriétés communes à tous les corps célestes.
 * Cette classe intègre les algorithmes 2, 5, 7, 13, 14, 24 et 28.
 *   
 ***************************************************************************************************************************/

using System;
using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public abstract class CorpsCeleste
    {
        // FIELDS PRIVES
        protected string nom; // Nom du corps céleste
        protected TypeCorpsCeleste type; // Type de corps céleste
        protected Angle ascensionDroiteGeocentrique; // Ascension droite géocentrique
        protected Angle declinaisonGeocentrique; // Déclinaison géocentrique
        protected Angle ascensionDroiteTopocentrique; // Ascension droite topocentrique
        protected Angle declinaisonTopocentrique; // Déclinaison topocentrique
        protected Angle azimutGeocentrique; // Azimut géocentrique
        protected Angle azimutTopocentrique; //Azimut topocentrique
        protected Angle altitudeGeocentrique; // Altitude géocentrique
        protected Angle altitudeTopocentrique; // Altitude topocentrique
        protected double heureSideraleVraieLocaleLever; // Heure sidérale vraie locale de lever
        protected double heureSideraleVraieAGreenwichLever; // Heure sidérale vraie à Greenwich de lever
        protected double heureTULever; // Heure TU de lever
        protected double azimutLever; // Azimut de lever
        protected double heureSideraleVraieLocaleCoucher; // Heure sidérale vraie locale de coucher
        protected double heureSideraleVraieAGreenwichCoucher; // Heure sidérale vraie à Greenwich de coucher
        protected double heureTUCoucher; // Heure TU de coucher
        protected double azimutCoucher; // Azimut de coucher
        protected DateTime heureLocaleLever; // Heure locale de lever
        protected DateTime heureLocaleCoucher; // Heure locale de coucher
        protected const double TOUJOURS_VISIBLE = 1000.0; // Constante utilisée pour indiquer qu'un corps céleste est toujours visible
        protected const double TOUJOURS_INVISIBLE = -1000.0; // Constante utilisée pour indiquer qu'un corps céleste est toujours invisible

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Nom du corps céleste.
        /// </summary>
        public String Nom
        {
            get { return nom; }
        }
        /// <summary>
        /// Type de corps céleste.
        /// </summary>
        public TypeCorpsCeleste Type
        {
            get { return type; }
        }
        /// <summary>
        /// Ascension droite géocentrique du corps céleste.
        /// </summary>
        public Angle AscensionDroiteGeocentrique
        {
            get { return ascensionDroiteGeocentrique; }
        }
        /// <summary>
        /// Déclinaison géocentrique du corps céleste.
        /// </summary>
        public Angle DeclinaisonGeocentrique
        {
            get { return declinaisonGeocentrique; }
        }
        /// <summary>
        /// Ascension droite topocentrique du corps céleste.
        /// </summary>
        public Angle AscensionDroiteTopocentrique
        {
            get { return ascensionDroiteTopocentrique; }
        }
        /// <summary>
        /// Déclinaison topocentrique du corps céleste.
        /// </summary>
        public Angle DeclinaisonTopocentrique
        {
            get { return declinaisonTopocentrique; }
        }
        /// <summary>
        /// Azimut géocentrique du corps céleste.
        /// </summary>
        public Angle AzimutGeocentrique
        {
            get { return azimutGeocentrique; }
        }
        /// <summary>
        /// Azimut topocentrique du corps céleste.
        /// </summary>
        public Angle AzimutTopocentrique
        {
            get { return azimutTopocentrique; }
        }
        /// <summary>
        /// Altitude géocentrique du corps céleste.
        /// </summary>
        public Angle AltitudeGeocentrique
        {
            get { return altitudeGeocentrique; }
        }
        /// <summary>
        /// Altitude topocentrique du corps céleste.
        /// </summary>
        public Angle AltitudeTopocentrique
        {
            get { return altitudeTopocentrique; }
        }
        /// <summary>
        /// Heure siderale vraie locale du lever du corps céleste en décimale.
        /// </summary>
        public double HeureSideraleVraieLocaleLever
        {
            get { return heureSideraleVraieLocaleLever; }
        }
        /// <summary>
        /// Heure siderale vraie à Greenwich du lever du corps céleste en décimale.
        /// </summary>
        public double HeureSideraleVraieAGreenwichLever
        {
            get { return heureSideraleVraieAGreenwichLever; }
        }
        /// <summary>
        /// Heure TU du lever du corps céleste en décimale.
        /// </summary>
        public double HeureTULever
        {
            get { return heureTULever; }
        }
        /// <summary>
        /// Azimut du lever du corps céleste.
        /// </summary>
        public double AzimutLever
        {
            get { return azimutLever; }
        }
        /// <summary>
        /// Heure siderale vraie locale du coucher du corps céleste en décimale.
        /// </summary>
        public double HeureSideraleVraieLocaleCoucher
        {
            get { return heureSideraleVraieLocaleCoucher; }
        }
        /// <summary>
        /// Heure siderale vraie à Greenwich du coucher du corps céleste en décimale.
        /// </summary>
        public double HeureSideraleVraieAGreenwichCoucher
        {
            get { return heureSideraleVraieAGreenwichCoucher; }
        }
        /// <summary>
        /// Heure TU du coucher du corps céleste en décimale.
        /// </summary>
        public double HeureTUCoucher
        {
            get { return heureTUCoucher; }
        }
        /// <summary>
        /// Azimut du coucher du corps céleste.
        /// </summary>
        public double AzimutCoucher
        {
            get { return azimutCoucher; }
        }
        /// <summary>
        /// Heure locale du lever du corps céleste.
        /// </summary>
        public DateTime HeureLocaleLever
        {
            get { return heureLocaleLever; }
        }
        /// <summary>
        /// Heure locale du coucher du corps céleste.
        /// </summary>
        public DateTime HeureLocaleCoucher
        {
            get { return heureLocaleCoucher; }
        }

        // CONSTRUCTEURS
        public CorpsCeleste(string a_nom, TypeCorpsCeleste a_type)
        {
            nom = a_nom;
            type = a_type;
        }

        // METHODES PUBLIQUES
        //Aucune

        // METHODES PROTEGEES
        // Algorithme 2
        /// <summary>
        /// Calcule l’heure locale du point d’observation à partir de la date et de l’heure à Greenwich, c’est-à-dire la date
        /// et l’heure TU, et du décalage horaire dû au fuseau horaire (en France +1h) et aux économies d’énergie liées
        /// à l’heure d’été et à l’heure d’hiver (en France +1h en été et 0h en hiver).
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        protected void CalculerHeuresLocalesLeverCoucher(PositionTemps a_lieuEtDateCalcul)
        {
            // Calcul de l'heure locale de lever
            double heureCalculee = heureTULever + (double)a_lieuEtDateCalcul.ZoneHoraire + (double)a_lieuEtDateCalcul.ChangementHeure;
            heureCalculee = Maths.Modulo(heureCalculee, TypeAngle.ANGLE_HEURES_24);
            int nombreHeure = (int)heureCalculee;
            int nombreMinute = (int)Math.Round((60.0 * (heureCalculee - (double)nombreHeure)), 0);
            if (nombreMinute == 60)
            {
                nombreMinute = 0;
                nombreHeure += 1;
                if (nombreHeure == 24)
                {
                    nombreHeure = 0;
                }
            }
            heureLocaleLever = new DateTime(a_lieuEtDateCalcul.HeureLocale.Year, a_lieuEtDateCalcul.HeureLocale.Month, a_lieuEtDateCalcul.HeureLocale.Day, nombreHeure, nombreMinute, 0);

            // Calcul de l'heure locale de coucher
            heureCalculee = heureTUCoucher + (double)a_lieuEtDateCalcul.ZoneHoraire + (double)a_lieuEtDateCalcul.ChangementHeure;
            heureCalculee = Maths.Modulo(heureCalculee, TypeAngle.ANGLE_HEURES_24);
            nombreHeure = (int)heureCalculee;
            nombreMinute = (int)Math.Round((60.0 * (heureCalculee - (double)nombreHeure)), 0);
            if (nombreMinute == 60)
            {
                nombreMinute = 0;
                nombreHeure += 1;
                if (nombreHeure == 24)
                {
                    nombreHeure = 0;
                }
            }
            heureLocaleCoucher = new DateTime(a_lieuEtDateCalcul.HeureLocale.Year, a_lieuEtDateCalcul.HeureLocale.Month, a_lieuEtDateCalcul.HeureLocale.Day, nombreHeure, nombreMinute, 0);
        }

        // Algorithme 5
        /// <summary>
        /// Calcule l’heure à Greenwich (heure TU) correspondant à une heure sidérale vraie à Greenwich à une date donnée par le Jour Julien à 0h TU
        /// à partir de l’obliquité moyenne et des corrections en nutation
        /// Si TU est proche de 0h TU, l'argument a_essai à 1 permet d'essayer d'incrémenter le temps TU de 23h56min04s.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_essai">Incrémentation de l'heure TU de 23h56min04s lorsque cet argument prend une valeur de 1 pour forcer la 2ème solution possible si l'heure TU est proche de 0h.</param>
        protected void CalculerHeuresTULeverCoucher(PositionTemps a_lieuEtDateCalcul, int a_essai)
        {
            // Déclaration des variables de la méthode
            double GSTm, T, T0, A; // variables de calcul

            // Calcul de l'heure sidérale moyenne à Greenwich du lever
            GSTm = HeureSideraleVraieAGreenwichLever - ((a_lieuEtDateCalcul.NutationLongitude * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite))) / 15.0);
            GSTm = Maths.Modulo(GSTm, TypeAngle.ANGLE_HEURES_24);

            // Calcul de l'heure sidérale moyenne à Greenwich à 0h TU
            T = (a_lieuEtDateCalcul.JourJulien0h - 2451545.0) / 36525.0; // Calcul du nombre de siècles depuis le 1,5 janvier 2000
            T0 = 6.6973745583 + 2400.0513369072 * T + 0.0000258622 * T * T - (1 / 580645161.0) * T * T * T;
            T0 = Maths.Modulo(T0, TypeAngle.ANGLE_HEURES_24);

            // Calcul de l'heure à Greenwich du lever
            A = GSTm - T0;
            A = Maths.Modulo(A, TypeAngle.ANGLE_HEURES_24);
            heureTULever = A * 0.9972695663;

            // Cas où la convergence du calcul itératif du lever à échoué (la fonction appelante passe alors l'argument essai à 1)
            if ((heureTULever < ((3.0 / 60.0) + (56.0 / 3600.0))) && (a_essai == 1))
            {
                heureTULever = heureTULever + (23.0 + (56.0 / 60.0) + (4.0 / 3600.0));
                heureTULever = Maths.Modulo(heureTULever, TypeAngle.ANGLE_HEURES_24);
            }

            // Calcul de l'heure sidérale moyenne à Greenwich du coucher
            GSTm = heureSideraleVraieAGreenwichCoucher - ((a_lieuEtDateCalcul.NutationLongitude * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.ObliquiteMoyenne + a_lieuEtDateCalcul.NutationObliquite))) / 15.0);
            GSTm = Maths.Modulo(GSTm, TypeAngle.ANGLE_HEURES_24);

            // Calcul de l'heure à Greenwich du coucher
            A = GSTm - T0;
            A = Maths.Modulo(A, TypeAngle.ANGLE_HEURES_24);
            heureTUCoucher = A * 0.9972695663;

            // Cas où la convergence du calcul itératif du coucher à échoué (la fonction appelante passe alors l'argument essai à 1)
            if ((heureTUCoucher < ((3.0 / 60.0)) + (56.0 / 3600.0)) && (a_essai == 1))
            {
                heureTUCoucher = heureTUCoucher + (23.0 + (56.0 / 60.0) + (4.0 / 3600.0));
                heureTUCoucher = Maths.Modulo(heureTUCoucher, TypeAngle.ANGLE_HEURES_24);
            }

            // Cas où le corps céleste est TOUJOURS VISIBLE ou TOUJOURS INVISIBLE, forçage de l'heure TU à TOUJOURS_VISIBLE ou TOUJOURS_INVISIBLE
            if (heureSideraleVraieLocaleLever == TOUJOURS_VISIBLE)
                heureTULever = TOUJOURS_VISIBLE;
            if (heureSideraleVraieLocaleLever == TOUJOURS_INVISIBLE)
                heureTULever = TOUJOURS_INVISIBLE;
            if (heureSideraleVraieLocaleCoucher == TOUJOURS_VISIBLE)
                heureTUCoucher = TOUJOURS_VISIBLE;
            if (heureSideraleVraieLocaleCoucher == TOUJOURS_INVISIBLE)
                heureTUCoucher = TOUJOURS_INVISIBLE;
        }

        // Algorithme 7
        /// <summary>
        /// Calcule les heures sidérales vraies de lever et de coucher à Greenwich à partir des heures sidérales vraies locales de lever et de coucher et de la longitude du lieu d’observation.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        protected void CalculerHeuresSideralesVraiesAGreenwichLeverCoucher(PositionTemps a_lieuEtDateCalcul)
        {
            // Calcul des heures sidérales vraies à Greenwich de lever et de coucher
            heureSideraleVraieAGreenwichLever = heureSideraleVraieLocaleLever - (a_lieuEtDateCalcul.LieuObservation.Longitude.Decimale / 15.0);
            heureSideraleVraieAGreenwichLever = Maths.Modulo(heureSideraleVraieAGreenwichLever, TypeAngle.ANGLE_HEURES_24);
            heureSideraleVraieAGreenwichCoucher = heureSideraleVraieLocaleCoucher - (a_lieuEtDateCalcul.LieuObservation.Longitude.Decimale / 15.0);
            heureSideraleVraieAGreenwichCoucher = Maths.Modulo(heureSideraleVraieAGreenwichCoucher, TypeAngle.ANGLE_HEURES_24);

            // Cas où le corps céleste est TOUJOURS VISIBLE ou TOUJOURS INVISIBLE, forçage de l'heure GST à TOUJOURS_VISIBLE ou TOUJOURS_INVISIBLE
            if (heureSideraleVraieLocaleLever == TOUJOURS_VISIBLE)
            {
                heureSideraleVraieAGreenwichLever = TOUJOURS_VISIBLE;
            }
            if (heureSideraleVraieLocaleLever == TOUJOURS_INVISIBLE)
            {
                heureSideraleVraieAGreenwichLever = TOUJOURS_INVISIBLE;
            }
            if (heureSideraleVraieLocaleCoucher == TOUJOURS_VISIBLE)
            {
                heureSideraleVraieAGreenwichCoucher = TOUJOURS_VISIBLE;
            }
            if (heureSideraleVraieLocaleCoucher == TOUJOURS_INVISIBLE)
            {
                heureSideraleVraieAGreenwichCoucher = TOUJOURS_INVISIBLE;
            }
        }

        // Algorithme 13
        /// <summary>
        /// Calcule les coordonnées équatoriales (géocentriques ou topocentriques) d’un corps céleste en coordonnées horizontales (géocentriques ou topocentriques)
        /// à partir de la latitude et de l’heure sidérale locale.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_geocentriqueOuTopocentrique">Type de coordonnées à convertir : géocentriques (true) ou topocentriques (false).</param>
        protected void ConvertirCoordonneesEquatorialesVersHorizontales(PositionTemps a_lieuEtDateCalcul, bool a_geocentriqueOuTopocentrique)
        {
            // Déclaration des variables de la méthode
            double H;   // variable de calcul

            // Cas des coordonnées équatoriales géocentriques
            if (a_geocentriqueOuTopocentrique)
            {
                // Calcul de l'angle horaire H
                H = Maths.CalculerHeureDecimale(a_lieuEtDateCalcul.HeureSideraleLocale) - ascensionDroiteGeocentrique.Decimale;
                H = Maths.Modulo(H, TypeAngle.ANGLE_HEURES_24); // H ramené dans l'intervalle [0h ; 24h]
                H = 15.0 * H; // Conversion de H en degrés

                // Calcul de l'altitude géocentrique
                double altGeo = Math.Asin(Math.Sin(Maths.DegToRad(declinaisonGeocentrique.Decimale)) * Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale))
                                + Math.Cos(Maths.DegToRad(declinaisonGeocentrique.Decimale)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(H)));
                altGeo = Maths.RadToDeg(altGeo); // Conversion de radians en degrées
                altitudeGeocentrique = new Angle(altGeo, TypeAngle.ANGLE_DEGRES_90);

                // Calcul de l'azimut géocentrique
                if (Math.Sin(Maths.DegToRad(H)) <= 0.0)
                {
                    double azgeo = Math.Acos((Math.Sin(Maths.DegToRad(declinaisonGeocentrique.Decimale)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale))
                                   * Math.Sin(Maths.DegToRad(altitudeGeocentrique.Decimale))) / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(altitudeGeocentrique.Decimale))));
                    azgeo = Maths.RadToDeg(azgeo);
                    azimutGeocentrique = new Angle(azgeo, TypeAngle.ANGLE_DEGRES_360);
                }
                if (Math.Sin(Maths.DegToRad(H)) > 0.0)
                {
                    double azGeo = Math.Acos((Math.Sin(Maths.DegToRad(declinaisonGeocentrique.Decimale)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Sin(Maths.DegToRad(altitudeGeocentrique.Decimale)))
                                   / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(altitudeGeocentrique.Decimale))));
                    azGeo = 360 - Maths.RadToDeg(azGeo);
                    azimutGeocentrique = new Angle(azGeo, TypeAngle.ANGLE_DEGRES_360);
                }
            }
            // Cas des coordonnées équatoriales topocentriques
            else
            {
                // Calcul de l'angle horaire H
                H = Maths.CalculerHeureDecimale(a_lieuEtDateCalcul.HeureSideraleLocale) - AscensionDroiteTopocentrique.Decimale;
                H = Maths.Modulo(H, TypeAngle.ANGLE_HEURES_24); // H ramené dans l'intervalle [0h ; 24h]
                H = 15.0 * H; // Conversion de H en degrés

                // Calcul de l'altitude topocentrique
                double altTop = Math.Asin((Math.Sin(Maths.DegToRad(declinaisonTopocentrique.Decimale)) * Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)))
                                + (Math.Cos(Maths.DegToRad(declinaisonTopocentrique.Decimale)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(H))));
                altTop = Maths.RadToDeg(altTop); // Conversion de radians en degrées
                altitudeTopocentrique = new Angle(altTop, TypeAngle.ANGLE_DEGRES_90);

                // Calcul de l'azimut topocentrique
                if (Math.Sin(Maths.DegToRad(H)) <= 0.0)
                {
                    double azTop = Math.Acos((Math.Sin(Maths.DegToRad(declinaisonTopocentrique.Decimale)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale))
                                   * Math.Sin(Maths.DegToRad(altitudeTopocentrique.Decimale))) / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(altitudeTopocentrique.Decimale))));
                    azTop = Maths.RadToDeg(azTop);
                    azimutTopocentrique = new Angle(azTop, TypeAngle.ANGLE_DEGRES_360);
                }
                if (Math.Sin(Maths.DegToRad(H)) > 0.0)
                {
                    double azTop = Math.Acos((Math.Sin(Maths.DegToRad(declinaisonTopocentrique.Decimale)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Sin(Maths.DegToRad(altitudeTopocentrique.Decimale)))
                                   / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(altitudeTopocentrique.Decimale))));
                    azTop = 360.0 - Maths.RadToDeg(azTop);
                    azimutTopocentrique = new Angle(azTop, TypeAngle.ANGLE_DEGRES_360);
                }
            }
        }
        
        // Algorithme 14
        /// <summary>
        /// Calcule les coordonnées horizontales topocentriques d’un corps céleste en coordonnées équatoriales topocentriques à partir de latitude et de l’heure sidérale vraie locale.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        protected void ConvertirCoordonneesHorizontalesVersEquatoriales(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double H = 0.0;   // variable de calcul

            // Calcul de la décinaison topocentrique
            double decTop = Math.Asin((Math.Sin(Maths.DegToRad(altitudeTopocentrique.Decimale)) * Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)))
                            + (Math.Cos(Maths.DegToRad(altitudeTopocentrique.Decimale)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(azimutTopocentrique.Decimale))));
            decTop = Maths.RadToDeg(decTop);
            declinaisonTopocentrique = new Angle(decTop, TypeAngle.ANGLE_DEGRES_90);

            // Calcul de l'angle horaire
            if (Math.Sin(Maths.DegToRad(azimutTopocentrique.Decimale)) <= 0)
            {
                H = Math.Acos((Math.Sin(Maths.DegToRad(altitudeTopocentrique.Decimale)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale))
                    * Math.Sin(Maths.DegToRad(declinaisonTopocentrique.Decimale))) / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(declinaisonTopocentrique.Decimale))));
                H = Maths.RadToDeg(H);
            }
            if (Math.Sin(Maths.DegToRad(azimutTopocentrique.Decimale)) > 0)
            {
                H = Math.Acos((Math.Sin(Maths.DegToRad(altitudeTopocentrique.Decimale)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale))
                    * Math.Sin(Maths.DegToRad(declinaisonTopocentrique.Decimale))) / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(declinaisonTopocentrique.Decimale))));
                H = 360 - Maths.RadToDeg(H);
            }

            // Calcul de l'ascension droite topocentrique
            H = H / 15.0; // Conversion de H en heures

            double alphaTop = Maths.CalculerHeureDecimale(a_lieuEtDateCalcul.HeureSideraleLocale) - H;
            alphaTop = Maths.Modulo(alphaTop, TypeAngle.ANGLE_HEURES_24);
            ascensionDroiteTopocentrique = new Angle(alphaTop, TypeAngle.ANGLE_HEURES_24);
        }

        // Algorithme 24
        /// <summary>
        /// Calcule la correction due à la réfraction atmosphérique à apporter sur l’altitude géocentrique d’un corps céleste à partir de l’altitude géocentrique, de la pression
        /// et de la température ambiante du lieu d’observation. En cas de non disponibilité de ces données, des valeurs par défaut (1013mbar pour la pression et 10°C pour la température)
        /// sont utilisées.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        protected void CalculerRefractionAtmospherique(PositionTemps a_lieuEtDateCalcul)
        {
            // Déclaration des variables de la méthode
            double z, R = 0.0; // variables de calcul

            // Calcul de l'angle au zénith
            z = 90.0 - altitudeGeocentrique.Decimale;

            // Calcul de la réfraction atmosphérique
            if (altitudeGeocentrique.Decimale >= 15.0)
            {
                R = (0.00452 * (double)a_lieuEtDateCalcul.LieuObservation.Pression * Math.Tan(Maths.DegToRad(z))) / (273.0 + (double)a_lieuEtDateCalcul.LieuObservation.Temperature);
            }
            else
            {
                R = (a_lieuEtDateCalcul.LieuObservation.Pression * (0.1594 + 0.0196 * altitudeGeocentrique.Decimale + 0.00002 * altitudeGeocentrique.Decimale * altitudeGeocentrique.Decimale))
                    / ((273.0 + (double)a_lieuEtDateCalcul.LieuObservation.Temperature) * (1 + 0.505 * altitudeGeocentrique.Decimale + 0.0845 * altitudeGeocentrique.Decimale * altitudeGeocentrique.Decimale));
            }

            // Calcul de l'altitude topocentrique corrigée de la réfraction atmosphérique
            altitudeTopocentrique = new Angle(altitudeGeocentrique.Decimale + R, TypeAngle.ANGLE_DEGRES_90);
            azimutTopocentrique = new Angle(azimutGeocentrique.Decimale, TypeAngle.ANGLE_DEGRES_360);
        }

        // Algorithme 28
        /// <summary>
        /// Calcule les heures sidérales vraies et les azimuts topocentriques de lever et de coucher d’un corps céleste à partir des coordonnées équatoriales géocentriques, de la latitude du lieu d’observation
        /// et dans le cas des corps du système solaire (en particulier le Soleil et la Lune) de la distance à la Terre et de la taille apparente.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul">Lieu d'observation et date pour le calcul.</param>
        /// <param name="a_r">Distance à la Terre du corps céleste (cas des corps célestes dans le système solaire), mettre n'importe quelle valeur autrement ou utiliser la méthode surchargée).</param>
        /// <param name="a_taille">Taille apparente du corps céleste (cas des corps célestes dans le système solaire), mettre n'importe quelle valeur autrement ou utiliser la méthode surchargée).</param>
        protected void CalculerHeuresSideralesEtAzimutsLeverCoucher(PositionTemps a_lieuEtDateCalcul, double a_r, double a_taille)
        {
            // Déclaration des variables de la méthode
            double parallaxe, nu, cosH, H; // variables de calcul

            // Calcul de la parallaxe
            switch (type)
            {
                case TypeCorpsCeleste.J2000:
                    parallaxe = 0.0;
                    break;
                case TypeCorpsCeleste.LUNE:
                    parallaxe = Maths.RadToDeg(Math.Asin((6378.14 / a_r)));
                    break;
                default:
                    parallaxe = Maths.RadToDeg(Math.Asin((Math.Sin(Maths.DegToRad(8.794 / 3600.0)) / a_r)));
                    break;
            }

            // Calcul du paramètre nu
            nu = parallaxe - (34.0 / 60.0) - (a_taille / 2.0);

            // Calcul de l'angle horaire
            cosH = (Math.Sin(Maths.DegToRad(nu)) - Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Sin(Maths.DegToRad(declinaisonGeocentrique.Decimale)))
                    / (Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)) * Math.Cos(Maths.DegToRad(declinaisonGeocentrique.Decimale)));
            if (cosH < -1) // Cas où le corps céleste est toujours visible
            {
                heureSideraleVraieLocaleLever = TOUJOURS_VISIBLE;
                heureSideraleVraieLocaleCoucher = TOUJOURS_VISIBLE;
            }
            if (cosH > 1) // Cas où le corps céleste est toujours invisible
            {
                heureSideraleVraieLocaleLever = TOUJOURS_INVISIBLE;
                heureSideraleVraieLocaleCoucher = TOUJOURS_INVISIBLE;
            }

            if ((cosH > -1) && (cosH < 1)) // Cas où le corps céleste se lève et se couche
            {
                H = Maths.RadToDeg(Math.Acos(cosH));
                H = H / 15.0; // H en heures

                // Calcul de l'heure sidérale vraie locale et de l'azimut de lever
                heureSideraleVraieLocaleLever = ascensionDroiteGeocentrique.Decimale - H;
                heureSideraleVraieLocaleLever = Maths.Modulo(heureSideraleVraieLocaleLever, TypeAngle.ANGLE_HEURES_24);
                azimutLever = Maths.RadToDeg(Math.Acos((Math.Sin(Maths.DegToRad(declinaisonGeocentrique.Decimale)) - Math.Sin(Maths.DegToRad(nu)) * Math.Sin(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)))
                                                   / (Math.Cos(Maths.DegToRad(nu)) * Math.Cos(Maths.DegToRad(a_lieuEtDateCalcul.LieuObservation.Latitude.Decimale)))));

                // Calcul de l'heure sidérale vraie locale et de l'azimut de coucher
                heureSideraleVraieLocaleCoucher = ascensionDroiteGeocentrique.Decimale + H;
                heureSideraleVraieLocaleCoucher = Maths.Modulo(heureSideraleVraieLocaleCoucher, TypeAngle.ANGLE_HEURES_24);
                azimutCoucher = 360.0 - azimutLever;
            }
        }
        // Méthode surchargée
        /// <summary>
        /// Calcule les heures sidérales vraies et les azimuts topocentriques de lever et de coucher d’un corps céleste à partir des coordonnées équatoriales géocentriques, de la latitude du lieu d’observation.
        /// Le calcul par cette version surchargée ne tient pas compte de la distance à la Terre et de la taille apparente (cas des corps du système solaire).
        /// Pour prendre en compte la distance à la Terre et la taille apparente, utiliser la version surchargée avec ces paramètres en argument.
        /// </summary>
        /// <param name="a_lieuEtDateCalcul"></param>
        protected void CalculerHeuresSideralesEtAzimutsLeverCoucher(PositionTemps a_lieuEtDateCalcul)
        {
            CalculerHeuresSideralesEtAzimutsLeverCoucher(a_lieuEtDateCalcul, double.MaxValue, 0.0);
        }
    }
}