using CategoryManagement.DTOs;
using CategoryManagement.Helpers;
using CategoryManagement.Models;
using CategoryManagement.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CategoryManagement.ViewModels
{
    public class CategoryListViewModel : INotifyPropertyChanged
    {
        /*handle a property changed in VIEW*/
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /*manage a categories of list and observe to change in list of not*/
        private ObservableCollection<CategoryDto> _categories = new();
        public ObservableCollection<CategoryDto> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        /*manage a popup form visible*/
        private bool _isFormVisible;
        public bool IsFormVisible
        {
            get { return _isFormVisible; }
            set
            {
                _isFormVisible = value;
                OnPropertyChanged(nameof(IsFormVisible));
            }
        }

        /*for adding and editing a category manage*/
        private CategoryFormViewModel? _formViewModel;
        public CategoryFormViewModel? FormViewModel
        {
            get { return _formViewModel; }
            set
            {
                _formViewModel = value;
                OnPropertyChanged(nameof(FormViewModel));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly CategoryRepository _repo;

        public CategoryListViewModel(CategoryRepository categoryRepository)
        {
            _repo = categoryRepository;

            AddCommand = new RelayCommand(OpenAddForm);
            EditCommand = new RelayCommand<int>(OpenEditForm);
            DeleteCommand = new RelayCommand<int>(DeleteCategory);

            LoadCategories();
        }

        private void LoadCategories()
        {
            var data = _repo.GetAllActive();

            Categories.Clear();
            foreach (var item in data)
            {
                Categories.Add(item);
            }
        }

        private void OpenAddForm()
        {
            FormViewModel = new CategoryFormViewModel(
                            repo: _repo,
                            category: null,
                            closePopup: () =>
                            {
                                IsFormVisible = false;
                                LoadCategories();
                            });
            IsFormVisible = true;
        }

        private void OpenEditForm(int id)
        {
            var category = _repo.GetById(id);
            if (category == null)
            {
                MessageBox.Show("Category not found.",
                        "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                LoadCategories();
                return;
            }
            FormViewModel = new CategoryFormViewModel(
                repo: _repo,
                category: category,
                closePopup: () =>
                {
                    IsFormVisible = false;
                    LoadCategories();
                });
            IsFormVisible = true;
        }

        private void DeleteCategory(int id)
        {
            var category = Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return;

            var result = MessageBox.Show(
                            $"Are you sure you want to delete \"{category.Name}\"?\n\nThis action cannot be undone.",
                            "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return;

            _repo.SoftDelete(id);
            LoadCategories();
        }
    }
}
