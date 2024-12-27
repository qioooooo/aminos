using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000738 RID: 1848
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartManagerInternals
	{
		// Token: 0x060059AB RID: 22955 RVA: 0x0016A4E8 File Offset: 0x001694E8
		internal WebPartManagerInternals(WebPartManager manager)
		{
			this._manager = manager;
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x0016A4F7 File Offset: 0x001694F7
		public void AddWebPart(WebPart webPart)
		{
			this._manager.AddWebPart(webPart);
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x0016A505 File Offset: 0x00169505
		public void CallOnClosing(WebPart webPart)
		{
			webPart.OnClosing(EventArgs.Empty);
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x0016A512 File Offset: 0x00169512
		public void CallOnConnectModeChanged(WebPart webPart)
		{
			webPart.OnConnectModeChanged(EventArgs.Empty);
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x0016A51F File Offset: 0x0016951F
		public void CallOnDeleting(WebPart webPart)
		{
			webPart.OnDeleting(EventArgs.Empty);
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x0016A52C File Offset: 0x0016952C
		public void CallOnEditModeChanged(WebPart webPart)
		{
			webPart.OnEditModeChanged(EventArgs.Empty);
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x0016A539 File Offset: 0x00169539
		public object CreateObjectFromType(Type type)
		{
			return WebPartUtil.CreateObjectFromType(type);
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x0016A541 File Offset: 0x00169541
		public bool ConnectionDeleted(WebPartConnection connection)
		{
			return connection.Deleted;
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x0016A549 File Offset: 0x00169549
		public void DeleteConnection(WebPartConnection connection)
		{
			connection.Deleted = true;
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x0016A552 File Offset: 0x00169552
		public string GetZoneID(WebPart webPart)
		{
			return webPart.ZoneID;
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x0016A55A File Offset: 0x0016955A
		public void LoadConfigurationState(WebPartTransformer transformer, object savedState)
		{
			transformer.LoadConfigurationState(savedState);
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x0016A563 File Offset: 0x00169563
		public void RemoveWebPart(WebPart webPart)
		{
			this._manager.RemoveWebPart(webPart);
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x0016A571 File Offset: 0x00169571
		public object SaveConfigurationState(WebPartTransformer transformer)
		{
			return transformer.SaveConfigurationState();
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x0016A579 File Offset: 0x00169579
		public void SetConnectErrorMessage(WebPart webPart, string connectErrorMessage)
		{
			webPart.SetConnectErrorMessage(connectErrorMessage);
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x0016A582 File Offset: 0x00169582
		public void SetHasUserData(WebPart webPart, bool hasUserData)
		{
			webPart.SetHasUserData(hasUserData);
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x0016A58B File Offset: 0x0016958B
		public void SetHasSharedData(WebPart webPart, bool hasSharedData)
		{
			webPart.SetHasSharedData(hasSharedData);
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x0016A594 File Offset: 0x00169594
		public void SetIsClosed(WebPart webPart, bool isClosed)
		{
			webPart.SetIsClosed(isClosed);
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x0016A59D File Offset: 0x0016959D
		public void SetIsShared(WebPartConnection connection, bool isShared)
		{
			connection.SetIsShared(isShared);
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x0016A5A6 File Offset: 0x001695A6
		public void SetIsShared(WebPart webPart, bool isShared)
		{
			webPart.SetIsShared(isShared);
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x0016A5AF File Offset: 0x001695AF
		public void SetIsStandalone(WebPart webPart, bool isStandalone)
		{
			webPart.SetIsStandalone(isStandalone);
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x0016A5B8 File Offset: 0x001695B8
		public void SetIsStatic(WebPartConnection connection, bool isStatic)
		{
			connection.SetIsStatic(isStatic);
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x0016A5C1 File Offset: 0x001695C1
		public void SetIsStatic(WebPart webPart, bool isStatic)
		{
			webPart.SetIsStatic(isStatic);
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x0016A5CA File Offset: 0x001695CA
		public void SetTransformer(WebPartConnection connection, WebPartTransformer transformer)
		{
			connection.SetTransformer(transformer);
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x0016A5D3 File Offset: 0x001695D3
		public void SetZoneID(WebPart webPart, string zoneID)
		{
			webPart.ZoneID = zoneID;
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x0016A5DC File Offset: 0x001695DC
		public void SetZoneIndex(WebPart webPart, int zoneIndex)
		{
			webPart.SetZoneIndex(zoneIndex);
		}

		// Token: 0x04003062 RID: 12386
		private WebPartManager _manager;
	}
}
