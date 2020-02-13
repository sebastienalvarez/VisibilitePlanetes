/****************************************************************************************************************************************
 * 
 * Classe GeolocalisationRemoveConverter
 * Auteur : S. ALVAREZ
 * Date : 01-08-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de convertir la collection des lieux d'observation brute (incluant potentiellement la Géolocalisation), en
 *         une collection des lieux d'observation sans la Géolocalisation.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VisibilitePlanetes.Model;
using Xamarin.Forms;

namespace VisibilitePlanetes.ViewModel.Converters
{
    public class GeolocalisationRemoveConverter : IValueConverter
    {
        // Méthode permettant de supprimer le lieu d'observation Géolocalisation de la collection
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<LieuObservationModel> rawList = (ObservableCollection<LieuObservationModel>)value;
            return new ObservableCollection<LieuObservationModel>(rawList.Where(lo => lo.LieuObservation.NomLieuObservation != "Géolocalisation").ToList());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion dans ce sens non nécessaire
            throw new NotImplementedException();
        }
    }
}