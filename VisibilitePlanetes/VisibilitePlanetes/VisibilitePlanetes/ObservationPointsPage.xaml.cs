/****************************************************************************************************************************************
 * 
 * Classe ObservationPointsPage
 * Auteur : S. ALVAREZ
 * Date : 30-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page à onglets conteneur des pages de sélection et de gestion deslieux d'observation.
 * 
 ****************************************************************************************************************************************/

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObservationPointsPage : TabbedPage
    {
        public ObservationPointsPage ()
        {
            InitializeComponent();
        }

        // Méthode redéfinie permettant de recréer les pages des onglets à chaque appel de cette page
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Initialisation des 2 pages liées et initialisation du Binding Context
            this.Children.Clear();
            CurrentObservationPointPage pageCurrentObservationPoint = new CurrentObservationPointPage()
            {
                Title = "Sélection",
                BindingContext = App.ObservationPointsViewModel
            };

            ObservationPointsManagementPage pageObservationPointsManagement = new ObservationPointsManagementPage()
            {
                Title = "Gérer",
                BindingContext = App.ObservationPointsViewModel
            };

            this.Children.Add(pageCurrentObservationPoint);
            this.Children.Add(pageObservationPointsManagement);
        }
    }   
}