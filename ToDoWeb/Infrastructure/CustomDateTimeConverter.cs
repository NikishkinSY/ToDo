using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoWeb.Infrastructure
{
    /// <summary>
    /// converter DateTime to JSON with such format
    /// </summary>
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.Culture = new System.Globalization.CultureInfo("ru-RU");
            base.DateTimeFormat = "dd.MM.yyyy HH:mm";
        }
    }
}
