/****************************************************************************************************************************************
 * 
 * Classe App
 * Auteur : S. ALVAREZ
 * Date : 28-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe de l'application créée par le template du projet.
 *         Application state réalisée par propriété static.
 * 
 ****************************************************************************************************************************************/

using VisibilitePlanetes.DataAccess;
using VisibilitePlanetes.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VisibilitePlanetes
{
    public partial class App : Application
    {
        // PROPRIETES STATIC
        /// <summary>
        /// Source des données enregistrées sur le téléphone
        /// </summary>
        public static IDataAccess DataProvider { get; set; }

        /// <summary>
        /// Objet View model pour la gestion des lieux d'observation (accessible dans toute l'application)
        /// </summary>
        public static ObservationPointsViewModel ObservationPointsViewModel { get; set; }

        /// <summary>
        /// Objet View model pour la sélection des corps célestes (accessible dans toute l'application)
        /// </summary>
        public static PlanetSelectionViewModel PlanetSelectionViewModel { get; set; }

        /// <summary>
        /// Objet View model pour pour l'affichage des données de visibilité du Soleil (accessible dans toute l'application)
        /// </summary>
        public static SunInfoViewModel SunInfoViewModel { get; set; }

        /// <summary>
        /// Objet View model pour pour l'affichage des données de visibilité de la Lune et des planètes (accessible dans toute l'application)
        /// </summary>
        public static MoonAndPlanetInfoViewModel MoonAndPlanetInfoViewModel { get; set; }

        // CONSTRUCTEUR
        public App(string a_filePath)
        {
            try
            {
                // Utilisation d'un fichier de base de données SQLite pour la source de données
                // Si un autre mécanisme de sauvegarde des données est à utiliser dans le futur : il faut modifier DataProvider ici, ainsi que le chemin vers le fichier dans les projets spécifiques plateforme
                DataProvider = new SQLiteDataAccess(a_filePath);

                InitializeComponent();

                // Récupération de la ressource de la couleur de fond
                Color backgoundColor = (Color)Resources["CouleurFondOrange"];

                MainPage = new NavigationPage(new MainPage());

                ((NavigationPage)MainPage).BarBackgroundColor = backgoundColor; // Nécessaire du fait de l'Aspect donné à l'image du bandeau supérieur (AspectFit), si l'image ne remplit pas en entier la zone du bandeau, la même couleur est appliquée
            }
            catch (System.Exception)
            {
                MainPage.DisplayAlert("Erreur inconnue", "Une erreur inconnue s'est produite, l'application va s'arrêter.", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}