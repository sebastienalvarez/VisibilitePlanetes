/****************************************************************************************************************************************
 * 
 * Classe BoolToImageSourceConverter
 * Auteur : S. ALVAREZ
 * Date : 28-07-2019
 * Statut : En Cours...
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de convertir une information booléenne (vusible ou pas) en ImageSource pour les contrôles.
 *         CLASSE NON UTILISEE
 * 
 ****************************************************************************************************************************************/

using System;
using System.Globalization;
using Xamarin.Forms;

// CONSERVE POUR RESOLUTION ULTERIEUR DU PROBLEME DE DATA BINDING AVEC UN ELEMENT DE LA COLLECTION ET UTILISATION DE CE CONVERTER POUR ENVOYER LA BONNE RESSOURCE

namespace VisibilitePlanetes.ViewModel.Converters
{
    public class BoolToImageSourceConverter : IValueConverter
    {
        // Méthode permettant de renvoyer la bonne ressource image en fonction de la sélection
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PlanetSelection selectedPlanet = (PlanetSelection)value;

            if(selectedPlanet != null)
            {
                if (selectedPlanet.IsSelected)
                {
                    return selectedPlanet.ImageSourceSelected;
                }
                else
                {
                    return selectedPlanet.ImageSourceNotSelected;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion dans ce sens non nécessaire
            throw new NotImplementedException();
        }
    }
}