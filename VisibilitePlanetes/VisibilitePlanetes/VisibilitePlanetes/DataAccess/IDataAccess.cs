/****************************************************************************************************************************************
 * 
 * Interface IDataAccess
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Interface définissant le contrat pour les classes d'accès aux données : ainsi le code en amont est indépendant du mécanisme
 *         d'accès aux données (base de données SQLite, serialization binaire, fichier xml...).
 * 
 ****************************************************************************************************************************************/

using System.Collections.Generic;
using VisibilitePlanetes.Model;
using VisibilitePlanetes.ViewModel;

namespace VisibilitePlanetes.DataAccess
{
    public interface IDataAccess
    {
        // INSERTION / MISE A JOUR
        /// <summary>
        /// Insère ou met à jour la sélection des corps célestes saisie par l'utilisateur
        /// </summary>
        /// <param name="a_selectedPlanets">Objet View Model avec les données saisies par l'utilisateur</param>
        /// <returns>Nombre de lignes insérées ou mises à jour, une valeur de -1 indique une erreur</returns>
        int UpdateSelectedPlanets(PlanetSelectionViewModel a_selectedPlanets);

        /// <summary>
        /// Insère ou met à jour la sélection d'un corp céleste saisie par l'utilisateur
        /// </summary>
        /// <param name="a_selectedPlanet">Objet PlanetSelection du corps céleste concerné</param>
        /// <returns>Nombre de lignes mises à jour, une valeur de -1 indique une erreur</returns>
        int UpdateSelectedPlanet(PlanetSelection a_selectedPlanet);

        /// <summary>
        /// Insère ou met à jour les lieux d'observation définis par l'utilisateur
        /// </summary>
        /// <param name="a_observationsPoints">Collection d'objets LieuObservationModel représentant les lieux d'observation</param>
        /// <returns>Nombre de lignes insérées ou mises à jour, une valeur de -1 indique une erreur</returns>
        int UpdateObservationPoints(List<LieuObservationModel> a_observationsPoints);

        /// <summary>
        /// Insère ou met à jour un lieu d'observation défini par l'utilisateur
        /// </summary>
        /// <param name="a_observationPoint">Objet LieuObservationModel représentant le lieu d'observation</param>
        /// <returns>Nombre de lignes mises à jour, une valeur de -1 indique une erreur</returns>
        int UpdateObservationPoint(LieuObservationModel a_observationPoint);

        // LECTURE
        /// <summary>
        /// Récupère la sélection des corps célestes saisie précédemment par l'utilisateur
        /// </summary>
        /// <returns>Collection d'objets PlanetSelectionModel correspondant à la sélection des corps célestes saisie précédemment par l'utilisateur ou null si pas de données ou erreur</returns>
        List<PlanetSelectionModel> GetSelectedPlanets();

        /// <summary>
        /// Récupère les lieux d'observation définis par l'utilsateur
        /// </summary>
        /// <returns>Collection d'objets LieuObservationModel correspondant aux lieux d'observation définis par l'utilisateur ou null si pas de données ou erreur</returns>
        List<LieuObservationModel> GetObservationPoints();

        // SUPRESSION
        // Pas de supression nécessaire
    }
}