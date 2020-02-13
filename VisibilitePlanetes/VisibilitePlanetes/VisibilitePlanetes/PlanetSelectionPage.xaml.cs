/****************************************************************************************************************************************
 * 
 * Classe PlanetSelectionPage
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page de sélection des corps célestes.
 * 
 ****************************************************************************************************************************************/

using VisibilitePlanetes.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlanetSelectionPage : ContentPage
	{
        // CONSTRUCTEUR
        public PlanetSelectionPage ()
		{
			InitializeComponent ();
		}

        // METHODES
        // Méthode permettant de sélectionner ou déselectionner un corps céleste
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentPlanet = (PlanetSelection)((ListView)sender).SelectedItem;
            currentPlanet.IsSelected = !currentPlanet.IsSelected;
            ((ListView)sender).SelectedItem = null; // Désactivation de la sélection (inutile et peu esthétique dans cette application)

            // Sauvegarde des données
            App.DataProvider.UpdateSelectedPlanet(currentPlanet);
            currentPlanet.ImageSource = currentPlanet.ImageSource; // Temporaire : je n'ai pas trouvé comment on fait le Data Binding sur l'item complet dans le ListView Cell...du coup ej force l'affectation pour envoyer la notification pour la MAJ de l'affichage par Data binding
        }
    }
}