namespace Web3Multitool.ViewModels;

public class EditCexDialogViewModel : BaseViewModel
{

    private string? _address;

    public string? Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged(nameof(Address));
        }
    }

}