/****************************************************************************************************************************************
 * 
 * Classe SplashScreenActivity
 * Auteur : S. ALVAREZ
 * Date : 01-08-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant d'ajouter une activity Android pour ajouter un Splash Screen.
 * 
 ****************************************************************************************************************************************/

using Android.App;
using Android.OS;

namespace VisibilitePlanetes.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}