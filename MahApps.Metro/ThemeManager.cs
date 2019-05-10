using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;
using System.Windows;

namespace MahApps.Metro
{
	public static class ThemeManager
	{
		private static IList<Accent> _accents;

		private static IList<AppTheme> _appThemes;

		public static IEnumerable<Accent> Accents
		{
			get
			{
				if (ThemeManager._accents != null)
				{
					return ThemeManager._accents;
				}
				string[] strArrays = new string[] { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
				ThemeManager._accents = new List<Accent>((int)strArrays.Length);
				try
				{
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						string str = strArrays1[i];
						Uri uri = new Uri(string.Format("pack://application:,,,/小喵谷登入器;component/Styles/Accents/{0}.xaml", str));
						ThemeManager._accents.Add(new Accent(str, uri));
					}
				}
				catch (Exception exception)
				{
					throw new MahAppsException("This exception happens because you are maybe running that code out of the scope of a WPF application. Most likely because you are testing your configuration inside a unit test.", exception);
				}
				return ThemeManager._accents;
			}
		}

		public static IEnumerable<AppTheme> AppThemes
		{
			get
			{
				if (ThemeManager._appThemes != null)
				{
					return ThemeManager._appThemes;
				}
				string[] strArrays = new string[] { "BaseLight", "BaseDark" };
				ThemeManager._appThemes = new List<AppTheme>((int)strArrays.Length);
				try
				{
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						string str = strArrays1[i];
						Uri uri = new Uri(string.Format("pack://application:,,,/小喵谷登入器;component/Styles/Accents/{0}.xaml", str));
						ThemeManager._appThemes.Add(new AppTheme(str, uri));
					}
				}
				catch (Exception exception)
				{
					throw new MahAppsException("This exception happens because you are maybe running that code out of the scope of a WPF application. Most likely because you are testing your configuration inside a unit test.", exception);
				}
				return ThemeManager._appThemes;
			}
		}

