namespace Web3Multitool.ViewModels;

public class EditManyCexDialogViewModel : BaseViewModel
{

    private string? _addresses;

    public string? Addresses
    {
        get => _addresses;
        set
        {
            _addresses = value;
            OnPropertyChanged(nameof(Addresses));
        }
    }

}