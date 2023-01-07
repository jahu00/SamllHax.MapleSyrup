using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SamllHax.MapleSyrup.Extensions
{
    public static class XAttributeExtensions
    {
        public static int ValueAsInt(this XAttribute attribute)
        {
            return Convert.ToInt32(attribute.Value.ToString());
        }
        public static float ValueAsFloat(this XAttribute attribute)
        {
            return float.Parse(attribute.Value.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
