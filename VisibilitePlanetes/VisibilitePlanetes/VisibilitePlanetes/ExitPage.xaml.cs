/****************************************************************************************************************************************
 * 
 * Classe ExitPage
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter la page de sortie de l'application.
 * 
 ****************************************************************************************************************************************/

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisibilitePlanetes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExitPage : ContentPage
	{
		public ExitPage ()
		{
			InitializeComponent ();
		}

        // Méthode permettant de quitter l'application suite au clic sur le bouton Quitter
        private void Button_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}