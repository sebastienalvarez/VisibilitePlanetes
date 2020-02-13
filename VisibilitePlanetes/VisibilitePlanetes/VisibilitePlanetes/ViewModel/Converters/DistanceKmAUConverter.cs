/****************************************************************************************************************************************
 * 
 * Classe DistanceKmAUConverter
 * Auteur : S. ALVAREZ
 * Date : 29-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de convertir la distance d'un corps céleste en un string formaté (la Lune a une indication en km tandis que
 *         les planètes ont une indication en Unité Astronomique).
 * 
 ****************************************************************************************************************************************/

using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisibilitePlanetes.ViewModel.Converters
{
    public class DistanceKmAUConverter : IValueConverter
    {
        // Méthode permettant de renvoyer un string formaté de la distance dépendant du type de corps céleste (Lune ou planète)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double distance = (double)value;
            string result = string.Empty;
            // Cas de la Lune
            if (distance > 300000)
            {
                result = string.Format("{0:f0} km", distance);
            }
            else
            {
                result = string.Format("{0:f3} UA", distance).Replace(".", ",");
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