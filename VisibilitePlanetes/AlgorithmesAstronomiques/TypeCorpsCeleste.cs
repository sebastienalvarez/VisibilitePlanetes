/****************************************************************************************************************************
 * Enumération TypeCorpsCeleste
 * 
 * Version      1.0 - Novembre 2018 (portage en C#)
 * Auteur       Sébastien ALVAREZ
 * Statut       Terminé
 * 
 * L'énumération TypeCorpsCeleste permet de définir le type de corps céleste.
 *   
 ***************************************************************************************************************************/

namespace AlgorithmesAstronomiques
{
    public enum TypeCorpsCeleste
    {
        TERRE_SOLEIL,
        MERCURE,
        VENUS,
        MARS,
        JUPITER,
        SATURNE,
        URANUS,
        NEPTUNE,
        LUNE,
        COMETE_PERIODIQUE,
        COMETE_PARABOLIQUE,
        J2000
    }
}