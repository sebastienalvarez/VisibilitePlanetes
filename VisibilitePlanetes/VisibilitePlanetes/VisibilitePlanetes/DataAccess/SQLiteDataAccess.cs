/****************************************************************************************************************************************
 * 
 * Classe SQLiteDataAccess
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant l'accès des données depuis un fichier de base de données SQLite. La classe implémente l'interface 
 *         IDataAccess définissant le contrat à respecter pour l'accès aux données de l'application.
 * 
 ****************************************************************************************************************************************/

using AlgorithmesAstronomiques;
using AlgorithmesAstronomiques.Utilitaires;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using VisibilitePlanetes.Model;
using VisibilitePlanetes.ViewModel;

namespace VisibilitePlanetes.DataAccess
{
    public class SQLiteDataAccess : IDataAccess
    {
        // PROPRIETES
        /// <summary>
        /// Chemin du fichier de la base de données SQLite (chemin spécifique à la plateforme défini dans chacun des projets spécifique plateforme)
        /// </summary>
        public string ConnectionString { get; }

        // CONSTRUCTEUR
        public SQLiteDataAccess(string a_connectionString)
        {
            ConnectionString = a_connectionString;
        }

        // METHODES
        // INSERTION / MISE A JOUR
        /// <summary>
        /// Insère ou met à jour la sélection des corps célestes saisie par l'utilisateur
        /// </summary>
        /// <param name="a_selectedPlanets">Objet View Model avec les données saisies par l'utilisateur</param>
        /// <returns>Nombre de lignes insérées ou mises à jour, une valeur de -1 indique une erreur</returns>
        public int UpdateSelectedPlanets(PlanetSelectionViewModel a_selectedPlanets)
        {
            int modifiedLineNumber = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.CreateTable<PlanetSelectionModel>();
                    foreach (PlanetSelection item in a_selectedPlanets.SelectedPlanets)
                    {
                        PlanetSelectionModel foundItem = connection.Table<PlanetSelectionModel>().Where(i => i.ID == item.ID).FirstOrDefault();
                        if(foundItem == null)
                        {
                            modifiedLineNumber += connection.Insert(new PlanetSelectionModel(item.ID, item.Name, item.IsSelected)); // Insertion (1ère utilisation)...
                        }
                        else
                        {
                            if(foundItem.IsSelected != item.IsSelected)
                            {
                                modifiedLineNumber += connection.Update(new PlanetSelectionModel(item.ID, item.Name, item.IsSelected)); // ...ou mise à jour (utilisations ultérieures)
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // On renvoit un nombre < 0 pour indiquer qu'il y a une erreur, c'est le code amont qui gérera cette erreur
                modifiedLineNumber = -1;
            }
            return modifiedLineNumber;
        }

        /// <summary>
        /// Insère ou met à jour la sélection d'un corp céleste saisie par l'utilisateur
        /// </summary>
        /// <param name="a_selectedPlanet">Objet PlanetSelection du corps céleste concerné</param>
        /// <returns>Nombre de lignes mises à jour, une valeur de -1 indique une erreur</returns>
        public int UpdateSelectedPlanet(PlanetSelection a_selectedPlanet)
        {
            int modifiedLineNumber = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.CreateTable<PlanetSelectionModel>();
                    PlanetSelectionModel foundItem = connection.Table<PlanetSelectionModel>().Where(i => i.ID == a_selectedPlanet.ID).FirstOrDefault();
                    if (foundItem == null)
                    {
                        modifiedLineNumber += connection.Insert(new PlanetSelectionModel(a_selectedPlanet.ID, a_selectedPlanet.Name, a_selectedPlanet.IsSelected)); // Insertion (1ère utilisation)...
                    }
                    else
                    {
                        if (foundItem.IsSelected != a_selectedPlanet.IsSelected)
                        {
                            modifiedLineNumber += connection.Update(new PlanetSelectionModel(a_selectedPlanet.ID, a_selectedPlanet.Name, a_selectedPlanet.IsSelected)); // ...ou mise à jour (utilisations ultérieures)
                        }
                    }
                }
            }
            catch (Exception)
            {
                // On renvoit un nombre < 0 pour indiquer qu'il y a une erreur, c'est le code amont qui gérera cette erreur
                modifiedLineNumber = -1;
            }
            return modifiedLineNumber;
        }

