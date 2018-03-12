using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

namespace AspNet.StarterKits.Classifieds.Web
{
    public sealed class Util
    {
        private Util()
        {
        }

        public static Control FindControlRecursively(string controlID, ControlCollection controls)
        {
            if (controlID == null || controls == null)
                return null;

            foreach (Control c in controls)
            {
                if (c.ID == controlID)
                    return c;

                if (c.HasControls())
                {
                    Control inner = FindControlRecursively(controlID, c.Controls);
                    if (inner != null)
                        return inner;
                }
            }
            return null;
        }
    }
}