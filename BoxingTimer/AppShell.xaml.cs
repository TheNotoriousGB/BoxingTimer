using Microsoft.Maui.Controls;

namespace BoxingTimer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Füge die MainPage als Shell-Element hinzu
            Items.Add(new ShellContent
            {
                Content = new MainPage()
            });
        }
    }
}

