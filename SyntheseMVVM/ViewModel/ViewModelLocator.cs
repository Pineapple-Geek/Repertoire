using SyntheseMVVM.View;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntheseMVVM;

namespace SyntheseMVVM.ViewModel
{
   class ViewModelLocator
  {
    private const string MainPage = "main";
    private const string AjouterContact = "pageAjouter";

    public ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      ////if (ViewModelBase.IsInDesignModeStatic)
      ////{
      ////    // Create design time view services and models
      ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
      ////}
      ////else
      ////{
      ////    // Create run time view services and models
      ////    SimpleIoc.Default.Register<IDataService, DataService>();
      ////}

      SimpleIoc.Default.Register<MainViewModel>();
      SimpleIoc.Default.Register<AjouterViewModel>();
      setUpNavigation();
    }

    private static void setUpNavigation()
    {
      var navigation = new NavigationService();
      navigation.Configure("main", typeof(MainPage));
      navigation.Configure("pageAjouter", typeof(AjouterPage));
      SimpleIoc.Default.Register<INavigationService>(() => navigation);
    }

    public MainViewModel Main
    {
      get
      {
        return ServiceLocator.Current.GetInstance<MainViewModel>();
      }
    }

    public AjouterViewModel Ajouter
    {
      get
      {
        return ServiceLocator.Current.GetInstance<AjouterViewModel>();
      }
    }
  }
}
