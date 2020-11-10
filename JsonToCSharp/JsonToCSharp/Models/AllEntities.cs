using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSharp.Models
{
    public class AllEntities
    {
        public List<Value> value { get; set; }
    }


    public class EntityModel
    {
        public string LogicalName { get; set; }
        public string DisplayName { get; set; }
        public string EntitySetName { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class LocalizedLabel
    {
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }

    public class UserLocalizedLabel
    {
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged { get; set; }
        public string MetadataId { get; set; }
        public object HasChanged { get; set; }
    }

    public class DisplayName
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public UserLocalizedLabel UserLocalizedLabel { get; set; }
    }

    public class Value
    {
        public string LogicalName { get; set; }
        public string MetadataId { get; set; }
        public DisplayName DisplayName { get; set; }

        public string EntitySetName { get; set; }
    }
    public class SelctionPattern
    {
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }
        public string Text { get; set; }
    }
}
