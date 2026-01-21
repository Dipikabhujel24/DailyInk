namespace DailyInk.Services;

public class SecurityService
{
    private const string PinKey = "dailyink_pin";

    public bool IsUnlocked { get; private set; }

    public bool IsPinSet =>
        Preferences.ContainsKey(PinKey);

    public void SetPin(string pin)
    {
        Preferences.Set(PinKey, pin);
        IsUnlocked = true;
    }

    public bool ValidatePin(string pin)
    {
        var savedPin = Preferences.Get(PinKey, "");

        if (pin == savedPin)
        {
            IsUnlocked = true;
            return true;
        }

        return false;
    }

    public void Lock()
    {
        IsUnlocked = false;
    }
}
