using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.Hosting;

namespace System.Web.Mail
{
	// Token: 0x02000785 RID: 1925
	[Obsolete("The recommended alternative is System.Net.Mail.SmtpClient. http://go.microsoft.com/fwlink/?linkid=14202")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SmtpMail
	{
		// Token: 0x06005CD0 RID: 23760 RVA: 0x00173F98 File Offset: 0x00172F98
		private SmtpMail()
		{
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x00173FA0 File Offset: 0x00172FA0
		// (set) Token: 0x06005CD2 RID: 23762 RVA: 0x00173FBD File Offset: 0x00172FBD
		public static string SmtpServer
		{
			get
			{
				string server = SmtpMail._server;
				if (server == null)
				{
					return string.Empty;
				}
				return server;
			}
			set
			{
				SmtpMail._server = value;
			}
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x00173FC8 File Offset: 0x00172FC8
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
		public static void Send(string from, string to, string subject, string messageText)
		{
			lock (SmtpMail._lockObject)
			{
				if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				{
					throw new PlatformNotSupportedException(SR.GetString("RequiresNT"));
				}
				if (!SmtpMail.CdoSysHelper.OsSupportsCdoSys())
				{
					throw new PlatformNotSupportedException(SR.GetString("SmtpMail_not_supported_on_Win7_and_higher"));
				}
				if (Environment.OSVersion.Version.Major <= 4)
				{
					SmtpMail.CdoNtsHelper.Send(from, to, subject, messageText);
				}
				else
				{
					SmtpMail.CdoSysHelper.Send(from, to, subject, messageText);
				}
			}
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x00174054 File Offset: 0x00173054
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
		public static void Send(MailMessage message)
		{
			lock (SmtpMail._lockObject)
			{
				if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				{
					throw new PlatformNotSupportedException(SR.GetString("RequiresNT"));
				}
				if (!SmtpMail.CdoSysHelper.OsSupportsCdoSys())
				{
					throw new PlatformNotSupportedException(SR.GetString("SmtpMail_not_supported_on_Win7_and_higher"));
				}
				if (Environment.OSVersion.Version.Major <= 4)
				{
					SmtpMail.CdoNtsHelper.Send(message);
				}
				else
				{
					SmtpMail.CdoSysHelper.Send(message);
				}
			}
		}

		// Token: 0x04003195 RID: 12693
		private static object _lockObject = new object();

		// Token: 0x04003196 RID: 12694
		private static string _server;

		// Token: 0x02000786 RID: 1926
		internal class LateBoundAccessHelper
		{
			// Token: 0x06005CD6 RID: 23766 RVA: 0x001740E8 File Offset: 0x001730E8
			internal LateBoundAccessHelper(string progId)
			{
				this._progId = progId;
			}

			// Token: 0x170017D4 RID: 6100
			// (get) Token: 0x06005CD7 RID: 23767 RVA: 0x001740F8 File Offset: 0x001730F8
			private Type LateBoundType
			{
				get
				{
					if (this._type == null)
					{
						try
						{
							this._type = Type.GetTypeFromProgID(this._progId);
						}
						catch
						{
						}
						if (this._type == null)
						{
							throw new HttpException(SR.GetString("SMTP_TypeCreationError", new object[] { this._progId }));
						}
					}
					return this._type;
				}
			}

			// Token: 0x06005CD8 RID: 23768 RVA: 0x00174164 File Offset: 0x00173164
			internal object CreateInstance()
			{
				return Activator.CreateInstance(this.LateBoundType);
			}

			// Token: 0x06005CD9 RID: 23769 RVA: 0x00174174 File Offset: 0x00173174
			internal object CallMethod(object obj, string methodName, object[] args)
			{
				object obj2;
				try
				{
					obj2 = SmtpMail.LateBoundAccessHelper.CallMethod(this.LateBoundType, obj, methodName, args);
				}
				catch (Exception ex)
				{
					throw new HttpException(SmtpMail.LateBoundAccessHelper.GetInnerMostException(ex).Message, ex);
				}
				return obj2;
			}

			// Token: 0x06005CDA RID: 23770 RVA: 0x001741B8 File Offset: 0x001731B8
			internal static object CallMethodStatic(object obj, string methodName, object[] args)
			{
				return SmtpMail.LateBoundAccessHelper.CallMethod(obj.GetType(), obj, methodName, args);
			}

			// Token: 0x06005CDB RID: 23771 RVA: 0x001741C8 File Offset: 0x001731C8
			private static object CallMethod(Type type, object obj, string methodName, object[] args)
			{
				return type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, obj, args, CultureInfo.InvariantCulture);
			}

			// Token: 0x06005CDC RID: 23772 RVA: 0x001741DE File Offset: 0x001731DE
			private static Exception GetInnerMostException(Exception e)
			{
				if (e.InnerException == null)
				{
					return e;
				}
				return SmtpMail.LateBoundAccessHelper.GetInnerMostException(e.InnerException);
			}

			// Token: 0x06005CDD RID: 23773 RVA: 0x001741F8 File Offset: 0x001731F8
			internal object GetProp(object obj, string propName)
			{
				object prop;
				try
				{
					prop = SmtpMail.LateBoundAccessHelper.GetProp(this.LateBoundType, obj, propName);
				}
				catch (Exception ex)
				{
					throw new HttpException(SmtpMail.LateBoundAccessHelper.GetInnerMostException(ex).Message, ex);
				}
				return prop;
			}

			// Token: 0x06005CDE RID: 23774 RVA: 0x0017423C File Offset: 0x0017323C
			internal static object GetPropStatic(object obj, string propName)
			{
				return SmtpMail.LateBoundAccessHelper.GetProp(obj.GetType(), obj, propName);
			}

			// Token: 0x06005CDF RID: 23775 RVA: 0x0017424B File Offset: 0x0017324B
			private static object GetProp(Type type, object obj, string propName)
			{
				return type.InvokeMember(propName, BindingFlags.GetProperty, null, obj, new object[0], CultureInfo.InvariantCulture);
			}

			// Token: 0x06005CE0 RID: 23776 RVA: 0x00174268 File Offset: 0x00173268
			internal void SetProp(object obj, string propName, object propValue)
			{
				try
				{
					SmtpMail.LateBoundAccessHelper.SetProp(this.LateBoundType, obj, propName, propValue);
				}
				catch (Exception ex)
				{
					throw new HttpException(SmtpMail.LateBoundAccessHelper.GetInnerMostException(ex).Message, ex);
				}
			}

			// Token: 0x06005CE1 RID: 23777 RVA: 0x001742A8 File Offset: 0x001732A8
			internal static void SetPropStatic(object obj, string propName, object propValue)
			{
				SmtpMail.LateBoundAccessHelper.SetProp(obj.GetType(), obj, propName, propValue);
			}

			// Token: 0x06005CE2 RID: 23778 RVA: 0x001742B8 File Offset: 0x001732B8
			private static void SetProp(Type type, object obj, string propName, object propValue)
			{
				if (propValue != null && propValue is string && ((string)propValue).IndexOf('\0') >= 0)
				{
					throw new ArgumentException();
				}
				type.InvokeMember(propName, BindingFlags.SetProperty, null, obj, new object[] { propValue }, CultureInfo.InvariantCulture);
			}

			// Token: 0x06005CE3 RID: 23779 RVA: 0x00174308 File Offset: 0x00173308
			internal void SetProp(object obj, string propName, object propKey, object propValue)
			{
				try
				{
					SmtpMail.LateBoundAccessHelper.SetProp(this.LateBoundType, obj, propName, propKey, propValue);
				}
				catch (Exception ex)
				{
					throw new HttpException(SmtpMail.LateBoundAccessHelper.GetInnerMostException(ex).Message, ex);
				}
			}

			// Token: 0x06005CE4 RID: 23780 RVA: 0x0017434C File Offset: 0x0017334C
			internal static void SetPropStatic(object obj, string propName, object propKey, object propValue)
			{
				SmtpMail.LateBoundAccessHelper.SetProp(obj.GetType(), obj, propName, propKey, propValue);
			}

			// Token: 0x06005CE5 RID: 23781 RVA: 0x00174360 File Offset: 0x00173360
			private static void SetProp(Type type, object obj, string propName, object propKey, object propValue)
			{
				if (propValue != null && propValue is string && ((string)propValue).IndexOf('\0') >= 0)
				{
					throw new ArgumentException();
				}
				type.InvokeMember(propName, BindingFlags.SetProperty, null, obj, new object[] { propKey, propValue }, CultureInfo.InvariantCulture);
			}

			// Token: 0x04003197 RID: 12695
			private string _progId;

			// Token: 0x04003198 RID: 12696
			private Type _type;
		}

		// Token: 0x02000787 RID: 1927
		internal class CdoNtsHelper
		{
			// Token: 0x06005CE6 RID: 23782 RVA: 0x001743B5 File Offset: 0x001733B5
			private CdoNtsHelper()
			{
			}

			// Token: 0x06005CE7 RID: 23783 RVA: 0x001743C0 File Offset: 0x001733C0
			internal static void Send(MailMessage message)
			{
				object obj = SmtpMail.CdoNtsHelper._helper.CreateInstance();
				if (message.From != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "From", message.From);
				}
				if (message.To != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "To", message.To);
				}
				if (message.Cc != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "Cc", message.Cc);
				}
				if (message.Bcc != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "Bcc", message.Bcc);
				}
				if (message.Subject != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "Subject", message.Subject);
				}
				if (message.Priority != MailPriority.Normal)
				{
					int num = 0;
					switch (message.Priority)
					{
					case MailPriority.Normal:
						num = 1;
						break;
					case MailPriority.Low:
						num = 0;
						break;
					case MailPriority.High:
						num = 2;
						break;
					}
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "Importance", num);
				}
				if (message.BodyEncoding != null)
				{
					SmtpMail.CdoNtsHelper._helper.CallMethod(obj, "SetLocaleIDs", new object[] { message.BodyEncoding.CodePage });
				}
				if (message.UrlContentBase != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "ContentBase", message.UrlContentBase);
				}
				if (message.UrlContentLocation != null)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "ContentLocation", message.UrlContentLocation);
				}
				int count = message.Headers.Count;
				if (count > 0)
				{
					IDictionaryEnumerator enumerator = message.Headers.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string text = (string)enumerator.Key;
						string text2 = (string)enumerator.Value;
						SmtpMail.CdoNtsHelper._helper.SetProp(obj, "Value", text, text2);
					}
				}
				if (message.BodyFormat == MailFormat.Html)
				{
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "BodyFormat", 0);
					SmtpMail.CdoNtsHelper._helper.SetProp(obj, "MailFormat", 0);
				}
				SmtpMail.CdoNtsHelper._helper.SetProp(obj, "Body", (message.Body != null) ? message.Body : string.Empty);
				foreach (object obj2 in message.Attachments)
				{
					MailAttachment mailAttachment = (MailAttachment)obj2;
					int num2 = 0;
					switch (mailAttachment.Encoding)
					{
					case MailEncoding.UUEncode:
						num2 = 0;
						break;
					case MailEncoding.Base64:
						num2 = 1;
						break;
					}
					SmtpMail.CdoNtsHelper._helper.CallMethod(obj, "AttachFile", new object[] { mailAttachment.Filename, null, num2 });
				}
				SmtpMail.LateBoundAccessHelper helper = SmtpMail.CdoNtsHelper._helper;
				object obj3 = obj;
				string text3 = "Send";
				object[] array = new object[5];
				helper.CallMethod(obj3, text3, array);
				Marshal.ReleaseComObject(obj);
			}

			// Token: 0x06005CE8 RID: 23784 RVA: 0x00174674 File Offset: 0x00173674
			internal static void Send(string from, string to, string subject, string messageText)
			{
				SmtpMail.CdoNtsHelper.Send(new MailMessage
				{
					From = from,
					To = to,
					Subject = subject,
					Body = messageText
				});
			}

			// Token: 0x04003199 RID: 12697
			private static SmtpMail.LateBoundAccessHelper _helper = new SmtpMail.LateBoundAccessHelper("CDONTS.NewMail");
		}

		// Token: 0x02000788 RID: 1928
		internal class CdoSysHelper
		{
			// Token: 0x06005CEA RID: 23786 RVA: 0x001746BA File Offset: 0x001736BA
			private CdoSysHelper()
			{
			}

			// Token: 0x06005CEB RID: 23787 RVA: 0x001746C4 File Offset: 0x001736C4
			private static void SetField(object m, string name, string value)
			{
				SmtpMail.CdoSysHelper._helper.SetProp(m, "Fields", "urn:schemas:mailheader:" + name, value);
				object prop = SmtpMail.CdoSysHelper._helper.GetProp(m, "Fields");
				SmtpMail.LateBoundAccessHelper.CallMethodStatic(prop, "Update", new object[0]);
				Marshal.ReleaseComObject(prop);
			}

			// Token: 0x06005CEC RID: 23788 RVA: 0x00174718 File Offset: 0x00173718
			private static bool CdoSysExists()
			{
				if (SmtpMail.CdoSysHelper.cdoSysLibraryInfo != SmtpMail.CdoSysHelper.CdoSysLibraryStatus.NotChecked)
				{
					return SmtpMail.CdoSysHelper.cdoSysLibraryInfo == SmtpMail.CdoSysHelper.CdoSysLibraryStatus.Exists;
				}
				IntPtr intPtr = UnsafeNativeMethods.LoadLibrary("cdosys.dll");
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.FreeLibrary(intPtr);
					SmtpMail.CdoSysHelper.cdoSysLibraryInfo = SmtpMail.CdoSysHelper.CdoSysLibraryStatus.Exists;
					return true;
				}
				SmtpMail.CdoSysHelper.cdoSysLibraryInfo = SmtpMail.CdoSysHelper.CdoSysLibraryStatus.DoesntExist;
				return false;
			}

			// Token: 0x06005CED RID: 23789 RVA: 0x00174764 File Offset: 0x00173764
			internal static bool OsSupportsCdoSys()
			{
				Version version = Environment.OSVersion.Version;
				return (version.Major < 7 && (version.Major != 6 || version.Minor < 1)) || SmtpMail.CdoSysHelper.CdoSysExists();
			}

			// Token: 0x06005CEE RID: 23790 RVA: 0x001747A0 File Offset: 0x001737A0
			internal static void Send(MailMessage message)
			{
				object obj = SmtpMail.CdoSysHelper._helper.CreateInstance();
				if (message.From != null)
				{
					SmtpMail.CdoSysHelper._helper.SetProp(obj, "From", message.From);
				}
				if (message.To != null)
				{
					SmtpMail.CdoSysHelper._helper.SetProp(obj, "To", message.To);
				}
				if (message.Cc != null)
				{
					SmtpMail.CdoSysHelper._helper.SetProp(obj, "Cc", message.Cc);
				}
				if (message.Bcc != null)
				{
					SmtpMail.CdoSysHelper._helper.SetProp(obj, "Bcc", message.Bcc);
				}
				if (message.Subject != null)
				{
					SmtpMail.CdoSysHelper._helper.SetProp(obj, "Subject", message.Subject);
				}
				if (message.Priority != MailPriority.Normal)
				{
					string text = null;
					switch (message.Priority)
					{
					case MailPriority.Normal:
						text = "normal";
						break;
					case MailPriority.Low:
						text = "low";
						break;
					case MailPriority.High:
						text = "high";
						break;
					}
					if (text != null)
					{
						SmtpMail.CdoSysHelper.SetField(obj, "importance", text);
					}
				}
				if (message.BodyEncoding != null)
				{
					object prop = SmtpMail.CdoSysHelper._helper.GetProp(obj, "BodyPart");
					SmtpMail.LateBoundAccessHelper.SetPropStatic(prop, "Charset", message.BodyEncoding.BodyName);
					Marshal.ReleaseComObject(prop);
				}
				if (message.UrlContentBase != null)
				{
					SmtpMail.CdoSysHelper.SetField(obj, "content-base", message.UrlContentBase);
				}
				if (message.UrlContentLocation != null)
				{
					SmtpMail.CdoSysHelper.SetField(obj, "content-location", message.UrlContentLocation);
				}
				int count = message.Headers.Count;
				if (count > 0)
				{
					IDictionaryEnumerator enumerator = message.Headers.GetEnumerator();
					while (enumerator.MoveNext())
					{
						SmtpMail.CdoSysHelper.SetField(obj, (string)enumerator.Key, (string)enumerator.Value);
					}
				}
				if (message.Body != null)
				{
					if (message.BodyFormat == MailFormat.Html)
					{
						SmtpMail.CdoSysHelper._helper.SetProp(obj, "HtmlBody", message.Body);
					}
					else
					{
						SmtpMail.CdoSysHelper._helper.SetProp(obj, "TextBody", message.Body);
					}
				}
				else
				{
					SmtpMail.CdoSysHelper._helper.SetProp(obj, "TextBody", string.Empty);
				}
				foreach (object obj2 in message.Attachments)
				{
					MailAttachment mailAttachment = (MailAttachment)obj2;
					SmtpMail.LateBoundAccessHelper helper = SmtpMail.CdoSysHelper._helper;
					object obj3 = obj;
					string text2 = "AddAttachment";
					object[] array = new object[3];
					array[0] = mailAttachment.Filename;
					object obj4 = helper.CallMethod(obj3, text2, array);
					if (mailAttachment.Encoding == MailEncoding.UUEncode)
					{
						SmtpMail.CdoSysHelper._helper.SetProp(obj, "MimeFormatted", false);
					}
					if (obj4 != null)
					{
						Marshal.ReleaseComObject(obj4);
					}
				}
				string smtpServer = SmtpMail.SmtpServer;
				if (!string.IsNullOrEmpty(smtpServer) || message.Fields.Count > 0)
				{
					object propStatic = SmtpMail.LateBoundAccessHelper.GetPropStatic(obj, "Configuration");
					if (propStatic != null)
					{
						SmtpMail.LateBoundAccessHelper.SetPropStatic(propStatic, "Fields", "http://schemas.microsoft.com/cdo/configuration/sendusing", 2);
						SmtpMail.LateBoundAccessHelper.SetPropStatic(propStatic, "Fields", "http://schemas.microsoft.com/cdo/configuration/smtpserverport", 25);
						if (!string.IsNullOrEmpty(smtpServer))
						{
							SmtpMail.LateBoundAccessHelper.SetPropStatic(propStatic, "Fields", "http://schemas.microsoft.com/cdo/configuration/smtpserver", smtpServer);
						}
						foreach (object obj5 in message.Fields)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj5;
							SmtpMail.LateBoundAccessHelper.SetPropStatic(propStatic, "Fields", (string)dictionaryEntry.Key, dictionaryEntry.Value);
						}
						object propStatic2 = SmtpMail.LateBoundAccessHelper.GetPropStatic(propStatic, "Fields");
						SmtpMail.LateBoundAccessHelper.CallMethodStatic(propStatic2, "Update", new object[0]);
						Marshal.ReleaseComObject(propStatic2);
						Marshal.ReleaseComObject(propStatic);
					}
				}
				if (HostingEnvironment.IsHosted)
				{
					using (new ProcessImpersonationContext())
					{
						SmtpMail.CdoSysHelper._helper.CallMethod(obj, "Send", new object[0]);
						goto IL_03C0;
					}
				}
				SmtpMail.CdoSysHelper._helper.CallMethod(obj, "Send", new object[0]);
				IL_03C0:
				Marshal.ReleaseComObject(obj);
			}

			// Token: 0x06005CEF RID: 23791 RVA: 0x00174B90 File Offset: 0x00173B90
			internal static void Send(string from, string to, string subject, string messageText)
			{
				SmtpMail.CdoSysHelper.Send(new MailMessage
				{
					From = from,
					To = to,
					Subject = subject,
					Body = messageText
				});
			}

			// Token: 0x0400319A RID: 12698
			private static SmtpMail.LateBoundAccessHelper _helper = new SmtpMail.LateBoundAccessHelper("CDO.Message");

			// Token: 0x0400319B RID: 12699
			private static SmtpMail.CdoSysHelper.CdoSysLibraryStatus cdoSysLibraryInfo = SmtpMail.CdoSysHelper.CdoSysLibraryStatus.NotChecked;

			// Token: 0x02000789 RID: 1929
			private enum CdoSysLibraryStatus
			{
				// Token: 0x0400319D RID: 12701
				NotChecked,
				// Token: 0x0400319E RID: 12702
				Exists,
				// Token: 0x0400319F RID: 12703
				DoesntExist
			}
		}
	}
}
