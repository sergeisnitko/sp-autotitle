using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPF.AutoTitle
{
    public class SPFTemplate
    {
        /// <summary>
        /// The List item for formatting string by template
        /// </summary>
        public ListItem Item { get; set; }
        /// <summary>
        /// The template string for formatting result string
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// The start string of the field mapping delimiter
        /// </summary>
        public string StartString { get; set; } = "{";
        /// <summary>
        /// The end string of the field mapping delimiter
        /// </summary>
        public string EndString { get; set; } = "}";
        /// <summary>
        /// The delimiter string of properties in mapped field name
        /// </summary>
        public string PropertySeparator { get; set; } = ":";
        /// <summary>
        /// The property for encode symbols
        /// </summary>
        public bool IsHtml { get; set; } = false;
        /// <summary>
        /// The structure of parent folders of the item for filling empty values
        /// </summary>
        public List<Folder> ParentFolders { get; set; } = new List<Folder>();

        /// <summary>
        /// The object that helps to format strings by template string and values of the item fields
        /// </summary>
        public SPFTemplate()
        {

        }
        /// <summary>
        /// The object that helps to format strings by template string and values of the item fields. Need to fill ListItem and Template
        /// </summary>
        public SPFTemplate(ListItem Item, string Template)
        {
            this.Item = Item;
            this.Template = Template;
        }
        /// <summary>
        /// Prepare the result string value by template
        /// </summary>
        public override string ToString()
        {
            return ToString(new Dictionary<string, string>());
        }
        /// <summary>
        /// Prepare the result string value by template and custom tags
        /// </summary>
        public string ToString(Dictionary<string, string> CustomTags)
        {
            var clientContext = this.Item.Context;
            Dictionary<string, string> CustomTagsInternalDictionary = CustomTags;
            string CheckTag = null;
            bool NeedLoading = false;

            Web oWeb = this.Item.ParentList.ParentWeb;
            if (!oWeb.IsObjectPropertyInstantiated("Url") || !oWeb.IsObjectPropertyInstantiated("ServerRelativeUrl") || !oWeb.IsObjectPropertyInstantiated("Language"))
            {
                clientContext.Load(oWeb,
                    web => web.Url,
                    web => web.ServerRelativeUrl,
                    web => web.Language
                );
                NeedLoading = true;
            }

            if (!this.Item.ParentList.IsObjectPropertyInstantiated("DefaultDisplayFormUrl") || !this.Item.ParentList.IsObjectPropertyInstantiated("Fields"))
            {
                clientContext.Load(this.Item.ParentList,
                    list => list.DefaultDisplayFormUrl,
                    list => list.Fields
                );
                NeedLoading = true;
            }

            this.ParentFolders.ForEach(ParentFolder =>
            {
                if (!ParentFolder.IsObjectPropertyInstantiated("ListItemAllFields"))
                {
                    clientContext.Load(ParentFolder,
                        folder => folder.ListItemAllFields
                    );
                    NeedLoading = true;
                }
            });


            if (NeedLoading)
            {
                clientContext.ExecuteQuery();
            }

            CheckTag = null;
            CustomTagsInternalDictionary.TryGetValue("/{Url}", out CheckTag);
            if (String.IsNullOrEmpty(CheckTag)) CustomTagsInternalDictionary.Add("/{Url}", oWeb.Url.Replace(oWeb.ServerRelativeUrl, "") + this.Item.ParentList.DefaultDisplayFormUrl + "?ID=" + this.Item.Id);

            CheckTag = null;
            CustomTagsInternalDictionary.TryGetValue("{Url}", out CheckTag);
            if (String.IsNullOrEmpty(CheckTag)) CustomTagsInternalDictionary.Add("{Url}", oWeb.Url.Replace(oWeb.ServerRelativeUrl, "") + this.Item.ParentList.DefaultDisplayFormUrl + "?ID=" + this.Item.Id);

            var InTemplate = "";
            foreach (KeyValuePair<string, string> CustomTag in CustomTagsInternalDictionary)
            {
                string Tag = CustomTag.Key;
                string Value = CustomTag.Value;

                //processing text
                InTemplate = this.Template.Replace(Tag, Value);
                //processing html
                if (IsHtml)
                {
                    Tag = Tag.Replace("{", "&#123;");
                    Tag = Tag.Replace("}", "&#125;");
                    Tag = Tag.Replace(":", "&#58;");
                    InTemplate = InTemplate.Replace(Tag, Value);
                }

            }

            //processing text
            InTemplate = ProcessingInternalNameTags(InTemplate, StartString, EndString, PropertySeparator);
            //processing html
            InTemplate = ProcessingInternalNameTags(InTemplate, "&#123;", "&#125;", "&#58;");
            return InTemplate;
        }

        public string ProcessingInternalNameTags(string Template, string StartString, string EndString, string PropertySeparator)
        {
            string OutcommingText = Template;
            int StartIndex = Template.IndexOf(StartString);

            while (StartIndex != -1)
            {
                int EndIndex = Template.IndexOf(EndString, StartIndex);
                if (EndIndex != -1)
                {
                    string Tag = Template.Substring(StartIndex, EndIndex + EndString.Length - StartIndex);
                    string TagName = Template.Substring(StartIndex + StartString.Length, EndIndex - StartIndex - StartString.Length);
                    string FieldName = TagName;
                    string PropertyName = "";

                    int PropertyIndex = TagName.IndexOf(PropertySeparator);
                    if (PropertyIndex != -1)
                    {
                        FieldName = TagName.Substring(0, PropertyIndex);
                        PropertyName = TagName.Substring(PropertyIndex + PropertySeparator.Length);
                    }

                    Field oField = this.Item.ParentList.Fields.Cast<Field>().FirstOrDefault(field => field.InternalName == FieldName);
                    if (oField != null)
                    {
                        string FieldValue = ProcessingField(oField, PropertyName);
                        OutcommingText = OutcommingText.Replace(Tag, FieldValue);
                    }
                }
                StartIndex = Template.IndexOf(StartString, EndIndex);
            }

            return OutcommingText;
        }

        public string ProcessingField(Field oField, string PropertyName)
        {
            return ProcessingField(this.Item, oField, PropertyName);
        }
        public string ProcessingField(ListItem Item, Field oField, string PropertyName)
        {
            string FieldValue = "";
            var FieldValueObject = Item[oField.InternalName];
            if (FieldValueObject == null)
            {
                Folder ValuedFolder = null;
                this.ParentFolders.ForEach(ParentFolder =>
                {
                    var ParentFolderValue = ParentFolder.ListItemAllFields[oField.InternalName];
                    if (!String.IsNullOrEmpty(ParentFolderValue.StringValueOrEmpty()))
                    {
                        if (ValuedFolder == null)
                        {
                            ValuedFolder = ParentFolder;
                        }
                    }
                });
                if (ValuedFolder != null)
                {
                    FieldValue = ProcessingField(ValuedFolder.ListItemAllFields, oField, PropertyName);
                }
            }
            else
            {
                switch (oField.FieldTypeKind)
                {
                    case FieldType.Lookup:
                        FieldLookupValue[] LookupValues = new FieldLookupValue[] { };
                        if (FieldValueObject.GetType() != LookupValues.GetType())
                        {
                            LookupValues = new FieldLookupValue[] { FieldValueObject as FieldLookupValue };
                        }
                        foreach (FieldLookupValue LookupValue in LookupValues)
                        {
                            if (LookupValue != null)
                            {
                                if (PropertyName == "LookupId")
                                {
                                    FieldValue += LookupValue.LookupId + "; ";
                                    continue;
                                }
                                FieldValue += LookupValue.LookupValue + "; ";
                            }
                        }
                        if (!String.IsNullOrEmpty(FieldValue)) FieldValue = FieldValue.Substring(0, FieldValue.Length - 2);
                        break;

                    case FieldType.User:
                        FieldUserValue[] UserValues = new FieldUserValue[] { };
                        if (FieldValueObject.GetType() != UserValues.GetType())
                        {
                            UserValues = new FieldUserValue[] { FieldValueObject as FieldUserValue };
                        }
                        foreach (FieldUserValue UserValue in UserValues)
                        {
                            if (UserValue != null)
                            {
                                if (PropertyName == "LookupId")
                                {
                                    FieldValue += UserValue.LookupId + "; ";
                                    continue;
                                }
                                FieldValue += UserValue.LookupValue + "; ";
                            }
                        }
                        if (!String.IsNullOrEmpty(FieldValue)) FieldValue = FieldValue.Substring(0, FieldValue.Length - 2);
                        break;

                    case FieldType.Boolean:
                        bool BoolValue = Convert.ToBoolean(FieldValueObject);
                        if (BoolValue)
                        {
                            FieldValue = "Yes";
                            if (this.Item.ParentList.ParentWeb.Language == 1049) FieldValue = "Да";
                        }
                        else
                        {
                            FieldValue = "No";
                            if (this.Item.ParentList.ParentWeb.Language == 1049) FieldValue = "Нет";
                        }
                        break;

                    case FieldType.URL:
                        FieldUrlValue UrlValue = FieldValueObject as FieldUrlValue;
                        FieldValue = UrlValue.Url;
                        if (PropertyName == "Description")
                        {
                            FieldValue = UrlValue.Description;
                        }
                        break;

                    case FieldType.DateTime:
                        var DateTimeValue = FieldValueObject;
                        FieldValue = DateTimeValue.DateTimeValueOrEmpty(PropertyName); //format
                        break;

                    case FieldType.Note:
                        string ServerUrl = this.Item.ParentList.ParentWeb.Url.Replace(this.Item.ParentList.ParentWeb.ServerRelativeUrl, "");
                        FieldValue = FieldValueObject.StringValueOrEmpty();
                        FieldValue = FieldValue.Replace("src=\"/", "src=\"" + ServerUrl + "/");
                        FieldValue = FieldValue.Replace("href=\"/", "href=\"" + ServerUrl + "/");
                        break;

                    default:
                        FieldValue = FieldValueObject.StringValueOrEmpty();
                        break;
                }
            }
            return FieldValue;
        }
    }
}
