using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.AutoTitle
{
    class ArtActions
    {
        public static UserCustomActionDefinition ListSettings()
        {
            return new UserCustomActionDefinition
            {
                Title = "AutoTitle Settings",
                Name = "SPF.AutoTitle.ListSettings",
                Url = "~sitecollection/_catalogs/masterpage/spf/modules/autotitle/Pages/ListSettings.aspx?SiteUrl={SiteUrl}&ListId={ListId}",
                Group = "GeneralSettings",
                Location = "Microsoft.SharePoint.ListEdit",
                Sequence = 100,
                TitleResource = new List<ValueForUICulture> {
                    new ValueForUICulture { CultureId = 1033, Value = "AutoTitle Settings" },
                    new ValueForUICulture { CultureId = 1049, Value = "Настройка автоподстановки" }
                }
            };
        }

        public static UserCustomActionDefinition FolderSettings()
        {
            return new UserCustomActionDefinition
            {
                Title = "AutoTitle Settings",
                Description = "AutoTitle Settings",
                Name = "SPF.AutoTitle.FolderSettings",
                Url = "~sitecollection/_catalogs/masterpage/spf/modules/autotitle/Pages/ListSettings.aspx?SiteUrl={SiteUrl}&ListId={ListId}&FolderID={ItemId}",
                RegistrationType = "ContentType",
                RegistrationId = "0x0120",
                Location = "EditControlBlock",
                Rights = new Collection<string> { "EditListItems" },
                Sequence = 1,
                TitleResource = new List<ValueForUICulture> {
                    new ValueForUICulture { CultureId = 1033, Value = "AutoTitle Settings" },
                    new ValueForUICulture { CultureId = 1049, Value = "Настройка автоподстановки" }
                }
            };
        }
    }
}
