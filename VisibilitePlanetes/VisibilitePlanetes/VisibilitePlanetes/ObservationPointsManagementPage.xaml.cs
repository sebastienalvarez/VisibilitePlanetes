/****************************************************************************************************************************************
 * 
 * Classe ObservationPointsManagementPage
 * Auteur : S. ALVAREZ
 * Date : 30-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page d'ajout et de modification des lieux d'observation existants.
 *         C'est une des pages de la page à onglets ObservationPointsPage.
 * 
 ****************************************************************************************************************************************/

using System;
using VisibilitePlanetes.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ObservationPointsManagementPage : ContentPage
	{
		public ObservationPointsManagementPage ()
		{
			InitializeComponent ();
		}

        // Méthode permettant d'ajouter un nouveau lieu d'observation
        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            App.ObservationPointsViewModel.LieuObservationEnCoursDeDefinition = new LieuObservationModel("Lieu observation", 2, 30, 30, 48, 30, 30, 0f);
        }

        // Méthode permettant de modifier ou d'ajouter le lieu d'observation défini
        private void ButtonValidate_Clicked(object sender, EventArgs e)
        {
            // Cas d'un ajout
            if (!App.ObservationPointsViewModel.ListeLieuxObservation.Contains(App.ObservationPointsViewModel.LieuObservationEnCoursDeDefinition))
            {
                LieuObservationModel newObservationPoint = new LieuObservationModel(NomLieuObservation.Text, (int)LongitudeDegres.Value, (int)LongitudeMinute.Value, (float)LongitudeSeconde.Value, (int)LatitudeDegres.Value, (int)LatitudeMinute.Value, (float)LatitudeSeconde.Value, (float)Altitude.Value);
                App.ObservationPointsViewModel.ListeLieuxObservation.Add(newObservationPoint);
                App.DataProvider.UpdateObservationPoint(newObservationPoint);
                App.ObservationPointsViewModel.ListeLieuxObservation = App.ObservationPointsViewModel.ListeLieuxObservation; // On force la notification
                App.ObservationPointsViewModel.NombreMaxLieuObservationNonAtteint = App.ObservationPointsViewModel.NombreMaxLieuObservationNonAtteint; // On force la notification
            }
            // Cas d'une modification
            else
            {
                int index = App.ObservationPointsViewModel.ListeLieuxObservation.IndexOf(App.ObservationPointsViewModel.LieuObservationEnCoursDeDefinition);
                int id = App.ObservationPointsViewModel.LieuObservationEnCoursDeDefinition.ID;
                bool lieuSelectionne = false;
                if (App.ObservationPointsViewModel.ListeLieuxObservation[index] == App.ObservationPointsViewModel.LieuObservationSelectionne)
                {
                    lieuSelectionne = true;                
                }
                LieuObservationModel modifiedObservationPoint = new LieuObservationModel(NomLieuObservation.Text, (int)LongitudeDegres.Value, (int)LongitudeMinute.Value, (float)LongitudeSeconde.Value, (int)LatitudeDegres.Value, (int)LatitudeMinute.Value, (float)LatitudeSeconde.Value, (float)Altitude.Value);
                modifiedObservationPoint.ID = id;
                App.ObservationPointsViewModel.ListeLieuxObservation.RemoveAt(index);
                App.ObservationPointsViewModel.ListeLieuxObservation.Insert(index, modifiedObservationPoint);
                if (lieuSelectionne)
                {
                    App.ObservationPointsViewModel.LieuObservationSelectionne = App.ObservationPointsViewModel.ListeLieuxObservation[index];
                }
                App.DataProvider.UpdateObservationPoint(modifiedObservationPoint);
                App.ObservationPointsViewModel.ListeLieuxObservation = App.ObservationPointsViewModel.ListeLieuxObservation; // On force la notification
            }
            App.ObservationPointsViewModel.LieuObservationEnCoursDeDefinition = null;
            PickerObservationPoints.SelectedItem = null;
        }

        // Méthode permettant de récupérer le lieu d'observation existant et de l'afficher
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            LieuObservationModel selectedItem = (LieuObservationModel)((Picker)sender).SelectedItem;
            App.ObservationPointsViewModel.LieuObservationEnCoursDeDefinition = selectedItem;
        }

        // Méthode permettant de forcer la non sélection du contrôle Picker (réinitialisation) 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PickerObservationPoints.SelectedItem = null;
        }
    }
}