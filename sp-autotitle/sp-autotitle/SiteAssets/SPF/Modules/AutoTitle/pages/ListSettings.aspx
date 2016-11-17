<%-- _lcid="1049" _version="15.0.4420" _dal="1" --%>
<%-- _LocalBinding --%>
<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document"  %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:ListItemProperty ID="ListItemProperty1" Property="BaseName" maxlength="40" runat="server"/>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
	<SharePoint:ScriptBlock ID="ScriptBlock1" runat="server">
	    var navBarHelpOverrideKey = "WSSEndUser";             		
	</SharePoint:ScriptBlock>

    <SharePoint:StyleBlock ID="StyleBlock1" runat="server">
        body #s4-leftpanel {
	        display:none;
        }
        .s4-ca {
	        margin-left:0px;
        }
    </SharePoint:StyleBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl ID="DelegateControl1" runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty ID="ProjectProperty1" Property="Description" runat="server"/>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<div>
    <!-- START Form to edit Settings-->  
        <script type="text/javascript">
            function spfatinit(){

            }

            (function () {
                EnsureScriptFunc("sp.js", "SP.ClientContext", function () {
                    EnsureScriptFunc("clientrenderer.js", "SPClientRenderer.ReplaceUrlTokens", function () {
                        var spfatapplyto = SPClientRenderer.ReplaceUrlTokens("~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.applyto.js");
                        var spfatcore = SPClientRenderer.ReplaceUrlTokens("~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.core.js");
                        var spfatsettings = SPClientRenderer.ReplaceUrlTokens("~sitecollection/_catalogs/masterpage/spf/modules/autotitle/js/spf.at.settings.js");

                        RegisterSod('spf.at.applyto', spfatapplyto);
                        RegisterSod('spf.at.core', spfatcore);
                        RegisterSod('spf.at.settings', spfatsettings);
                        EnsureScriptFunc("spf.at.applyto", "SPF.AT", function () {
                            EnsureScriptFunc("spf.at.settings", "SPF.AT.Settings", function () {
                                EnsureScriptFunc("spf.at.core", "SPFAutoTitle", function () {
                                    if (_spBodyOnLoadCalled) {
                                        spfatinit();
                                    } else {
                                        _spBodyOnLoadFunctions.push(spfatinit);
                                    }

                                    function spfatinit() {
                                        window.SPFAutoTitle = new spf.autotitle();
                                        SPFATSettings.ShowSettingsDOM();
                                    } 


                                    SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs(spfatcore);
                                });
                            });
                        });
                        
                    });
                });
                
            })();

        </script>
        <div id="LoadingImage"><img src="/_layouts/15/images/progress-circle-24.gif?rev=38" alt="Loading..."/></div>
        <div id="SettingsBlock" style="display:none;">
        <p><a href="#" resourced="true" onclick="if(typeof(window.frameElement) != 'undefined' &amp;&amp; window.frameElement != null) { window.frameElement.cancelPopUp(); return false;} if (window.history.length > 0) { window.history.back(); return false;} else { return false;}"> Назад</a></p>                     

	    <div style="margin-bottom: 20px">
		        <table width="600px" cellpadding="1">
                    <tr>
                        <td style="width: 400px;vertical-align: top;">
                            <h3 class="ms-standardheader">Настройки уведомлений</h3>
                            <div class="ms-descriptiontext">
                                <span>Укажете, необходима ли настройка уведомлений на данном списке. Если установите галочку, то добавится требуемое поле и туда-сюда</span>
                            </div>
                        </td>
                        <td style="vertical-align: top;" class="ms-authoringcontrols">
                            <table width="100%">
                                <tr>
                                    <td colspan="4">Включить настройку?</td>
                                </tr>
                                <tr>
                                    <td><input type="radio" name="SettingGroup" value="1" disabled></td><td>Да</td>
                                    <td><input type="radio" name="SettingGroup" value="0" disabled></td><td>Нет</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
		        </table>
        </div> 

	    <table border="0" width="100%" cellspacing="0" cellpadding="0" class="ms-v4propertysheetspacing" id="FormTable" style="display:none;">
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
		    <td class="ms-authoringcontrols" id="onetidDefaultTextValue1">
			    <table border="0" cellspacing="1">
			        <tr nowrap="nowrap">
			            <td colspan="2" class="ms-propertysheet"><label resourced="true">Выберите поле для автоподстановки:</label><br/>
                            <select name="SPFFieldSelect" id="SPFFieldSelect"></select>
			            </td>
                    </tr>
				    <tr nowrap="nowrap">
					    <td class="ms-propertysheet"> <label resourced="true">Шаблон:</label></td>  
                        <td class="ms-propertysheet"> <label resourced="true">Поля списка:</label></td>
				    </tr>
				    <tr nowrap="nowrap">
					    <td class="ms-propertysheet" nowrap="nowrap" style="vertical-align: top;">
					        <textarea class="ms-formula" style="width: 500px; height: 176px;" name="SPFTemplate" rows="8" cols="24" id="SPFTemplateValue" dir="ltr" disabled></textarea>
					    </td>
					    <td>
						    <select name="SPFFields" size="10" id="SPFFieldsValue" disabled>

						    </select>
					    </td>
				    </tr>
                    <tr nowrap="nowrap">
				        <td>&nbsp;</td>
				        <td class="ms-propertysheet" nowrap="nowrap" align="right">
					        <a href="javascript:;" id="SPFAddLink" style="display: none;" resourced="true">Добавить в шаблон</a>
					    </td>
				    </tr>
			    </table>
		     </td><td>
	    </td></tr>
		    <tr style="display: none;">
		        <td class="ms-authoringcontrols"><label resourced="true">Формат даты</label><br/>
				    <table border="0" cellspacing="1">
					    <tr>
						    <td colspan="2">
							    <input class="ms-input" type="text" id="SPFDateFormat" value="" disabled/>
						    </td>
					    </tr>
				    </table>
			    </td>
		    </tr>
	    <tr>
            <tr>
		    <td valign="top" class="ms-formlabel">
		        <input type="checkbox" id="SPFRequired" title="Требовать обязательное заполнение" disabled /><label resourced="true">Обязательное поле</label>
		    </td>
	        </tr>
            <tr>
		    <td valign="top" class="ms-formlabel">
		        <input type="checkbox" id="SPFReadOnly" title="Название доступно только на чтение" disabled/><label resourced="true">Только чтение</label>
		    </td>
	        </tr>
            <tr>
		    <td valign="top" class="ms-formlabel">
		        <input type="checkbox" id="SPFNewEditForm" title="Скрыть название из формы создания/изменения" disabled/><label resourced="true">Отображать на форме создания/изменения</label>
		    </td>
	        </tr>
            <tr>
		    <td valign="top" class="ms-formlabel">
		        <input type="checkbox" id="SPFDisplayForm" title="Скрыть название из формы отображения" disabled/><label resourced="true">Отображать на форме просмотра</label>
		    </td>
	        </tr>

            <!-- Кнопки -->
    <tr><td>
		    <input class="ms-ButtonHeightWidth" id="onetidSaveItem" accesskey="O" type="button" value="Сохранить" onclick="javascript:;" disabled resourced="true">
		    <span id="idSpace" class="ms-SpaceBetButtons"></span>
		    <input class="ms-ButtonHeightWidth" id="onetidCancelItem" accesskey="c" type="button" value="Отменить" disabled onclick="if(typeof(window.frameElement) != 'undefined' &amp;&amp; window.frameElement != null) { window.frameElement.cancelPopUp(); return false;} if (window.history.length > 0) { window.history.back(); return false;} else { return false;}" resourced="true">

    </td> </tr> 
	    </table>	  
    </div>      
    <!-- END Form to edit Settings-->   
	<WebPartPages:WebPartZone runat="server" title="loc:TitleBar" id="TitleBar" AllowLayoutChange="false" AllowPersonalization="false" Style="display:none;"><ZoneTemplate>
	

	</ZoneTemplate></WebPartPages:WebPartZone>
  </div>
  <table class="ms-core-tableNoSpace ms-webpartPage-root" width="100%">
				<tr>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" width="100%"> 
					<WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone> </td>
				</tr>
				<SharePoint:ScriptBlock ID="ScriptBlock2" runat="server">if(typeof(MSOLayout_MakeInvisibleIfEmpty) == "function") {MSOLayout_MakeInvisibleIfEmpty();}</SharePoint:ScriptBlock>
		</table>
</asp:Content>
