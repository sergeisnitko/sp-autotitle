using Microsoft.SharePoint.Client;
using SPMeta2.BuiltInDefinitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPF.AutoTitle
{
    public class FieldMask:IDisposable
    {
        public ClientContext ctx { get; set; }
        public ListItemCollection SettingsListItems { get; private set; }
        public FieldMaskSetting RunSettings { get; private set; }

        private string SettingsFileName = "RunSettings.spf";
        private static string FieldMaskSuffix = "SPF.Mask";
        public FieldMask()
        {
            Init();
        }
        public FieldMask(ClientContext ctx)
        {
            this.ctx = ctx;
            Init();
        }
        public void Init()
        {
            var SettingsList = ctx.Site.RootWeb.GetListByUrl(ArtLists.CustomAutoTitleList().CustomUrl);

            var AllItemsQuery = CamlQuery.CreateAllItemsQuery();
            SettingsListItems = SettingsList.GetItems(AllItemsQuery);

            ctx.Load(SettingsListItems);
            ctx.ExecuteQuery();
            LoadSettings();
        }

        public void Execute()
        {
            FindAllChanges(i =>
            {
                SetUnmaskedValues(i);
            });
            Dispose();
        }

        public List<ListItem> FindAllChanges()
        {
            return FindAllChanges(null);
        }
        public List<ListItem> FindAllChanges(Action<ListItem> ExecuteFunc)
        {
            var CurrentRunDate = DateTime.Now;
            var AllChangedItems = new List<ListItem>();
            SettingsListItems.ToList().ForEach(SettingsListItem =>
            {
                AllChangedItems.AddRange(FindListChanges(SettingsListItem));
            });

            AllChangedItems.ForEach(AllChangedItem =>
            {
                if (ExecuteFunc != null)
                {
                    ExecuteFunc(AllChangedItem);
                }
            });

            RunSettings.PreviousRunDate = CurrentRunDate;
            return AllChangedItems;
        }

        public List<ListItem> FindListChanges(ListItem SettingsListItem)
        {
            var TargetListUrl = SettingsListItem[BuiltInFieldDefinitions.Title.InternalName].StringValueOrEmpty();
            var TargetListWebUrl = SettingsListItem[ArtFields.SpfWebUrl().InternalName].StringValueOrEmpty();
            var InWeb = ctx.Site.OpenWeb(TargetListWebUrl);
            var InList = InWeb.GetList(TargetListUrl);

            var Query = String.Format("<View Scope='RecursiveAll'><Query><Where><Gt><FieldRef Name='Modified' /><Value IncludeTimeValue='TRUE' Type='DateTime'>{0}</Value></Gt></Where></Query></View>",
                RunSettings.PreviousRunDate.DateTimeValueOrEmpty("yyyy-MM-ddTHH:mm:ssZ"));
            var ChangeQuery = new CamlQuery();
            ChangeQuery.ViewXml = Query;
            var ChangedItems = InList.GetItems(ChangeQuery);
            InList.Context.Load(ChangedItems);
            InList.Context.ExecuteQuery();
            return ChangedItems.ToList();
        }

        public List<Folder> GetParentFolders(ListItem Item)
        {
            var ParentFolders = new List<Folder>();
            var FileDirRef = Item["FileDirRef"].StringValueOrEmpty();
            var RootFolder = Item.ParentList.RootFolder;
            var Folder = Item.ParentList.ParentWeb.GetFolderByServerRelativeUrl(FileDirRef);
            ctx.Load(Folder, f=>f.ListItemAllFields, f => f.ServerRelativeUrl, f => f.Properties);
            ctx.Load(RootFolder, f => f.ListItemAllFields, f => f.ServerRelativeUrl, f => f.Properties);
            ctx.ExecuteQuery();

            ParentFolders.Add(Folder);
            while(Folder.ServerRelativeUrl.ToLower() != RootFolder.ServerRelativeUrl.ToLower())
            {
                Folder = Folder.ParentFolder;
                ctx.Load(Folder, f => f.ListItemAllFields, f => f.ServerRelativeUrl, f => f.Properties);
                ctx.ExecuteQuery();
                ParentFolders.Add(Folder);
            }

            return ParentFolders;
        }

        public void SetUnmaskedValues(ListItem Item)
        {
            var ParentFolders = GetParentFolders(Item);
            var Props = FilterProperties(ParentFolders);

            foreach (var Prop in Props)
            {
                var Template = new SPFTemplate
                {
                    Item = Item,
                    Template = Prop.Value,
                    StartString = "[",
                    EndString = "]"
                };

                var Value = Template.ToString();
                var FieldInternalName = Prop.Key.Replace("_"+ FieldMaskSuffix,"").Trim();
                Item[FieldInternalName] = Value;
                Item.Update();
            }
            Item[BuiltInFieldDefinitions.Modified.InternalName] = Item[BuiltInFieldDefinitions.Modified.InternalName];
            Item.Update();
            ctx.ExecuteQuery();
        }

        public Dictionary<string, string> FilterProperties(List<Folder> ParentFolders)
        {
            var ReturnDir = new Dictionary<string, string>();

            foreach (var ParentFolder in ParentFolders)
            {
                var Fields = ParentFolder.Properties.FieldValues
                    .Where(prop => prop.Key.ToString().IndexOf(FieldMaskSuffix, System.StringComparison.Ordinal) != -1)
                    .ToDictionary(
                        prop => prop.Key.ToString(),
                        prop => prop.Value.ToString());

                foreach (var Field in Fields)
                {
                    if (!ReturnDir.ContainsKey(Field.Key))
                    {
                        ReturnDir.Add(Field.Key, Field.Value);
                    }
                }
            }

            return ReturnDir;
        }
        public void SaveSettings()
        {
            var Serializer = new XmlSerializer(typeof(FieldMaskSetting));
            var Writer = new StreamWriter(SettingsFileName);
            Serializer.Serialize(Writer, RunSettings);
            Writer.Close();
        }

        public void LoadSettings()
        {
            if (System.IO.File.Exists(SettingsFileName))
            {
                var Serializer = new XmlSerializer(typeof(FieldMaskSetting));
                var Reader = new FileStream(SettingsFileName, FileMode.Open);
                RunSettings = (FieldMaskSetting)Serializer.Deserialize(Reader);
            }
            else
            {
                RunSettings = new FieldMaskSetting();
                RunSettings.PreviousRunDate = DateTime.MinValue;
            }
        }
        public void Dispose()
        {
            SaveSettings();
        }
    }


    [Serializable]
    public class FieldMaskSetting
    {
        public DateTime PreviousRunDate { get; set; }
    }
}


