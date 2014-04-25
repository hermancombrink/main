using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ParallaxTheme.App_Code
{
    public static class LinkHelper
    {
        public static MvcHtmlString HoverLink(this HtmlHelper htmlHelper, string name, string description, string type, string icon, string hexcolor, string action, string controller = "")
        {

            //<figure class="alignleft dropcap-icon type-1"><i class="icon-user" style="color: #484485; font-size: 30px; line-height: 1.2em;"></i></figure>
            //<div class="extra-wrap">
          //  <h5>About</h5>
            //<p>@Html.ActionLink("What is between the book's cover", "About")
           // </p>
        //</div> 
            var link = "";
            if(string.IsNullOrEmpty(controller))
                link = "/Home/" + action;
            else
                link = "/" + controller + "/" + action;
            var tag = new StringBuilder();
            tag.AppendLine(string.Format("<a href='{0}' ><figure class='alignleft dropcap-icon {2}'><i class='{1}' style='color: #{3}; font-size: 30px; line-height: 1.2em;'></i></figure>", link, icon, type, hexcolor));
            tag.AppendLine("<div class='extra-wrap'>");
            tag.AppendLine(string.Format("<h5>{0}</h5>", name));
            tag.AppendLine(string.Format("<p><a href='{1}'>{0}</a></p></div></a>", description, link));
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}