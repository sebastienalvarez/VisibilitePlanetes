/****************************************************************************************************************************************
 * 
 * Classe ObservationPointsViewModel
 * Auteur : S. ALVAREZ
 * Date : 27-07-2019
 * Statut : Release
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter les données affichées pour la gestion des lieux d'observation dans la page.
 * 
 ****************************************************************************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using VisibilitePlanetes.Model;

namespace VisibilitePlanetes.ViewModel
{
    public class ObservationPointsViewModel : INotifyPropertyChanged
    {
        // PROPRIETES
        private const int NOMBRE_MAX_LIEU_OBSERVATION = 5;

        private LieuObservationModel lieuObservationSelectionne;
        /// <summary>
        /// Lieu d'obervation sélectionné : c'est le lieu d'observation utilisé pour les calculs de visibilité
        /// </summary>
        public LieuObservationModel LieuObservationSelectionne
        {
            get { return lieuObservationSelectionne; }
            set
            {
                lieuObservationSelectionne = value;

                // Intégration de l'information dans ListeLieuxObservation
                lieuObservationSelectionne.LieuSelectionne = true;
                foreach(LieuObservationModel lieu in ListeLieuxObservation)
                {
                    if(lieu != lieuObservationSelectionne)
                    {
                        lieu.LieuSelectionne = false;
                    }
                }

                OnPropertyChanged("LieuObservationSelectionne");
            }
        }

        private LieuObservationModel lieuObservationGeolocalise;
        /// <summary>
        /// Lieu d'observation spécifique à la récupération de la géolocalistaion du téléphone, cet objet peut être nul si la géolocalisation n'est pas autorisée ou n'est pas activée
        /// </summary>
        public LieuObservationModel LieuObservationGeolocalise
        {
            get { return lieuObservationGeolocalise; }
            set
            {
                lieuObservationGeolocalise = value;
                OnPropertyChanged("LieuObservationGeolocalise");
            }
        }

        private LieuObservationModel lieuObservationEnCoursDeDefinition;
        /// <summary>
        /// Lieu d'observation pour une création ou une modification d'un lieu d'observation
        /// </summary>
        public LieuObservationModel LieuObservationEnCoursDeDefinition
        {
            get { return lieuObservationEnCoursDeDefinition; }
            set
            {
                lieuObservationEnCoursDeDefinition = value;
                LieuObservationADefinir = LieuObservationADefinir; // On force l'affectation pour forcer l'envoi de la notification à l'IU
                OnPropertyChanged("LieuObservationEnCoursDeDefinition");
            }
        }

        private bool lieuObservationADefinir;
        /// <summary>
        /// Flag permettant d'activer ou de griser les contrôles de saisie des données d'un lieu d'observation
        /// </summary>
        public bool LieuObservationADefinir
        {
            get
            {
                if(LieuObservationEnCoursDeDefinition == null)
                {
                    lieuObservationADefinir = false;
                }
                else
                {
                    lieuObservationADefinir = true;
                }
                return lieuObservationADefinir;
            }
            set
            {
                lieuObservationADefinir = value;
                OnPropertyChanged("LieuObservationADefinir");
            }
        }


        private bool nombreMaxLieuObservationNonAtteint;
        /// <summary>
        /// Flag permettant d'activer ou de griser une commande d'ajout de lieu d'observation en fonction du nombre de lieux d'observation existants (sans la Géolocalisation) et du nombre maximum autorisé de lieux d'observation (NOMBRE_MAX_LIEU_OBSERVATION)
        /// </summary>
        public bool NombreMaxLieuObservationNonAtteint
        {
            get
            {
                nombreMaxLieuObservationNonAtteint = ListeLieuxObservation.Where(lo => lo.LieuObservation.NomLieuObservation != "Géolocalisation").ToList().Count < NOMBRE_MAX_LIEU_OBSERVATION;
                return nombreMaxLieuObservationNonAtteint;
            }
            set
            {
                nombreMaxLieuObservationNonAtteint = value;
                OnPropertyChanged("NombreMaxLieuObservationNonAtteint");
            }
        }

        private ObservableCollection<LieuObservationModel> listeLieuxObservation;
        /// <summary>
        /// Collection des lieux d'observation existants (y compris la Géolocalisation)
        /// </summary>
        public ObservableCollection<LieuObservationModel> ListeLieuxObservation
        {
            get { return listeLieuxObservation; }
            set
            {
                listeLieuxObservation = value;
                OnPropertyChanged("ListeLieuxObservation");
            }
        }


        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public ObservationPointsViewModel(List<LieuObservationModel> a_listeLieuxObservation)
        {
            ListeLieuxObservation = new ObservableCollection<LieuObservationModel>(a_listeLieuxObservation);

            foreach(LieuObservationModel lieu in ListeLieuxObservation)
            {
                if (lieu.LieuSelectionne)
                {
                    LieuObservationSelectionne = lieu;
                }
            }

            if(LieuObservationSelectionne == null)
            {
                LieuObservationSelectionne = ListeLieuxObservation[0];
            }
        }

        // METHODES
        // Méthode permettant l'envoi de notification de changement de valeur pour le Data Binding
        private void OnPropertyChanged(string a_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(a_propertyName));
            }
        }
    }
}