using Microsoft.SharePoint.Client;
using SP.Cmd.Deploy;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace SPF.AutoTitle
{
    public static class Model
    {
        public static SiteModelNode DeployModel()
        {
            return SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(ArtFields.SpfWebUrl())
                    .AddContentType(ArtContentTypes.CustomAutoTitleCT(), contentType =>
                    {
                        contentType
                            .AddContentTypeFieldLink(ArtFields.SpfWebUrl())
                        ;
                    })
                    .AddRootWeb(new RootWebDefinition(), RootWeb =>
                    {
                        RootWeb
                            .AddList(ArtLists.CustomAutoTitleList(), SpfList =>
                            {
                                SpfList
                                    .AddContentTypeLink(ArtContentTypes.CustomAutoTitleCT())
                                    .AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                                    {
                                        ContentTypes = new List<ContentTypeLinkValue>
                                        {
                                            new ContentTypeLinkValue{ ContentTypeName = ArtContentTypes.CustomAutoTitleCT().Name }
                                        }
                                    })
                                    .AddListView(ArtViews.SettingsView());
                            })
                            .AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                            {
                                var FolderPath = Path.Combine(ArtConsts.SystemPath, ArtConsts.Assets);
                                if (Directory.Exists(FolderPath))
                                {
                                    Extentions.FillApplytoFile();
                                    ModuleFileUtils.LoadModuleFilesFromLocalFolder(list, FolderPath);
                                }

                            })
                            ;

                    })
                    .AddUserCustomAction(ArtActions.ListSettings())
                    .AddUserCustomAction(ArtActions.FolderSettings())
                    ;
            });
        }

        public static void ExecuteModel(this SiteModelNode Model, string url, ICredentials Credential = null)
        {
            SharePoint.Session(url, Credential, ctx =>
            {
                var provisionService = new CSOMProvisionService();
                provisionService.DeployModel(SiteModelHost.FromClientContext(ctx), Model);

            });
        }

        public static void Retract(SPDeployOptions options)
        {
            SharePoint.Session(options.url, options.Credentials, Ctx =>
            {
                var Site = Ctx.Site;
                var CustomActions = Site.UserCustomActions;
                Ctx.Load(CustomActions);
                Ctx.ExecuteQuery();
                var ListSettingsAction = CustomActions.Where(x => x.Name == ArtActions.ListSettings().Name).FirstOrDefault();
                if (ListSettingsAction != null)
                {
                    ListSettingsAction.DeleteObject();
                    Ctx.ExecuteQuery();
                }

                var FolderSettingsAction = CustomActions.Where(x => x.Name == ArtActions.FolderSettings().Name).FirstOrDefault();
                if (FolderSettingsAction != null)
                {
                    FolderSettingsAction.DeleteObject();
                    Ctx.ExecuteQuery();
                }
            });
        }
        public static void Deploy(SPDeployOptions options)
        {
            DeployModel().ExecuteModel(options.url, options.Credentials);
        }
        public static void Execute(SPDeployOptions options)
        {
            SharePoint.Session(options.url, options.Credentials, ctx =>
            {
                var Mask = new FieldMask(ctx);
                Mask.Execute();
            });
        }
    }
}
