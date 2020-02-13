using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VisibilitePlanetes.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Utilisation d'un fichier de base de données SQLite pour la source de données
            // Si un autre mécanisme de sauvegarde des données est à utiliser dans le futur : il faut modifier le chemin ici ainsi que dans les autres projets spécifiques plateforme, ainsi que la propriété DataProvider de la classe App dans le projet commun
            string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "VisibilitePlanetes.sqlite");

            LoadApplication(new VisibilitePlanetes.App(filePath));
        }
    }
}
