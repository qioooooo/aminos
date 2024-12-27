using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Web.Hosting;

namespace System.Web.Util
{
	// Token: 0x02000780 RID: 1920
	internal static class SecUtility
	{
		// Token: 0x06005CB2 RID: 23730 RVA: 0x00173870 File Offset: 0x00172870
		internal static string GetDefaultAppName()
		{
			string text2;
			try
			{
				string text = HostingEnvironment.ApplicationVirtualPath;
				if (string.IsNullOrEmpty(text))
				{
					text = Process.GetCurrentProcess().MainModule.ModuleName;
					int num = text.IndexOf('.');
					if (num != -1)
					{
						text = text.Remove(num);
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text2 = "/";
				}
				else
				{
					text2 = text;
				}
			}
			catch
			{
				text2 = "/";
			}
			return text2;
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x001738E0 File Offset: 0x001728E0
		internal static bool ValidatePasswordParameter(ref string param, int maxSize)
		{
			return param != null && param.Length >= 1 && (maxSize <= 0 || param.Length <= maxSize);
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x00173908 File Offset: 0x00172908
		internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
		{
			if (param == null)
			{
				return !checkForNull;
			}
			param = param.Trim();
			return (!checkIfEmpty || param.Length >= 1) && (maxSize <= 0 || param.Length <= maxSize) && (!checkForCommas || !param.Contains(","));
		}

		// Token: 0x06005CB5 RID: 23733 RVA: 0x00173958 File Offset: 0x00172958
		internal static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (param.Length < 1)
			{
				throw new ArgumentException(SR.GetString("Parameter_can_not_be_empty", new object[] { paramName }), paramName);
			}
			if (maxSize > 0 && param.Length > maxSize)
			{
				throw new ArgumentException(SR.GetString("Parameter_too_long", new object[]
				{
					paramName,
					maxSize.ToString(CultureInfo.InvariantCulture)
				}), paramName);
			}
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x001739D4 File Offset: 0x001729D4
		internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
		{
			if (param == null)
			{
				if (checkForNull)
				{
					throw new ArgumentNullException(paramName);
				}
				return;
			}
			else
			{
				param = param.Trim();
				if (checkIfEmpty && param.Length < 1)
				{
					throw new ArgumentException(SR.GetString("Parameter_can_not_be_empty", new object[] { paramName }), paramName);
				}
				if (maxSize > 0 && param.Length > maxSize)
				{
					throw new ArgumentException(SR.GetString("Parameter_too_long", new object[]
					{
						paramName,
						maxSize.ToString(CultureInfo.InvariantCulture)
					}), paramName);
				}
				if (checkForCommas && param.Contains(","))
				{
					throw new ArgumentException(SR.GetString("Parameter_can_not_contain_comma", new object[] { paramName }), paramName);
				}
				return;
			}
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x00173A94 File Offset: 0x00172A94
		internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (param.Length < 1)
			{
				throw new ArgumentException(SR.GetString("Parameter_array_empty", new object[] { paramName }), paramName);
			}
			Hashtable hashtable = new Hashtable(param.Length);
			for (int i = param.Length - 1; i >= 0; i--)
			{
				SecUtility.CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
				if (hashtable.Contains(param[i]))
				{
					throw new ArgumentException(SR.GetString("Parameter_duplicate_array_element", new object[] { paramName }), paramName);
				}
				hashtable.Add(param[i], param[i]);
			}
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00173B58 File Offset: 0x00172B58
		internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
		{
			string text = config[valueName];
			if (text == null)
			{
				return defaultValue;
			}
			bool flag;
			if (bool.TryParse(text, out flag))
			{
				return flag;
			}
			throw new ProviderException(SR.GetString("Value_must_be_boolean", new object[] { valueName }));
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x00173B9C File Offset: 0x00172B9C
		internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
		{
			string text = config[valueName];
			if (text == null)
			{
				return defaultValue;
			}
			int num;
			if (!int.TryParse(text, out num))
			{
				if (zeroAllowed)
				{
					throw new ProviderException(SR.GetString("Value_must_be_non_negative_integer", new object[] { valueName }));
				}
				throw new ProviderException(SR.GetString("Value_must_be_positive_integer", new object[] { valueName }));
			}
			else
			{
				if (zeroAllowed && num < 0)
				{
					throw new ProviderException(SR.GetString("Value_must_be_non_negative_integer", new object[] { valueName }));
				}
				if (!zeroAllowed && num <= 0)
				{
					throw new ProviderException(SR.GetString("Value_must_be_positive_integer", new object[] { valueName }));
				}
				if (maxValueAllowed > 0 && num > maxValueAllowed)
				{
					throw new ProviderException(SR.GetString("Value_too_big", new object[]
					{
						valueName,
						maxValueAllowed.ToString(CultureInfo.InvariantCulture)
					}));
				}
				return num;
			}
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x00173C84 File Offset: 0x00172C84
		internal static void CheckSchemaVersion(ProviderBase provider, SqlConnection connection, string[] features, string version, ref int schemaVersionCheck)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (features == null)
			{
				throw new ArgumentNullException("features");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (schemaVersionCheck == -1)
			{
				throw new ProviderException(SR.GetString("Provider_Schema_Version_Not_Match", new object[]
				{
					provider.ToString(),
					version
				}));
			}
			if (schemaVersionCheck == 0)
			{
				lock (provider)
				{
					if (schemaVersionCheck == -1)
					{
						throw new ProviderException(SR.GetString("Provider_Schema_Version_Not_Match", new object[]
						{
							provider.ToString(),
							version
						}));
					}
					if (schemaVersionCheck == 0)
					{
						foreach (string text in features)
						{
							SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_CheckSchemaVersion", connection);
							sqlCommand.CommandType = CommandType.StoredProcedure;
							SqlParameter sqlParameter = new SqlParameter("@Feature", text);
							sqlCommand.Parameters.Add(sqlParameter);
							sqlParameter = new SqlParameter("@CompatibleSchemaVersion", version);
							sqlCommand.Parameters.Add(sqlParameter);
							sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
							sqlParameter.Direction = ParameterDirection.ReturnValue;
							sqlCommand.Parameters.Add(sqlParameter);
							sqlCommand.ExecuteNonQuery();
							int num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
							if (num != 0)
							{
								schemaVersionCheck = -1;
								throw new ProviderException(SR.GetString("Provider_Schema_Version_Not_Match", new object[]
								{
									provider.ToString(),
									version
								}));
							}
						}
						schemaVersionCheck = 1;
					}
				}
			}
		}
	}
}
