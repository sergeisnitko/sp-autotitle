var SPF = SPF || {};
SPF.AT = SPF.AT || {};


SPF.AT.Settings = function () {
    var self = this;
    var stdJSLink = "clienttemplates.js";
    var customReadonlyLink = '~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.title.csr.js';
    var staticsrc = "~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.static.js"

    var propertyName = "SPF.Mask";
    var dateFormatName = "SPF.DateFormat";
    var availableTypes = ["text", "note"];

    self.AutoTitleTable = $("#FormTable");

    this.GetCommon = function (callback) {
        var PathToWeb = GetUrlKeyValue('SiteUrl');
        var ListId = GetUrlKeyValue('ListId');
        if (PathToWeb && ListId) {
            self.ctx = new SP.ClientContext(PathToWeb);
            self.web = self.ctx.get_web();
            self.rootWeb = self.ctx.get_site().get_rootWeb();
            self.currentList = self.web.get_lists().getById(ListId);
            self.currentListRootFolder = self.currentList.get_rootFolder();

            self.CurrentUser = self.web.get_currentUser();
            self.ctx.load(self.CurrentUser);

            var settingsListUrl = (_spPageContextInfo.webServerRelativeUrl + "/" + SPF.AT.SettingsList)
                            .replace(new RegExp("///", "g"), "//").replace(new RegExp("//", "g"), "/");


            self.settingsList = self.rootWeb.getList(settingsListUrl);

            self.ctx.load(self.web);
            self.ctx.load(self.rootWeb);
            self.ctx.load(self.currentList);
            self.ctx.load(self.currentListRootFolder);
            self.ctx.load(self.currentListRootFolder,"ServerRelativeUrl");

            self.ctx.executeQueryAsync(function () {
                self.currentListTitle = self.currentList.get_title();
                self.currentListUrl = self.currentListRootFolder.get_serverRelativeUrl();

                if (callback) {
                    callback();
                }
            }, function (sender, args) {
                self.ChangeStatus("Ошибка загрузки настроек", args);
            });

        }

    };

    this.SettingExists = function (callback) {
        var query = "<View>" +
                        "<Query>" +
                            "<Where>" +
                                "<Eq>" +
                                    "<FieldRef Name='Title'/>" +
                                    "<Value Type='Text'>" + self.currentListUrl + "</Value>" +
                                 "</Eq>" +
                            "</Where>" +
                        "</Query>" +
                     "</View>";
        var camlQuery = new SP.CamlQuery();
        camlQuery.set_viewXml(query);
        var SettingsItems = self.settingsList.getItems(camlQuery);

        self.ctx.load(SettingsItems);

        self.ctx.executeQueryAsync(function () {
            var SettingsItemsArr = SettingsItems.get_data();

            if (SettingsItemsArr.length > 0) {
                self.settingsListItem = SettingsItemsArr[0];
            }

            if (callback) {
                callback();
            }
        }, function (sender, args) {
            self.ChangeStatus("Ошибка получения элемента настроек", args);
        });
    };

    this.CreateSettingItemNoExecute = function () {
        if (!self.settingsListItem) {
            var settingsListItemInfo = new SP.ListItemCreationInformation();
            self.settingsListItem = self.settingsList.addItem(settingsListItemInfo);
        };
        self.settingsListItem.set_item("ContentTypeId", SPF.AT.ListsSettingsCtId);
        self.settingsListItem.set_item("Title", self.currentListUrl);
        self.settingsListItem.set_item(SPF.AT.WebFieldName, self.web.get_serverRelativeUrl());
        self.settingsListItem.update();
    };
    this.RemoveSettingItemNoExecute = function () {
        if (self.settingsListItem) {
            self.settingsListItem.deleteObject();
            self.settingsListItem = null;
        }
    };

    this.ShowSettingsDOM = function (NoCode) {
        function RedefineObject(){
            self.init();
            self.ShowSettingsDOM(true);
        }
        function BindChangeValue() {
            var SettingGroupValId = "0";
            if (self.settingsListItem) {
                SettingGroupValId = "1";
                self.AutoTitleTable.show();
            }
            var RadioSelector = 'input[name=SettingGroup]';

            $(RadioSelector + '[value=' + SettingGroupValId + ']').attr("checked", "checked");
            $(RadioSelector).removeAttr("disabled");
            $(RadioSelector).on("click", function () {
                var SelectedValue = $(RadioSelector + ':checked').val();
                if (SelectedValue == "1") {
                    self.CreateSettings(RedefineObject);
                } else {
                    self.RemoveSettings(RedefineObject);
                }
            });
        }
        self.GetCommon(function () {
            self.SettingExists(function () {
                if (!NoCode) {
                    BindChangeValue();
                }

            });
            document.title = "Настройки списка " + self.currentListTitle;
        });
    };

    this.CreateSettings = function (callback) {
        self.CreateSettingItemNoExecute();

        self.ctx.executeQueryAsync(function () {
            self.ChangeStatus("Настройки применены");
            self.AutoTitleTable.show();

            if (callback)
                callback();

        }, function (sender, args) {
            self.ChangeStatus("Ошибка обновления настроек", args);
        });
    };

    this.RemoveSettings = function (callback) {
        self.RemoveSettingItemNoExecute();
        self.ctx.executeQueryAsync(function () {
            self.ChangeStatus("Настройки удалены");
            self.AutoTitleTable.hide();

            if (callback)
                callback();
        }, function (sender, args) {
            self.ChangeStatus("Ошибка удаления настроек", args);
        });
    };
    this.ChangeStatus = function (message, args) {
        SP.UI.Status.removeAllStatus();

        var statusId = SP.UI.Status.addStatus(message);
        var color = "green";
        if (args) {
            color = "red";
            console.log("SPFAlertError: ", message + " - " + args.get_message() + " - " + args.get_stackTrace())
        }
        SP.UI.Status.setStatusPriColor(statusId, color);
    }

    this.init = function () {
        window.console = window.console || {};
        window.console.log = window.console.log || function () { };
        console.clear()

        this.ctx = null;
        this.web = null;
        this.rootWeb = null;
        this.settingsList = null;

        this.currentList = null;
        this.currentListTitle = null;
        this.currentListUrl = null;

        this.currentUser = null;
    };

    this.init();
};

(function () {
    window.SPFATSettings = new SPF.AT.Settings();
})();