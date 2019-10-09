using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SyntheseMVVM.Model;
using SyntheseMVVM.View;
using Windows.UI.Popups;
using System.IO;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace SyntheseMVVM.ViewModel
{
  class AjouterViewModel:ViewModelBase
  {
    #region Attributs
    private RelayCommand mainCommand;
    private RelayCommand validerCommand;
    private readonly INavigationService navigationService;
    private List<Contact> contact = new List<Contact>();
    private string tbNom, tbPrenom, tbNum, tbMail;
    #endregion

    #region Propriétés
    public RelayCommand NavigateCommand { get; private set; }
    #endregion
    
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

    public AjouterViewModel(INavigationService navigationService)
    {
      this.navigationService = navigationService;
    }

    #region Commandes

    public RelayCommand MainCommand
    {
      get
      {
        return mainCommand ?? (mainCommand = new RelayCommand(MainAppli, CanMain));
      }
    }

    public RelayCommand ValiderCommand
    {
      get
      {
        return validerCommand ?? (validerCommand = new RelayCommand(ValiderContact, CanValider));
      }
    }


    #endregion

    #region Methodes des commandes

    private void MainAppli()
    {
      Prenom = "";
      Nom = "";
      Num = "";
      Mail = "";
      navigationService.GoBack();
    }

    public async void ValiderContact()
    {
      var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
      var folderPath = localFolder.Path;
      var chemin = Path.Combine(folderPath, @"Contact.bin");

      int test = 0;
      bool isNumberTel = int.TryParse(tbNum, out test);
      bool isContainsMail = tbMail.Contains("@");

      if (isNumberTel == true && isContainsMail == true)
      {
        if (File.Exists(chemin) == true)
        {
          contact = (List<Contact>)Serialisation.Deserialiser(chemin);
          contact.Add(new Contact() { Nom = tbNom, Prenom = tbPrenom, Numtel = tbNum, Mail = tbMail });
          Serialisation.Serialiser(contact, chemin);
        }
        else
        {
          contact.Add(new Contact() { Nom = tbNom, Prenom = tbPrenom, Numtel = tbNum, Mail = tbMail });
          Serialisation.Serialiser(contact, chemin);
        }
        MessengerInstance.Send<NotificationMessage>(new NotificationMessage("ajouterContact"));
        Prenom = "";
        Nom = "";
        Num = "";
        Mail = "";
        navigationService.GoBack();
      }
      else
      {
        if (isNumberTel == false)
        {
          var dialog = new MessageDialog("Le numéro de téléphone est incorrect!");
          await dialog.ShowAsync();
        }
        else
        {
          var dialog = new MessageDialog("Le mail est incorrect!");
          await dialog.ShowAsync();
        }
      }
    }

    private bool CanMain()
    {
      return true;
    }

    private bool CanValider()
    {
      return true;
    }
    #endregion
  }
}
