/****************************************************************************************************************************************
 * 
 * Classe MoonAndPlanetInfoPage
 * Auteur : S. ALVAREZ
 * Date : 28-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page des données de visibilité avec la Lune et les planètes.
 *         C'est une des pages de la page à onglets InfoPage.
 * 
 ****************************************************************************************************************************************/

using System;
using VisibilitePlanetes.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoonAndPlanetInfoPage : ContentPage
	{
		public MoonAndPlanetInfoPage ()
		{
			InitializeComponent ();
		}

        // Méthode permettant de recalculer les données de visibilité (pour raffraichissement)
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Refresh();
        }

        // Méthode permettant de rafraichir la page suite à une action utilisateur
        private void ListView_Refreshing(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            Refresh();
            lv.IsRefreshing = false;
        }

        // Méthode permettant de rafraichir la page
        private void Refresh()
        {
            MoonAndPlanetInfoViewModel vm = null;
            if (BindingContext != null)
            {
                vm = (MoonAndPlanetInfoViewModel)BindingContext;
            }

            if (vm != null)
            {
                vm.MoonAndPlanetsList.Clear();
                vm.Compute();
            }
        }

        // Méthode permettant de désactiver la mise en évidence d'une sélection (inutile et peu esthétique dans cette application)
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}