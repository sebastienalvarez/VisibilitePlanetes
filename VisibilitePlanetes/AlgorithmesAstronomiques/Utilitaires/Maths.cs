/****************************************************************************************************************************
 * Classe Maths
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe Maths fournit des méthodes statiques Maths :
 * CalculerAngleDecimal() => Retourne l'angle en décimale de degré (ou d'heure) calculé à partir des valeurs en degré,
 *                           minute et seconde d'arc (ou en heure, minute et seconde)
 * Modulo() => Retourne un angle dans l'intervalle spécifié
 * DegToRad() => Retourne l'angle en radians calculé à partir d'un angle en degrés
 * RadToDeg() => Retourne l'angle en degrés calculé à partir d'un angle en radians
 * CalculerHeureDecimale() => Retourne l'heure sous forme décimale d'un objet DateTime
 * EstimerDeltaT() => Retourne une estimation de Delta T entre le Temps Terrestre et le Temps Universel pour une année entre
 *                    1950 et 2100 (0 pour une autre année)
 * CalculerAnomalieExcentriqueAvecKepler() => Retourne l'anomalie excentrique à partir de l'anomalie moyenne et de
 *                                            l'excentricité de l'orbite en utilisant l'équation de Kepler                   
 * 
 ***************************************************************************************************************************/

using System;

namespace AlgorithmesAstronomiques.Utilitaires
{
    public class Maths
    {
        /// <summary>
        /// Retourne l'angle en décimale de degré (ou d'heure) calculé à partir des valeurs en degré, minute et seconde d'arc (ou en heure, minute et seconde).
        /// </summary>
        /// <param name="a_degreOuHeure">Nombre de degré ou d'heure.</param>
        /// <param name="a_minute">Nombre de minute d'arc.</param>
        /// <param name="a_seconde">Nombre de seconde d'arc.</param>
        /// <returns>Angle calculé en décimale de degré (ou d'heure).</returns>
        public static double CalculerAngleDecimal(int a_degreOuHeure, int a_minute, float a_seconde)
        {
            if (a_degreOuHeure < 0)
            {
                return (double)a_degreOuHeure - (double)Math.Abs(a_minute) / 60.0 - (double)Math.Abs(a_seconde) / 3600.0;
            }
            return (double)a_degreOuHeure + (double)a_minute / 60.0 + (double)a_seconde / 3600.0;
        }

        /// <summary>
        /// Retourne un angle dans l'intervalle spécifié.
        /// </summary>
        /// <param name="a_angle">Angle à ramener dans le bon intervalle.</param>
        /// <param name="a_modulo">Type d'angle déterminant l'intervalle : : [0 , 360°] pour TypeAngle.ANGLE_DEGRES_360, ]-180° , 180°] pour TypeAngle.ANGLE_DEGRES_180, [-90° , 90°] pour TypeAngle.ANGLE_DEGRES_90 ou [0h , 24h[ pour TypeAngle.ANGLE_HEURES_24. Ce paramètre est optionnel, par défaut c'est TypeAngle.ANGLE_DEGRES_360.</param>
        /// <returns>Angle ramené dans l'intervalle spécifié.</returns>
        public static double Modulo(double a_angle, TypeAngle a_modulo = TypeAngle.ANGLE_DEGRES_360)
        {
            // Déclaration des variables
            double resultat = a_angle;    // variable de retour de l'angle dans le bon intervalle
            int arrondi;        // variable intermédiaire pour le forçage du type
            double facteur = 360;
            switch (a_modulo)
            {
                case TypeAngle.ANGLE_DEGRES_180:
                    facteur = 180;
                    break;
                case TypeAngle.ANGLE_DEGRES_90:
                    facteur = 90;
                    break;
                case TypeAngle.ANGLE_HEURES_24:
                    facteur = 24;
                    break;
            }

            if (facteur == 180)
            {
                a_angle += 180;
                if (a_angle < 0)
                {
                    arrondi = (int)(Math.Abs(a_angle / 360));
                    resultat = a_angle + 360 * (double)(1 + arrondi) - 180;
                }
                else
                {
                    arrondi = (int)(a_angle / 360);
                    resultat = a_angle - 360 * (double)arrondi - 180;
                }
            }

            if (facteur == 360 || facteur == 24)
            {
                if (a_angle < 0)
                {
                    arrondi = (int)(a_angle / facteur);
                    resultat = a_angle + facteur * (double)(1 + Math.Abs(arrondi));
                }
                else
                {
                    arrondi = (int)(System.Math.Abs(a_angle / facteur));
                    resultat = a_angle - facteur * (double)(arrondi);
                }
            }

            if (facteur == 90)
            {
                // A FAIRE SI BESOIN
            }

            return resultat;
        }

