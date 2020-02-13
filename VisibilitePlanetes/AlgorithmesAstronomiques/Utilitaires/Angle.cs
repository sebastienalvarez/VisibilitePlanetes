/****************************************************************************************************************************
 * Classe Angle
 * 
 * Version      1.0 - Novembre 2018
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe Angle permet de définir un angle et ses composantes en degrés ou heures, en minutes d'arc et en secondes d'arc.
 *   
 ***************************************************************************************************************************/

using System;
using System.Text;

namespace AlgorithmesAstronomiques.Utilitaires
{
    public class Angle
    {
        // FIELDS PRIVES
        private readonly double decimale; // Valeur décimale de l'angle
        private readonly int degresOuHeures; // Nombre de degrés ou d'heures de l'angle
        private readonly int minute; // Nombre de minutes d'arc de l'angle
        private readonly float seconde; // Nombre de secondes d'arc de l'angle
        private TypeAngle typeAngle; // Type d'angle

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Valeur décimale de l'angle.
        /// </summary>
        public double Decimale
        {
            get { return decimale; }
        }
        /// <summary>
        /// Nombre de degrés ou d'heures de l'angle.
        /// </summary>
        public int DegresOuHeures
        {
            get { return degresOuHeures; }
        }
        /// <summary>
        /// Nombre de minutes d'arc de l'angle.
        /// </summary>
        public int Minute
        {
            get { return minute; }
        }
        /// <summary>
        /// Nombre de secondes d'arc de l'angle.
        /// </summary>
        public float Seconde
        {
            get { return seconde; }
        }

