using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x0200044C RID: 1100
	public sealed class InputLanguage
	{
		// Token: 0x06004186 RID: 16774 RVA: 0x000EABC4 File Offset: 0x000E9BC4
		internal InputLanguage(IntPtr handle)
		{
			this.handle = handle;
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004187 RID: 16775 RVA: 0x000EABD3 File Offset: 0x000E9BD3
		public CultureInfo Culture
		{
			get
			{
				return new CultureInfo((int)this.handle & 65535);
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06004188 RID: 16776 RVA: 0x000EABEB File Offset: 0x000E9BEB
		// (set) Token: 0x06004189 RID: 16777 RVA: 0x000EAC00 File Offset: 0x000E9C00
		public static InputLanguage CurrentInputLanguage
		{
			get
			{
				Application.OleRequired();
				return new InputLanguage(SafeNativeMethods.GetKeyboardLayout(0));
			}
			set
			{
				IntSecurity.AffectThreadBehavior.Demand();
				Application.OleRequired();
				if (value == null)
				{
					value = InputLanguage.DefaultInputLanguage;
				}
				IntPtr intPtr = SafeNativeMethods.ActivateKeyboardLayout(new HandleRef(value, value.handle), 0);
				if (intPtr == IntPtr.Zero)
				{
					throw new ArgumentException(SR.GetString("ErrorBadInputLanguage"), "value");
				}
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x000EAC5C File Offset: 0x000E9C5C
		public static InputLanguage DefaultInputLanguage
		{
			get
			{
				IntPtr[] array = new IntPtr[1];
				UnsafeNativeMethods.SystemParametersInfo(89, 0, array, 0);
				return new InputLanguage(array[0]);
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x000EAC8C File Offset: 0x000E9C8C
		public IntPtr Handle
		{
			get
			{
				return this.handle;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x000EAC94 File Offset: 0x000E9C94
		public static InputLanguageCollection InstalledInputLanguages
		{
			get
			{
				int keyboardLayoutList = SafeNativeMethods.GetKeyboardLayoutList(0, null);
				IntPtr[] array = new IntPtr[keyboardLayoutList];
				SafeNativeMethods.GetKeyboardLayoutList(keyboardLayoutList, array);
				InputLanguage[] array2 = new InputLanguage[keyboardLayoutList];
				for (int i = 0; i < keyboardLayoutList; i++)
				{
					array2[i] = new InputLanguage(array[i]);
				}
				return new InputLanguageCollection(array2);
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x000EACE8 File Offset: 0x000E9CE8
		public string LayoutName
		{
			get
			{
				string text = null;
				IntPtr intPtr = this.handle;
				int num = (int)intPtr & 65535;
				int num2 = ((int)intPtr >> 16) & 4095;
				new RegistryPermission(PermissionState.Unrestricted).Assert();
				try
				{
					if (num2 == num || num2 == 0)
					{
						string text2 = Convert.ToString(num, 16);
						text2 = InputLanguage.PadWithZeroes(text2, 8);
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layouts\\" + text2);
						text = InputLanguage.GetLocalizedKeyboardLayoutName(registryKey.GetValue("Layout Display Name") as string);
						if (text == null)
						{
							text = (string)registryKey.GetValue("Layout Text");
						}
						registryKey.Close();
					}
					else
					{
						RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Keyboard Layout\\Substitutes");
						string[] array = null;
						if (registryKey2 != null)
						{
							array = registryKey2.GetValueNames();
							foreach (string text3 in array)
							{
								int num3 = Convert.ToInt32(text3, 16);
								if (num3 == (int)intPtr || (num3 & 268435455) == ((int)intPtr & 268435455) || (num3 & 65535) == num)
								{
									intPtr = (IntPtr)Convert.ToInt32((string)registryKey2.GetValue(text3), 16);
									num = (int)intPtr & 65535;
									num2 = ((int)intPtr >> 16) & 4095;
									break;
								}
							}
							registryKey2.Close();
						}
						RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layouts");
						if (registryKey3 != null)
						{
							array = registryKey3.GetSubKeyNames();
							foreach (string text4 in array)
							{
								if (intPtr == (IntPtr)Convert.ToInt32(text4, 16))
								{
									RegistryKey registryKey4 = registryKey3.OpenSubKey(text4);
									text = InputLanguage.GetLocalizedKeyboardLayoutName(registryKey4.GetValue("Layout Display Name") as string);
									if (text == null)
									{
										text = (string)registryKey4.GetValue("Layout Text");
									}
									registryKey4.Close();
									break;
								}
							}
						}
						if (text == null)
						{
							foreach (string text5 in array)
							{
								if (num == (65535 & Convert.ToInt32(text5.Substring(4, 4), 16)))
								{
									RegistryKey registryKey5 = registryKey3.OpenSubKey(text5);
									string text6 = (string)registryKey5.GetValue("Layout Id");
									if (text6 != null)
									{
										int num4 = Convert.ToInt32(text6, 16);
										if (num4 == num2)
										{
											text = InputLanguage.GetLocalizedKeyboardLayoutName(registryKey5.GetValue("Layout Display Name") as string);
											if (text == null)
											{
												text = (string)registryKey5.GetValue("Layout Text");
											}
										}
									}
									registryKey5.Close();
									if (text != null)
									{
										break;
									}
								}
							}
						}
						registryKey3.Close();
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (text == null)
				{
					text = SR.GetString("UnknownInputLanguageLayout");
				}
				return text;
			}
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x000EAFC4 File Offset: 0x000E9FC4
		private static string GetLocalizedKeyboardLayoutName(string layoutDisplayName)
		{
			if (layoutDisplayName != null && Environment.OSVersion.Version.Major >= 5)
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				if (UnsafeNativeMethods.SHLoadIndirectString(layoutDisplayName, stringBuilder, (uint)stringBuilder.Capacity, IntPtr.Zero) == 0U)
				{
					return stringBuilder.ToString();
				}
			}
			return null;
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x000EB00F File Offset: 0x000EA00F
		internal static InputLanguageChangedEventArgs CreateInputLanguageChangedEventArgs(Message m)
		{
			return new InputLanguageChangedEventArgs(new InputLanguage(m.LParam), (byte)(long)m.WParam);
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x000EB030 File Offset: 0x000EA030
		internal static InputLanguageChangingEventArgs CreateInputLanguageChangingEventArgs(Message m)
		{
			InputLanguage inputLanguage = new InputLanguage(m.LParam);
			bool flag = !(m.WParam == IntPtr.Zero);
			return new InputLanguageChangingEventArgs(inputLanguage, flag);
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x000EB066 File Offset: 0x000EA066
		public override bool Equals(object value)
		{
			return value is InputLanguage && this.handle == ((InputLanguage)value).handle;
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x000EB088 File Offset: 0x000EA088
		public static InputLanguage FromCulture(CultureInfo culture)
		{
			int keyboardLayoutId = culture.KeyboardLayoutId;
			foreach (object obj in InputLanguage.InstalledInputLanguages)
			{
				InputLanguage inputLanguage = (InputLanguage)obj;
				if (((int)inputLanguage.handle & 65535) == keyboardLayoutId)
				{
					return inputLanguage;
				}
			}
			return null;
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x000EB100 File Offset: 0x000EA100
		public override int GetHashCode()
		{
			return (int)this.handle;
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x000EB10D File Offset: 0x000EA10D
		private static string PadWithZeroes(string input, int length)
		{
			return "0000000000000000".Substring(0, length - input.Length) + input;
		}

		// Token: 0x04001F9C RID: 8092
		private readonly IntPtr handle;
	}
}
