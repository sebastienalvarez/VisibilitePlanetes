/****************************************************************************************************************************************
 * 
 * Classe MoonAndPlanet
 * Auteur : S. ALVAREZ
 * Date : 29-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter les données affichées pour les informations de visibilité des corps célestes (Lune et 
 *         planètes) dans la page.
 * 
 ****************************************************************************************************************************************/

using AlgorithmesAstronomiques;

namespace VisibilitePlanetes.ViewModel
{
    public class MoonAndPlanet
    {
        // PROPRIETES
        public CorpsSystemeSolaire Planet { get; set; }
        public PlanetSelection Selection { get; set; }
        public double Phase { get; set; }
        public double Magnitude { get; set; }

        // CONSTRUCTEUR
        public MoonAndPlanet(CorpsSystemeSolaire a_planet, PlanetSelection a_Selection, double a_phase = -100, double a_magnitude = -100)
        {
            Planet = a_planet;
            Selection = a_Selection;
            Phase = a_phase;
            Magnitude = a_magnitude;
        }

        // METHODES
        // Pas de méthodes
    }
}