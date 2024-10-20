
namespace BoxingTimer
{
  
public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel.MainViewModel(Navigation);
        }


       

    }
    }


