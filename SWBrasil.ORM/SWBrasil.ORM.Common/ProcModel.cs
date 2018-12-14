using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.Common
{
    public class TableModel
    {
        private string _modelName = "";
        private string _aliasName = "";

        public string Name { get; set; }
        public string ModelName {
            get
            {
                if (string.IsNullOrEmpty(_modelName))
                    _modelName = this.Name.Replace("tb_", "") + "Model";

                return _modelName;
            }
            set { _modelName = value; }
        }
        public List<ColumnModel> Columns { get; set; }
        public string Group { get; set; }
        public enumTableType Type { get; set; }

        public string Alias
        {
            get
            {
                if (string.IsNullOrEmpty(_aliasName))
                    _aliasName = this.Name.Replace("tb_", "") + "DTO";

                return _aliasName;
            }
            set { _aliasName = value; }
        }
        public bool IgnoreDTO { get; set; }
        public bool MainDTO { get; set; }

        public string Label { get; set; }
    }

    public class ColumnModel
    {
        private string _label = "";
        public int Position { get; set; }
        public string ColumnName { get; set; }
        
        public bool Required { get; set; }
        public string DataType { get; set; }
        public string DbType { get; set; }
        public int? Size { get; set; }
        public int? Precision { get; set; }

        public string DefaultValue { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPK { get; set; }
        public bool IsUniqueKey { get; set; }
        public string RelatedTable{get;set;}
        
        public string ExtendedProperty { get; set; }
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(_label))
                    _label = this.ColumnName;
                return _label;
            }
            set { _label = value; }
        }

        public bool UseAsLabelOnComboBox { get; set; }
        public bool UseAsSearchParameter { get; set; }
        public bool IgnoreOnDTO { get; set; }
        public bool ShowOnResultGrid { get; set; }

        public string DTOName { get; set; }
        public bool UseAsRelatedObject { get; set; }

        public enumSelectionType SelectionType { get; set; }
    }

    public enum enumTableType
    {
        [Description("Basic")]
        Basic = 0,

        [Description("Dictionary")]
        Dictionary = 1,

        [Description("Relation_1_To_N")]
        Relation_1_To_N = 2,

        [Description("Relation_N_To_N")]
        Relation_N_To_N = 3
    }

    public enum enumSelectionType
    {
        [Description("ComboBox")]
        ComboBox = 0,

        [Description("SearchModal")]
        SearchModal = 1
    }
}


