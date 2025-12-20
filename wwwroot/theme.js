using System;
using System.Collections.Generic;
using System.Text;

namespace DailyInk.wwwroot
{
    class Theme
    {
        window.setThemeClass = (themeClass) => {
            document.body.className = themeClass || '';
        };

    }
}
