using System;
using System.Collections.Generic;
using System.Text;

namespace DailyInk.Services
{
    public class ThemeService
    {
        public bool IsDark { get; private set; }

        public event Action? OnThemeChanged;

        public void ToggleTheme()
        {
            IsDark = !IsDark;
            OnThemeChanged?.Invoke();
        }

        public string CssClass => IsDark ? "dark-theme" : "light-theme";
    }
}
