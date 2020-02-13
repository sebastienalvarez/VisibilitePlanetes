/****************************************************************************************************************************************
 * 
 * Classe BoolToColorConverter
 * Auteur : S. ALVAREZ
 * Date : 28-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de convertir une information booléenne (sélectionné ou pas) en couleur (vert ou rouge) pour les contrôles.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisibilitePlanetes.ViewModel.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        // Méthode permettant de renvoyer une couleur verte pour une sélection true ou une couleur rouge pour une sélection false 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            if (isSelected)
            {
                return Color.ForestGreen;
            }
            else
            {
                return Color.DarkRed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion dans ce sens non nécessaire
            throw new NotImplementedException();
        }
    }
}