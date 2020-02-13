/****************************************************************************************************************************************
 * 
 * Classe PlanetSelection
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe intermédiaire permettant de représenter les données affichées pour la sélection d'un corps céleste dans la page.
 *         Cette classe dérive de PlanetSelectionModel.
 * 
 ****************************************************************************************************************************************/

using VisibilitePlanetes.Model;

namespace VisibilitePlanetes.ViewModel
{
    public class PlanetSelection : PlanetSelectionModel
    {
        // PROPRIETES
        /// <summary>
        /// Chemin de la ressource image associée au corps céleste lorsque celui-ci est sélectionné
        /// </summary>
        public string ImageSourceSelected  { get; }

        /// <summary>
        /// Chemin de la ressource image associée au corps céleste lorsque celui-ci n'est pas sélectionné
        /// </summary>
        public string ImageSourceNotSelected { get; }

        private string imageSource;
        /// <summary>
        /// Chemin de la ressource à utiliser dépendant de la propriété héritée IsSelected, c'est une solution temporaire jusqu'à résolution du problème de Data Binding à l'élément complet de la collection avec utilisation d'un Converter
        /// </summary>
        public string ImageSource
        {
            get
            {
                if (IsSelected)
                {
                    imageSource = ImageSourceSelected;
                }
                else
                {
                    imageSource = ImageSourceNotSelected;
                }
                ImageSource = imageSource;
                return imageSource;
            }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        /// <summary>
        /// Description sommaire du corps céleste
        /// </summary>
        public string Details { get; }

        // CONSTRUCTEUR
        public PlanetSelection(string a_imageSourceSelected, string a_imageSourceNotSelected, int a_id, string a_name, string a_details, bool a_isSelected) : base(a_id, a_name, a_isSelected)
        {
            ImageSourceSelected = a_imageSourceSelected;
            ImageSourceNotSelected = a_imageSourceNotSelected;
            Details = a_details;
        }

        // METHODES
        // Pas de méthode
    }
}