        /// <summary>
        /// Retourne l'angle en radians calculé à partir d'un angle en degrés.
        /// </summary>
        /// <param name="a_angle">Angle en degrés.</param>
        /// <returns>Angle en radians.</returns>
        public static double DegToRad(double a_angle)
        {
            return a_angle * Math.PI / 180.0;
        }

        /// <summary>
        /// Retourne l'angle en degrés calculé à partir d'un angle en radians.
        /// </summary>
        /// <param name="a_angle">Angle en radians.</param>
        /// <returns>Angle en degrés.</returns>
        public static double RadToDeg(double a_angle)
        {
            return a_angle * 180.0 / Math.PI;
        }

        /// <summary>
        /// Retourne l'heure sous forme décimale d'un objet DateTime.
        /// </summary>
        /// <param name="a_heure">Objet DateTime pour lequel on veut calculer l'heure décimale.</param>
        /// <returns>Heure décimale.</returns>
        public static double CalculerHeureDecimale(DateTime a_heure)
        {
            return (double)a_heure.Hour + (double)a_heure.Minute / 60.0 + ((double)a_heure.Second + (double)a_heure.Millisecond / 1000.0) / 3600.0;
        }

        /// <summary>
        /// Retourne une estimation de Delta T entre le Temps Terrestre et le Temps Universel pour une année entre 1950 et 2100 (0 pour une autre année). L'estimation est calculée avec l'équation Delta T = 62.92 + 0.32217 x (année - 2000) + 0.005589 x (année - 2000)².
        /// </summary>
        /// <param name="a_annee"></param>
        /// <returns>Delta T en s.</returns>
        public static float EstimerDeltaT(int a_annee)
        {
            float deltaT = 0f;
            if (a_annee >= 1950 && a_annee <= 2150)
            {
                deltaT = 62.92f + 0.32217f * (float)(a_annee - 2000) + 0.005589f * (float)(a_annee - 2000) * (float)(a_annee - 2000);
            }
            return deltaT;
        }

        /// <summary>
        /// Retourne l'anomalie excentrique à partir de l'anomalie moyenne et de l'excentricité de l'orbite en utilisant l'équation de Kepler.
        /// </summary>
        /// <param name="a_anomalieMoyenne">Anomalie moyenne de l'orbite en degrés.</param>
        /// <param name="a_exentricite">Exentricité de l'orbite.</param>
        /// <returns>Anomalie excentrique en radians.</returns>
        public static double CalculerAnomalieExcentriqueAvecKelper(double a_anomalieMoyenne, double a_exentricite)
        {
            // Déclaration des variables
            double precision;   // variable de calcul
            double resultat;    // variable de retour de l'anomalie excentrique

            // Calcul itératif de l'anomalie excentrique par l'équation de kepler (E - e*sinE = M)
            resultat = DegToRad(a_anomalieMoyenne);  // Conversion de l'anomalie moyenne en radians 
            precision = resultat - a_exentricite * Math.Sin(resultat) - a_anomalieMoyenne * (Math.PI / 180.0);
            do
            {
                resultat = resultat - (precision / (1 - a_exentricite * Math.Cos(resultat)));
                precision = resultat - a_exentricite * Math.Sin(resultat) - a_anomalieMoyenne * (Math.PI / 180.0);
            }
            //while (precision > 0.0000000001 || precision < -0.0000000001);
            while (precision > 0.0000001 || precision < -0.0000001) ;

            return resultat; // Retour de l'anomalie excentrique en radians
        } 
    }
}