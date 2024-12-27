using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200012E RID: 302
	internal class ApplicationContext
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x000096DE File Offset: 0x000086DE
		internal ApplicationContext(IActContext a)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = a;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000096F6 File Offset: 0x000086F6
		public ApplicationContext(DefinitionAppId appid)
		{
			if (appid == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = IsolationInterop.CreateActContext(appid._id);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00009718 File Offset: 0x00008718
		public ApplicationContext(ReferenceAppId appid)
		{
			if (appid == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = IsolationInterop.CreateActContext(appid._id);
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000973C File Offset: 0x0000873C
		public DefinitionAppId Identity
		{
			get
			{
				object obj;
				this._appcontext.GetAppId(out obj);
				return new DefinitionAppId(obj as IDefinitionAppId);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00009764 File Offset: 0x00008764
		public string BasePath
		{
			get
			{
				string text;
				this._appcontext.ApplicationBasePath(0U, out text);
				return text;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00009780 File Offset: 0x00008780
		public string ReplaceStrings(string culture, string toreplace)
		{
			string text;
			this._appcontext.ReplaceStringMacros(0U, culture, toreplace, out text);
			return text;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000097A0 File Offset: 0x000087A0
		internal ICMS GetComponentManifest(DefinitionIdentity component)
		{
			object obj;
			this._appcontext.GetComponentManifest(0U, component._id, ref IsolationInterop.IID_ICMS, out obj);
			return obj as ICMS;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000097CC File Offset: 0x000087CC
		internal string GetComponentManifestPath(DefinitionIdentity component)
		{
			object obj;
			this._appcontext.GetComponentManifest(0U, component._id, ref IsolationInterop.IID_IManifestInformation, out obj);
			string text;
			((IManifestInformation)obj).get_FullPath(out text);
			return text;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00009800 File Offset: 0x00008800
		public string GetComponentPath(DefinitionIdentity component)
		{
			string text;
			this._appcontext.GetComponentPayloadPath(0U, component._id, out text);
			return text;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00009824 File Offset: 0x00008824
		public DefinitionIdentity MatchReference(ReferenceIdentity TheRef)
		{
			object obj;
			this._appcontext.FindReferenceInContext(0U, TheRef._id, out obj);
			return new DefinitionIdentity(obj as IDefinitionIdentity);
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00009850 File Offset: 0x00008850
		public EnumDefinitionIdentity Components
		{
			get
			{
				object obj;
				this._appcontext.EnumComponents(0U, out obj);
				return new EnumDefinitionIdentity(obj as IEnumDefinitionIdentity);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00009876 File Offset: 0x00008876
		public void PrepareForExecution()
		{
			this._appcontext.PrepareForExecution(IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00009890 File Offset: 0x00008890
		public ApplicationContext.ApplicationStateDisposition SetApplicationState(ApplicationContext.ApplicationState s)
		{
			uint num;
			this._appcontext.SetApplicationRunningState(0U, (uint)s, out num);
			return (ApplicationContext.ApplicationStateDisposition)num;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x000098B0 File Offset: 0x000088B0
		public string StateLocation
		{
			get
			{
				string text;
				this._appcontext.GetApplicationStateFilesystemLocation(0U, UIntPtr.Zero, IntPtr.Zero, out text);
				return text;
			}
		}

		// Token: 0x04000E7B RID: 3707
		private IActContext _appcontext;

		// Token: 0x0200012F RID: 303
		public enum ApplicationState
		{
			// Token: 0x04000E7D RID: 3709
			Undefined,
			// Token: 0x04000E7E RID: 3710
			Starting,
			// Token: 0x04000E7F RID: 3711
			Running
		}

		// Token: 0x02000130 RID: 304
		public enum ApplicationStateDisposition
		{
			// Token: 0x04000E81 RID: 3713
			Undefined,
			// Token: 0x04000E82 RID: 3714
			Starting,
			// Token: 0x04000E83 RID: 3715
			Starting_Migrated = 65537,
			// Token: 0x04000E84 RID: 3716
			Running = 2,
			// Token: 0x04000E85 RID: 3717
			Running_FirstTime = 131074
		}
	}
}
