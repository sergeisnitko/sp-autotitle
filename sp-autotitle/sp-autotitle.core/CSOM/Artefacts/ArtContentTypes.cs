using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.AutoTitle
{
    class ArtContentTypes
    {
        public static ContentTypeDefinition CustomAutoTitleCT()
        {
            return new ContentTypeDefinition
            {
                Name = "Custom AutoTitle List",
                Id = new Guid("3ff3e1f9-88b3-4ba6-a792-a49b89ccfe5f"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = ArtConsts.DefaultGroupName,
                NameResource = new List<ValueForUICulture> {
                    new ValueForUICulture { CultureId = 1033, Value = "Custom AutoTitle List" },
                    new ValueForUICulture { CultureId = 1049, Value = "Списки автоподстановок"}
                }
            };
        }
    }
}
