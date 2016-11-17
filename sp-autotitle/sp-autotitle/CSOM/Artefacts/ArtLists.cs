using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.AutoTitle
{
    class ArtLists
    {
        public static ListDefinition CustomAutoTitleList()
        {
            return new ListDefinition
            {
                Title = "Настройки автоподстановок",
                CustomUrl = "Lists/CustomAutoTitleList",
                ContentTypesEnabled = true,
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                EnableVersioning = true,
                //TitleResource = new List<ValueForUICulture> {
                //        new ValueForUICulture { CultureId = 1033, Value = "Lists For Custom AutoTitle" },
                //        new ValueForUICulture { CultureId = 1049, Value = "Настройки уведомлений" } }
            };
        }
    }
}
