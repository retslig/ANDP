using System;
using System.Text;
using System.Web.Mvc;

namespace Common.Lib.MVC.Helpers
{
    /// <summary>
    /// Helper class that will emit javascript code for us lazy C# guys that don't like 
    /// Javascript
    /// </summary>
    public static class JavascriptHelper
    {
        /// <summary>
        /// Helper method that will emit Jquery settings for
        /// remote validation to not fire on each key stroke.
        /// Make sure your view references jquery.validate.min.js, jquery.validate.unobtrusive.js. Should be used
        /// as a stand alone function call.
        /// /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString SetJQueryValidationSettings(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script type=\"text/javascript\">");

            sb.AppendLine("(function ($) {");
            sb.AppendLine("$.validator.setDefaults({");
            sb.AppendLine("onkeyup: function(element) {");
            sb.AppendLine("if ($(element).attr('data-val-remote-url')) {");
            sb.AppendLine("return false;");
            sb.AppendLine("} else {");
            sb.AppendLine("$(element).validate();");
            sb.AppendLine("return $(element).valid();");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("});");
            sb.AppendLine("} (jQuery));");

            sb.AppendLine("</script>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Helper that will emit the proper javascript to show a user friendly error message popup to 
        /// the right of the errored element. This is much more user friendly than requiring the user to click
        /// next to the element to get the message to show. Requires jquery.qtip.min.js and qtip CSS jquery.qtip.min.css.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString BuildJQueryValidationPopup(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("var settings = $.data($('form')[0], 'validator').settings;");
            sb.AppendLine("settings.errorPlacement = function (error, inputElement) {");
            sb.AppendLine("var container = $(this).find(\"[data-valmsg-for='\" + inputElement[0].name + \"']\"),replace = $.parseJSON(container.attr(\"data-valmsg-replace\")) !== false;");
            sb.AppendLine("container.removeClass(\"field-validation-valid\").addClass(\"field-validation-error\");");
            sb.AppendLine("error.data(\"unobtrusiveContainer\", container);");
            sb.AppendLine("if (replace) {");
            sb.AppendLine("container.empty();");
            sb.AppendLine("error.removeClass(\"input-validation-error\").appendTo(container);");
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine("error.hide();");
            sb.AppendLine("}");
            sb.AppendLine("var element = inputElement;");
            sb.AppendLine("var elem = $(element),corners = ['left center', 'right center'], flipIt = elem.parents('span.right').length > 0;");
            sb.AppendLine("if (!error.is(':empty')) {");
            sb.AppendLine("elem.filter(':not(.valid)').qtip({overwrite: false,content: error, position: { my: 'left center',at: 'right center', viewport: $(window) }, show: { event: false, ready: true}, hide: false, style: {  classes: 'ui-tooltip-red'}");
            sb.AppendLine(" })");
            sb.AppendLine(".qtip('option', 'content.text', error);");
            sb.AppendLine("}");
            sb.AppendLine("else { elem.qtip('destroy'); }");
            sb.AppendLine("};");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static string BuildJsPrototype(this HtmlHelper helper)
        {
            var modelType = helper.ViewData.Model.GetType();
            var d = ModelToJavascript.Build(modelType);
            return d;
        }
        public static string BuildJsPrototype(this HtmlHelper helper, string targetName)
        {
            var modelType = helper.ViewData.Model.GetType();
            var d = ModelToJavascript.Build(modelType, targetName);
            return d;
        }
        public static string BuildJsPrototype(this HtmlHelper helper, Type modelType)
        {
            var d = ModelToJavascript.Build(modelType);
            return d;
        }
        public static string BuildJsPrototype(this HtmlHelper helper, Type modelType, string targetName)
        {
            var d = ModelToJavascript.Build(modelType, targetName);
            return d;
        }

    }
    
}
