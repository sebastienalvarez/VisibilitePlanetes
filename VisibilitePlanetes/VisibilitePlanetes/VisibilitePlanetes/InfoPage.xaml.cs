/****************************************************************************************************************************************
 * 
 * Classe InfoPage
 * Auteur : S. ALVAREZ
 * Date : 28-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page à onglets conteneur des pages de données de visibilité.
 * 
 ****************************************************************************************************************************************/

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : TabbedPage
    {
        public InfoPage ()
        {
            InitializeComponent();
        }

        // Méthode redéfinie permettant de recréer les pages des onglets à chaque appel de cette page
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Initialisation des 2 pages liées et initialisation du Binding Context
            this.Children.Clear();
            SunInfoPage pageSunInfo = new SunInfoPage()
            {
                Title = "Soleil",
                BindingContext = App.SunInfoViewModel
            };

            MoonAndPlanetInfoPage pageMoonAndPlanetsInfo = new MoonAndPlanetInfoPage()
            {
                Title = "Lune et planètes",
                BindingContext = App.MoonAndPlanetInfoViewModel
            };

            this.Children.Add(pageMoonAndPlanetsInfo);
            this.Children.Add(pageSunInfo);
        }
    }
}