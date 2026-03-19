using CategoryManagement.Helpers;
using CategoryManagement.Models;
using CategoryManagement.Repository;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace CategoryManagement.ViewModels
{
    public class CategoryFormViewModel : INotifyPropertyChanged
    {
        /*handle a property changed in VIEW*/
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /*for adding and editing a name in list*/
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /*for adding and editing a description in list*/
        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly CategoryRepository _repo;
        private readonly Action _closePopup;
        private readonly bool _isEditMode;
        private int _editingId;

        public CategoryFormViewModel(CategoryRepository repo, Category? category, Action closePopup)
        {
            _repo = repo;
            _closePopup = closePopup;

            if (category == null)
            {
                _isEditMode = false;
            }
            else
            {
                _isEditMode = true;
                _editingId = category.Id;
                Name = category.Name;
                Description = category.Description;
            }

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private void Save()
        {
            if (_isEditMode)
            {
                var updated = new Category
                {
                    Id = _editingId,
                    Name = (this.Name ?? string.Empty).Trim(),
                    Description = (this.Description ?? string.Empty).Trim(),
                };

                _repo.Update(updated);
            }
            else
            {
                var newCategory = new Category
                {
                    Name = (this.Name ?? string.Empty).Trim(),
                    Description = (this.Description ?? string.Empty).Trim(),
                };
                _repo.Insert(newCategory);
            }
            _closePopup();
        }

        private void Cancel()
        {
            _closePopup();
        }
    }
}
