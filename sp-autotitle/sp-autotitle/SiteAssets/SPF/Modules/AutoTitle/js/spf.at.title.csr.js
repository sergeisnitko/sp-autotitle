var CIB = CIB || {};
CIB.Controls = CIB.Controls || {};

CIB.Controls.TitleRendering = function () {   
    var TitleRenderer = {};
    TitleRenderer.Templates = {};

    TitleRenderer.Templates.Fields =
    {
        'Title': {
            'NewForm': this.RenderReadOnlyTitle,
            'EditForm': this.RenderReadOnlyTitle,
            'DisplayForm': this.RenderReadOnlyTitle,
            'View': this.GridReadOnlyTitle
        }
    };
    SPClientTemplates.TemplateManager.RegisterTemplateOverrides(TitleRenderer);
    
    this.RenderReadOnlyTitle = function (ctx) {
        return "<span>" + ctx.CurrentFieldValue + "</span";
    };
    this.GridReadOnlyTitle = function (ctx, field) {
        field.AllowGridEditing = false;
        return "<span>" + ctx.CurrentFieldValue + "</span";
    };
};




(function () {
    CIB.Controls.TitleRendering();
})();