using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000157 RID: 343
	internal class ApplicationContext
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x00020902 File Offset: 0x0001F902
		internal ApplicationContext(IActContext a)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = a;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0002091A File Offset: 0x0001F91A
		public ApplicationContext(DefinitionAppId appid)
		{
			if (appid == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = IsolationInterop.CreateActContext(appid._id);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002093C File Offset: 0x0001F93C
		public ApplicationContext(ReferenceAppId appid)
		{
			if (appid == null)
			{
				throw new ArgumentNullException();
			}
			this._appcontext = IsolationInterop.CreateActContext(appid._id);
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00020960 File Offset: 0x0001F960
		public DefinitionAppId Identity
		{
			get
			{
				object obj;
				this._appcontext.GetAppId(out obj);
				return new DefinitionAppId(obj as IDefinitionAppId);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00020988 File Offset: 0x0001F988
		public string BasePath
		{
			get
			{
				string text;
				this._appcontext.ApplicationBasePath(0U, out text);
				return text;
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x000209A4 File Offset: 0x0001F9A4
		public string ReplaceStrings(string culture, string toreplace)
		{
			string text;
			this._appcontext.ReplaceStringMacros(0U, culture, toreplace, out text);
			return text;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000209C4 File Offset: 0x0001F9C4
		internal ICMS GetComponentManifest(DefinitionIdentity component)
		{
			object obj;
			this._appcontext.GetComponentManifest(0U, component._id, ref IsolationInterop.IID_ICMS, out obj);
			return obj as ICMS;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000209F0 File Offset: 0x0001F9F0
		internal string GetComponentManifestPath(DefinitionIdentity component)
		{
			object obj;
			this._appcontext.GetComponentManifest(0U, component._id, ref IsolationInterop.IID_IManifestInformation, out obj);
			string text;
			((IManifestInformation)obj).get_FullPath(out text);
			return text;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00020A24 File Offset: 0x0001FA24
		public string GetComponentPath(DefinitionIdentity component)
		{
			string text;
			this._appcontext.GetComponentPayloadPath(0U, component._id, out text);
			return text;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00020A48 File Offset: 0x0001FA48
		public DefinitionIdentity MatchReference(ReferenceIdentity TheRef)
		{
			object obj;
			this._appcontext.FindReferenceInContext(0U, TheRef._id, out obj);
			return new DefinitionIdentity(obj as IDefinitionIdentity);
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00020A74 File Offset: 0x0001FA74
		public EnumDefinitionIdentity Components
		{
			get
			{
				object obj;
				this._appcontext.EnumComponents(0U, out obj);
				return new EnumDefinitionIdentity(obj as IEnumDefinitionIdentity);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00020A9A File Offset: 0x0001FA9A
		public void PrepareForExecution()
		{
			this._appcontext.PrepareForExecution(IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00020AB4 File Offset: 0x0001FAB4
		public ApplicationContext.ApplicationStateDisposition SetApplicationState(ApplicationContext.ApplicationState s)
		{
			uint num;
			this._appcontext.SetApplicationRunningState(0U, (uint)s, out num);
			return (ApplicationContext.ApplicationStateDisposition)num;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x00020AD4 File Offset: 0x0001FAD4
		public string StateLocation
		{
			get
			{
				string text;
				this._appcontext.GetApplicationStateFilesystemLocation(0U, UIntPtr.Zero, IntPtr.Zero, out text);
				return text;
			}
		}

		// Token: 0x040005ED RID: 1517
		private IActContext _appcontext;

		// Token: 0x02000158 RID: 344
		public enum ApplicationState
		{
			// Token: 0x040005EF RID: 1519
			Undefined,
			// Token: 0x040005F0 RID: 1520
			Starting,
			// Token: 0x040005F1 RID: 1521
			Running
		}

		// Token: 0x02000159 RID: 345
		public enum ApplicationStateDisposition
		{
			// Token: 0x040005F3 RID: 1523
			Undefined,
			// Token: 0x040005F4 RID: 1524
			Starting,
			// Token: 0x040005F5 RID: 1525
			Starting_Migrated = 65537,
			// Token: 0x040005F6 RID: 1526
			Running = 2,
			// Token: 0x040005F7 RID: 1527
			Running_FirstTime = 131074
		}
	}
}
