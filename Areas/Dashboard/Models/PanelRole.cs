using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Fazilat.Areas.Dashboard.Models
{
    public abstract class PanelRole
    {
        private static readonly HttpContext Context;

        private static Dictionary<string, string> roleTitle =
            new Dictionary<string, string>
            {
                {"User", "کاربر"},
                {"Parent", "والدین"},
                {"Adviser", "مشاور"},
                {"Counsellor", "مشاور"},
                {"Manager", "مدیر"},
                {"Administrator", "مدیر ارشد"}
            };

        public static string Key = "PanelRole";

        public static string GetTitle(string Name)
        {
            return roleTitle.GetValueOrDefault(Name);
        }

        public static void Set(string Role)
        {
            Context.Session.SetString(Key, Role);
        }
    }
}
