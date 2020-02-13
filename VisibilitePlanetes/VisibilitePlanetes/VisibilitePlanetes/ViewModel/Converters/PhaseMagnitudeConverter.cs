/****************************************************************************************************************************************
 * 
 * ClassePhaseMagnitudeConverter
 * Auteur : S. ALVAREZ
 * Date : 29-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de convertir la phase ou la magnitude d'un corps céleste en un string formaté (ces informations n'étant pas
 *         nécessaires pour tous les corps célestes, "NA" est affichée si non nécessaire).
 * 
 ****************************************************************************************************************************************/

using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisibilitePlanetes.ViewModel.Converters
{
    public class PhaseMagnitudeConverter : IValueConverter
    {
        // Méthode permettant de renvoyer un string formaté de la phase ou de la distance dépendant du type de corps céleste (Lune, planète)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            string result = string.Empty;
            if(val == -100)
            {
                result = "NA";
            }
            else
            {
                result = string.Format("{0:f2}", val).Replace(".", ",");
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