        // METHODES PUBLIQUES
        // CONSTRUCTEURS
        /// <summary>
        /// Constructeur d'une instance de Angle. La validité des paramètres est contrôlée, une exception de type ArgumentOutRangeException est levée en cas de non validité.
        /// </summary>
        /// <param name="a_degresOuHeures">Nombre de degrés ou d'heures de l'angle (compris entre 0 et 359 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_360, ou entre -179 et 180 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_180, ou entre -90 et 90 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_90, ou entre 0 et 23 lorsque a_modulo = TypeAngle.ANGLE_HEURES_90).</param>
        /// <param name="a_minute">Nombre de minutes de l'angle (compris entre 0 et 59 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_360 ou TypeAngle.ANGLE_HEURES_24, ou entre -59 et 59 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_180 ou TypeAngle.ANGLE_DEGRES_90).</param>
        /// <param name="a_seconde">Nombre de secondes de l'angle (compris entre [0f , 60f[ lorsque a_modulo = TypeAngle.ANGLE_DEGRES_360 ou TypeAngle.ANGLE_HEURES_24, ou entre ]-60f , 60f[ lorsque a_modulo = TypeAngle.ANGLE_DEGRES_180 ou TypeAngle.ANGLE_DEGRES_90).</param>
        /// <param name="a_modulo">Type d'angle : [0 , 360°] pour TypeAngle.ANGLE_DEGRES_360, ]-180° , 180°] pour TypeAngle.ANGLE_DEGRES_180, [-90° , 90°] pour TypeAngle.ANGLE_DEGRES_90 ou [0h , 24h[ pour TypeAngle.ANGLE_HEURES_24. Ce paramètre est optionnel, par défaut c'est TypeAngle.ANGLE_DEGRES_360.</param>
        public Angle(int a_degresOuHeures, int a_minute, float a_seconde, TypeAngle a_modulo = TypeAngle.ANGLE_DEGRES_360)
        {
            // Validation des arguments
            if(a_modulo != TypeAngle.ANGLE_DEGRES_360 && a_modulo != TypeAngle.ANGLE_DEGRES_180 && a_modulo != TypeAngle.ANGLE_DEGRES_90 && a_modulo != TypeAngle.ANGLE_HEURES_24)
            {
                throw new ArgumentOutOfRangeException("a_modulo", "Le paramètre a_modulo doit être une valeur de l'énumération TypeAngle");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_360 && (a_degresOuHeures < 0 || a_degresOuHeures > 359))
            {
                throw new ArgumentOutOfRangeException("a_degresOuHeures", "Le paramètre a_degresOuHeures doit être compris entre 0 et 359 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_360");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_180 && (a_degresOuHeures < -179 || a_degresOuHeures > 180))
            {
                throw new ArgumentOutOfRangeException("a_degresOuHeures", "Le paramètre a_degresOuHeures doit être compris entre -179 et 180 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_180");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_90 && (a_degresOuHeures < -90 || a_degresOuHeures > 90))
            {
                throw new ArgumentOutOfRangeException("a_degresOuHeures", "Le paramètre a_degresOuHeures doit être compris entre -90 et 90 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_90");
            }
            if (a_modulo == TypeAngle.ANGLE_HEURES_24 && (a_degresOuHeures < 0 || a_degresOuHeures > 23))
            {
                throw new ArgumentOutOfRangeException("a_degresOuHeures", "Le paramètre a_degresOuHeures doit être compris entre 0 et 23 lorsque a_modulo = TypeAngle.ANGLE_HEURES_24");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_180 && a_degresOuHeures == 180 && a_minute != 0)
            {
                throw new ArgumentOutOfRangeException("a_minute", "Le paramètre a_minute doit être égale à 0 si a_degresOuHeures = 180 et a_modulo = TypeAngle.ANGLE_DEGRES_180");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_180 && a_degresOuHeures == 180 && a_minute == 0 && a_seconde != 0f)
            {
                throw new ArgumentOutOfRangeException("a_seconde", "Le paramètre a_seconde doit être égale à 0f si a_degresOuHeures = 180 et a_modulo = TypeAngle.ANGLE_DEGRES_180");
            }
            if ((a_modulo == TypeAngle.ANGLE_DEGRES_180 || a_modulo == TypeAngle.ANGLE_DEGRES_90) && a_degresOuHeures == 0 && (a_minute < -59 || a_minute > 59))
            {
                throw new ArgumentOutOfRangeException("a_minute", "Le paramètre a_minute doit être compris entre -59 et 59 si a_degresOuHeures = 0 et a_modulo = TypeAngle.ANGLE_DEGRES_180 ou TypeAngle.ANGLE_DEGRES_90");
            }
            if ((a_modulo == TypeAngle.ANGLE_DEGRES_180 || a_modulo == TypeAngle.ANGLE_DEGRES_90) && a_degresOuHeures == 0 && (a_seconde <= -60f || a_seconde >= 60f))
            {
                throw new ArgumentOutOfRangeException("a_seconde", "Le paramètre a_seconde doit être compris entre ]60.0f , 60.0f[ si a_degresOuHeures = 0 et a_modulo = TypeAngle.ANGLE_DEGRES_180 ou TypeAngle.ANGLE_DEGRES_90");
            }
            if ((a_modulo == TypeAngle.ANGLE_DEGRES_360 || a_modulo == TypeAngle.ANGLE_HEURES_24) && (a_minute < 0 || a_minute > 59))
            {
                throw new ArgumentOutOfRangeException("a_minute", "Le paramètre a_minute doit être compris entre 0 et 59 lorsque a_modulo = TypeAngle.ANGLE_DEGRES_360 ou TypeAngle.ANGLE_HEURES_24");
            }
            if ((a_modulo == TypeAngle.ANGLE_DEGRES_360 || a_modulo == TypeAngle.ANGLE_HEURES_24) && (a_seconde < 0f || a_seconde >= 60f))
            {
                throw new ArgumentOutOfRangeException("a_seconde", "Le paramètre a_seconde doit être compris entre [0f , 60.0f[ lorsque a_modulo = TypeAngle.ANGLE_DEGRES_360 ou TypeAngle.ANGLE_HEURES_24");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_90 && (a_degresOuHeures == 90 || a_degresOuHeures == -90) && a_minute != 0)
            {
                throw new ArgumentOutOfRangeException("a_minute", "Le paramètre a_minute doit être égale à 0 si a_degresOuHeures = 90 ou -90 et a_modulo = TypeAngle.ANGLE_HEURES_24");
            }
            if (a_modulo == TypeAngle.ANGLE_DEGRES_90 && (a_degresOuHeures == 90 || a_degresOuHeures == -90) && a_minute == 0 && a_seconde != 0f)
            {
                throw new ArgumentOutOfRangeException("a_seconde", "Le paramètre a_seconde doit être égale à 0f si a_degresOuHeures = 90 ou -90 et a_modulo = TypeAngle.ANGLE_HEURES_24");
            }
            if ((a_modulo == TypeAngle.ANGLE_DEGRES_180 || a_modulo == TypeAngle.ANGLE_DEGRES_90) && a_degresOuHeures > 0 && a_minute < 0)
            {
                throw new ArgumentOutOfRangeException("a_minute", "Le paramètre a_minute doit être compris entre 0 et 59");
            }
            if ((a_modulo == TypeAngle.ANGLE_DEGRES_180 || a_modulo == TypeAngle.ANGLE_DEGRES_90) && a_degresOuHeures > 0 && a_seconde < 0f)
            {
                throw new ArgumentOutOfRangeException("a_seconde", "Le paramètre a_seconde doit être compris entre [0f et 60f[");
            }

            degresOuHeures = a_degresOuHeures;
            minute = a_minute;
            seconde = a_seconde;
            decimale = Maths.CalculerAngleDecimal(a_degresOuHeures, a_minute, a_seconde);
            typeAngle = a_modulo;
            if (decimale < 0 && degresOuHeures == 0)
            {
                if (minute < 0)
                {
                    seconde = Math.Abs(seconde);
                }
            }
        }

