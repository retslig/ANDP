using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Common.Lib.Extensions;
using Common.Lib.MVC.Extensions;

namespace Common.Lib.MVC.Helpers
{
	public static class ExtendedHtmlHelper
	{
		/// <summary>
		/// Embeds the image.
		/// Example use: @Html.EmbeddedImage("ajax-loader.gif", null) or @Html.EmbeddedImage("corporate.gif", new { width = 150, height = 50})
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="imageName">Name of the image.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <returns></returns>
		public static MvcHtmlString EmbeddedImage(this HtmlHelper htmlHelper, string imageName, dynamic htmlAttributes)
		{
			var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
			var anchor = new TagBuilder("img");
			anchor.Attributes["src"] = url.Action("GetEmbeddedResource", "Shared",
												  new
													  {
														  resourceName = "Common.Lib.MVC.Content.Images." + imageName,
														  pluginAssemblyName = url.Content("~/" + "bin/Common.Lib.MVC.dll")
													  });

			if (htmlAttributes != null)
			{
				string width = "";
				string height = "";
				PropertyInfo pi = htmlAttributes.GetType().GetProperty("width");
				if (pi != null)
					width = pi.GetValue(htmlAttributes, null).ToString();

				pi = htmlAttributes.GetType().GetProperty("height");
				if (pi != null)
					height = pi.GetValue(htmlAttributes, null).ToString();

				if (!string.IsNullOrEmpty(height))
					anchor.Attributes["height"] = height;

				if (!string.IsNullOrEmpty(width))
					anchor.Attributes["width"] = width;
			}
			return MvcHtmlString.Create(anchor.ToString());
		}

		public static MvcHtmlString FormLineDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string labelText = null, string customHelpText = null, object htmlAttributes = null)
		{
			return FormLine(
				(labelText == null ? "" : helper.LabelFor(expression, labelText).ToString()) +
				(customHelpText == null ? MvcHtmlString.Create("") : helper.HelpTextFor(expression, customHelpText)),
				helper.DropDownListFor(expression, selectList, htmlAttributes).ToString() + 
				helper.ValidationMessageFor(expression));
		}

