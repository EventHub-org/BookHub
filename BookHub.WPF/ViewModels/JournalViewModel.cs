using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class JournalViewModel : INotifyPropertyChanged
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;  // Injected AutoMapper
    private ObservableCollection<JournalEntryDto> _journalEntries;
    private int _currentPage;
    private const int _pageSize = 3;
    private int _totalPages;

    public JournalViewModel(IBookService bookService, IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;  // Initialized AutoMapper
        CurrentPage = 1;
        LoadJournalAsync().ConfigureAwait(false);
        PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
        NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
    }

    public ObservableCollection<JournalEntryDto> JournalEntries
    {
        get => _journalEntries;
        set
        {
            _journalEntries = value;
            OnPropertyChanged(nameof(JournalEntries));
        }
    }

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
            LoadJournalAsync().ConfigureAwait(false);
        }
    }

    public int TotalPages
    {
        get => _totalPages;
        private set
        {
            _totalPages = value;
            OnPropertyChanged(nameof(TotalPages));
        }
    }

    public ICommand PreviousPageCommand { get; }
    public ICommand NextPageCommand { get; }

    private async Task LoadJournalAsync()
    {
        var result = await _bookService.GetPaginatedBooksAsync(new Pageable { Page = CurrentPage, Size = _pageSize });

        if (result.Success)
        {
            // Map from BookDto to JournalEntryDto
            JournalEntries = new ObservableCollection<JournalEntryDto>(
                _mapper.Map<IEnumerable<BookDto>, IEnumerable<JournalEntryDto>>(result.Data.Items)
            );
            TotalPages = result.Data.TotalPages; // Assuming your service returns total pages
        }
    }

    private void PreviousPage()
    {
        if (CanGoToPreviousPage())
        {
            CurrentPage--;
        }
    }

    private void NextPage()
    {
        if (CanGoToNextPage())
        {
            CurrentPage++;
        }
    }

    private bool CanGoToPreviousPage() => CurrentPage > 1;

    private bool CanGoToNextPage() => CurrentPage < TotalPages;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
