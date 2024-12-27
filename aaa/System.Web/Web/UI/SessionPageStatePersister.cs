using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.SessionState;

namespace System.Web.UI
{
	// Token: 0x0200045D RID: 1117
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SessionPageStatePersister : PageStatePersister
	{
		// Token: 0x060034EF RID: 13551 RVA: 0x000E51A0 File Offset: 0x000E41A0
		public SessionPageStatePersister(Page page)
			: base(page)
		{
			HttpSessionState httpSessionState = null;
			try
			{
				httpSessionState = page.Session;
			}
			catch
			{
			}
			if (httpSessionState == null)
			{
				throw new ArgumentException(SR.GetString("SessionPageStatePersister_SessionMustBeEnabled"));
			}
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000E51E8 File Offset: 0x000E41E8
		public override void Load()
		{
			if (base.Page.RequestValueCollection == null)
			{
				return;
			}
			try
			{
				string requestViewStateString = base.Page.RequestViewStateString;
				string text = null;
				bool flag = false;
				if (!string.IsNullOrEmpty(requestViewStateString))
				{
					Pair pair = (Pair)Util.DeserializeWithAssert(base.StateFormatter, requestViewStateString);
					if ((bool)pair.First)
					{
						text = (string)pair.Second;
						flag = true;
					}
					else
					{
						Pair pair2 = (Pair)pair.Second;
						text = (string)pair2.First;
						base.ControlState = pair2.Second;
					}
				}
				if (text != null)
				{
					object obj = base.Page.Session["__SESSIONVIEWSTATE" + text];
					if (flag)
					{
						Pair pair3 = obj as Pair;
						if (pair3 != null)
						{
							base.ViewState = pair3.First;
							base.ControlState = pair3.Second;
						}
					}
					else
					{
						base.ViewState = obj;
					}
				}
			}
			catch (Exception ex)
			{
				HttpException ex2 = new HttpException(SR.GetString("Invalid_ControlState"), ex);
				ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
				throw ex2;
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000E5304 File Offset: 0x000E4304
		public override void Save()
		{
			bool flag = false;
			object obj = null;
			Triplet triplet = base.ViewState as Triplet;
			if (base.ControlState != null || ((triplet == null || triplet.Second != null || triplet.Third != null) && base.ViewState != null))
			{
				HttpSessionState session = base.Page.Session;
				string text = Convert.ToString(DateTime.Now.Ticks, 16);
				flag = base.Page.Request.Browser.RequiresControlStateInSession;
				object obj2;
				if (flag)
				{
					obj2 = new Pair(base.ViewState, base.ControlState);
					obj = text;
				}
				else
				{
					obj2 = base.ViewState;
					obj = new Pair(text, base.ControlState);
				}
				string text2 = "__SESSIONVIEWSTATE" + text;
				session[text2] = obj2;
				Queue queue = session["__VIEWSTATEQUEUE"] as Queue;
				if (queue == null)
				{
					queue = new Queue();
					session["__VIEWSTATEQUEUE"] = queue;
				}
				queue.Enqueue(text2);
				SessionPageStateSection sessionPageState = RuntimeConfig.GetConfig(base.Page.Request.Context).SessionPageState;
				int count = queue.Count;
				if ((sessionPageState != null && count > sessionPageState.HistorySize) || (sessionPageState == null && count > 9))
				{
					string text3 = (string)queue.Dequeue();
					session.Remove(text3);
				}
			}
			if (obj != null)
			{
				base.Page.ClientState = Util.SerializeWithAssert(base.StateFormatter, new Pair(flag, obj));
			}
		}

		// Token: 0x0400250A RID: 9482
		private const string _viewStateSessionKey = "__SESSIONVIEWSTATE";

		// Token: 0x0400250B RID: 9483
		private const string _viewStateQueueKey = "__VIEWSTATEQUEUE";
	}
}
