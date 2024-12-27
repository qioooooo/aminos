using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design
{
	// Token: 0x02000329 RID: 809
	public class ConnectionStringsExpressionEditor : ExpressionEditor
	{
		// Token: 0x06001E56 RID: 7766 RVA: 0x000AC588 File Offset: 0x000AB588
		private ConnectionStringSettingsCollection GetConnectionStringSettingsCollection(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
				if (webApplication != null)
				{
					Configuration configuration = webApplication.OpenWebConfiguration(true);
					if (configuration != null)
					{
						ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
						if (connectionStringsSection != null)
						{
							return connectionStringsSection.ConnectionStrings;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000AC5D8 File Offset: 0x000AB5D8
		public override ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet(expression, this, serviceProvider);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000AC5E4 File Offset: 0x000AB5E4
		public override object EvaluateExpression(string expression, object parseTimeData, Type propertyType, IServiceProvider serviceProvider)
		{
			Pair pair = (Pair)parseTimeData;
			string text = (string)pair.First;
			bool flag = (bool)pair.Second;
			ConnectionStringSettingsCollection connectionStringSettingsCollection = this.GetConnectionStringSettingsCollection(serviceProvider);
			ConnectionStringSettings connectionStringSettings = null;
			foreach (object obj in connectionStringSettingsCollection)
			{
				ConnectionStringSettings connectionStringSettings2 = (ConnectionStringSettings)obj;
				if (string.Equals(text, connectionStringSettings2.Name, StringComparison.OrdinalIgnoreCase))
				{
					connectionStringSettings = connectionStringSettings2;
					break;
				}
			}
			if (connectionStringSettings == null)
			{
				return null;
			}
			if (flag)
			{
				return connectionStringSettings.ConnectionString;
			}
			return connectionStringSettings.ProviderName;
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000AC694 File Offset: 0x000AB694
		private static string ParseExpression(string expression, out bool isConnectionString)
		{
			isConnectionString = true;
			expression = expression.Trim();
			if (expression.EndsWith(".connectionstring", StringComparison.OrdinalIgnoreCase))
			{
				return expression.Substring(0, expression.Length - ".connectionstring".Length);
			}
			if (expression.EndsWith(".providername", StringComparison.OrdinalIgnoreCase))
			{
				isConnectionString = false;
				return expression.Substring(0, expression.Length - ".providername".Length);
			}
			return expression;
		}

		// Token: 0x0200032A RID: 810
		private class ConnectionStringsExpressionEditorSheet : ExpressionEditorSheet
		{
			// Token: 0x06001E5B RID: 7771 RVA: 0x000AC708 File Offset: 0x000AB708
			public ConnectionStringsExpressionEditorSheet(string expression, ConnectionStringsExpressionEditor owner, IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this._owner = owner;
				bool flag;
				this._connectionName = ConnectionStringsExpressionEditor.ParseExpression(expression, out flag);
				this._connectionType = (flag ? ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType.ConnectionString : ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType.ProviderName);
			}

			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x06001E5C RID: 7772 RVA: 0x000AC73E File Offset: 0x000AB73E
			// (set) Token: 0x06001E5D RID: 7773 RVA: 0x000AC746 File Offset: 0x000AB746
			[TypeConverter(typeof(ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionStringsTypeConverter))]
			[SRDescription("ConnectionStringsExpressionEditor_ConnectionName")]
			[DefaultValue("")]
			public string ConnectionName
			{
				get
				{
					return this._connectionName;
				}
				set
				{
					this._connectionName = value;
				}
			}

			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x06001E5E RID: 7774 RVA: 0x000AC74F File Offset: 0x000AB74F
			public override bool IsValid
			{
				get
				{
					return !string.IsNullOrEmpty(this.ConnectionName);
				}
			}

			// Token: 0x1700054A RID: 1354
			// (get) Token: 0x06001E5F RID: 7775 RVA: 0x000AC75F File Offset: 0x000AB75F
			// (set) Token: 0x06001E60 RID: 7776 RVA: 0x000AC767 File Offset: 0x000AB767
			[SRDescription("ConnectionStringsExpressionEditor_ConnectionType")]
			[DefaultValue(ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType.ConnectionString)]
			public ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType Type
			{
				get
				{
					return this._connectionType;
				}
				set
				{
					this._connectionType = value;
				}
			}

			// Token: 0x06001E61 RID: 7777 RVA: 0x000AC770 File Offset: 0x000AB770
			public override string GetExpression()
			{
				if (string.IsNullOrEmpty(this._connectionName))
				{
					return string.Empty;
				}
				string text = this._connectionName;
				if (this.Type == ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType.ProviderName)
				{
					text += ".ProviderName";
				}
				return text;
			}

			// Token: 0x04001740 RID: 5952
			private string _connectionName;

			// Token: 0x04001741 RID: 5953
			private ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType _connectionType;

			// Token: 0x04001742 RID: 5954
			private ConnectionStringsExpressionEditor _owner;

			// Token: 0x0200032B RID: 811
			public enum ConnectionType
			{
				// Token: 0x04001744 RID: 5956
				ConnectionString,
				// Token: 0x04001745 RID: 5957
				ProviderName
			}

			// Token: 0x0200032C RID: 812
			private class ConnectionStringsTypeConverter : TypeConverter
			{
				// Token: 0x06001E62 RID: 7778 RVA: 0x000AC7AD File Offset: 0x000AB7AD
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
				}

				// Token: 0x06001E63 RID: 7779 RVA: 0x000AC7C6 File Offset: 0x000AB7C6
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					if (!(value is string))
					{
						return base.ConvertFrom(context, culture, value);
					}
					if (string.Equals((string)value, ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionStringsTypeConverter.NoConnectionName, StringComparison.OrdinalIgnoreCase))
					{
						return string.Empty;
					}
					return value;
				}

				// Token: 0x06001E64 RID: 7780 RVA: 0x000AC7F4 File Offset: 0x000AB7F4
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
				{
					return destType == typeof(string) || base.CanConvertTo(context, destType);
				}

				// Token: 0x06001E65 RID: 7781 RVA: 0x000AC80D File Offset: 0x000AB80D
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (!(value is string))
					{
						return base.ConvertTo(context, culture, value, destinationType);
					}
					if (((string)value).Length == 0)
					{
						return ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionStringsTypeConverter.NoConnectionName;
					}
					return value;
				}

				// Token: 0x06001E66 RID: 7782 RVA: 0x000AC837 File Offset: 0x000AB837
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return false;
				}

				// Token: 0x06001E67 RID: 7783 RVA: 0x000AC83C File Offset: 0x000AB83C
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					if (context != null)
					{
						ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet connectionStringsExpressionEditorSheet = (ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet)context.Instance;
						ConnectionStringsExpressionEditor owner = connectionStringsExpressionEditorSheet._owner;
						ConnectionStringSettingsCollection connectionStringSettingsCollection = owner.GetConnectionStringSettingsCollection(connectionStringsExpressionEditorSheet.ServiceProvider);
						if (connectionStringSettingsCollection != null)
						{
							return connectionStringSettingsCollection.Count > 0;
						}
					}
					return base.GetStandardValuesSupported(context);
				}

				// Token: 0x06001E68 RID: 7784 RVA: 0x000AC880 File Offset: 0x000AB880
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					if (context != null)
					{
						ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet connectionStringsExpressionEditorSheet = (ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet)context.Instance;
						ConnectionStringsExpressionEditor owner = connectionStringsExpressionEditorSheet._owner;
						ConnectionStringSettingsCollection connectionStringSettingsCollection = owner.GetConnectionStringSettingsCollection(connectionStringsExpressionEditorSheet.ServiceProvider);
						if (connectionStringSettingsCollection != null)
						{
							ArrayList arrayList = new ArrayList();
							foreach (object obj in connectionStringSettingsCollection)
							{
								ConnectionStringSettings connectionStringSettings = (ConnectionStringSettings)obj;
								arrayList.Add(connectionStringSettings.Name);
							}
							arrayList.Sort();
							arrayList.Add(string.Empty);
							return new TypeConverter.StandardValuesCollection(arrayList);
						}
					}
					return base.GetStandardValues(context);
				}

				// Token: 0x04001746 RID: 5958
				private static readonly string NoConnectionName = "(None)";
			}
		}
	}
}
