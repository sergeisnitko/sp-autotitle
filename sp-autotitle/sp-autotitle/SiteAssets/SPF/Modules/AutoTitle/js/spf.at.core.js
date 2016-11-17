var spf = spf || {};

spf.autotitle = function () {
    var _self = this;
    var _DOM = {};
    var _ctx = {};
    var stdJSLink = "clienttemplates.js";
    var customReadonlyLink = '~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.title.csr.js';

    var staticsrc = "~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.static.js"
    var propertyName = "SPF.Mask";
    var dateFormatName = "SPF.DateFormat";
    var availableTypes = ["text", "note"];
    

    this._init = function () {

        EnsureScriptFunc("sp.js", "SP.ClientContext", function () {
            EnsureScriptFunc("clientrenderer.js", "SPClientRenderer.ReplaceUrlTokens", function () {
                staticsrc = SPClientRenderer.ReplaceUrlTokens(staticsrc);
                RegisterSod('spf.at.static', staticsrc);
                EnsureScriptFunc("spf.at.static", "SPFAutoTitle", function () {
                    spf.ensureMQuery(function () {
                        _self._localizePage();
                        _self._fillStandardParams();
                        _self.firstLoadData();
                    });
                });

            });
        });

    };


    this._localizePage = function () {
        var Labels = m$("label[resourced='true'], a[resourced='true']");
        Labels.forEach(function (a, b, c) {
            a.innerHTML = spf.GetLocalized(a.innerHTML);
        });

        var Inputs = m$("input[resourced='true']");
        Inputs.forEach(function (a, b, c) {
            var type = a.getAttribute("type");

            switch (type.toLowerCase()) {
                case "button":
                    a.value = spf.GetLocalized(a.value);
                    break;
                default:
                    a.innerHTML = spf.GetLocalized(a.innerHTML);
                    break;
            }
        });

    };

    this._parseInternalName = function(InternalName) {
        if (InternalName.indexOf('[') == 0) {
            InternalName = InternalName.substring(1, InternalName.length);
        }
        if (InternalName.indexOf(']') == (InternalName.length - 1)) {
            InternalName = InternalName.substring(0, InternalName.length - 1);
        }

        return InternalName;
    };

    this.maskForm = function (message) {
        if (_DOM.table) {
            _DOM.formElements.forEach(function (el, i, a) {
                m$(a[i]).attr("disabled", "disabled");
            });
            _DOM.link.style.display = "none";
        }
        if (SP.UI.Notify) {
            _ctx.notification = SP.UI.Notify.addNotification(message, false);
        }
        return _ctx.notification;
    };

    this.unMask = function (notification) {
        if (_DOM.loadingImage) {
            _DOM.loadingImage.style.display = "none";
        }
        if (_DOM.table) {
            _DOM.formElements.forEach(function (el, i, a) {
                a[i].removeAttribute("disabled");
            });

            _DOM.link.style.display = "inline";
            _DOM.settingsBlock.style.display = "block";
        }
        if (notification) {
            SP.UI.Notify.removeNotification(notification);
        }
    };

    this.reuseContext = function () {
        if (!_ctx.ClientContext) {
            var PathToWeb = GetUrlKeyValue('SiteUrl');
            var FolderUrl = decodeURIComponent(GetUrlKeyValue("RootFolder"));
            _ctx.clientContext = new SP.ClientContext(PathToWeb);
            _ctx.web = _ctx.clientContext.get_web();

            _ctx.lists = _ctx.web.get_lists();
            _ctx.targetList = _ctx.lists.getById(_DOM.listId);
            _ctx.fields = _ctx.targetList.get_fields();
            _ctx.rootFolder = _ctx.targetList.get_rootFolder();
            if (_DOM.FolderId) {
                _ctx.item = _ctx.targetList.getItemById(_DOM.FolderId);
                _ctx.clientContext.load(_ctx.item);
                _ctx.clientContext.load(_ctx.item, "Folder");
                _ctx.rootFolder = _ctx.item.get_folder();
            }

            if (FolderUrl) {
                _ctx.rootFolder = _ctx.web.getFolderByServerRelativeUrl(FolderUrl);
            }

            _ctx.props = _ctx.rootFolder.get_properties();
        }

    };

    this.ChangeStatus = function (message, args) {
        SP.UI.Status.removeAllStatus();

        var statusId = SP.UI.Status.addStatus(message);
        var color = "green";
        if (args) {
            color = "red";
            console.log("SPF AutoTitle Error: ", message + " - " + args.get_message() + " - " + args.get_stackTrace())
        }
        SP.UI.Status.setStatusPriColor(statusId, color);
    };

    this._fillStandardParams = function () {
        _DOM.listId = decodeURIComponent(spf.urlParam("ListId"));
        _DOM.FolderId = spf.urlParam("FolderID");

        _DOM.loadingImage = m$("#LoadingImage")[0];
        _DOM.settingsBlock = m$("#SettingsBlock")[0];
        //
        _DOM.table = m$("#FormTable");
        _DOM.formElements = _DOM.table.find("input, textarea, select");

        _DOM.link = m$("#SPFAddLink")[0];
        _DOM.selectedValue = m$("#SPFFieldsValue")[0];

        _DOM.selectedValue.ondblclick = _self.addFieldToTemplate;
        _DOM.link.onclick = _self.addFieldToTemplate;
        _DOM.template = m$("#SPFTemplateValue")[0];
        _DOM.required = m$("#SPFRequired")[0];
        _DOM.newEditForm = m$("#SPFNewEditForm")[0];
        _DOM.displayForm = m$("#SPFDisplayForm")[0];
        _DOM.readOnly = m$("#SPFReadOnly")[0];
        _DOM.dateFormatField = m$("#SPFDateFormat")[0];
        _DOM.fieldSelect = m$("#SPFFieldSelect")[0];
        _DOM.saveButton = m$("#onetidSaveItem")[0];
        _DOM.saveButton.onclick = _self.saveData;               

        $addHandler(_DOM.fieldSelect, 'change', this.anotherLoadData);
    };

    this.addFieldToTemplate = function () {
        var fldselected = TrimSpaces(GetSelectedValue(_DOM.selectedValue));
        _DOM.template.value = _DOM.template.value + fldselected;
    };

    this.firstLoadData = function () {
        _self.reuseContext();
        _ctx.clientContext.load(_ctx.fields);
        _ctx.clientContext.load(_ctx.rootFolder);
        _ctx.clientContext.load(_ctx.props);

        _ctx.clientContext.executeQueryAsync(
            _self._firstLoadData_Success,
            _self._LoadData_Exception
        );
    };

    this._firstLoadData_Success = function (sender, args) {
        var selectedField = "";
        if (_ctx.item) {
            _ctx.rootFolder = _ctx.item.get_folder();
        }
        var fields = _ctx.fields.getEnumerator();

        var option = '<option value="[NextNumber]">Очередной рег.номер (NextNumber)</option>';
        m$(_DOM.selectedValue).append(option);

        while (fields.moveNext()) {
            var field = fields.get_current();
            //console.log(field.get_typeAsString());
            var AdditProperty = "";
            if (field.get_typeAsString() == "DateTime") {
                AdditProperty = ":dd.MM.yyyy HH:mm:ss";
            }
            if (field.get_typeAsString() == "Lookup") {
                AdditProperty = ":LookupValue";
            }
            var option = String.format('<option value="[{0}{2}]">{1} ({0})</option>', field.get_internalName(), field.get_title(), AdditProperty);

            m$(_DOM.selectedValue).append(option);
            if ((availableTypes.indexOf(field.get_typeAsString().trim().toLowerCase()) != -1)
            && (!field.get_readOnlyField())) {
                m$(_DOM.fieldSelect).append(option);
                if (selectedField == "") {
                    selectedField = field.get_internalName();
                }
            }
            if (field.get_internalName().toLowerCase() == selectedField.toLowerCase()) {
                _ctx.selectedField = field;
                _self._fillFieldData(_ctx.selectedField);
            }
        }
        if (window.location.hash) {
            var hash = window.location.hash.replace("#","");
            _DOM.fieldSelect.value = "[" + hash + "]";
            _self.anotherLoadData();
        } else {
            _self.unMask(_ctx.notification);
        }
    };

    this._LoadData_Exception = function (sender, args) {
        _self.ChangeStatus("Ошибка загрузки", args);
    };


    this.anotherLoadData = function () {
        _self.maskForm(spf.GetLocalized("Loading"));

        var inValue = _DOM.fieldSelect.value;
        if (inValue) {            
            inValue = _self._parseInternalName(inValue);
            window.location.hash = "#" + inValue;
            
            _self.reuseContext();
            _ctx.selectedField = _ctx.fields.getByInternalNameOrTitle(inValue);
            _ctx.clientContext.load(_ctx.selectedField);
            _ctx.clientContext.load(_ctx.props);

            _ctx.clientContext.executeQueryAsync(
                _self._anotherLoadData_Success,
                _self._LoadData_Exception
            );
        }

    };
    
    this._anotherLoadData_Success = function (sender, args) {
        if (_ctx.item) {
            _ctx.rootFolder = _ctx.item.get_folder();
        }
        _self._fillFieldData(_ctx.selectedField);
        _self.unMask(_ctx.notification);
    };


    this._fillFieldData = function (field) {

        var allProperties = _ctx.props.get_fieldValues();
        var propValue = allProperties[field.get_internalName() + "_" + propertyName];
        if (propValue) {
            _DOM.template.value = propValue;
        } else {
            _DOM.template.value = "";
        }
        

        var dateFormatValue = allProperties[field.get_internalName() + "_" + dateFormatName];
        if (dateFormatValue) {
            _DOM.dateFormatField.value = dateFormatValue;
        } else {
            _DOM.dateFormatField.value = "dd.MM.yyyy HH:mm:ss";
        }

        var fieldAttrs = spf.parseFieldXml(field.get_schemaXml());

        var vShowIdNewForm = 'TRUE';
        if (fieldAttrs["ShowInNewForm"]) {
            vShowIdNewForm = fieldAttrs["ShowInNewForm"].value;
        }
        var vShowInDisplayForm = 'TRUE';
        if (fieldAttrs["ShowInDisplayForm"]) {
            vShowInDisplayForm = fieldAttrs["ShowInDisplayForm"].value;
        }
        
        var jslink = field.get_jsLink();
        _DOM.required.checked = field.get_required();
        var readonly = false;
        if (jslink.toLowerCase() != stdJSLink) {
            readonly = true;
        }
        _DOM.readOnly.checked = readonly;
        

        _DOM.newEditForm.checked = spf.checkBoolFromString(vShowIdNewForm);
        _DOM.displayForm.checked = spf.checkBoolFromString(vShowInDisplayForm);
    };

    this.saveData = function() {
        _self.maskForm(spf.GetLocalized("Loading"));
        var selectedFieldName = _DOM.fieldSelect.value;
        if (selectedFieldName) {

            selectedFieldName = _self._parseInternalName(selectedFieldName);

            _self.reuseContext();
            _ctx.selectedField = _ctx.fields.getByInternalNameOrTitle(selectedFieldName);

            _ctx.props.set_item(selectedFieldName + "_" + propertyName, _DOM.template.value);
            _ctx.props.set_item(selectedFieldName + "_" + dateFormatName, _DOM.dateFormatField.value);

            _ctx.rootFolder.update();
            _ctx.selectedField.set_required(_DOM.required.checked);
            
            var link = stdJSLink;
            if (_DOM.readOnly.checked) {
                link = customReadonlyLink;
            }
            _ctx.selectedField.set_jsLink(link);

            _ctx.selectedField.setShowInNewForm(_DOM.newEditForm.checked);
            _ctx.selectedField.setShowInEditForm(_DOM.newEditForm.checked);

            _ctx.selectedField.setShowInDisplayForm(_DOM.displayForm.checked);

            _ctx.selectedField.update();

            _ctx.clientContext.load(_ctx.selectedField);

            _ctx.clientContext.load(_ctx.props);
            _ctx.clientContext.load(_ctx.rootFolder);

            _ctx.clientContext.executeQueryAsync(
                function () {
                   // console.log("hello!", _ctx.selectedField.get_schemaXml())
                    _self.unMask(_ctx.notification);
                },
                _self._LoadData_Exception
            );
        }
    };

    this._init();
};


