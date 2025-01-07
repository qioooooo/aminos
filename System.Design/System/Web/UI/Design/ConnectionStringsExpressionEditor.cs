using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design
{
	public class ConnectionStringsExpressionEditor : ExpressionEditor
	{
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

		public override ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet(expression, this, serviceProvider);
		}

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

		private class ConnectionStringsExpressionEditorSheet : ExpressionEditorSheet
		{
			public ConnectionStringsExpressionEditorSheet(string expression, ConnectionStringsExpressionEditor owner, IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this._owner = owner;
				bool flag;
				this._connectionName = ConnectionStringsExpressionEditor.ParseExpression(expression, out flag);
				this._connectionType = (flag ? ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType.ConnectionString : ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType.ProviderName);
			}

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

			public override bool IsValid
			{
				get
				{
					return !string.IsNullOrEmpty(this.ConnectionName);
				}
			}

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

			private string _connectionName;

			private ConnectionStringsExpressionEditor.ConnectionStringsExpressionEditorSheet.ConnectionType _connectionType;

			private ConnectionStringsExpressionEditor _owner;

			public enum ConnectionType
			{
				ConnectionString,
				ProviderName
			}

			private class ConnectionStringsTypeConverter : TypeConverter
			{
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
				}

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

				public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
				{
					return destType == typeof(string) || base.CanConvertTo(context, destType);
				}

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

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return false;
				}

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

				private static readonly string NoConnectionName = "(None)";
			}
		}
	}
}
