/****************************************************************************************************************************
 * Enumération TypeAngle
 * 
 * Version      1.0 - Novembre 2018
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * L'énumération TypeModulo permet de définir le type d'angles et leurs intervalles :
 * - ANGLE_DEGRES_360 pour un angle en degrés de 0° à 360°,
 * - ANGLE_DEGRES_180 pour un angle en degrés de -180° à +180°,
 * - ANGLE_DEGRES_90 pour un angle en degrés de -90° à +90°,
 * - ANGLE_HEURES_24 pour un angle en heures de 0h à 24h.
 * 
 ***************************************************************************************************************************/

namespace AlgorithmesAstronomiques.Utilitaires
{
    public enum TypeAngle
    {
        ANGLE_DEGRES_360,
        ANGLE_DEGRES_180,
        ANGLE_DEGRES_90,
        ANGLE_HEURES_24
    }
}