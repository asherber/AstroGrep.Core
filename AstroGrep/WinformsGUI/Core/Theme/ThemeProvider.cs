using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroGrep.Core.Theme
{
	/// <summary>
	/// Reference to the current <see cref="ITheme"/>.
	/// </summary>
	/// <history>
	/// [Curtis_Beard]      08/26/2022	CHG: 146, created
	/// </history>
	public class ThemeProvider
	{
		private static ITheme theme;

		/// <summary>
		/// Available theme types for this application.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      08/26/2022	CHG: 146, created
		/// </history>
		public enum ThemeType
		{
			/// <summary>
			/// Follows system defined setting (Windows 10+)
			/// </summary>
			System = 0,

			/// <summary>
			/// A light/default color theme.
			/// </summary>
			Light = 1,

			/// <summary>
			/// A dark color theme.
			/// </summary>
			Dark = 2
		}

		/// <summary>
		/// The currently selected <see cref="ITheme"/>.  If non selected, then the <see cref="LightTheme"/> is loaded by default.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      08/26/2022	CHG: 146, created
		/// </history>
		public static ITheme Theme
		{
			get
			{
				if (theme == null)
				{
					if (Enum.TryParse(GeneralSettings.ThemeType.ToString(), out ThemeType themeType))
					{
						ChangeTheme(themeType);
					}
					else
					{
						ChangeTheme(ThemeType.System);
					}
				}

				return theme;
			}
			set
			{
				theme = value;
			}
		}

		/// <summary>
		/// Change the current them to the provide theme (based on theme code: 2 is dark, 1 is light, 0 is system).
		/// </summary>
		/// <param name="themeType">Desired <see cref="ThemeType"/></param>
		/// <history>
		/// [Curtis_Beard]      08/26/2022	CHG: 146, created
		/// </history>
		public static void ChangeTheme(ThemeType themeType)
		{
			switch (themeType)
			{
				case ThemeType.System:
					ChangeThemeBySystem();
					break;

				case ThemeType.Dark:
					theme = new DarkTheme();
					break;

				case ThemeType.Light:
				default:
					theme = new LightTheme();
					break;
			}
		}

		/// <summary>
		/// Change the theme based on the system value.
		/// </summary>
		public static void ChangeThemeBySystem()
		{
			if (Windows.Registry.GetSystemThemeType() == Windows.Registry.ThemeType.Light)
			{
				ChangeTheme(ThemeType.Light);
			}
			else
			{
				ChangeTheme(ThemeType.Dark);
			}
		}

		/// <summary>
		/// Reload the current <see cref="ITheme"/>.  If non selected, then the <see cref="LightTheme"/> is loaded by default.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      08/26/2022	CHG: 146, created
		/// </history>
		public static void Reload()
		{
			if (theme != null)
			{
				theme = (ITheme)Activator.CreateInstance(theme.GetType());
			}
			else
			{
				theme = new LightTheme();
			}
		}
	}
}