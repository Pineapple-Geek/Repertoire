using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using SyntheseMVVM.Model;

namespace SyntheseMVVM.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    #region Attributs
    private RelayCommand ajouterCommand;
    private RelayCommand supprimerCommand;
    private Model.Contact contactSelectionne;
    private List<Contact> contact = new List<Contact>();
    private string tbNom, tbPrenom, tbNum, tbMail, recherche;
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly INavigationService navigationService;
    public string chemin;
    #endregion

    #region Propriétés
    public RelayCommand NavigateCommand { get; private set; }

    public string Nom
    {
      get { return tbNom; }
      set
      {
        Set(() => Nom, ref tbNom, value);//j'appelle la méthode set pour mettre à jour mon attribut

      }
    }

    public string Prenom
    {
      get { return tbPrenom; }
      set
      {
        Set(() => Prenom, ref tbPrenom, value);//j'appelle la méthode set pour mettre à jour mon attribut

      }
    }

    public string Num
    {
      get { return tbNum; }
      set
      {
        Set(() => Num, ref tbNum, value);//j'appelle la méthode set pour mettre à jour mon attribut

      }
    }

    public string Mail
    {
      get { return tbMail; }
      set
      {
        Set(() => Mail, ref tbMail, value);//j'appelle la méthode set pour mettre à jour mon attribut

      }
    }

    public Model.Contact ContactSelectionne
    {
      get { return contactSelectionne; }
      set
      {
        Set(() => ContactSelectionne, ref contactSelectionne, value);
        OnPropertyChanged("ContactSelectionne");
        if (contactSelectionne != null)
        {
          Nom = contactSelectionne.Nom;
          Prenom = contactSelectionne.Prenom;
          Num = contactSelectionne.Numtel;
          Mail = contactSelectionne.Mail;

        }
        Messenger.Default.Send<NotificationMessage>(new NotificationMessage("afficherTextBlock"));
      }
    }

    public List<Contact> Contact
    {
      get { return contact; }
      set
      {
        Set(() => Contact, ref contact, value);
        OnPropertyChanged("Contact");
      }
    }

    public string Recherche
    {
      get { return recherche; }
      set
      {
        Set(() => Recherche, ref recherche, value);
        OnPropertyChanged("Recherche");
        if (recherche != "")
        {
          List<Contact> contacts = (List<Model.Contact>)Serialisation.Deserialiser(chemin);
          List<Contact> findContact = new List<Contact>(Contact.FindAll(x => x.Nom.Contains(recherche)));
          Contact = findContact;
        }
        else
        {
          Contact = (List<Model.Contact>)Serialisation.Deserialiser(chemin);
        }

      }
    }
    #endregion

    public MainViewModel(INavigationService navigationService)
    {
      this.navigationService = navigationService;
      MessengerInstance.Register<NotificationMessage>(this, ReceiveMessage);
      Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
      string folderPath = localFolder.Path;
      chemin = Path.Combine(folderPath, @"Contact.bin");
      if(File.Exists(chemin) == true)
      {
        Contact = (List<Model.Contact>)Serialisation.Deserialiser(chemin);
      }
    }

    #region Commandes

    public RelayCommand AjouterCommand
    {
      get
      {
        return ajouterCommand ?? (ajouterCommand = new RelayCommand(AjouterAppli, CanAjouter));
      }
    }

    public RelayCommand SupprimerCommand
    {
      get
      {
        return supprimerCommand ?? (supprimerCommand = new RelayCommand(SupprimerContact, CanSupprimer));
      }
    }

    


    #endregion

    #region Methodes des commandes

    private void AjouterAppli()
    {
      navigationService.NavigateTo("pageAjouter");
    }

    private void SupprimerContact()
    {
      if (ContactSelectionne.Nom != null)
      {
        Contact.Remove(ContactSelectionne as Contact);
        Update();
      }
    }

    private bool CanAjouter()
    {
      return true;
    }

    private bool CanSupprimer()
    {
      return true;
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public void Update()
    {
      if (Contact.Count() != 0)
      {
        Serialisation.Serialiser(Contact, chemin);
        Contact = (List<Model.Contact>)Serialisation.Deserialiser(chemin);
      }
      else
      {
        File.Delete(chemin);
      }
    }

    public void ReceiveMessage(NotificationMessage notificationMessage)
    {
      string notification = notificationMessage.Notification;
      if (notification == "ajouterContact")
      {
        if (File.Exists(chemin) == true)
        {
          Contact = (List<Model.Contact>)Serialisation.Deserialiser(chemin);
        }
      }
      //do your work
    }
    #endregion
  }
}
