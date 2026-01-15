using System.Security.Cryptography;
using System.Text;
using DailyInk.Data;
using DailyInk.Models;

namespace DailyInk.Services;

public class SecurityService
{
    private readonly AppDatabase _db;
    private SecuritySettings? _settings;

    // 🔑 Session state
    public bool IsUnlocked { get; private set; } = false;

    public SecurityService(AppDatabase db)
    {
        _db = db;
        _settings = _db.GetSecuritySettings();
    }

    public bool IsPinSet => _settings != null;

    public void SetPin(string pin)
    {
        var hash = HashPin(pin);

        _settings = new SecuritySettings
        {
            Id = 1,
            PinHash = hash
        };

        _db.SaveSecuritySettings(_settings);
        IsUnlocked = true; // unlock after setting
    }

    public bool VerifyPin(string pin)
    {
        if (_settings == null) return false;

        var isValid = _settings.PinHash == HashPin(pin);
        if (isValid)
        {
            IsUnlocked = true;
        }

        return isValid;
    }

    public void Lock()
    {
        IsUnlocked = false;
    }

    private static string HashPin(string pin)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(pin);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }
}
