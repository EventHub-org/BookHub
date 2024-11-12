using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class JournalViewModel : INotifyPropertyChanged
{
    private readonly IBookService _bookService;
    private readonly IReadingProgressService _readingProgressService;
    private readonly UserDto _userDto;
    private ObservableCollection<JournalEntryDto> _journalEntries;
    //private 
    private int _currentPage;
    private const int _pageSize = 3;
    private int _totalPages;

    public JournalViewModel(UserDto user, IReadingProgressService readingProgressService, IBookService bookService)
    {
        _readingProgressService = readingProgressService;
        CurrentPage = 1;
        _bookService = bookService;
        //LoadJournalAsync().ConfigureAwait(false);
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
            //LoadJournalAsync().ConfigureAwait(false);
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
        var result = await _readingProgressService.GetReadingProgressByUserIdAsync(_userDto.Id);

        if (result.Success)
        {
            // Initialize an empty collection to hold journal entries
            var journalEntries = new List<JournalEntryDto>();

            for (var i = 0; i < result.Data.Count; i++)
            {
                var readingProgress = result.Data[i];

                // Fetch the book details for each reading progress entry
                var bookResult = await _bookService.GetBookAsync(readingProgress.BookId);

                if (bookResult.Success && bookResult.Data != null)
                {
                    // Manually create a JournalEntryDto instance
                    var journalEntry = new JournalEntryDto
                    {
                        BookTitle = bookResult.Data.Title, // Assuming BookDto has a Title property
                        Progress = $"{readingProgress.CurrentPage} pages read",
                        LastOpened = readingProgress.DateFinished.HasValue
                            ? readingProgress.DateFinished.Value.ToString("g")
                            : "Not finished"
                    };

                    // Add the created JournalEntryDto to the list
                    journalEntries.Add(journalEntry);
                }
            }

            // Assign the populated list to JournalEntries as an ObservableCollection
            JournalEntries = new ObservableCollection<JournalEntryDto>(journalEntries);
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
