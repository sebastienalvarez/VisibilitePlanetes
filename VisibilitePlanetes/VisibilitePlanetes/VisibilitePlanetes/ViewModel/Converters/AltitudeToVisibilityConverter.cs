/****************************************************************************************************************************************
 * 
 * Classe AltitudeToVisibilityConverter
 * Auteur : S. ALVAREZ
 * Date : 29-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de convertir l'altitude topocentrique calculée d'un corps céleste en information de visibilité pour les 
 *         contrôles.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisibilitePlanetes.ViewModel.Converters
{
    public class AltitudeToVisibilityConverter : IValueConverter
    {
        // Méthode permettant de renvoyer un bool dépendant de l'altitude topocentrique calculée d'un corps céleste
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double altitude = (double)value;
            bool param = Boolean.Parse((string)parameter);
            bool result = false;
            if(altitude > 0.0)
            {
                result = true;
            }
            if (!param)
            {
                result = !result;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion dans ce sens non nécessaire
            throw new NotImplementedException();
        }
    }
}