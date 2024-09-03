

namespace BoxingTimer
{
  
        public partial class MainPage : ContentPage
        {
            public MainPage()
            {
                InitializeComponent();
                BindingContext = new ViewModel.MainViewModel();
            }

            // Wenn du eine Logik hast, die bei einem Button-Klick oder einem anderen Event ausgeführt werden soll, kannst du diese in der Code-Behind-Datei definieren
        }
    }


