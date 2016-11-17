using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPF.AutoTitle
{
    public class ArtConsts
    {
        public static string DefaultGroupName = "SPF";

        public static string Assets = @"SiteAssets";
        public static string SystemPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string ApplyToPath = @"SiteAssets\SPF\Modules\AutoTitle\js\spf.at.applyto.js";

    }
}
