using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System.ComponentModel;

namespace BookHub.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IBookService _bookService;
        private ObservableCollection<BookDto> _books;

        public ObservableCollection<BookDto> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        public ICommand LoadBooksCommand { get; }

        public MainViewModel(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            LoadBooksCommand = new RelayCommand(async () => await LoadBooks());
        }

        private async Task LoadBooks()
        {
            Debug.WriteLine("Loading books...");
            var pageable = new Pageable(size: 2, page: 1);
            var result = await _bookService.GetPaginatedBooksAsync(pageable);

            if (result.Success)
            {
                Books = new ObservableCollection<BookDto>(result.Data.Items);
            }
            else
            {
                // Обробка помилок (можливо, через діалогове вікно)
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Команда реалізації
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public async void Execute(object parameter) => await _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
