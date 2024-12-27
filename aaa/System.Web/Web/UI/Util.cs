using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000483 RID: 1155
	internal static class Util
	{
		// Token: 0x06003608 RID: 13832 RVA: 0x000E999C File Offset: 0x000E899C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		internal static string SerializeWithAssert(IStateFormatter formatter, object stateGraph)
		{
			return formatter.Serialize(stateGraph);
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000E99A5 File Offset: 0x000E89A5
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		internal static object DeserializeWithAssert(IStateFormatter formatter, string serializedState)
		{
			return formatter.Deserialize(serializedState);
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x000E99AE File Offset: 0x000E89AE
		internal static bool CanConvertToFrom(TypeConverter converter, Type type)
		{
			return converter != null && converter.CanConvertTo(type) && converter.CanConvertFrom(type) && !(converter is ReferenceConverter);
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000E99D4 File Offset: 0x000E89D4
		internal static void CopyBaseAttributesToInnerControl(WebControl control, WebControl child)
		{
			short tabIndex = control.TabIndex;
			string accessKey = control.AccessKey;
			try
			{
				control.AccessKey = string.Empty;
				control.TabIndex = 0;
				child.CopyBaseAttributes(control);
			}
			finally
			{
				control.TabIndex = tabIndex;
				control.AccessKey = accessKey;
			}
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000E9A2C File Offset: 0x000E8A2C
		internal static long GetRecompilationHash(PagesSection ps)
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddObject(ps.Buffer);
			hashCodeCombiner.AddObject(ps.EnableViewState);
			hashCodeCombiner.AddObject(ps.EnableViewStateMac);
			hashCodeCombiner.AddObject(ps.EnableEventValidation);
			hashCodeCombiner.AddObject(ps.SmartNavigation);
			hashCodeCombiner.AddObject(ps.ValidateRequest);
			hashCodeCombiner.AddObject(ps.AutoEventWireup);
			if (ps.PageBaseTypeInternal != null)
			{
				hashCodeCombiner.AddObject(ps.PageBaseTypeInternal.FullName);
			}
			if (ps.UserControlBaseTypeInternal != null)
			{
				hashCodeCombiner.AddObject(ps.UserControlBaseTypeInternal.FullName);
			}
			if (ps.PageParserFilterTypeInternal != null)
			{
				hashCodeCombiner.AddObject(ps.PageParserFilterTypeInternal.FullName);
			}
			hashCodeCombiner.AddObject(ps.MasterPageFile);
			hashCodeCombiner.AddObject(ps.Theme);
			hashCodeCombiner.AddObject(ps.StyleSheetTheme);
			hashCodeCombiner.AddObject(ps.EnableSessionState);
			hashCodeCombiner.AddObject(ps.CompilationMode);
			hashCodeCombiner.AddObject(ps.MaxPageStateFieldLength);
			hashCodeCombiner.AddObject(ps.ViewStateEncryptionMode);
			hashCodeCombiner.AddObject(ps.MaintainScrollPositionOnPostBack);
			NamespaceCollection namespaces = ps.Namespaces;
			hashCodeCombiner.AddObject(namespaces.AutoImportVBNamespace);
			if (namespaces.Count == 0)
			{
				hashCodeCombiner.AddObject("__clearnamespaces");
			}
			else
			{
				foreach (object obj in namespaces)
				{
					NamespaceInfo namespaceInfo = (NamespaceInfo)obj;
					hashCodeCombiner.AddObject(namespaceInfo.Namespace);
				}
			}
			TagPrefixCollection controls = ps.Controls;
			if (controls.Count == 0)
			{
				hashCodeCombiner.AddObject("__clearcontrols");
			}
			else
			{
				foreach (object obj2 in controls)
				{
					TagPrefixInfo tagPrefixInfo = (TagPrefixInfo)obj2;
					hashCodeCombiner.AddObject(tagPrefixInfo.TagPrefix);
					if (tagPrefixInfo.TagName != null && tagPrefixInfo.TagName.Length != 0)
					{
						hashCodeCombiner.AddObject(tagPrefixInfo.TagName);
						hashCodeCombiner.AddObject(tagPrefixInfo.Source);
					}
					else
					{
						hashCodeCombiner.AddObject(tagPrefixInfo.Namespace);
						hashCodeCombiner.AddObject(tagPrefixInfo.Assembly);
					}
				}
			}
			TagMapCollection tagMapping = ps.TagMapping;
			if (tagMapping.Count == 0)
			{
				hashCodeCombiner.AddObject("__cleartagmapping");
			}
			else
			{
				foreach (object obj3 in tagMapping)
				{
					TagMapInfo tagMapInfo = (TagMapInfo)obj3;
					hashCodeCombiner.AddObject(tagMapInfo.TagType);
					hashCodeCombiner.AddObject(tagMapInfo.MappedTagType);
				}
			}
			return hashCodeCombiner.CombinedHash;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000E9D08 File Offset: 0x000E8D08
		internal static Encoding GetEncodingFromConfigPath(VirtualPath configPath)
		{
			GlobalizationSection globalization = RuntimeConfig.GetConfig(configPath).Globalization;
			Encoding encoding = globalization.FileEncoding;
			if (encoding == null)
			{
				encoding = Encoding.Default;
			}
			return encoding;
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000E9D34 File Offset: 0x000E8D34
		internal static StreamReader ReaderFromFile(string filename, VirtualPath configPath)
		{
			Encoding encoding = Encoding.Default;
			if (configPath != null)
			{
				encoding = Util.GetEncodingFromConfigPath(configPath);
			}
			StreamReader streamReader;
			try
			{
				streamReader = new StreamReader(filename, encoding, true, 4096);
			}
			catch (UnauthorizedAccessException)
			{
				if (FileUtil.DirectoryExists(filename))
				{
					throw new HttpException(SR.GetString("Unexpected_Directory", new object[] { HttpRuntime.GetSafePath(filename) }));
				}
				throw;
			}
			return streamReader;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000E9DA4 File Offset: 0x000E8DA4
		internal static void DeleteFileNoException(string path)
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000E9DCC File Offset: 0x000E8DCC
		internal static void DeleteFileIfExistsNoException(string path)
		{
			if (File.Exists(path))
			{
				Util.DeleteFileNoException(path);
			}
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000E9DDC File Offset: 0x000E8DDC
		internal static bool IsNonEmptyDirectory(string dir)
		{
			if (!Directory.Exists(dir))
			{
				return false;
			}
			bool flag;
			try
			{
				string[] fileSystemEntries = Directory.GetFileSystemEntries(dir);
				flag = fileSystemEntries.Length > 0;
			}
			catch
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000E9E1C File Offset: 0x000E8E1C
		internal static bool IsValidFileName(string fileName)
		{
			return !(fileName == ".") && !(fileName == "..") && fileName.IndexOfAny(Util.invalidFileNameChars) < 0;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000E9E4C File Offset: 0x000E8E4C
		internal static string MakeValidFileName(string fileName)
		{
			if (Util.IsValidFileName(fileName))
			{
				return fileName;
			}
			for (int i = 0; i < Util.invalidFileNameChars.Length; i++)
			{
				fileName = fileName.Replace(Util.invalidFileNameChars[i], '_');
			}
			return fileName;
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x000E9E88 File Offset: 0x000E8E88
		internal static bool HasWriteAccessToDirectory(string dir)
		{
			if (!Directory.Exists(dir))
			{
				return false;
			}
			string text = Path.Combine(dir, "~AspAccessCheck_" + HostingEnvironment.AppDomainUniqueInteger.ToString("x", CultureInfo.InvariantCulture) + ".tmp");
			FileStream fileStream = null;
			bool flag = false;
			try
			{
				fileStream = new FileStream(text, FileMode.Create);
			}
			catch
			{
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					File.Delete(text);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000E9F10 File Offset: 0x000E8F10
		internal static VirtualPath GetScriptLocation()
		{
			string text = (string)RuntimeConfig.GetRootWebConfig().WebControls["clientScriptsLocation"];
			if (text.IndexOf("{0}", StringComparison.Ordinal) >= 0)
			{
				string text2 = "system_web";
				string text3 = VersionInfo.EngineVersion.Substring(0, VersionInfo.EngineVersion.LastIndexOf('.')).Replace('.', '_');
				text = string.Format(CultureInfo.InvariantCulture, text, new object[] { text2, text3 });
			}
			return VirtualPath.Create(text);
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x000E9F90 File Offset: 0x000E8F90
		internal static StreamReader ReaderFromStream(Stream stream, VirtualPath configPath)
		{
			Encoding encodingFromConfigPath = Util.GetEncodingFromConfigPath(configPath);
			return new StreamReader(stream, encodingFromConfigPath, true, 4096);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000E9FB4 File Offset: 0x000E8FB4
		internal static string StringFromVirtualPath(VirtualPath virtualPath)
		{
			string text;
			using (Stream stream = virtualPath.OpenFile())
			{
				TextReader textReader = Util.ReaderFromStream(stream, virtualPath);
				text = textReader.ReadToEnd();
			}
			return text;
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000E9FF4 File Offset: 0x000E8FF4
		internal static string StringFromFile(string path)
		{
			Encoding @default = Encoding.Default;
			return Util.StringFromFile(path, ref @default);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000EA010 File Offset: 0x000E9010
		internal static string StringFromFile(string path, ref Encoding encoding)
		{
			StreamReader streamReader = new StreamReader(path, encoding, true);
			string text2;
			try
			{
				string text = streamReader.ReadToEnd();
				encoding = streamReader.CurrentEncoding;
				text2 = text;
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			return text2;
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000EA058 File Offset: 0x000E9058
		internal static string StringFromFileIfExists(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			return Util.StringFromFile(path);
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x000EA06C File Offset: 0x000E906C
		internal static void RemoveOrRenameFile(string filename)
		{
			FileInfo fileInfo = new FileInfo(filename);
			Util.RemoveOrRenameFile(fileInfo);
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000EA088 File Offset: 0x000E9088
		internal static bool RemoveOrRenameFile(FileInfo f)
		{
			try
			{
				f.Delete();
				return true;
			}
			catch
			{
				try
				{
					if (f.Extension != ".delete")
					{
						string text = DateTime.Now.Ticks.GetHashCode().ToString("x", CultureInfo.InvariantCulture);
						string text2 = f.FullName + "." + text + ".delete";
						f.MoveTo(text2);
					}
				}
				catch
				{
				}
			}
			return false;
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000EA120 File Offset: 0x000E9120
		internal static void ClearReadOnlyAttribute(string path)
		{
			FileAttributes attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
			{
				File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
			}
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000EA144 File Offset: 0x000E9144
		internal static void CheckVirtualFileExists(VirtualPath virtualPath)
		{
			if (!virtualPath.FileExists())
			{
				throw new HttpException(404, SR.GetString("FileName_does_not_exist", new object[] { virtualPath.VirtualPathString }));
			}
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000EA180 File Offset: 0x000E9180
		internal static bool VirtualFileExistsWithAssert(VirtualPath virtualPath)
		{
			string text = virtualPath.MapPathInternal();
			if (text != null)
			{
				InternalSecurityPermissions.PathDiscovery(text).Assert();
			}
			return virtualPath.FileExists();
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000EA1A8 File Offset: 0x000E91A8
		internal static void CheckThemeAttribute(string themeName)
		{
			if (themeName.Length > 0)
			{
				if (!FileUtil.IsValidDirectoryName(themeName))
				{
					throw new HttpException(SR.GetString("Page_theme_invalid_name", new object[] { themeName }));
				}
				if (!Util.ThemeExists(themeName))
				{
					throw new HttpException(SR.GetString("Page_theme_not_found", new object[] { themeName }));
				}
			}
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x000EA208 File Offset: 0x000E9208
		internal static bool ThemeExists(string themeName)
		{
			VirtualPath virtualPath = ThemeDirectoryCompiler.GetAppThemeVirtualDir(themeName);
			if (!Util.VirtualDirectoryExistsWithAssert(virtualPath))
			{
				virtualPath = ThemeDirectoryCompiler.GetGlobalThemeVirtualDir(themeName);
				if (!Util.VirtualDirectoryExistsWithAssert(virtualPath))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000EA238 File Offset: 0x000E9238
		private static bool VirtualDirectoryExistsWithAssert(VirtualPath virtualDir)
		{
			bool flag;
			try
			{
				string text = virtualDir.MapPathInternal();
				if (text != null)
				{
					new FileIOPermission(FileIOPermissionAccess.Read, text).Assert();
				}
				flag = virtualDir.DirectoryExists();
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000EA27C File Offset: 0x000E927C
		internal static void CheckAssignableType(Type baseType, Type type)
		{
			if (!baseType.IsAssignableFrom(type))
			{
				throw new HttpException(SR.GetString("Type_doesnt_inherit_from_type", new object[] { type.FullName, baseType.FullName }));
			}
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000EA2BC File Offset: 0x000E92BC
		internal static int LineCount(string text, int offset, int newoffset)
		{
			int num = 0;
			while (offset < newoffset)
			{
				if (text[offset] == '\r' || (text[offset] == '\n' && (offset == 0 || text[offset - 1] != '\r')))
				{
					num++;
				}
				offset++;
			}
			return num;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000EA304 File Offset: 0x000E9304
		internal static object InvokeMethod(MethodInfo methodInfo, object obj, object[] parameters)
		{
			object obj2;
			try
			{
				obj2 = methodInfo.Invoke(obj, parameters);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return obj2;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000EA338 File Offset: 0x000E9338
		internal static Type GetNonPrivateFieldType(Type classType, string fieldName)
		{
			FieldInfo field = classType.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null || field.IsPrivate)
			{
				return null;
			}
			return field.FieldType;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000EA364 File Offset: 0x000E9364
		internal static Type GetNonPrivatePropertyType(Type classType, string propName)
		{
			PropertyInfo propertyInfo = null;
			BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			try
			{
				propertyInfo = classType.GetProperty(propName, bindingFlags);
			}
			catch (AmbiguousMatchException)
			{
				bindingFlags |= BindingFlags.DeclaredOnly;
				propertyInfo = classType.GetProperty(propName, bindingFlags);
			}
			if (propertyInfo == null)
			{
				return null;
			}
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod == null || setMethod.IsPrivate)
			{
				return null;
			}
			return propertyInfo.PropertyType;
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000EA3C4 File Offset: 0x000E93C4
		private static string FirstDictionaryKey(IDictionary dict)
		{
			IDictionaryEnumerator enumerator = dict.GetEnumerator();
			enumerator.MoveNext();
			return (string)enumerator.Key;
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000EA3EC File Offset: 0x000E93EC
		private static string GetAndRemove(IDictionary dict, string key)
		{
			string text = (string)dict[key];
			if (text != null)
			{
				dict.Remove(key);
				text = text.Trim();
			}
			return text;
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000EA418 File Offset: 0x000E9418
		internal static string GetAndRemoveNonEmptyAttribute(IDictionary directives, string key, bool required)
		{
			string andRemove = Util.GetAndRemove(directives, key);
			if (andRemove != null)
			{
				return Util.GetNonEmptyAttribute(key, andRemove);
			}
			if (required)
			{
				throw new HttpException(SR.GetString("Missing_attr", new object[] { key }));
			}
			return null;
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000EA458 File Offset: 0x000E9458
		internal static string GetNonEmptyAttribute(string name, string value)
		{
			value = value.Trim();
			if (value.Length == 0)
			{
				throw new HttpException(SR.GetString("Empty_attribute", new object[] { name }));
			}
			return value;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000EA494 File Offset: 0x000E9494
		internal static string GetNoSpaceAttribute(string name, string value)
		{
			if (Util.ContainsWhiteSpace(value))
			{
				throw new HttpException(SR.GetString("Space_attribute", new object[] { name }));
			}
			return value;
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000EA4C6 File Offset: 0x000E94C6
		internal static string GetAndRemoveNonEmptyAttribute(IDictionary directives, string key)
		{
			return Util.GetAndRemoveNonEmptyAttribute(directives, key, false);
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000EA4D0 File Offset: 0x000E94D0
		internal static VirtualPath GetAndRemoveVirtualPathAttribute(IDictionary directives, string key)
		{
			return Util.GetAndRemoveVirtualPathAttribute(directives, key, false);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000EA4DC File Offset: 0x000E94DC
		internal static VirtualPath GetAndRemoveVirtualPathAttribute(IDictionary directives, string key, bool required)
		{
			string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directives, key, required);
			if (andRemoveNonEmptyAttribute == null)
			{
				return null;
			}
			return VirtualPath.Create(andRemoveNonEmptyAttribute);
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000EA500 File Offset: 0x000E9500
		public static string ParsePropertyDeviceFilter(string input, out string propName)
		{
			string text = string.Empty;
			if (input.IndexOf(':') < 0)
			{
				propName = input;
			}
			else if (StringUtil.StringStartsWithIgnoreCase(input, "xmlns:"))
			{
				propName = input;
			}
			else
			{
				string[] array = input.Split(new char[] { ':' });
				if (array.Length > 2)
				{
					throw new HttpException(SR.GetString("Too_many_filters", new object[] { input }));
				}
				text = array[0];
				propName = array[1];
			}
			return text;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000EA575 File Offset: 0x000E9575
		public static string CreateFilteredName(string deviceName, string name)
		{
			if (deviceName.Length > 0)
			{
				return deviceName + ':' + name;
			}
			return name;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000EA590 File Offset: 0x000E9590
		internal static string GetAndRemoveRequiredAttribute(IDictionary directives, string key)
		{
			return Util.GetAndRemoveNonEmptyAttribute(directives, key, true);
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000EA59C File Offset: 0x000E959C
		internal static string GetAndRemoveNonEmptyNoSpaceAttribute(IDictionary directives, string key, bool required)
		{
			string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directives, key, required);
			if (andRemoveNonEmptyAttribute == null)
			{
				return null;
			}
			return Util.GetNonEmptyNoSpaceAttribute(key, andRemoveNonEmptyAttribute);
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000EA5BE File Offset: 0x000E95BE
		internal static string GetAndRemoveNonEmptyNoSpaceAttribute(IDictionary directives, string key)
		{
			return Util.GetAndRemoveNonEmptyNoSpaceAttribute(directives, key, false);
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000EA5C8 File Offset: 0x000E95C8
		internal static string GetNonEmptyNoSpaceAttribute(string name, string value)
		{
			value = Util.GetNonEmptyAttribute(name, value);
			return Util.GetNoSpaceAttribute(name, value);
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000EA5DC File Offset: 0x000E95DC
		internal static string GetAndRemoveNonEmptyIdentifierAttribute(IDictionary directives, string key, bool required)
		{
			string andRemoveNonEmptyNoSpaceAttribute = Util.GetAndRemoveNonEmptyNoSpaceAttribute(directives, key, required);
			if (andRemoveNonEmptyNoSpaceAttribute == null)
			{
				return null;
			}
			return Util.GetNonEmptyIdentifierAttribute(key, andRemoveNonEmptyNoSpaceAttribute);
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000EA600 File Offset: 0x000E9600
		internal static string GetNonEmptyIdentifierAttribute(string name, string value)
		{
			value = Util.GetNonEmptyNoSpaceAttribute(name, value);
			if (!CodeGenerator.IsValidLanguageIndependentIdentifier(value))
			{
				throw new HttpException(SR.GetString("Invalid_attribute_value", new object[] { value, name }));
			}
			return value;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000EA640 File Offset: 0x000E9640
		internal static string GetNonEmptyFullClassNameAttribute(string name, string value, ref string ns)
		{
			value = Util.GetNonEmptyNoSpaceAttribute(name, value);
			string[] array = value.Split(new char[] { '.' });
			foreach (string text in array)
			{
				if (!CodeGenerator.IsValidLanguageIndependentIdentifier(text))
				{
					throw new HttpException(SR.GetString("Invalid_attribute_value", new object[] { value, name }));
				}
			}
			if (array.Length > 1)
			{
				ns = string.Join(".", array, 0, array.Length - 1);
			}
			return array[array.Length - 1];
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000EA6CF File Offset: 0x000E96CF
		internal static void CheckUnknownDirectiveAttributes(string directiveName, IDictionary directive)
		{
			Util.CheckUnknownDirectiveAttributes(directiveName, directive, "Attr_not_supported_in_directive");
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000EA6E0 File Offset: 0x000E96E0
		internal static void CheckUnknownDirectiveAttributes(string directiveName, IDictionary directive, string resourceKey)
		{
			if (directive.Count > 0)
			{
				throw new HttpException(SR.GetString(resourceKey, new object[]
				{
					Util.FirstDictionaryKey(directive),
					directiveName
				}));
			}
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000EA718 File Offset: 0x000E9718
		internal static bool GetAndRemoveBooleanAttribute(IDictionary directives, string key, ref bool val)
		{
			string andRemove = Util.GetAndRemove(directives, key);
			if (andRemove == null)
			{
				return false;
			}
			val = Util.GetBooleanAttribute(key, andRemove);
			return true;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000EA73C File Offset: 0x000E973C
		internal static bool GetBooleanAttribute(string name, string value)
		{
			bool flag;
			try
			{
				flag = bool.Parse(value);
			}
			catch
			{
				throw new HttpException(SR.GetString("Invalid_boolean_attribute", new object[] { name }));
			}
			return flag;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000EA780 File Offset: 0x000E9780
		internal static bool GetAndRemoveNonNegativeIntegerAttribute(IDictionary directives, string key, ref int val)
		{
			string andRemove = Util.GetAndRemove(directives, key);
			if (andRemove == null)
			{
				return false;
			}
			val = Util.GetNonNegativeIntegerAttribute(key, andRemove);
			return true;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000EA7A4 File Offset: 0x000E97A4
		internal static int GetNonNegativeIntegerAttribute(string name, string value)
		{
			int num;
			try
			{
				num = int.Parse(value, CultureInfo.InvariantCulture);
			}
			catch
			{
				throw new HttpException(SR.GetString("Invalid_nonnegative_integer_attribute", new object[] { name }));
			}
			if (num < 0)
			{
				throw new HttpException(SR.GetString("Invalid_nonnegative_integer_attribute", new object[] { name }));
			}
			return num;
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000EA810 File Offset: 0x000E9810
		internal static bool GetAndRemovePositiveIntegerAttribute(IDictionary directives, string key, ref int val)
		{
			string andRemove = Util.GetAndRemove(directives, key);
			if (andRemove == null)
			{
				return false;
			}
			try
			{
				val = int.Parse(andRemove, CultureInfo.InvariantCulture);
			}
			catch
			{
				throw new HttpException(SR.GetString("Invalid_positive_integer_attribute", new object[] { key }));
			}
			if (val <= 0)
			{
				throw new HttpException(SR.GetString("Invalid_positive_integer_attribute", new object[] { key }));
			}
			return true;
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000EA888 File Offset: 0x000E9888
		internal static object GetAndRemoveEnumAttribute(IDictionary directives, Type enumType, string key)
		{
			string andRemove = Util.GetAndRemove(directives, key);
			if (andRemove == null)
			{
				return null;
			}
			return Util.GetEnumAttribute(key, andRemove, enumType);
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000EA8AA File Offset: 0x000E98AA
		internal static object GetEnumAttribute(string name, string value, Type enumType)
		{
			return Util.GetEnumAttribute(name, value, enumType, false);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000EA8B8 File Offset: 0x000E98B8
		internal static object GetEnumAttribute(string name, string value, Type enumType, bool allowMultiple)
		{
			object obj;
			try
			{
				if (char.IsDigit(value[0]) || value[0] == '-' || (!allowMultiple && value.IndexOf(',') >= 0))
				{
					throw new FormatException(SR.GetString("EnumAttributeInvalidString", new object[] { value, name, enumType.FullName }));
				}
				obj = Enum.Parse(enumType, value, true);
			}
			catch
			{
				string text = null;
				foreach (string text2 in Enum.GetNames(enumType))
				{
					if (text == null)
					{
						text = text2;
					}
					else
					{
						text = text + ", " + text2;
					}
				}
				throw new HttpException(SR.GetString("Invalid_enum_attribute", new object[] { name, text }));
			}
			return obj;
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000EA990 File Offset: 0x000E9990
		internal static bool IsWhiteSpaceString(string s)
		{
			return s.Trim().Length == 0;
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000EA9A0 File Offset: 0x000E99A0
		internal static bool ContainsWhiteSpace(string s)
		{
			for (int i = s.Length - 1; i >= 0; i--)
			{
				if (char.IsWhiteSpace(s[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000EA9D4 File Offset: 0x000E99D4
		internal static int FirstNonWhiteSpaceIndex(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000EAA03 File Offset: 0x000E9A03
		internal static bool IsTrueString(string s)
		{
			return s != null && StringUtil.EqualsIgnoreCase(s, "true");
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000EAA15 File Offset: 0x000E9A15
		internal static bool IsFalseString(string s)
		{
			return s != null && StringUtil.EqualsIgnoreCase(s, "false");
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000EAA27 File Offset: 0x000E9A27
		internal static string GetStringFromBool(bool flag)
		{
			if (!flag)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000EAA37 File Offset: 0x000E9A37
		internal static string MakeFullTypeName(string ns, string typeName)
		{
			if (string.IsNullOrEmpty(ns))
			{
				return typeName;
			}
			return ns + "." + typeName;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000EAA50 File Offset: 0x000E9A50
		internal static string MakeValidTypeNameFromString(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				if (i == 0 && char.IsDigit(s[0]))
				{
					stringBuilder.Append('_');
				}
				if (char.IsLetterOrDigit(s[i]))
				{
					stringBuilder.Append(s[i]);
				}
				else
				{
					stringBuilder.Append('_');
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000EAABC File Offset: 0x000E9ABC
		internal static string GetNamespaceAndTypeNameFromVirtualPath(VirtualPath virtualPath, int chunksToIgnore, out string typeName)
		{
			string fileName = virtualPath.FileName;
			string[] array = fileName.Split(new char[] { '.' });
			int num = array.Length - chunksToIgnore;
			if (Util.IsWhiteSpaceString(array[num - 1]))
			{
				throw new HttpException(SR.GetString("Unsupported_filename", new object[] { fileName }));
			}
			typeName = Util.MakeValidTypeNameFromString(array[num - 1]);
			for (int i = 0; i < num - 1; i++)
			{
				if (Util.IsWhiteSpaceString(array[i]))
				{
					throw new HttpException(SR.GetString("Unsupported_filename", new object[] { fileName }));
				}
				array[i] = Util.MakeValidTypeNameFromString(array[i]);
			}
			return string.Join(".", array, 0, num - 1);
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000EAB78 File Offset: 0x000E9B78
		internal static string GetNamespaceFromVirtualPath(VirtualPath virtualPath)
		{
			string text;
			return Util.GetNamespaceAndTypeNameFromVirtualPath(virtualPath, 1, out text);
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000EAB90 File Offset: 0x000E9B90
		internal static string FilePathFromFileUrl(string url)
		{
			Uri uri = new Uri(url);
			string localPath = uri.LocalPath;
			return HttpUtility.UrlDecode(localPath);
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000EABB4 File Offset: 0x000E9BB4
		internal static bool IsCultureName(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return false;
			}
			foreach (string text in Util.s_invalidCultureNames)
			{
				if (StringUtil.EqualsIgnoreCase(text, s))
				{
					return false;
				}
			}
			CultureInfo cultureInfo = null;
			try
			{
				cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(s);
			}
			catch
			{
			}
			return cultureInfo != null;
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000EAC1C File Offset: 0x000E9C1C
		internal static string GetCultureName(string virtualPath)
		{
			if (virtualPath == null)
			{
				return null;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(virtualPath);
			if (fileNameWithoutExtension == null)
			{
				return null;
			}
			int num = fileNameWithoutExtension.LastIndexOf('.');
			if (num < 0)
			{
				return null;
			}
			string text = fileNameWithoutExtension.Substring(num + 1);
			if (!Util.IsCultureName(text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000EAC5E File Offset: 0x000E9C5E
		internal static bool TypeNameContainsAssembly(string typeName)
		{
			return Util.CommaIndexInTypeName(typeName) > 0;
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000EAC6C File Offset: 0x000E9C6C
		internal static int CommaIndexInTypeName(string typeName)
		{
			int num = typeName.LastIndexOf(',');
			if (num < 0)
			{
				return -1;
			}
			int num2 = typeName.LastIndexOf(']');
			if (num2 > num)
			{
				return -1;
			}
			return typeName.IndexOf(',', num2 + 1);
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000EACA4 File Offset: 0x000E9CA4
		internal static string GetAssemblyPathFromType(Type t)
		{
			return Util.FilePathFromFileUrl(t.Assembly.EscapedCodeBase);
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000EACB6 File Offset: 0x000E9CB6
		internal static string GetAssemblySafePathFromType(Type t)
		{
			return HttpRuntime.GetSafePath(Util.GetAssemblyPathFromType(t));
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000EACC3 File Offset: 0x000E9CC3
		internal static string GetAssemblyQualifiedTypeName(Type t)
		{
			if (t.Assembly.GlobalAssemblyCache)
			{
				return t.AssemblyQualifiedName;
			}
			return t.FullName + ", " + t.Assembly.GetName().Name;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000EACF9 File Offset: 0x000E9CF9
		internal static string GetAssemblyShortName(Assembly a)
		{
			InternalSecurityPermissions.Unrestricted.Assert();
			return a.GetName().Name;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000EAD10 File Offset: 0x000E9D10
		internal static bool IsLateBoundComClassicType(Type t)
		{
			return string.Compare(t.FullName, "System.__ComObject", StringComparison.Ordinal) == 0;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000EAD28 File Offset: 0x000E9D28
		internal static string GetAssemblyCodeBase(Assembly assembly)
		{
			string location = assembly.Location;
			if (string.IsNullOrEmpty(location))
			{
				return null;
			}
			return location;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000EAD48 File Offset: 0x000E9D48
		internal static void AddAssemblyToStringCollection(Assembly assembly, StringCollection toList)
		{
			string assemblyCodeBase = Util.GetAssemblyCodeBase(assembly);
			if (!toList.Contains(assemblyCodeBase))
			{
				toList.Add(assemblyCodeBase);
			}
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000EAD70 File Offset: 0x000E9D70
		internal static void AddAssembliesToStringCollection(ICollection fromList, StringCollection toList)
		{
			if (fromList == null || toList == null)
			{
				return;
			}
			foreach (object obj in fromList)
			{
				Assembly assembly = (Assembly)obj;
				Util.AddAssemblyToStringCollection(assembly, toList);
			}
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000EADCC File Offset: 0x000E9DCC
		internal static AssemblySet GetReferencedAssemblies(Assembly a)
		{
			AssemblySet assemblySet = new AssemblySet();
			AssemblyName[] referencedAssemblies = a.GetReferencedAssemblies();
			foreach (AssemblyName assemblyName in referencedAssemblies)
			{
				Assembly assembly = Assembly.Load(assemblyName);
				if (assembly != typeof(string).Assembly)
				{
					assemblySet.Add(assembly);
				}
			}
			return assemblySet;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000EAE23 File Offset: 0x000E9E23
		internal static string GetAssemblyNameFromFileName(string fileName)
		{
			if (StringUtil.EqualsIgnoreCase(Path.GetExtension(fileName), ".dll"))
			{
				return fileName.Substring(0, fileName.Length - 4);
			}
			return fileName;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000EAE48 File Offset: 0x000E9E48
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		internal static Type GetTypeFromAssemblies(ICollection assemblies, string typeName, bool ignoreCase)
		{
			if (assemblies == null)
			{
				return null;
			}
			Type type = null;
			foreach (object obj in assemblies)
			{
				Assembly assembly = (Assembly)obj;
				Type type2 = assembly.GetType(typeName, false, ignoreCase);
				if (type2 != null)
				{
					if (type != null && type2 != type)
					{
						throw new HttpException(SR.GetString("Ambiguous_type", new object[]
						{
							typeName,
							Util.GetAssemblySafePathFromType(type),
							Util.GetAssemblySafePathFromType(type2)
						}));
					}
					type = type2;
				}
			}
			return type;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000EAEEC File Offset: 0x000E9EEC
		internal static string GetCurrentAccountName()
		{
			string text;
			try
			{
				text = WindowsIdentity.GetCurrent().Name;
			}
			catch
			{
				text = "?";
			}
			return text;
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000EAF20 File Offset: 0x000E9F20
		internal static string QuoteJScriptString(string value)
		{
			return Util.QuoteJScriptString(value, false);
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000EAF2C File Offset: 0x000E9F2C
		internal static string QuoteJScriptString(string value, bool forUrl)
		{
			StringBuilder stringBuilder = null;
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			int num = 0;
			int num2 = 0;
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				if (c <= '"')
				{
					switch (c)
					{
					case '\t':
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 5);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("\\t");
						num = i + 1;
						num2 = 0;
						break;
					case '\n':
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 5);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("\\n");
						num = i + 1;
						num2 = 0;
						break;
					case '\v':
					case '\f':
						goto IL_01EE;
					case '\r':
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 5);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("\\r");
						num = i + 1;
						num2 = 0;
						break;
					default:
						if (c != '"')
						{
							goto IL_01EE;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 5);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("\\\"");
						num = i + 1;
						num2 = 0;
						break;
					}
				}
				else
				{
					switch (c)
					{
					case '%':
						if (!forUrl)
						{
							goto IL_01EE;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 6);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("%25");
						num = i + 1;
						num2 = 0;
						break;
					case '&':
						goto IL_01EE;
					case '\'':
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 5);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("\\'");
						num = i + 1;
						num2 = 0;
						break;
					default:
						if (c != '\\')
						{
							goto IL_01EE;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value.Length + 5);
						}
						if (num2 > 0)
						{
							stringBuilder.Append(value, num, num2);
						}
						stringBuilder.Append("\\\\");
						num = i + 1;
						num2 = 0;
						break;
					}
				}
				IL_01F2:
				i++;
				continue;
				IL_01EE:
				num2++;
				goto IL_01F2;
			}
			if (stringBuilder == null)
			{
				return value;
			}
			if (num2 > 0)
			{
				stringBuilder.Append(value, num, num2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000EB154 File Offset: 0x000EA154
		private static ArrayList GetSpecificCultures(string shortName)
		{
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < cultures.Length; i++)
			{
				if (StringUtil.StringStartsWith(cultures[i].Name, shortName))
				{
					arrayList.Add(cultures[i]);
				}
			}
			return arrayList;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000EB198 File Offset: 0x000EA198
		internal static string GetSpecificCulturesFormattedList(CultureInfo cultureInfo)
		{
			ArrayList specificCultures = Util.GetSpecificCultures(cultureInfo.Name);
			string text = null;
			foreach (object obj in specificCultures)
			{
				CultureInfo cultureInfo2 = (CultureInfo)obj;
				if (text == null)
				{
					text = cultureInfo2.Name;
				}
				else
				{
					text = text + ", " + cultureInfo2.Name;
				}
			}
			return text;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000EB218 File Offset: 0x000EA218
		internal static string GetClientValidateEvent(string validationGroup)
		{
			if (validationGroup == null)
			{
				validationGroup = string.Empty;
			}
			return "if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate('" + validationGroup + "'); ";
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000EB234 File Offset: 0x000EA234
		internal static string GetClientValidatedPostback(Control control, string validationGroup)
		{
			return Util.GetClientValidatedPostback(control, validationGroup, string.Empty);
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000EB244 File Offset: 0x000EA244
		internal static string GetClientValidatedPostback(Control control, string validationGroup, string argument)
		{
			string postBackEventReference = control.Page.ClientScript.GetPostBackEventReference(control, argument, true);
			return Util.GetClientValidateEvent(validationGroup) + postBackEventReference;
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000EB274 File Offset: 0x000EA274
		internal static void WriteOnClickAttribute(HtmlTextWriter writer, HtmlControl control, bool submitsAutomatically, bool submitsProgramatically, bool causesValidation, string validationGroup)
		{
			AttributeCollection attributes = control.Attributes;
			string text = null;
			if (submitsAutomatically)
			{
				if (causesValidation)
				{
					text = Util.GetClientValidateEvent(validationGroup);
				}
				control.Page.ClientScript.RegisterForEventValidation(control.UniqueID);
			}
			else if (submitsProgramatically)
			{
				if (causesValidation)
				{
					text = Util.GetClientValidatedPostback(control, validationGroup);
				}
				else
				{
					text = control.Page.ClientScript.GetPostBackEventReference(control, string.Empty, true);
				}
			}
			else
			{
				control.Page.ClientScript.RegisterForEventValidation(control.UniqueID);
			}
			if (text != null)
			{
				string text2 = attributes["onclick"];
				if (text2 != null)
				{
					attributes.Remove("onclick");
					writer.WriteAttribute("onclick", text2 + " " + text);
					return;
				}
				writer.WriteAttribute("onclick", text);
			}
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000EB334 File Offset: 0x000EA334
		internal static string EnsureEndWithSemiColon(string value)
		{
			if (value != null)
			{
				int length = value.Length;
				if (length > 0 && value[length - 1] != ';')
				{
					return value + ";";
				}
			}
			return value;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000EB369 File Offset: 0x000EA369
		internal static string MergeScript(string firstScript, string secondScript)
		{
			if (!string.IsNullOrEmpty(firstScript))
			{
				return firstScript + secondScript;
			}
			if (secondScript.TrimStart(new char[0]).StartsWith("javascript:", StringComparison.Ordinal))
			{
				return secondScript;
			}
			return "javascript:" + secondScript;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000EB3A1 File Offset: 0x000EA3A1
		internal static bool IsUserAllowedToPath(HttpContext context, VirtualPath virtualPath)
		{
			if (FileAuthorizationModule.IsWindowsIdentity(context))
			{
				if (HttpRuntime.IsFullTrust)
				{
					if (!Util.IsUserAllowedToPathWithNoAssert(context, virtualPath))
					{
						return false;
					}
				}
				else if (!Util.IsUserAllowedToPathWithAssert(context, virtualPath))
				{
					return false;
				}
			}
			return UrlAuthorizationModule.IsUserAllowedToPath(context, virtualPath);
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000EB3CF File Offset: 0x000EA3CF
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool IsUserAllowedToPathWithAssert(HttpContext context, VirtualPath virtualPath)
		{
			return Util.IsUserAllowedToPathWithNoAssert(context, virtualPath);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000EB3D8 File Offset: 0x000EA3D8
		private static bool IsUserAllowedToPathWithNoAssert(HttpContext context, VirtualPath virtualPath)
		{
			return FileAuthorizationModule.IsUserAllowedToPath(context, virtualPath);
		}

		// Token: 0x0400257E RID: 9598
		internal const char DeviceFilterSeparator = ':';

		// Token: 0x0400257F RID: 9599
		internal const string XmlnsAttribute = "xmlns:";

		// Token: 0x04002580 RID: 9600
		private static string[] s_invalidCultureNames = new string[] { "aspx", "ascx", "master" };

		// Token: 0x04002581 RID: 9601
		private static char[] invalidFileNameChars = new char[] { '/', '\\', '?', '*', ':' };
	}
}
