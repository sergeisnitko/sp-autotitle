using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.AutoTitle
{
    class ArtViews
    {
        public static ListViewDefinition SettingsView()
        {
            return new ListViewDefinition
            {
                Title = "Все элементы",
                RowLimit = 25,
                Url = "AllItems.aspx",
                Query = "<OrderBy><FieldRef Name='Created' Ascending='False'/></OrderBy>",
                IsDefault = true,
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.DocIcon,
                    BuiltInInternalFieldNames.LinkTitle,
                    ArtFields.SpfWebUrl().InternalName,
                    BuiltInInternalFieldNames.Editor,
                    BuiltInInternalFieldNames.Modified
                }
            };
        }
    }
}
