using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace AppFramework.Common
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly PropertyInfo nameProperty;

        public LocalizedDisplayNameAttribute(Type resourceType = null, [CallerMemberName] string displayNameKey = null)
            : base(displayNameKey)
        {
            if (resourceType != null)
            {
                var q = (from a in resourceType.GetRuntimeProperties() where a.Name.Equals(displayNameKey) select a);
                if (q.ToList().Count > 0)
                {
                    nameProperty = q.ToList().FirstOrDefault();
                }
            }
        }

        public override string DisplayName
        {
            get
            {
                if (nameProperty == null)
                {
                    return base.DisplayName;
                }
                return (string)nameProperty.GetValue(nameProperty.DeclaringType, null);
            }
        }
    }
}