		public static MvcHtmlString HelpTextFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string customText = null)
		{
			// Can do all sorts of things here -- eg: reflect over attributes and add hints, etc...
			//var title = new TagBuilder("div");
			return MvcHtmlString.Create("");
		}

		public static MvcHtmlString FormLineComboBox<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string labelText = null, string customHelpText = null, object htmlAttributes = null)
		{
			return FormLine(
				(labelText == null ? "" : helper.LabelFor(expression, labelText).ToString()) +
				(customHelpText == null ? MvcHtmlString.Create("") : helper.HelpTextFor(expression, customHelpText)),
				helper.TextBoxFor(expression, htmlAttributes).ToString() + helper.Raw("<br/>") + 
				helper.ValidationMessageFor(expression));
		}

		private static MvcHtmlString FormLine(string labelContent, string fieldContent, object htmlAttributes = null)
		{
			var editorLabel = new TagBuilder("div");
			editorLabel.AddCssClass("editor-label");
			editorLabel.InnerHtml += labelContent;

			var editorField = new TagBuilder("div");
			editorField.AddCssClass("editor-field");
			editorField.InnerHtml += fieldContent;

			var container = new TagBuilder("div");
			if (htmlAttributes != null)
				container.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			container.AddCssClass("form-line");
			container.InnerHtml += editorLabel;
			container.InnerHtml += editorField;

			return MvcHtmlString.Create(container.ToString());
		}

		/// <summary>
		///  This helper creates a button with the given attributes in the htmlAttributes pram and will autogenerate the onclick event if set to true.
		///  Use in cshtml file like this: @Html.ActionLinkButton("Supervisor Reporting", "Index", "Reporting", new RouteValueDictionary { { "reportType", "SUP" } }, new { id = "SupButtonClick" }, true)
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="buttonText">The button text.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <param name="routeValues">The route values.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <param name="autoGenerateOnClick">if set to <c>true</c> [auto generate on click].</param>
		/// <returns></returns>
		public static MvcHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string buttonText, string actionName,
													 string controllerName, RouteValueDictionary routeValues, dynamic htmlAttributes, bool autoGenerateOnClick)
		{
			string id = "";
			string onclick = "";
			string target = "";
			string style = "";
			string cssclass = "";

			if (autoGenerateOnClick)
			{
				string data = "";
				if (routeValues != null)
				{
					foreach (var value in routeValues)
					{
						if (string.IsNullOrEmpty(data))
							data = "?";
						else
							data += "&";
						data += value.Key + "=" + value.Value;
					}
				}
				var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
			    if (htmlHelper.ViewContext.HttpContext.Request.Url != null)
			        onclick = "location.href='" + url.Action(actionName, controllerName, null, htmlHelper.ViewContext.HttpContext.Request.Url.Scheme) + data + "'";
			}

			if (htmlAttributes != null)
			{
				PropertyInfo pi = htmlAttributes.GetType().GetProperty("id");
				if (pi != null)
					id = pi.GetValue(htmlAttributes, null).ToString();

				pi = htmlAttributes.GetType().GetProperty("onclick");
				if (pi != null)
					onclick = pi.GetValue(htmlAttributes, null).ToString();

				pi = htmlAttributes.GetType().GetProperty("target");
				if (pi != null)
					target = pi.GetValue(htmlAttributes, null).ToString();

				pi = htmlAttributes.GetType().GetProperty("style");
				if (pi != null)
					style = pi.GetValue(htmlAttributes, null).ToString();

				pi = htmlAttributes.GetType().GetProperty("cssclass");
				if (pi != null)
					cssclass = pi.GetValue(htmlAttributes, null).ToString();
			}
			var anchor = new TagBuilder("input");
			anchor.Attributes["type"] = "button";
			anchor.Attributes["name"] = actionName;
			anchor.Attributes["title"] = buttonText;
			anchor.Attributes["value"] = buttonText;
			if (!string.IsNullOrEmpty(cssclass))
				anchor.Attributes["class"] = cssclass;
			if (!string.IsNullOrEmpty(id))
				anchor.Attributes["id"] = id;
			if (!string.IsNullOrEmpty(style))
				anchor.Attributes["style"] = style;
			if (!string.IsNullOrEmpty(onclick))
				anchor.Attributes["onclick"] = onclick;
			if (!string.IsNullOrEmpty(target))
				anchor.Attributes["target"] = target;

			return MvcHtmlString.Create(anchor.ToString());
		}

		/// <summary>
		///  This helper creates a submit button with the given attributes in the htmlAttributes pram.
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="buttonText">The button text.</param>
		/// <param name="actionName">Name of the action.</param>
		/// <param name="controllerName">Name of the controller.</param>
		/// <param name="routeValues">The route values.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <returns></returns>
		public static MvcHtmlString ActionLinkSubmitButton(this HtmlHelper htmlHelper, string buttonText, string actionName,
											 string controllerName, RouteValueDictionary routeValues, dynamic htmlAttributes)
		{
			string id = "";
			if (htmlAttributes != null)
			{
				PropertyInfo pi = htmlAttributes.GetType().GetProperty("id");
				if (pi != null)
					id = pi.GetValue(htmlAttributes, null).ToString();
			}

			var anchor = new TagBuilder("input");
			anchor.Attributes["type"] = "submit";
			anchor.Attributes["id"] = id;
			anchor.Attributes["name"] = actionName;
			anchor.Attributes["title"] = buttonText;
			anchor.Attributes["value"] = buttonText;

			return MvcHtmlString.Create(anchor.ToString());
		}

		/// <summary>
		/// Creates a table from a IEnumerable object using the dataannotations on the model for visibility and displa names.
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="objList">The obj list.</param>
		/// <param name="htmlAttributes">The HTML attributes.</param>
		/// <param name="tableTitle">The table title.</param>
		/// <returns></returns>
		public static MvcHtmlString Table(this HtmlHelper htmlHelper, IEnumerable<object> objList, dynamic htmlAttributes, string tableTitle)
		{
		    var enumerable = objList as object[] ?? objList.ToArray();
		    if (objList == null || !enumerable.Any())
				return new MvcHtmlString("");

			var fieldsToDisplay = (from property in enumerable.FirstOrDefault().GetType().GetProperties()
											where GetAttributeScaffoldColumn(property) && GetAttributeHideColumn(property)
											select property.Name).ToList();

			//At some point set the htmlAttributes variable to what properties you want below. Or update css.
			var fullTable = new Table() { Width = 800, BorderWidth = 0, CellPadding = 1, CellSpacing = 1 };
			var fullTableRow = new TableRow();
			fullTableRow.Cells.Add(new TableCell() { BackColor = Color.FromArgb(000066), ColumnSpan = 3, Height = 20 });
			fullTable.Rows.Add(fullTableRow);

			var title = new TableRow();
			title.Cells.Add(new TableCell()
			{
				Text = "<center><h2>" + tableTitle + "</h2></center>",
				ColumnSpan = fieldsToDisplay.Count
			});
			fullTable.Rows.Add(title);

			var table = new Table { CssClass = "sortable", BorderWidth = 0, CellPadding = 1, CellSpacing = 1 };

			var headerRow = new TableHeaderRow { TableSection = TableRowSection.TableHeader };
			foreach (var property in enumerable.FirstOrDefault().GetType().GetProperties().Where(obj => fieldsToDisplay.Contains(obj.Name)))
			{
				headerRow.Cells.Add(new TableCell() { Text = GetAttributeDisplayName(property), ForeColor = Color.Black });
			}
			table.Rows.Add(headerRow);

			int count = 1;
			foreach (var obj in enumerable)
			{
				int classstyle = count % 2 + 1;
				var row = new TableRow { TableSection = TableRowSection.TableBody, CssClass = "iter" + classstyle };

				foreach (var property in obj.GetType().GetProperties().Where(obj2 => fieldsToDisplay.Contains(obj2.Name)))
				{
					string text = "";
					try
					{
						text = property.GetValue(obj, null).ToString();
					}
					catch
					{
					}

					row.Cells.Add(new TableCell() { Text = text });
				}
				table.Rows.Add(row);
				count++;
			}

			fullTableRow = new TableRow();
			fullTableRow.Cells.Add(new TableCell() { Text = RenderTableToString(table) });
			fullTable.Rows.Add(fullTableRow);

			return MvcHtmlString.Create(RenderTableToString(table));
		}

		private static string RenderTableToString(Table fullTable)
		{
			using (var ts = new StringWriter())
			using (var html = new System.Web.UI.HtmlTextWriter(ts))
			{
				// Not entirely sure about this part
				fullTable.RenderControl(html);
				html.Flush();
				return HttpUtility.HtmlDecode(ts.ToString().Replace("\t", "").Replace(Environment.NewLine, ""));
			}
		}

		private static string GetMetaDisplayName(PropertyInfo property)
		{
			var atts = property.DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true);
			if (atts.Length == 0)
				return null;

			var metaAttr = atts[0] as MetadataTypeAttribute;
			var metaProperty =
				metaAttr.MetadataClassType.GetProperty(property.Name);
			if (metaProperty == null)
				return null;
			return GetAttributeDisplayName(metaProperty);
		}

		private static string GetAttributeDisplayName(PropertyInfo property)
		{
			var atts = property.GetCustomAttributes(
				typeof(DisplayAttribute), true);
			if (atts.Length == 0)
				return property.Name;
			return (atts[0] as DisplayAttribute).Name;
		}

		private static bool GetAttributeScaffoldColumn(PropertyInfo property)
		{
			var atts = property.GetCustomAttributes(typeof(ScaffoldColumnAttribute), true);
			if (atts.Length == 0)
				return true;
			return false;
		}

		private static bool GetAttributeHideColumn(PropertyInfo property)
		{
			var atts = property.GetCustomAttributes(typeof(ExtendedDataAnnotationsModelMetadataProvider.HideColumnAttribute), true);
			if (atts.Length == 0)
				return true;

			return !(atts[0] as ExtendedDataAnnotationsModelMetadataProvider.HideColumnAttribute).HideColumnInGrid;
		}

		public static MvcHtmlString GetDisplayNameAttributeValue<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
		{
			// get the metdata
			ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			// get the field name 
			//var fieldName = ExpressionHelper.GetExpressionText(expression);
			//var columnProperty = typeof(TModel).GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(DisplayNameAttribute)) && prop.Name == fieldName);
			return MvcHtmlString.Create(fieldmetadata.DisplayName);
		}

		public static ModelMetadata GetMetadata<TModel>(this TModel model)
		{
			return ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(TModel));
		}
        
        public static MvcHtmlString CreateBootstrapMenuItem(this HtmlHelper htmlHelper)
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (htmlHelper.ViewContext.HttpContext.Request.Url == null)
                throw new Exception("No HttpContext available.");

            TagBuilder li;
            TagBuilder href;
            TagBuilder caret;
            var mainContainer = new TagBuilder("ul");
            mainContainer.Attributes["class"] = "nav navbar-nav";
            li = new TagBuilder("li");
            href = new TagBuilder("a");
            href.Attributes["href"] = url.Action("Index", "Home", null, htmlHelper.ViewContext.HttpContext.Request.Url.Scheme);
            href.InnerHtml = "Home";
            li.InnerHtml += href;
            mainContainer.InnerHtml += li;

            if (UserContext.IsAuthenticated)
            {
                var claims = UserContext.RetrieveMenuClaims();
                var groupedMenu = MenuItems.Items.GroupBy(u => u.GroupName).ToList();

                foreach (IGrouping<string, Menu> group in groupedMenu)
                {
                    var mainli = new TagBuilder("li");
                    mainli.Attributes["class"] = "dropdown";
                    href = new TagBuilder("a");
                    href.Attributes["href"] = "#";
                    href.Attributes["class"] = "dropdown-toggle";
                    href.Attributes["data-toggle"] = "dropdown";
                    caret = new TagBuilder("b");
                    caret.Attributes["class"] = "caret";
                    href.InnerHtml = group.Key + " " + caret.ToString();
                    mainli.InnerHtml = href.ToString();
                    var ul = new TagBuilder("ul");
                    ul.Attributes["class"] = "dropdown-menu";

                    foreach (var activity in group)
                    {
                        li = new TagBuilder("li");
                        href = new TagBuilder("a");
                        href.Attributes["href"] = url.Action("Index", activity.Controller, null, htmlHelper.ViewContext.HttpContext.Request.Url.Scheme);
                        href.InnerHtml = activity.DisplayName;
                        li.InnerHtml += href;
                        ul.InnerHtml += li;
                    }

                    mainli.InnerHtml += ul;
                    mainContainer.InnerHtml += mainli;
                }
            }
            
            var langLi = new TagBuilder("li");
            langLi.Attributes["class"] = "dropdown";
            href = new TagBuilder("a");
            href.Attributes["href"] = "#";
            href.Attributes["class"] = "dropdown-toggle";
            href.Attributes["data-toggle"] = "dropdown";
            caret = new TagBuilder("b");
            caret.Attributes["class"] = "caret";
            href.InnerHtml = "Languages" + " " + caret.ToString();
            langLi.InnerHtml = href.ToString();
            var langUl = new TagBuilder("ul");
            langUl.Attributes["class"] = "dropdown-menu";

            foreach (var language in Languages.Items)
            {
                li = new TagBuilder("li");
                var selector =
                    htmlHelper.LanguageSelectorLink(language.Value, language.SelectedName, language.UnSelectedName,
                        null).ToHtmlString();
                li.InnerHtml = selector;
                langUl.InnerHtml += li;
            }

            langLi.InnerHtml += langUl;
            mainContainer.InnerHtml += langLi;

            if (UserContext.IsAuthenticated)
            {
                li = new TagBuilder("li");
                var span = new TagBuilder("span");
                //span.Attributes["class"] = "username";
                span.SetInnerText("Welcome " + UserContext.Identity.Name + " ");
                href = new TagBuilder("a");
                href.Attributes["href"] = url.Action("logout", "auth", null,
                    htmlHelper.ViewContext.HttpContext.Request.Url.Scheme);
                href.Attributes["class"] = "btn btn-info btn-sm detail-btn";
                href.InnerHtml = "Log Out";
                li.InnerHtml += span;
                li.InnerHtml += href;

                mainContainer.InnerHtml += li;
            }
            
            return MvcHtmlString.Create(mainContainer.ToString());
        }

        public static MvcHtmlString HiddenObjectFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> orginalExpression)
        {
            var objectDepth = new List<PropertyInfo>();
            var expressionBody = (MemberExpression)orginalExpression.Body;
            var body = (MemberExpression)orginalExpression.Body;
            while (body.NodeType == ExpressionType.MemberAccess)
            {
                var propertyInfo = (PropertyInfo)body.Member;

                objectDepth.Add(propertyInfo);

                body = body.Expression as MemberExpression;
                if (body == null)
                    break;
            }
            objectDepth.Reverse();

            Type baseType = expressionBody.Member.DeclaringType;
            string basePropertyName = expressionBody.Member.Name;

            ParameterExpression parmExpression = Expression.Parameter(typeof(TModel), "p");
            //build object depth first in expression
            Expression baseExpression = objectDepth.Aggregate<PropertyInfo, Expression>(parmExpression, Expression.Property);
            return MvcHtmlString.Create(GetHtmlForObject(htmlHelper, baseType.GetProperties().FirstOrDefault(p => p.Name == basePropertyName).PropertyType, baseExpression, parmExpression));
        }

        private static string GetHtmlForObject<TModel>(HtmlHelper<TModel> htmlHelper, Type objectType, Expression baseExpression, ParameterExpression parmExpression)
        {
            string html = "";
            //Now get all properties within object.
            foreach (var property in objectType.GetProperties())
            {
                Expression expression = Expression.Property(baseExpression, property);
                //html += "      " + htmlHelper.HiddenFor(Convert.ChangeType(Expression.Lambda(expression, parmExpression), property.PropertyType)).ToHtmlString() + Environment.NewLine;
                //html += "      " + htmlHelper.HiddenFor(Expression.Lambda(expression, parmExpression) as Expression<Func<TModel, T>>).ToHtmlString() + Environment.NewLine;
                string propertyName = property.PropertyType.Name;
                if (propertyName == "Nullable`1")
                    propertyName = "Nullable<" + Nullable.GetUnderlyingType(property.PropertyType).Name + ">";

                switch (propertyName)
                {
                    case "Int32":
                        html += "      " + htmlHelper.HiddenFor(Expression.Lambda(expression, parmExpression) as Expression<Func<TModel, int>>).ToHtmlString() + Environment.NewLine;
                        break;
                    case "Nullable<Int32>":
                        html += "      " + htmlHelper.HiddenFor(Expression.Lambda(expression, parmExpression) as Expression<Func<TModel, Nullable<int>>>).ToHtmlString() + Environment.NewLine;
                        break;
                    case "String":
                        html += "      " + htmlHelper.HiddenFor(Expression.Lambda(expression, parmExpression) as Expression<Func<TModel, string>>).ToHtmlString() + Environment.NewLine;
                        break;
                    case "DateTime":
                        html += "      " + htmlHelper.HiddenFor(Expression.Lambda(expression, parmExpression) as Expression<Func<TModel, DateTime>>).ToHtmlString() + Environment.NewLine;
                        break;
                    case "Nullable<DateTime>":
                        html += "      " + htmlHelper.HiddenFor(Expression.Lambda(expression, parmExpression) as Expression<Func<TModel, Nullable<DateTime>>>).ToHtmlString() + Environment.NewLine;
                        break;
                    default:
                        if (property.PropertyType.BaseType == typeof(object))
                        {
                            //Make recursive call
                            html += GetHtmlForObject(htmlHelper, property.PropertyType, expression, parmExpression);
                            break;
                        }

                        throw new NotImplementedException(property.PropertyType.Name);
                }
            }

            return html;
        }

        public static Uri FullyQualifiedUri(this HtmlHelper html, string relativeOrAbsolutePath)
        {
            var baseUri = HttpContext.Current.Request.Url;
            string path = UrlHelper.GenerateContentUrl(relativeOrAbsolutePath, new HttpContextWrapper(HttpContext.Current));
            Uri instance;
            Uri.TryCreate(baseUri, path, out instance);
            return instance; // instance will be null if the uri could not be created
        }
	}
}