        /// <summary>
        /// Insère ou met à jour les lieux d'observation définis par l'utilisateur
        /// </summary>
        /// <param name="a_observationsPoints">Collection d'objets LieuObservationModel représentant les lieux d'observation</param>
        /// <returns>Nombre de lignes insérées ou mises à jour, une valeur de -1 indique une erreur</returns>
        public int UpdateObservationPoints(List<LieuObservationModel> a_observationsPoints)
        {
            int modifiedLineNumber = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.CreateTable<LieuObservationModel>();
                    foreach (LieuObservationModel item in a_observationsPoints.Where(lo => lo.LieuObservation.NomLieuObservation != "Géolocalisation").ToList())
                    {
                        LieuObservationModel foundItem = connection.Table<LieuObservationModel>().Where(i => i.ID == item.ID).FirstOrDefault();
                        if (foundItem == null)
                        {
                            modifiedLineNumber += connection.Insert(item); // Insertion (1ère utilisation)...
                        }
                        else
                        {
                            if (foundItem.NomLieuObservation != item.NomLieuObservation || foundItem.Longitude != item.Longitude || foundItem.Latitude != item.Latitude || foundItem.Altitude != item.Altitude || foundItem.LieuSelectionne != item.LieuSelectionne)
                            {
                                modifiedLineNumber += connection.Update(item); // ...ou mise à jour (utilisations ultérieures)
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // On renvoit un nombre < 0 pour indiquer qu'il y a une erreur, c'est le code amont qui gérera cette erreur
                modifiedLineNumber = -1;
            }
            return modifiedLineNumber;
        }

        /// <summary>
        /// Insère ou met à jour un lieu d'observation défini par l'utilisateur
        /// </summary>
        /// <param name="a_observationPoint">Objet LieuObservationModel représentant le lieu d'observation</param>
        /// <returns>Nombre de lignes mises à jour, une valeur de -1 indique une erreur</returns>
        public int UpdateObservationPoint(LieuObservationModel a_observationPoint)
        {
            int modifiedLineNumber = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.CreateTable<LieuObservationModel>();
                    LieuObservationModel foundItem = connection.Table<LieuObservationModel>().Where(i => i.ID == a_observationPoint.ID).FirstOrDefault();
                    if (foundItem == null)
                    {
                        modifiedLineNumber += connection.Insert(a_observationPoint); // Insertion (1ère utilisation)...
                    }
                    else
                    {
                        modifiedLineNumber += connection.Update(a_observationPoint); // ...ou mise à jour (utilisations ultérieures)
                    }
                }
            }
            catch (Exception)
            {
                // On renvoit un nombre < 0 pour indiquer qu'il y a une erreur, c'est le code amont qui gérera cette erreur
                modifiedLineNumber = -1;
            }
            return modifiedLineNumber;
        }

        // LECTURE
        /// <summary>
        /// Récupère la sélection des corps célestes saisie précédemment par l'utilisateur
        /// </summary>
        /// <returns>Collection d'objets PlanetSelectionModel correspondant à la sélection des corps célestes saisie précédemment par l'utilisateur ou null si pas de données ou erreur</returns>
        public List<PlanetSelectionModel> GetSelectedPlanets()
        {
            List<PlanetSelectionModel> results = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    results = connection.Table<PlanetSelectionModel>().ToList();
                }
            }
            catch (Exception)
            {
                // Pas de traitement particulier, si pas d'accès, on renvoit null et il y aura une initialisation par défaut de la sélection des corps célestes à true
            }
            return results;
        }

        /// <summary>
        /// Récupère les lieux d'observation définis par l'utilsateur
        /// </summary>
        /// <returns>Collection d'objets LieuObservationModel correspondant aux lieux d'observation définis par l'utilisateur ou null si pas de données ou erreur</returns>
        public List<LieuObservationModel> GetObservationPoints()
        {
            List<LieuObservationModel> results = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    results = connection.Table<LieuObservationModel>().ToList();
                    foreach(LieuObservationModel lieu in results)
                    {
                        Angle longitude = new Angle(lieu.Longitude);
                        Angle latitude = new Angle(lieu.Latitude);
                        lieu.LieuObservation = new Position(lieu.NomLieuObservation, longitude.DegresOuHeures, longitude.Minute, longitude.Seconde, latitude.DegresOuHeures, latitude.Minute, latitude.Seconde, lieu.Altitude);
                    }
                }
            }
            catch (Exception)
            {
                // Pas de traitement particulier, si pas d'accès, on renvoit null et il y aura une initialisation par défaut de la sélection des corps célestes à true
            }
            return results;
        }
    }
}