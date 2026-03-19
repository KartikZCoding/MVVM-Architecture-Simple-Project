using AutoMapper;
using CategoryManagement.Mapping;
using CategoryManagement.Repository;
using CategoryManagement.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Windows;

namespace CategoryManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<CategoryProfile>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            IMapper mapper = mapperConfig.CreateMapper();

            var repo = new CategoryRepository(mapper);

            var viewModel = new CategoryListViewModel(repo);

            var mainWindow = new MainWindow();
            mainWindow.DataContext = viewModel;
            mainWindow.Show();
        }
    }
}
