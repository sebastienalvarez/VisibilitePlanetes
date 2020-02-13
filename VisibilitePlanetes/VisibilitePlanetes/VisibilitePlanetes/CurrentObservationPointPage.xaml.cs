/****************************************************************************************************************************************
 * 
 * Classe CurrentObservationPointPage
 * Auteur : S. ALVAREZ
 * Date : 30-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page de sélection du lieu d'observation pour les calculs de visibilité.
 *         C'est une des pages de la page à onglets ObservationPointsPage.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Linq;
using VisibilitePlanetes.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrentObservationPointPage : ContentPage
	{
		public CurrentObservationPointPage ()
		{
            InitializeComponent();
		}

        // Méthode permettant de gérer la sélection du contrôle Picker de sélection du lieu d'observation
        private void ObservationPointsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker control = (Picker)sender;
            LieuObservationModel selectedItem = (LieuObservationModel)control.SelectedItem;
            if(selectedItem != null)
            {
                App.ObservationPointsViewModel.LieuObservationSelectionne = selectedItem;
                App.DataProvider.UpdateObservationPoints(App.ObservationPointsViewModel.ListeLieuxObservation.ToList());
            }
        }
    }
}