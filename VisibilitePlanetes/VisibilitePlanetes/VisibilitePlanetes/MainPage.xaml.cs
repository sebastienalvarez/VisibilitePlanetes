/****************************************************************************************************************************************
 * 
 * Classe MainPage
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page principale qui est également une NavigationPage avec menu.
 *         Le logique d'utilisation de la Géolocalisation est gérée par cette classe : 
 *         - à la première utilisation, une demande d'autorisation est faite, si oui la Géolocalisation peut être utilisée, si non
 *           au prochain lancement, un message d'information précisera que la Géolocalisation est nécessaire en indiquant comment
 *           l'activer
 *         - à l'apparition de cette page (quand on lance l'application), si la localisation est activée, on demande à l'utilisateur
 *           si il souhaite que la Géolocalisation soit utilisée pour les calculs (c'est alors le lieu d'observation sélectionné)
 *         - la position de la Géolocalisation est mise à jour dès que l'utilisateur se déplace à plus de 500m du point initial.
 * 
 ****************************************************************************************************************************************/

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisibilitePlanetes.Model;
using VisibilitePlanetes.ViewModel;
using Xamarin.Forms;

namespace VisibilitePlanetes
{
    public partial class MainPage : TabbedPage
    {
        // PROPRIETES
        /// <summary>
        /// Collection d'objet PlanetSelectionModel contenant les données enregistrées saisies précédemment par l'utilisateur
        /// </summary>
        public List<PlanetSelectionModel> SelectedPlanets { get; }

        /// <summary>
        /// Collection d'objet LieuObservationModel contenant les données enregistrées des lieux d'observation définis précédemment par l'utilisateur
        /// </summary>
        public List<LieuObservationModel> ObservationPoints { get; }

        // CONSTRUCTEUR
        public MainPage()
        {
            InitializeComponent();

            // Récupération des données utilisateur des lieux d'observation et de sélection des corps célestes
            ObservationPoints = App.DataProvider.GetObservationPoints();
            SelectedPlanets = App.DataProvider.GetSelectedPlanets();

            // Création du lieu d'observation par défaut (Paris) (1ère utilisation ou fichier non accessible)
            if (ObservationPoints == null)
            {
                ObservationPoints = new List<LieuObservationModel>();
                ObservationPoints.Add(new LieuObservationModel("Paris (Défaut)", 2, 20, 55f, 48, 51, 12f, 0f));
            }

            // Création des données de sélection des corps célestes par défaut (1ère utilisation ou fichier non accessible)
            if (SelectedPlanets == null)
            {
                SelectedPlanets = new List<PlanetSelectionModel>();
                SelectedPlanets.Add(new PlanetSelectionModel(1, "La Lune", true));
                SelectedPlanets.Add(new PlanetSelectionModel(2, "Mercure", true));
                SelectedPlanets.Add(new PlanetSelectionModel(3, "Venus", true));
                SelectedPlanets.Add(new PlanetSelectionModel(4, "Mars", true));
                SelectedPlanets.Add(new PlanetSelectionModel(5, "Jupiter", true));
                SelectedPlanets.Add(new PlanetSelectionModel(6, "Saturne", true));
                SelectedPlanets.Add(new PlanetSelectionModel(7, "Uranus", true));
                SelectedPlanets.Add(new PlanetSelectionModel(8, "Neptune", true));
            }

            // Création de l'objet ViewModel pour la gestion des lieux d'observation et création du BindingContext
            App.ObservationPointsViewModel = new ObservationPointsViewModel(ObservationPoints);
            PageObservationPoints.BindingContext = App.ObservationPointsViewModel;

            // Création de l'objet ViewModel pour la sélection des corps céleste et création du BindingContext
            App.PlanetSelectionViewModel = new PlanetSelectionViewModel(SelectedPlanets);
            PagePlanetSelection.BindingContext = App.PlanetSelectionViewModel;

            // Création des objets ViewModel pour l'affichage des données de visibilité
            App.SunInfoViewModel = new SunInfoViewModel();
            App.MoonAndPlanetInfoViewModel = new MoonAndPlanetInfoViewModel();

            // Sauvegarde des données
            if (App.DataProvider.UpdateObservationPoints(ObservationPoints) == -1 || App.DataProvider.UpdateSelectedPlanets(App.PlanetSelectionViewModel) == -1)
            {
                DisplayAlert("Erreur I/O", "Les données utilisateur ne peuvent pas être sauvegardées : problème d'accès disque", "Ok");
            }
        }

        // METHODES
        // Méthode permettant de demander la permission pour récupérer la géolocalisation du téléphone
        private async Task GetPermissions()
        {
            try
            {
                App.ObservationPointsViewModel.LieuObservationGeolocalise = null;
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Géolocalisation nécessaire", "L'accès à la géolocalistion du téléphone est nécessaire pour calculer la visibilité des corps célestes à votre position géographique. Aller dans Paramètres > Applications > Visibilite Planetes et activer la permission de localisation.", "Ok");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (results.ContainsKey(Permission.Location))
                    {
                        status = results[Permission.Location];
                    }
                }
                // La permission a été donnée
                if (status == PermissionStatus.Granted)
                {
                    var locator = CrossGeolocator.Current;
                    locator.PositionChanged += GetGeolocation;
                    await locator.StartListeningAsync(TimeSpan.Zero, 500);
                    var phonePosition = await locator.GetPositionAsync();
                    ComputePhonePosition(phonePosition);
                }
            }
            catch (System.Exception)
            {
                await DisplayAlert("Erreur", "Erreur inconnue dans la géolocalisation...", "Ok");
            }
        }

