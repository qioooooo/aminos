using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Design;
using System.Web.Compilation;
using System.Web.Configuration;

namespace System.Web.UI.Design
{
	// Token: 0x02000319 RID: 793
	public abstract class ExpressionEditor
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x000AB307 File Offset: 0x000AA307
		public string ExpressionPrefix
		{
			get
			{
				return this._expressionPrefix;
			}
		}

		// Token: 0x06001DFB RID: 7675
		public abstract object EvaluateExpression(string expression, object parseTimeData, Type propertyType, IServiceProvider serviceProvider);

		// Token: 0x06001DFC RID: 7676 RVA: 0x000AB310 File Offset: 0x000AA310
		private static IDictionary GetExpressionEditorsCache(IWebApplication webApp)
		{
			IDictionaryService dictionaryService = (IDictionaryService)webApp.GetService(typeof(IDictionaryService));
			if (dictionaryService == null)
			{
				return null;
			}
			IDictionary dictionary = (IDictionary)dictionaryService.GetValue("ExpressionEditors");
			if (dictionary == null)
			{
				dictionary = new HybridDictionary(true);
				dictionaryService.SetValue("ExpressionEditors", dictionary);
			}
			return dictionary;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000AB360 File Offset: 0x000AA360
		private static IDictionary GetExpressionEditorsByTypeCache(IWebApplication webApp)
		{
			IDictionaryService dictionaryService = (IDictionaryService)webApp.GetService(typeof(IDictionaryService));
			if (dictionaryService == null)
			{
				return null;
			}
			IDictionary dictionary = (IDictionary)dictionaryService.GetValue("ExpressionEditorsByType");
			if (dictionary == null)
			{
				dictionary = new HybridDictionary();
				dictionaryService.SetValue("ExpressionEditorsByType", dictionary);
			}
			return dictionary;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000AB3B0 File Offset: 0x000AA3B0
		public static ExpressionEditor GetExpressionEditor(Type expressionBuilderType, IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			if (expressionBuilderType == null)
			{
				throw new ArgumentNullException("expressionBuilderType");
			}
			ExpressionEditor expressionEditor = null;
			IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
			if (webApplication != null)
			{
				IDictionary expressionEditorsByTypeCache = ExpressionEditor.GetExpressionEditorsByTypeCache(webApplication);
				if (expressionEditorsByTypeCache != null)
				{
					expressionEditor = (ExpressionEditor)expressionEditorsByTypeCache[expressionBuilderType];
				}
				if (expressionEditor == null)
				{
					Configuration configuration = webApplication.OpenWebConfiguration(true);
					if (configuration != null)
					{
						CompilationSection compilationSection = (CompilationSection)configuration.GetSection("system.web/compilation");
						ExpressionBuilderCollection expressionBuilderCollection = compilationSection.ExpressionBuilders;
						bool flag = false;
						string fullName = expressionBuilderType.FullName;
						foreach (object obj in expressionBuilderCollection)
						{
							global::System.Web.Configuration.ExpressionBuilder expressionBuilder = (global::System.Web.Configuration.ExpressionBuilder)obj;
							if (string.Equals(expressionBuilder.Type, fullName, StringComparison.OrdinalIgnoreCase))
							{
								expressionEditor = ExpressionEditor.GetExpressionEditorInternal(expressionBuilderType, expressionBuilder.ExpressionPrefix, webApplication, serviceProvider);
								flag = true;
							}
						}
						if (!flag)
						{
							object[] customAttributes = expressionBuilderType.GetCustomAttributes(typeof(ExpressionPrefixAttribute), true);
							ExpressionPrefixAttribute expressionPrefixAttribute = null;
							if (customAttributes.Length > 0)
							{
								expressionPrefixAttribute = (ExpressionPrefixAttribute)customAttributes[0];
							}
							if (expressionPrefixAttribute != null)
							{
								global::System.Web.Configuration.ExpressionBuilder expressionBuilder2 = new global::System.Web.Configuration.ExpressionBuilder(expressionPrefixAttribute.ExpressionPrefix, expressionBuilderType.FullName);
								configuration = webApplication.OpenWebConfiguration(false);
								compilationSection = (CompilationSection)configuration.GetSection("system.web/compilation");
								expressionBuilderCollection = compilationSection.ExpressionBuilders;
								expressionBuilderCollection.Add(expressionBuilder2);
								configuration.Save();
								expressionEditor = ExpressionEditor.GetExpressionEditorInternal(expressionBuilderType, expressionBuilder2.ExpressionPrefix, webApplication, serviceProvider);
							}
						}
					}
				}
			}
			return expressionEditor;
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000AB540 File Offset: 0x000AA540
		internal static ExpressionEditor GetExpressionEditorInternal(Type expressionBuilderType, string expressionPrefix, IWebApplication webApp, IServiceProvider serviceProvider)
		{
			if (expressionBuilderType == null)
			{
				throw new ArgumentNullException("expressionBuilderType");
			}
			ExpressionEditor expressionEditor = null;
			object[] customAttributes = expressionBuilderType.GetCustomAttributes(typeof(ExpressionEditorAttribute), true);
			ExpressionEditorAttribute expressionEditorAttribute = null;
			if (customAttributes.Length > 0)
			{
				expressionEditorAttribute = (ExpressionEditorAttribute)customAttributes[0];
			}
			if (expressionEditorAttribute != null)
			{
				string editorTypeName = expressionEditorAttribute.EditorTypeName;
				Type type = Type.GetType(editorTypeName);
				if (type == null)
				{
					ITypeResolutionService typeResolutionService = (ITypeResolutionService)serviceProvider.GetService(typeof(ITypeResolutionService));
					if (typeResolutionService != null)
					{
						type = typeResolutionService.GetType(editorTypeName);
					}
				}
				if (type != null && typeof(ExpressionEditor).IsAssignableFrom(type))
				{
					expressionEditor = (ExpressionEditor)Activator.CreateInstance(type);
					expressionEditor.SetExpressionPrefix(expressionPrefix);
				}
				IDictionary expressionEditorsCache = ExpressionEditor.GetExpressionEditorsCache(webApp);
				if (expressionEditorsCache != null)
				{
					expressionEditorsCache[expressionPrefix] = expressionEditor;
				}
				IDictionary expressionEditorsByTypeCache = ExpressionEditor.GetExpressionEditorsByTypeCache(webApp);
				if (expressionEditorsByTypeCache != null)
				{
					expressionEditorsByTypeCache[expressionBuilderType] = expressionEditor;
				}
			}
			return expressionEditor;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000AB614 File Offset: 0x000AA614
		public static ExpressionEditor GetExpressionEditor(string expressionPrefix, IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			if (expressionPrefix.Length == 0)
			{
				return null;
			}
			ExpressionEditor expressionEditor = null;
			IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
			if (webApplication != null)
			{
				IDictionary expressionEditorsCache = ExpressionEditor.GetExpressionEditorsCache(webApplication);
				if (expressionEditorsCache != null)
				{
					expressionEditor = (ExpressionEditor)expressionEditorsCache[expressionPrefix];
				}
				if (expressionEditor == null)
				{
					string text;
					Type expressionBuilderType = ExpressionEditor.GetExpressionBuilderType(expressionPrefix, serviceProvider, out text);
					if (expressionBuilderType != null)
					{
						expressionEditor = ExpressionEditor.GetExpressionEditorInternal(expressionBuilderType, text, webApplication, serviceProvider);
					}
				}
			}
			return expressionEditor;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000AB68C File Offset: 0x000AA68C
		internal static Type GetExpressionBuilderType(string expressionPrefix, IServiceProvider serviceProvider, out string trueExpressionPrefix)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			trueExpressionPrefix = expressionPrefix;
			if (expressionPrefix.Length == 0)
			{
				return null;
			}
			Type type = null;
			IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
			if (webApplication != null)
			{
				Configuration configuration = webApplication.OpenWebConfiguration(true);
				if (configuration != null)
				{
					CompilationSection compilationSection = (CompilationSection)configuration.GetSection("system.web/compilation");
					ExpressionBuilderCollection expressionBuilders = compilationSection.ExpressionBuilders;
					foreach (object obj in expressionBuilders)
					{
						global::System.Web.Configuration.ExpressionBuilder expressionBuilder = (global::System.Web.Configuration.ExpressionBuilder)obj;
						if (string.Equals(expressionPrefix, expressionBuilder.ExpressionPrefix, StringComparison.OrdinalIgnoreCase))
						{
							trueExpressionPrefix = expressionBuilder.ExpressionPrefix;
							type = Type.GetType(expressionBuilder.Type);
							if (type == null)
							{
								ITypeResolutionService typeResolutionService = (ITypeResolutionService)serviceProvider.GetService(typeof(ITypeResolutionService));
								if (typeResolutionService != null)
								{
									type = typeResolutionService.GetType(expressionBuilder.Type);
								}
							}
						}
					}
				}
			}
			return type;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000AB798 File Offset: 0x000AA798
		public virtual ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new ExpressionEditor.GenericExpressionEditorSheet(expression, serviceProvider);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000AB7A1 File Offset: 0x000AA7A1
		internal void SetExpressionPrefix(string expressionPrefix)
		{
			this._expressionPrefix = expressionPrefix;
		}

		// Token: 0x04001722 RID: 5922
		private const string expressionEditorsByTypeKey = "ExpressionEditorsByType";

		// Token: 0x04001723 RID: 5923
		private const string expressionEditorsKey = "ExpressionEditors";

		// Token: 0x04001724 RID: 5924
		private string _expressionPrefix;

		// Token: 0x0200031B RID: 795
		private class GenericExpressionEditorSheet : ExpressionEditorSheet
		{
			// Token: 0x06001E09 RID: 7689 RVA: 0x000AB7CC File Offset: 0x000AA7CC
			public GenericExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this._expression = expression;
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x06001E0A RID: 7690 RVA: 0x000AB7DC File Offset: 0x000AA7DC
			// (set) Token: 0x06001E0B RID: 7691 RVA: 0x000AB7F2 File Offset: 0x000AA7F2
			[SRDescription("ExpressionEditor_Expression")]
			[DefaultValue("")]
			public string Expression
			{
				get
				{
					if (this._expression == null)
					{
						return string.Empty;
					}
					return this._expression;
				}
				set
				{
					this._expression = value;
				}
			}

			// Token: 0x06001E0C RID: 7692 RVA: 0x000AB7FB File Offset: 0x000AA7FB
			public override string GetExpression()
			{
				return this._expression;
			}

			// Token: 0x04001726 RID: 5926
			private string _expression;
		}
	}
}
