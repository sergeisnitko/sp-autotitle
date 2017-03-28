using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.AutoTitle
{
    class ArtFields
    {
        public static TextFieldDefinition SpfWebUrl()
        {
            return new TextFieldDefinition
            {
                Id = new Guid("688b2444-c308-4035-b485-bade93b5e70e"),
                Title = "Web Url",
                InternalName = "SpfWebUrl",
                Group = ArtConsts.DefaultGroupName,
                Required = true,
                TitleResource = new List<ValueForUICulture> {
                        new ValueForUICulture { CultureId = 1033, Value = "Web Url" },
                        new ValueForUICulture { CultureId = 1049, Value = "Путь к узлу" }
                }
            };
        }
    }
}
