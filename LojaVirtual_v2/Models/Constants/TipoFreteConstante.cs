using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models.Constants
{
    public class TipoFreteConstante
    {
        public const string SEDEX = "04014";
        public const string SEDEX10 = "04782";
        public const string PAC = "04510";

        public static string GetNames(string code)
        {
            foreach (var field in typeof(TipoFreteConstante).GetFields())
            {
                if ((string)field.GetValue(null) == code)
                    return field.Name.ToString();
            }
            return "";
        }
    }
}
