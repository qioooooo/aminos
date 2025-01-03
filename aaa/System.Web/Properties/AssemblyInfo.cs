﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Web.UI;

[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyFileVersion("2.0.50727.9175")]
[assembly: WebResource("TreeView_Windows_Help_Collapse.gif", "image/gif")]
[assembly: AssemblyInformationalVersion("2.0.50727.9175")]
[assembly: AssemblyTitle("System.Web.dll")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: WebResource("TreeView_XP_Explorer_RootNode.gif", "image/gif")]
[assembly: AssemblyDefaultAlias("System.Web.dll")]
[assembly: AssemblyDescription("System.Web.dll")]
[assembly: WebResource("TreeView_XP_Explorer_Collapse.gif", "image/gif")]
[assembly: SatelliteContractVersion("2.0.0.0")]
[assembly: WebResource("WebPartMenu_Check.gif", "image/gif")]
[assembly: WebResource("TreeView_Simple_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Simple2_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_News_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_News_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_News_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Inbox_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Inbox_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Inbox_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_FAQ_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_FAQ_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_FAQ_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Events_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Events_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Events_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Contacts_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Contacts_Expand.gif", "image/gif")]
[assembly: WebResource("TreeView_Contacts_Collapse.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList4_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList4_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList4_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList3_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList3_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList3_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList2_RootNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList2_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_BulletedList2_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_Arrows_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Arrows_Expand.gif", "image/gif")]
[assembly: WebResource("TreeView_Arrows_Collapse.gif", "image/gif")]
[assembly: WebResource("TreeView_Windows_Help_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Windows_Help_Expand.gif", "image/gif")]
[assembly: WebResource("TreeView_MSDN_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_MSDN_Collapse.gif", "image/gif")]
[assembly: WebResource("TreeView_MSDN_Expand.gif", "image/gif")]
[assembly: WebResource("TreeView_XP_Explorer_LeafNode.gif", "image/gif")]
[assembly: WebResource("TreeView_XP_Explorer_ParentNode.gif", "image/gif")]
[assembly: WebResource("TreeView_XP_Explorer_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_XP_Explorer_Expand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_NoExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_Collapse.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_Expand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_TExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_TCollapse.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_T.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_RExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_RCollapse.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_R.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_LExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_LCollapse.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_L.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_I.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_DashExpand.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_DashCollapse.gif", "image/gif")]
[assembly: WebResource("TreeView_Default_Dash.gif", "image/gif")]
[assembly: WebResource("Menu_ScrollUp.gif", "image/gif")]
[assembly: WebResource("Menu_ScrollDown.gif", "image/gif")]
[assembly: WebResource("Menu_Popout.gif", "image/gif")]
[assembly: WebResource("Menu_Default_Separator.gif", "image/gif")]
[assembly: WebResource("Spacer.gif", "image/gif")]
[assembly: WebResource("DetailsView.js", "application/x-javascript")]
[assembly: WebResource("GridView.js", "application/x-javascript")]
[assembly: WebResource("WebParts.js", "application/x-javascript")]
[assembly: WebResource("Menu.js", "application/x-javascript")]
[assembly: WebResource("TreeView.js", "application/x-javascript")]
[assembly: WebResource("WebUIValidation.js", "application/x-javascript")]
[assembly: WebResource("SmartNav.js", "application/x-javascript")]
[assembly: WebResource("SmartNav.htm", "text/html")]
[assembly: WebResource("Focus.js", "application/x-javascript")]
[assembly: WebResource("WebForms.js", "application/x-javascript")]
[assembly: TagPrefix("System.Web.UI.WebControls", "asp")]
[assembly: Dependency("System,", LoadHint.Always)]
[assembly: InternalsVisibleTo("System.Web.WebNetTest, PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: InternalsVisibleTo("System.Web.Extensions, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: WebResource("properties_security_tab.gif", "image/gif")]
[assembly: WebResource("properties_security_tab_w_user.gif", "image/gif")]
[assembly: WebResource("add_permissions_for_users.gif", "image/gif")]
[assembly: AssemblyKeyFile("f:\\dd\\Tools\\devdiv\\FinalPublicKey.snk")]
[assembly: AssemblyDelaySign(true)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
