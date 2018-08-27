using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.MVC.Helpers
{
    //htmlHelper.LanguageSelectorLink("en-US", "[English]", "English", null);
    public class Language
    {
        public string SelectedName { get; set; }
        public string UnSelectedName { get; set; }
        public string Value { get; set; }
    }


    //                <section id="login">
    //    <ul id="langmenu">
    //        <li>
    //            @Html.LanguageSelectorLink("en-US", "[English]", "English", null)
    //        </li>
    //        <li>
    //            @Html.LanguageSelectorLink("de", "[German]", "German", null)
    //        </li>
    //        <li>
    //            @Html.LanguageSelectorLink("es", "[Spanish]", "Spanish", null)
    //        </li>
    //        <li>
    //            @Html.LanguageSelectorLink("zh-CN", "[中文]", "中文", null)
    //        </li>
    //    </ul>
    //</section>

    public static class Languages
    {
        public static List<Language> Items
        {
            get
            {
                var languages = new List<Language>
                {
                    new Language
                    {
                        SelectedName = "[English]",
                        UnSelectedName = "English",
                        Value = "en-US"
                    },
                    new Language
                    {
                        SelectedName = "[German]",
                        UnSelectedName = "German",
                        Value = "de"
                    },
                    new Language
                    {
                        SelectedName = "[Spanish]",
                        UnSelectedName = "Spanish",
                        Value = "es"
                    },
                    new Language
                    {
                        SelectedName = "[中文]",
                        UnSelectedName = "中文",
                        Value = "zh-CN"
                    }
                };
                return languages;
            }
        }
    }
}
