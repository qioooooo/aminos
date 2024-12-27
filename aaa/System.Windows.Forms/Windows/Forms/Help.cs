using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200041F RID: 1055
	public class Help
	{
		// Token: 0x06003EC3 RID: 16067 RVA: 0x000E46DB File Offset: 0x000E36DB
		private Help()
		{
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x000E46E3 File Offset: 0x000E36E3
		public static void ShowHelp(Control parent, string url)
		{
			Help.ShowHelp(parent, url, HelpNavigator.TableOfContents, null);
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x000E46F2 File Offset: 0x000E36F2
		public static void ShowHelp(Control parent, string url, HelpNavigator navigator)
		{
			Help.ShowHelp(parent, url, navigator, null);
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x000E46FD File Offset: 0x000E36FD
		public static void ShowHelp(Control parent, string url, string keyword)
		{
			if (keyword != null && keyword.Length != 0)
			{
				Help.ShowHelp(parent, url, HelpNavigator.Topic, keyword);
				return;
			}
			Help.ShowHelp(parent, url, HelpNavigator.TableOfContents, null);
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x000E4728 File Offset: 0x000E3728
		public static void ShowHelp(Control parent, string url, HelpNavigator command, object parameter)
		{
			switch (Help.GetHelpFileType(url))
			{
			case 2:
				Help.ShowHTML10Help(parent, url, command, parameter);
				return;
			case 3:
				Help.ShowHTMLFile(parent, url, command, parameter);
				return;
			default:
				return;
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x000E4760 File Offset: 0x000E3760
		public static void ShowHelpIndex(Control parent, string url)
		{
			Help.ShowHelp(parent, url, HelpNavigator.Index, null);
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x000E4770 File Offset: 0x000E3770
		public static void ShowPopup(Control parent, string caption, Point location)
		{
			NativeMethods.HH_POPUP hh_POPUP = new NativeMethods.HH_POPUP();
			IntPtr intPtr = Marshal.StringToCoTaskMemAuto(caption);
			try
			{
				hh_POPUP.pszText = intPtr;
				hh_POPUP.idString = 0;
				hh_POPUP.pt = new NativeMethods.POINT(location.X, location.Y);
				hh_POPUP.clrBackground = Color.FromKnownColor(KnownColor.Window).ToArgb() & 16777215;
				Help.ShowHTML10Help(parent, null, HelpNavigator.Topic, hh_POPUP);
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x000E47F4 File Offset: 0x000E37F4
		private static void ShowHTML10Help(Control parent, string url, HelpNavigator command, object param)
		{
			IntSecurity.UnmanagedCode.Demand();
			string text = url;
			Uri uri = Help.Resolve(url);
			if (uri != null)
			{
				text = uri.AbsoluteUri;
			}
			if (uri == null || uri.IsFile)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text2 = ((uri != null && uri.IsFile) ? uri.LocalPath : url);
				uint num = UnsafeNativeMethods.GetShortPathName(text2, stringBuilder, 0U);
				if (num > 0U)
				{
					stringBuilder.Capacity = (int)num;
					num = UnsafeNativeMethods.GetShortPathName(text2, stringBuilder, num);
					text = stringBuilder.ToString();
				}
			}
			HandleRef handleRef;
			if (parent != null)
			{
				handleRef = new HandleRef(parent, parent.Handle);
			}
			else
			{
				handleRef = new HandleRef(null, UnsafeNativeMethods.GetActiveWindow());
			}
			string text3 = param as string;
			if (text3 != null)
			{
				object obj;
				int num2 = Help.MapCommandToHTMLCommand(command, text3, out obj);
				string text4 = obj as string;
				if (text4 != null)
				{
					SafeNativeMethods.HtmlHelp(handleRef, text, num2, text4);
					return;
				}
				if (obj is int)
				{
					SafeNativeMethods.HtmlHelp(handleRef, text, num2, (int)obj);
					return;
				}
				if (obj is NativeMethods.HH_FTS_QUERY)
				{
					SafeNativeMethods.HtmlHelp(handleRef, text, num2, (NativeMethods.HH_FTS_QUERY)obj);
					return;
				}
				if (obj is NativeMethods.HH_AKLINK)
				{
					SafeNativeMethods.HtmlHelp(NativeMethods.NullHandleRef, text, 0, null);
					SafeNativeMethods.HtmlHelp(handleRef, text, num2, (NativeMethods.HH_AKLINK)obj);
					return;
				}
				SafeNativeMethods.HtmlHelp(handleRef, text, num2, (string)param);
				return;
			}
			else
			{
				if (param == null)
				{
					object obj;
					SafeNativeMethods.HtmlHelp(handleRef, text, Help.MapCommandToHTMLCommand(command, null, out obj), 0);
					return;
				}
				if (param is NativeMethods.HH_POPUP)
				{
					SafeNativeMethods.HtmlHelp(handleRef, text, 14, (NativeMethods.HH_POPUP)param);
					return;
				}
				if (param.GetType() == typeof(int))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "param", "Integer" }));
				}
				return;
			}
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x000E49BC File Offset: 0x000E39BC
		private static void ShowHTMLFile(Control parent, string url, HelpNavigator command, object param)
		{
			Uri uri = Help.Resolve(url);
			if (uri == null)
			{
				throw new ArgumentException(SR.GetString("HelpInvalidURL", new object[] { url }), "url");
			}
			string scheme;
			if ((scheme = uri.Scheme) != null && (scheme == "http" || scheme == "https"))
			{
				new WebPermission(NetworkAccess.Connect, url).Demand();
			}
			else
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			switch (command)
			{
			case HelpNavigator.Topic:
				if (param != null && param is string)
				{
					uri = new Uri(uri.ToString() + "#" + (string)param);
				}
				break;
			}
			HandleRef handleRef;
			if (parent != null)
			{
				handleRef = new HandleRef(parent, parent.Handle);
			}
			else
			{
				handleRef = new HandleRef(null, UnsafeNativeMethods.GetActiveWindow());
			}
			UnsafeNativeMethods.ShellExecute_NoBFM(handleRef, null, uri.ToString(), null, null, 1);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x000E4AB4 File Offset: 0x000E3AB4
		private static Uri Resolve(string partialUri)
		{
			Uri uri = null;
			if (!string.IsNullOrEmpty(partialUri))
			{
				try
				{
					uri = new Uri(partialUri);
				}
				catch (UriFormatException)
				{
				}
				catch (ArgumentNullException)
				{
				}
			}
			if (uri != null && uri.Scheme == "file")
			{
				string localPath = NativeMethods.GetLocalPath(partialUri);
				new FileIOPermission(FileIOPermissionAccess.Read, localPath).Assert();
				try
				{
					if (!File.Exists(localPath))
					{
						uri = null;
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			if (uri == null)
			{
				try
				{
					uri = new Uri(new Uri(AppDomain.CurrentDomain.SetupInformation.ApplicationBase), partialUri);
				}
				catch (UriFormatException)
				{
				}
				catch (ArgumentNullException)
				{
				}
				if (uri != null && uri.Scheme == "file")
				{
					string text = uri.LocalPath + uri.Fragment;
					new FileIOPermission(FileIOPermissionAccess.Read, text).Assert();
					try
					{
						if (!File.Exists(text))
						{
							uri = null;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			return uri;
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x000E4BE0 File Offset: 0x000E3BE0
		private static int GetHelpFileType(string url)
		{
			if (url == null)
			{
				return 3;
			}
			Uri uri = Help.Resolve(url);
			if (uri == null || uri.Scheme == "file")
			{
				string text = Path.GetExtension((uri == null) ? url : (uri.LocalPath + uri.Fragment)).ToLower(CultureInfo.InvariantCulture);
				if (text == ".chm" || text == ".col")
				{
					return 2;
				}
			}
			return 3;
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x000E4C60 File Offset: 0x000E3C60
		private static int MapCommandToHTMLCommand(HelpNavigator command, string param, out object htmlParam)
		{
			htmlParam = param;
			if (string.IsNullOrEmpty(param) && (command == HelpNavigator.AssociateIndex || command == HelpNavigator.KeywordIndex))
			{
				return 2;
			}
			switch (command)
			{
			case HelpNavigator.Topic:
				return 0;
			case HelpNavigator.TableOfContents:
				return 1;
			case HelpNavigator.Index:
				return 2;
			case HelpNavigator.Find:
				htmlParam = new NativeMethods.HH_FTS_QUERY
				{
					pszSearchQuery = param
				};
				return 3;
			case HelpNavigator.AssociateIndex:
			case HelpNavigator.KeywordIndex:
				break;
			case HelpNavigator.TopicId:
				try
				{
					htmlParam = int.Parse(param, CultureInfo.InvariantCulture);
					return 15;
				}
				catch
				{
					return 2;
				}
				break;
			default:
				return (int)command;
			}
			htmlParam = new NativeMethods.HH_AKLINK
			{
				pszKeywords = param,
				fIndexOnFail = true,
				fReserved = false
			};
			if (command != HelpNavigator.KeywordIndex)
			{
				return 19;
			}
			return 13;
		}

		// Token: 0x04001ED7 RID: 7895
		private const int HH_DISPLAY_TOPIC = 0;

		// Token: 0x04001ED8 RID: 7896
		private const int HH_HELP_FINDER = 0;

		// Token: 0x04001ED9 RID: 7897
		private const int HH_DISPLAY_TOC = 1;

		// Token: 0x04001EDA RID: 7898
		private const int HH_DISPLAY_INDEX = 2;

		// Token: 0x04001EDB RID: 7899
		private const int HH_DISPLAY_SEARCH = 3;

		// Token: 0x04001EDC RID: 7900
		private const int HH_SET_WIN_TYPE = 4;

		// Token: 0x04001EDD RID: 7901
		private const int HH_GET_WIN_TYPE = 5;

		// Token: 0x04001EDE RID: 7902
		private const int HH_GET_WIN_HANDLE = 6;

		// Token: 0x04001EDF RID: 7903
		private const int HH_ENUM_INFO_TYPE = 7;

		// Token: 0x04001EE0 RID: 7904
		private const int HH_SET_INFO_TYPE = 8;

		// Token: 0x04001EE1 RID: 7905
		private const int HH_SYNC = 9;

		// Token: 0x04001EE2 RID: 7906
		private const int HH_ADD_NAV_UI = 10;

		// Token: 0x04001EE3 RID: 7907
		private const int HH_ADD_BUTTON = 11;

		// Token: 0x04001EE4 RID: 7908
		private const int HH_GETBROWSER_APP = 12;

		// Token: 0x04001EE5 RID: 7909
		private const int HH_KEYWORD_LOOKUP = 13;

		// Token: 0x04001EE6 RID: 7910
		private const int HH_DISPLAY_TEXT_POPUP = 14;

		// Token: 0x04001EE7 RID: 7911
		private const int HH_HELP_CONTEXT = 15;

		// Token: 0x04001EE8 RID: 7912
		private const int HH_TP_HELP_CONTEXTMENU = 16;

		// Token: 0x04001EE9 RID: 7913
		private const int HH_TP_HELP_WM_HELP = 17;

		// Token: 0x04001EEA RID: 7914
		private const int HH_CLOSE_ALL = 18;

		// Token: 0x04001EEB RID: 7915
		private const int HH_ALINK_LOOKUP = 19;

		// Token: 0x04001EEC RID: 7916
		private const int HH_GET_LAST_ERROR = 20;

		// Token: 0x04001EED RID: 7917
		private const int HH_ENUM_CATEGORY = 21;

		// Token: 0x04001EEE RID: 7918
		private const int HH_ENUM_CATEGORY_IT = 22;

		// Token: 0x04001EEF RID: 7919
		private const int HH_RESET_IT_FILTER = 23;

		// Token: 0x04001EF0 RID: 7920
		private const int HH_SET_INCLUSIVE_FILTER = 24;

		// Token: 0x04001EF1 RID: 7921
		private const int HH_SET_EXCLUSIVE_FILTER = 25;

		// Token: 0x04001EF2 RID: 7922
		private const int HH_SET_GUID = 26;

		// Token: 0x04001EF3 RID: 7923
		private const int HTML10HELP = 2;

		// Token: 0x04001EF4 RID: 7924
		private const int HTMLFILE = 3;

		// Token: 0x04001EF5 RID: 7925
		internal static readonly TraceSwitch WindowsFormsHelpTrace;
	}
}