		public static bool AddAccent(string name, Uri resourceAddress)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceAddress == null)
			{
				throw new ArgumentNullException("resourceAddress");
			}
			if (ThemeManager.GetAccent(name) != null)
			{
				return false;
			}
			ThemeManager._accents.Add(new Accent(name, resourceAddress));
			return true;
		}

		public static bool AddAppTheme(string name, Uri resourceAddress)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceAddress == null)
			{
				throw new ArgumentNullException("resourceAddress");
			}
			if (ThemeManager.GetAppTheme(name) != null)
			{
				return false;
			}
			ThemeManager._appThemes.Add(new AppTheme(name, resourceAddress));
			return true;
		}

		[SecurityCritical]
		private static void ApplyResourceDictionary(ResourceDictionary newRd, ResourceDictionary oldRd)
		{
			oldRd.BeginInit();
			foreach (DictionaryEntry dictionaryEntry in newRd)
			{
				if (oldRd.Contains(dictionaryEntry.Key))
				{
					oldRd.Remove(dictionaryEntry.Key);
				}
				oldRd.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
			oldRd.EndInit();
		}

		private static bool AreResourceDictionarySourcesEqual(Uri first, Uri second)
		{
			return Uri.Compare(first, second, UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		[SecurityCritical]
		public static void ChangeAppStyle(Application app, Accent newAccent, AppTheme newTheme)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(app);
			ThemeManager.ChangeAppStyle(app.Resources, tuple, newAccent, newTheme);
		}

		[SecurityCritical]
		public static void ChangeAppStyle(Window window, Accent newAccent, AppTheme newTheme)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(window);
			ThemeManager.ChangeAppStyle(window.Resources, tuple, newAccent, newTheme);
		}

		[SecurityCritical]
		private static void ChangeAppStyle(ResourceDictionary resources, Tuple<AppTheme, Accent> oldThemeInfo, Accent newAccent, AppTheme newTheme)
		{
			bool flag = false;
			if (oldThemeInfo == null)
			{
				ThemeManager.ChangeAppStyle(resources, newAccent, newTheme);
				flag = true;
			}
			else
			{
				Accent item2 = oldThemeInfo.Item2;
				if (item2 != null && item2.Name != newAccent.Name)
				{
					string lower = item2.Resources.Source.ToString().ToLower();
					ResourceDictionary resourceDictionaries = (
						from x in resources.MergedDictionaries
						where x.Source != null
						select x).FirstOrDefault<ResourceDictionary>((ResourceDictionary d) => d.Source.ToString().ToLower() == lower);
					if (resourceDictionaries != null)
					{
						resources.MergedDictionaries.Add(newAccent.Resources);
						resources.MergedDictionaries.Remove(resourceDictionaries);
						flag = true;
					}
				}
				AppTheme item1 = oldThemeInfo.Item1;
				if (item1 != null && item1 != newTheme)
				{
					string str = item1.Resources.Source.ToString().ToLower();
					ResourceDictionary resourceDictionaries1 = (
						from x in resources.MergedDictionaries
						where x.Source != null
						select x).FirstOrDefault<ResourceDictionary>((ResourceDictionary d) => d.Source.ToString().ToLower() == str);
					if (resourceDictionaries1 != null)
					{
						resources.MergedDictionaries.Add(newTheme.Resources);
						resources.MergedDictionaries.Remove(resourceDictionaries1);
						flag = true;
					}
				}
			}
			if (flag)
			{
				ThemeManager.OnThemeChanged(newAccent, newTheme);
			}
		}

		[SecurityCritical]
		public static void ChangeAppStyle(ResourceDictionary resources, Accent newAccent, AppTheme newTheme)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			if (newAccent == null)
			{
				throw new ArgumentNullException("newAccent");
			}
			if (newTheme == null)
			{
				throw new ArgumentNullException("newTheme");
			}
			ThemeManager.ApplyResourceDictionary(newAccent.Resources, resources);
			ThemeManager.ApplyResourceDictionary(newTheme.Resources, resources);
		}

		[SecurityCritical]
		public static void ChangeAppTheme(Application app, string themeName)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			if (themeName == null)
			{
				throw new ArgumentNullException("themeName");
			}
			Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(app);
			AppTheme appTheme = ThemeManager.GetAppTheme(themeName);
			AppTheme appTheme1 = appTheme;
			if (appTheme != null)
			{
				ThemeManager.ChangeAppStyle(app.Resources, tuple, tuple.Item2, appTheme1);
			}
		}

		[SecurityCritical]
		public static void ChangeAppTheme(Window window, string themeName)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (themeName == null)
			{
				throw new ArgumentNullException("themeName");
			}
			Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(window);
			AppTheme appTheme = ThemeManager.GetAppTheme(themeName);
			AppTheme appTheme1 = appTheme;
			if (appTheme != null)
			{
				ThemeManager.ChangeAppStyle(window.Resources, tuple, tuple.Item2, appTheme1);
			}
		}

		internal static void CopyResource(ResourceDictionary fromRD, ResourceDictionary toRD)
		{
			if (fromRD == null)
			{
				throw new ArgumentNullException("fromRD");
			}
			if (toRD == null)
			{
				throw new ArgumentNullException("toRD");
			}
			ThemeManager.ApplyResourceDictionary(fromRD, toRD);
			foreach (ResourceDictionary mergedDictionary in fromRD.MergedDictionaries)
			{
				ThemeManager.CopyResource(mergedDictionary, toRD);
			}
		}

		public static Tuple<AppTheme, Accent> DetectAppStyle()
		{
			Tuple<AppTheme, Accent> tuple;
			try
			{
				tuple = ThemeManager.DetectAppStyle(Application.Current.MainWindow);
			}
			catch (Exception exception)
			{
				tuple = ThemeManager.DetectAppStyle(Application.Current);
			}
			return tuple;
		}

		public static Tuple<AppTheme, Accent> DetectAppStyle(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(window.Resources) ?? ThemeManager.DetectAppStyle(Application.Current.Resources);
			return tuple;
		}

		public static Tuple<AppTheme, Accent> DetectAppStyle(Application app)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			return ThemeManager.DetectAppStyle(app.Resources);
		}

		private static Tuple<AppTheme, Accent> DetectAppStyle(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			AppTheme appTheme = null;
			Tuple<AppTheme, Accent> tuple = null;
			if (!ThemeManager.DetectThemeFromResources(ref appTheme, resources) || !ThemeManager.GetThemeFromResources(appTheme, resources, ref tuple))
			{
				return null;
			}
			return new Tuple<AppTheme, Accent>(tuple.Item1, tuple.Item2);
		}

		internal static bool DetectThemeFromAppResources(out AppTheme detectedTheme)
		{
			detectedTheme = null;
			return ThemeManager.DetectThemeFromResources(ref detectedTheme, Application.Current.Resources);
		}

		private static bool DetectThemeFromResources(ref AppTheme detectedTheme, ResourceDictionary dict)
		{
			IEnumerator<ResourceDictionary> enumerator = dict.MergedDictionaries.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ResourceDictionary current = enumerator.Current;
				AppTheme appTheme = ThemeManager.GetAppTheme(current);
				AppTheme appTheme1 = appTheme;
				if (appTheme != null)
				{
					detectedTheme = appTheme1;
					enumerator.Dispose();
					return true;
				}
				if (!ThemeManager.DetectThemeFromResources(ref detectedTheme, current))
				{
					continue;
				}
				return true;
			}
			enumerator.Dispose();
			return false;
		}

		public static Accent GetAccent(string accentName)
		{
			if (accentName == null)
			{
				throw new ArgumentNullException("accentName");
			}
			return ThemeManager.Accents.FirstOrDefault<Accent>((Accent x) => x.Name.Equals(accentName, StringComparison.InvariantCultureIgnoreCase));
		}

		public static Accent GetAccent(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			Accent accent = ThemeManager.Accents.FirstOrDefault<Accent>((Accent x) => ThemeManager.AreResourceDictionarySourcesEqual(x.Resources.Source, resources.Source));
			if (accent != null)
			{
				return accent;
			}
			if (!(resources.Source == null) || !ThemeManager.IsAccentDictionary(resources))
			{
				return null;
			}
			return new Accent()
			{
				Name = "Runtime accent",
				Resources = resources
			};
		}

		public static AppTheme GetAppTheme(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			return ThemeManager.AppThemes.FirstOrDefault<AppTheme>((AppTheme x) => ThemeManager.AreResourceDictionarySourcesEqual(x.Resources.Source, resources.Source));
		}

		public static AppTheme GetAppTheme(string appThemeName)
		{
			if (appThemeName == null)
			{
				throw new ArgumentNullException("appThemeName");
			}
			return ThemeManager.AppThemes.FirstOrDefault<AppTheme>((AppTheme x) => x.Name.Equals(appThemeName, StringComparison.InvariantCultureIgnoreCase));
		}

		public static AppTheme GetInverseAppTheme(AppTheme appTheme)
		{
			if (appTheme == null)
			{
				throw new ArgumentNullException("appTheme");
			}
			if (appTheme.Name.EndsWith("dark", StringComparison.InvariantCultureIgnoreCase))
			{
				return ThemeManager.GetAppTheme(string.Concat(appTheme.Name.ToLower().Replace("dark", string.Empty), "light"));
			}
			if (!appTheme.Name.EndsWith("light", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			return ThemeManager.GetAppTheme(string.Concat(appTheme.Name.ToLower().Replace("light", string.Empty), "dark"));
		}

		public static object GetResourceFromAppStyle(Window window, string key)
		{
			Tuple<AppTheme, Accent> tuple = (window != null ? ThemeManager.DetectAppStyle(window) : ThemeManager.DetectAppStyle(Application.Current));
			if (tuple == null && window != null)
			{
				tuple = ThemeManager.DetectAppStyle(Application.Current);
			}
			if (tuple == null)
			{
				return null;
			}
			object item = tuple.Item1.Resources[key];
			object obj = tuple.Item2.Resources[key];
			if (obj != null)
			{
				return obj;
			}
			return item;
		}

		internal static bool GetThemeFromResources(AppTheme presetTheme, ResourceDictionary dict, ref Tuple<AppTheme, Accent> detectedAccentTheme)
		{
			bool flag;
			AppTheme appTheme = presetTheme;
			Accent accent = ThemeManager.GetAccent(dict);
			Accent accent1 = accent;
			if (accent != null)
			{
				detectedAccentTheme = Tuple.Create<AppTheme, Accent>(appTheme, accent1);
				return true;
			}
			using (IEnumerator<ResourceDictionary> enumerator = dict.MergedDictionaries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!ThemeManager.GetThemeFromResources(presetTheme, enumerator.Current, ref detectedAccentTheme))
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			return flag;
		}

		public static bool IsAccentDictionary(ResourceDictionary resources)
		{
			bool flag;
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			List<string>.Enumerator enumerator = (new List<string>(new string[] { "HighlightColor", "AccentColor", "AccentColor2", "AccentColor3", "AccentColor4", "HighlightBrush", "AccentColorBrush", "AccentColorBrush2", "AccentColorBrush3", "AccentColorBrush4" })).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					if ((
						from object resourceKey in resources.Keys
						select resourceKey as string).Any<string>((string keyAsString) => string.Equals(keyAsString, current)))
					{
						continue;
					}
					flag = false;
					return flag;
				}
				return true;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		[SecurityCritical]
		private static void OnThemeChanged(Accent newAccent, AppTheme newTheme)
		{
			SafeRaise.Raise<OnThemeChangedEventArgs>(ThemeManager.IsThemeChanged, Application.Current, new OnThemeChangedEventArgs()
			{
				AppTheme = newTheme,
				Accent = newAccent
			});
		}

		public static event EventHandler<OnThemeChangedEventArgs> IsThemeChanged;
	}
}