        // Méthode permettant de mettre à jour la position géolocalisé en cas de déplacement
        private void GetGeolocation(object sender, PositionEventArgs e)
        {
            ComputePhonePosition(e.Position);
        }

        // Méthode permettant d'extraire la position géographique du GPS du téléphone dans App.ObservationPointsViewModel.LieuObservationGeolocalise
        private void ComputePhonePosition(Position a_phonePosition)
        {
            if (a_phonePosition != null)
            {
                int longitudeDegre = (int)a_phonePosition.Longitude;
                int longitudeMinute = (int)(60.0 * (a_phonePosition.Longitude - (double)longitudeDegre));
                float longitudeSeconde = (float)(60.0 * (60.0 * (a_phonePosition.Longitude - (double)longitudeDegre) - (double)longitudeMinute));
                int latitudeDegre = (int)a_phonePosition.Latitude;
                int latitudeMinute = (int)(60.0 * (a_phonePosition.Latitude - (double)latitudeDegre));
                float latitudeSeconde = (float)(60.0 * (60.0 * (a_phonePosition.Latitude - (double)latitudeDegre) - (double)latitudeMinute));

                if (App.ObservationPointsViewModel.LieuObservationGeolocalise != null)
                {
                    bool isSelected = App.ObservationPointsViewModel.LieuObservationGeolocalise.LieuSelectionne;
                    if (App.ObservationPointsViewModel.ListeLieuxObservation.Contains(App.ObservationPointsViewModel.LieuObservationGeolocalise))
                    {
                        App.ObservationPointsViewModel.ListeLieuxObservation.Remove(App.ObservationPointsViewModel.LieuObservationGeolocalise);
                    }
                    App.ObservationPointsViewModel.LieuObservationGeolocalise = new LieuObservationModel("Géolocalisation", longitudeDegre, longitudeMinute, longitudeSeconde, latitudeDegre, latitudeMinute, latitudeSeconde, (float)a_phonePosition.Altitude);
                    App.ObservationPointsViewModel.ListeLieuxObservation.Add(App.ObservationPointsViewModel.LieuObservationGeolocalise);
                    if(isSelected){
                        App.ObservationPointsViewModel.LieuObservationSelectionne = App.ObservationPointsViewModel.LieuObservationGeolocalise;
                    }
                }
                else
                {
                    App.ObservationPointsViewModel.LieuObservationGeolocalise = new LieuObservationModel("Géolocalisation", longitudeDegre, longitudeMinute, longitudeSeconde, latitudeDegre, latitudeMinute, latitudeSeconde, (float)a_phonePosition.Altitude);
                    App.ObservationPointsViewModel.ListeLieuxObservation.Add(App.ObservationPointsViewModel.LieuObservationGeolocalise);
                }
                App.ObservationPointsViewModel.LieuObservationSelectionne = App.ObservationPointsViewModel.LieuObservationSelectionne; // On force la notification
                App.ObservationPointsViewModel.ListeLieuxObservation = App.ObservationPointsViewModel.ListeLieuxObservation; // On force la notification              
            }
            else
            {               
                // Si il y a une erreur dans la récupération de la position : on converse la dernière position géolocalisée

                // Code ci-après conservé pour mémoire
                //if(App.ObservationPointsViewModel.LieuObservationGeolocalise != null && App.ObservationPointsViewModel.ListeLieuxObservation.Contains(App.ObservationPointsViewModel.LieuObservationGeolocalise))
                //{
                //    App.ObservationPointsViewModel.ListeLieuxObservation.Remove(App.ObservationPointsViewModel.LieuObservationGeolocalise);
                //}
                //if (App.ObservationPointsViewModel.LieuObservationGeolocalise.LieuSelectionne)
                //{
                //    App.ObservationPointsViewModel.LieuObservationSelectionne = App.ObservationPointsViewModel.ListeLieuxObservation[0];
                //}
                //App.ObservationPointsViewModel.LieuObservationGeolocalise = null;
            }
        }

        // Méthode permettant de demander la permission pour récupérer la géolocalisation du téléphone et si la permission a été donnée de récupérer la position du téléphone
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Récupération de la géolocalisation du téléphone
            await GetPermissions();

            if (App.ObservationPointsViewModel.LieuObservationGeolocalise != null && App.ObservationPointsViewModel.LieuObservationGeolocalise.LieuSelectionne == false)
            {
                var response = await DisplayAlert("Géolocalisation", "La position de votre téléphone a été récupérée, souhaitez-vous utiliser celle-ci pour les calculs ?", "Oui", "Non");
                if (response)
                {
                    App.ObservationPointsViewModel.LieuObservationSelectionne = App.ObservationPointsViewModel.LieuObservationGeolocalise;
                }
            }
        }

    }
}