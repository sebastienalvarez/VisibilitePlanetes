/****************************************************************************************************************************
 * Classe CielProfond
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * La classe CielProfond permet de de définir les coordonnées équatoriales d'un objet du ciel profond à l'époque J2000
 * ainsi que son mouvement propre et sa magnitude.
 *   
 ***************************************************************************************************************************/

using AlgorithmesAstronomiques.Utilitaires;

namespace AlgorithmesAstronomiques
{
    public class CielProfond
    {
        // FIELDS PRIVES
        private readonly string nomCommun; // Nom commun du corps céleste
        private readonly string nomCatalogue; // Nom catalogue du corps céleste
        private readonly Angle alphaJ2000; // Ascension droite du corps céleste à l'époque J2000
        private readonly Angle deltaJ2000; // Déclinaison du corps céleste à l'époque J2000
        private readonly double mouvementPropreAlpha; // Mouvement propre annuel du corps céleste en ascension droite en milliarcsec 
        private readonly double mouvementPropreDelta; // Mouvement propre annuel du corps céleste en déclinaison en milliarcsec
        private readonly double magnitude; // Magnitude apparente du corps céleste

        // PROPRIETES PUBLIQUES
        /// <summary>
        /// Nom du corps céleste.
        /// </summary>
        public string NomCommun
        {
            get { return nomCommun; }
        }
        /// <summary>
        /// Nom catalogue du corps céleste.
        /// </summary>
        public string NomCatalogue
        {
            get { return nomCatalogue; }
        }
        /// <summary>
        /// Ascension droite du corps céleste à l'époque J2000.
        /// </summary>
        public Angle AlphaJ2000
        {
            get { return alphaJ2000; }
        }
        /// <summary>
        /// Déclinaison du corps céleste à l'époque J2000.
        /// </summary>
        public Angle DeltaJ2000
        {
            get { return deltaJ2000; }
        }
        /// <summary>
        /// Mouvement propre annuel du corps céleste en ascension droite en milliarcsec.
        /// </summary>
        public double MouvementPropreAlpha
        {
            get { return mouvementPropreAlpha; }
        }
        /// <summary>
        /// Mouvement propre annuel du corps céleste en déclinaison en milliarcsec.
        /// </summary>
        public double MouvementPropreDelta
        {
            get { return mouvementPropreDelta; }
        }
        /// <summary>
        /// Magnitude apparente du corps céleste.
        /// </summary>
        public double Magnitude
        {
            get { return magnitude; }
        }

        // CONSTRUCTEURS
        public CielProfond(string a_nomCommun, string a_nomCatalogue, int a_alphaJ2000Heure, int a_alphaJ2000Minute, float a_alphaJ2000Seconde, int a_deltaJ2000Heure, int a_deltaJ2000Minute, float a_deltaJ2000Seconde, double a_magnitude, double a_mouvementPropreAlpha = 0, double a_mouvementPropreDelta = 0)
        {
            nomCommun = a_nomCommun;
            nomCatalogue = a_nomCatalogue;
            alphaJ2000 = new Angle(a_alphaJ2000Heure, a_alphaJ2000Minute, a_alphaJ2000Seconde, TypeAngle.ANGLE_HEURES_24);
            deltaJ2000 = new Angle(a_deltaJ2000Heure, a_deltaJ2000Minute, a_deltaJ2000Seconde, TypeAngle.ANGLE_DEGRES_90);
            mouvementPropreAlpha = a_mouvementPropreAlpha;
            mouvementPropreDelta = a_mouvementPropreDelta;
            magnitude = a_magnitude;
        }
        public CielProfond(string a_nomCommun, int a_alphaJ2000Heure, int a_alphaJ2000Minute, float a_alphaJ2000Seconde, int a_deltaJ2000Heure, int a_deltaJ2000Minute, float a_deltaJ2000Seconde, double a_magnitude, double a_mouvementPropreAlpha = 0, double a_mouvementPropreDelta = 0)
        {
            nomCommun = a_nomCommun;
            nomCatalogue = "";
            alphaJ2000 = new Angle(a_alphaJ2000Heure, a_alphaJ2000Minute, a_alphaJ2000Seconde, TypeAngle.ANGLE_HEURES_24);
            deltaJ2000 = new Angle(a_deltaJ2000Heure, a_deltaJ2000Minute, a_deltaJ2000Seconde, TypeAngle.ANGLE_DEGRES_90);
            mouvementPropreAlpha = a_mouvementPropreAlpha;
            mouvementPropreDelta = a_mouvementPropreDelta;
            magnitude = a_magnitude;
        }
    }
}