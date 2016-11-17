using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Collections;
using SPMeta2.Syntax.Default;
using System.IO;

namespace SPF.AutoTitle
{
    public static class Extentions
    {
        public static string StringValueOrEmpty(this object Value)
        {
            return Value != null ? Value.ToString() : "";
        }

        public static string DateTimeValueOrEmpty(this object Value)
        {
            return Value.DateTimeValueOrEmpty("o");
        }

        public static string DateTimeValueOrEmpty(this object Value, string Format)
        {
            return Value != null ? ((DateTime)Value).ToString(Format) : "";
        }
        public static int IntValueOrEmpty(this object Value)
        {
            var returnValue = 0;
            if (Value != null)
            {
                Int32.TryParse(Value.ToString(), out returnValue);
            }
            return returnValue;
        }
        public static List GetListByUrl(this Web InWeb, string Url)
        {
            if (!InWeb.IsObjectPropertyInstantiated("ServerRelativeUrl"))
            {
                InWeb.Context.Load(InWeb,
                    web => web.ServerRelativeUrl
                );
                InWeb.Context.ExecuteQuery();
            }
            var ListUrl = (InWeb.ServerRelativeUrl + "/" + Url).Replace("//", "/");
            var InList = InWeb.GetList(ListUrl);

            return InList;
        }
        public static void GenerateJavascriptFile(string Path, string[] JavascriptRows)
        {
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            System.IO.File.Create(Path).Dispose();
            using (TextWriter tw = new StreamWriter(Path, true, Encoding.UTF8))
            {
                var Builder = new StringBuilder();
                Builder.AppendLine("\t" + string.Join("\n\t", JavascriptRows));
                tw.WriteLine(Builder.ToString());
                tw.Close();
            }
        }

        public static void FillApplytoFile()
        {
            var SettingsArr = new string[]
            {
                "var SPF = SPF || {};",
                "SPF.AT = SPF.AT || {};",

                "SPF.AT.WebFieldName = \""+ArtFields.SpfWebUrl().InternalName+"\";",
                "SPF.AT.SettingsList = \""+ArtLists.CustomAutoTitleList().CustomUrl+"\";",

                "SPF.AT.ListsSettingsCtId = \""+ArtContentTypes.CustomAutoTitleCT().GetContentTypeId()+"\";"
            };
            GenerateJavascriptFile(Path.Combine(ArtConsts.SystemPath, ArtConsts.ApplyToPath), SettingsArr);
        }
    }
}
