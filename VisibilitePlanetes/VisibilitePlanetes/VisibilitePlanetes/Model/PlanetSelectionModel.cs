/****************************************************************************************************************************************
 * 
 * Classe PlanetSelectionModel
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter l'état de sélection de l'utilisateur d'un corps céleste : si le corps céleste est sélectionné
 *         par l'utilisateur, le résultat des calculs de visibilité est présenté, dans le cas contraire, les résultats ne sont pas
 *         affichés.
 *         Cette classe permet la persistance des données dans le téléphone (fichier de base de données SQLite).
 * 
 ****************************************************************************************************************************************/

using System.ComponentModel;
using SQLite;

namespace VisibilitePlanetes.Model
{
    public class PlanetSelectionModel : INotifyPropertyChanged
    {
        // PROPRIETES
        /// <summary>
        /// ID du corps céleste (pour clé primaire d'une base de données)
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// Nom du corps céleste
        /// </summary>
        public string Name { get; set; }

        private bool isSelected;
        /// <summary>
        /// Etat de sélection de l'utilisateur (true : corps céleste sélectionné pour affichage des resultats)
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public PlanetSelectionModel()
        {
        }
        public PlanetSelectionModel(int a_id, string a_name, bool a_isSelected)
        {
            ID = a_id;
            Name = a_name;
            IsSelected = a_isSelected;
        }

        // METHODES
        // Méthode permettant l'envoi de notification de changement de valeur pour le Data Binding
        protected void OnPropertyChanged(string a_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(a_propertyName));
            }
        }
    }
}