        /// <summary>
        /// Constructeur d'une instance de Angle.
        /// </summary>
        /// <param name="a_valeur">Angle en décimale. Un angle modulo est accepté et est ramené dans le bon intervalle.</param>
        /// <param name="a_modulo">Type d'angle : [0 , 360°] pour TypeAngle.ANGLE_DEGRES_360, ]-180° , 180°] pour TypeAngle.ANGLE_DEGRES_180, [-90° , 90°] pour TypeAngle.ANGLE_DEGRES_90 ou [0h , 24h[ pour TypeAngle.ANGLE_HEURES_24. Ce paramètre est optionnel, par défaut c'est TypeAngle.ANGLE_DEGRES_360.</param>
        public Angle(double a_valeur, TypeAngle a_modulo = TypeAngle.ANGLE_DEGRES_360)
        {
            decimale = a_valeur;
            decimale = Maths.Modulo(decimale, a_modulo);

            int signe = 1;
            if (decimale < 0.0)
            {
                signe = -1;
                decimale = -decimale;
            }
            degresOuHeures = (int)decimale;
            minute = (int)(60.0 * (decimale - (double)degresOuHeures));
            seconde = (float)(60 * (60.0 * (decimale - (double)degresOuHeures) - (double)minute));

            if ((signe == -1) && (degresOuHeures != 0))
            {
                degresOuHeures = -degresOuHeures;
            }
            if ((signe == -1) && (degresOuHeures == 0) && (minute != 0))
            {
                minute = -minute;
            }
            if ((signe == -1) && (degresOuHeures == 0) && (minute == 0))
            {
                seconde = -seconde;
            }
            if (signe == -1)
            {
                decimale = -decimale;
            }

            typeAngle = a_modulo;
        }

        // METHODES PUBLIQUES
        /// <summary>
        /// Redéfinition de la méthode ToString pour l'écriture d'un angle en degrés (ou heures), minutes d'arc et secondes d'arc.
        /// </summary>
        /// <returns>Angle formaté en degrés (ou heures), minutes d'arc et secondes d'arc.</returns>
        public override string ToString()
        {
            StringBuilder angle = new StringBuilder();
            if (decimale < 0)
            {
                angle.Append("-");
            }
            if (typeAngle == TypeAngle.ANGLE_HEURES_24)
            {
                angle.Append(string.Format("{0:d2}h", Math.Abs(degresOuHeures)));                
            }
            else if (typeAngle == TypeAngle.ANGLE_DEGRES_90)
            {
                angle.Append(string.Format("{0:d2}°", Math.Abs(degresOuHeures)));
            }
            else
            {
                angle.Append(string.Format("{0:d3}°", Math.Abs(degresOuHeures)));
            }
            angle.Append(string.Format("{0:d2}'", Math.Abs(minute)));
            angle.Append(string.Format("{0:00.0}\"", Math.Abs(seconde)));
            return angle.ToString();
        }

        // METHODES PRIVEES
        // Aucune
    }
}