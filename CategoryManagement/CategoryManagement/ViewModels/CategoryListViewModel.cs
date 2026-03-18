using CategoryManagement.Helpers;
using CategoryManagement.Models;
using CategoryManagement.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CategoryManagement.ViewModels
{
    public class CategoryListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private readonly CategoryRepository _repo = new();

        private ObservableCollection<Category> _categories = new();
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

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

        public CategoryListViewModel()
        {
            AddCommand = new RelayCommand(OpenAddForm);
            EditCommand = new RelayCommand<int>(OpenEditForm);
            DeleteCommand = new RelayCommand<int>(DeleteCategory);

            LoadCategories();
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
                category: category,
                closePopup: () =>
                {
                    IsFormVisible = false;
                    LoadCategories();
                });
            IsFormVisible = true;
        }

        private void OpenAddForm()
        {
            FormViewModel = new CategoryFormViewModel(
                            category: null,
                            closePopup: () =>
                            {
                                IsFormVisible = false;
                                LoadCategories();
                            });
            IsFormVisible = true;
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
    }
}
