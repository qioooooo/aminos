using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Web.Profile
{
	// Token: 0x0200030C RID: 780
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileModule : IHttpModule
	{
		// Token: 0x06002683 RID: 9859 RVA: 0x000A5277 File Offset: 0x000A4277
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public ProfileModule()
		{
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06002684 RID: 9860 RVA: 0x000A527F File Offset: 0x000A427F
		// (remove) Token: 0x06002685 RID: 9861 RVA: 0x000A5298 File Offset: 0x000A4298
		public event ProfileEventHandler Personalize
		{
			add
			{
				this._eventHandler = (ProfileEventHandler)Delegate.Combine(this._eventHandler, value);
			}
			remove
			{
				this._eventHandler = (ProfileEventHandler)Delegate.Remove(this._eventHandler, value);
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06002686 RID: 9862 RVA: 0x000A52B1 File Offset: 0x000A42B1
		// (remove) Token: 0x06002687 RID: 9863 RVA: 0x000A52CA File Offset: 0x000A42CA
		public event ProfileMigrateEventHandler MigrateAnonymous
		{
			add
			{
				this._MigrateEventHandler = (ProfileMigrateEventHandler)Delegate.Combine(this._MigrateEventHandler, value);
			}
			remove
			{
				this._MigrateEventHandler = (ProfileMigrateEventHandler)Delegate.Remove(this._MigrateEventHandler, value);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06002688 RID: 9864 RVA: 0x000A52E3 File Offset: 0x000A42E3
		// (remove) Token: 0x06002689 RID: 9865 RVA: 0x000A52FC File Offset: 0x000A42FC
		public event ProfileAutoSaveEventHandler ProfileAutoSaving
		{
			add
			{
				this._AutoSaveEventHandler = (ProfileAutoSaveEventHandler)Delegate.Combine(this._AutoSaveEventHandler, value);
			}
			remove
			{
				this._AutoSaveEventHandler = (ProfileAutoSaveEventHandler)Delegate.Remove(this._AutoSaveEventHandler, value);
			}
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000A5315 File Offset: 0x000A4315
		public void Dispose()
		{
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000A5317 File Offset: 0x000A4317
		public void Init(HttpApplication app)
		{
			if (ProfileManager.Enabled)
			{
				app.AcquireRequestState += this.OnEnter;
				if (ProfileManager.AutomaticSaveEnabled)
				{
					app.EndRequest += this.OnLeave;
				}
			}
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000A534B File Offset: 0x000A434B
		private void OnPersonalize(ProfileEventArgs e)
		{
			if (this._eventHandler != null)
			{
				this._eventHandler(this, e);
			}
			if (e.Profile != null)
			{
				e.Context._Profile = e.Profile;
				return;
			}
			e.Context._ProfileDelayLoad = true;
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000A5388 File Offset: 0x000A4388
		private void OnEnter(object source, EventArgs eventArgs)
		{
			HttpContext context = ((HttpApplication)source).Context;
			this.OnPersonalize(new ProfileEventArgs(context));
			if (context.Request.IsAuthenticated && !string.IsNullOrEmpty(context.Request.AnonymousID) && this._MigrateEventHandler != null)
			{
				ProfileMigrateEventArgs profileMigrateEventArgs = new ProfileMigrateEventArgs(context, context.Request.AnonymousID);
				this._MigrateEventHandler(this, profileMigrateEventArgs);
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000A53F4 File Offset: 0x000A43F4
		private void OnLeave(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (context._Profile == null || context._Profile == ProfileBase.SingletonInstance)
			{
				return;
			}
			if (this._AutoSaveEventHandler != null)
			{
				ProfileAutoSaveEventArgs profileAutoSaveEventArgs = new ProfileAutoSaveEventArgs(context);
				this._AutoSaveEventHandler(this, profileAutoSaveEventArgs);
				if (!profileAutoSaveEventArgs.ContinueWithProfileAutoSave)
				{
					return;
				}
			}
			context.Profile.Save();
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000A5458 File Offset: 0x000A4458
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		internal static void ParseDataFromDB(string[] names, string values, byte[] buf, SettingsPropertyValueCollection properties)
		{
			if (names == null || values == null || buf == null || properties == null)
			{
				return;
			}
			try
			{
				for (int i = 0; i < names.Length / 4; i++)
				{
					string text = names[i * 4];
					SettingsPropertyValue settingsPropertyValue = properties[text];
					if (settingsPropertyValue != null)
					{
						int num = int.Parse(names[i * 4 + 2], CultureInfo.InvariantCulture);
						int num2 = int.Parse(names[i * 4 + 3], CultureInfo.InvariantCulture);
						if (num2 == -1 && !settingsPropertyValue.Property.PropertyType.IsValueType)
						{
							settingsPropertyValue.PropertyValue = null;
							settingsPropertyValue.IsDirty = false;
							settingsPropertyValue.Deserialized = true;
						}
						if (names[i * 4 + 1] == "S" && num >= 0 && num2 > 0 && values.Length >= num + num2)
						{
							settingsPropertyValue.SerializedValue = values.Substring(num, num2);
						}
						if (names[i * 4 + 1] == "B" && num >= 0 && num2 > 0 && buf.Length >= num + num2)
						{
							byte[] array = new byte[num2];
							Buffer.BlockCopy(buf, num, array, 0, num2);
							settingsPropertyValue.SerializedValue = array;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000A557C File Offset: 0x000A457C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		internal static void PrepareDataForSaving(ref string allNames, ref string allValues, ref byte[] buf, bool binarySupported, SettingsPropertyValueCollection properties, bool userIsAuthenticated)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			MemoryStream memoryStream = (binarySupported ? new MemoryStream() : null);
			try
			{
				try
				{
					bool flag = false;
					foreach (object obj in properties)
					{
						SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj;
						if (settingsPropertyValue.IsDirty)
						{
							if (!userIsAuthenticated)
							{
								bool flag2 = (bool)settingsPropertyValue.Property.Attributes["AllowAnonymous"];
								if (!flag2)
								{
									continue;
								}
							}
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return;
					}
					foreach (object obj2 in properties)
					{
						SettingsPropertyValue settingsPropertyValue2 = (SettingsPropertyValue)obj2;
						if (!userIsAuthenticated)
						{
							bool flag3 = (bool)settingsPropertyValue2.Property.Attributes["AllowAnonymous"];
							if (!flag3)
							{
								continue;
							}
						}
						if (settingsPropertyValue2.IsDirty || !settingsPropertyValue2.UsingDefaultValue)
						{
							int num = 0;
							string text = null;
							int num2;
							if (settingsPropertyValue2.Deserialized && settingsPropertyValue2.PropertyValue == null)
							{
								num2 = -1;
							}
							else
							{
								object obj3 = settingsPropertyValue2.SerializedValue;
								if (obj3 == null)
								{
									num2 = -1;
								}
								else
								{
									if (!(obj3 is string) && !binarySupported)
									{
										obj3 = Convert.ToBase64String((byte[])obj3);
									}
									if (obj3 is string)
									{
										text = (string)obj3;
										num2 = text.Length;
										num = stringBuilder2.Length;
									}
									else
									{
										byte[] array = (byte[])obj3;
										num = (int)memoryStream.Position;
										memoryStream.Write(array, 0, array.Length);
										memoryStream.Position = (long)(num + array.Length);
										num2 = array.Length;
									}
								}
							}
							stringBuilder.Append(string.Concat(new string[]
							{
								settingsPropertyValue2.Name,
								":",
								(text != null) ? "S" : "B",
								":",
								num.ToString(CultureInfo.InvariantCulture),
								":",
								num2.ToString(CultureInfo.InvariantCulture),
								":"
							}));
							if (text != null)
							{
								stringBuilder2.Append(text);
							}
						}
					}
					if (binarySupported)
					{
						buf = memoryStream.ToArray();
					}
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
			}
			catch
			{
				throw;
			}
			allNames = stringBuilder.ToString();
			allValues = stringBuilder2.ToString();
		}

		// Token: 0x04001DCB RID: 7627
		private static object s_Lock = new object();

		// Token: 0x04001DCC RID: 7628
		private ProfileEventHandler _eventHandler;

		// Token: 0x04001DCD RID: 7629
		private ProfileMigrateEventHandler _MigrateEventHandler;

		// Token: 0x04001DCE RID: 7630
		private ProfileAutoSaveEventHandler _AutoSaveEventHandler;
	}
}
