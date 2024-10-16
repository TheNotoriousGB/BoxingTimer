using Microsoft.Maui.Controls;

namespace BoxingTimer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            
            Items.Add(new ShellContent
            {
                Content = new MainPage()
            });
        }
    }
}

