using CategoryManagement.ViewModels;
using System.Windows;

namespace CategoryManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CategoryListViewModel();
        }

    }
}