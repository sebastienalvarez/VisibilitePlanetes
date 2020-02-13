/****************************************************************************************************************************************
 * 
 * Classe SunInfoPage
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page des données de visibilité avec le Soleil.
 *         C'est une des pages de la page à onglets InfoPage.
 * 
 ****************************************************************************************************************************************/

using VisibilitePlanetes.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SunInfoPage : ContentPage
	{
		public SunInfoPage ()
		{
			InitializeComponent ();
		}

        // Méthode permettant de recalculer les données de visibilité (pour raffraichissement)
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SunInfoViewModel vm = null;
            if (BindingContext != null)
            {
                vm = (SunInfoViewModel)BindingContext;
            }

            if(vm != null)
            {
                vm.Compute();
            }
        }
    }
}