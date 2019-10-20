using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{

    public enum GridDataType
    {
        alpha,
        date,
        numeric,
        index,
        amount,
        usercontrol,
        none
    }
    public class GridHeader
    {
        private String _column_Name;
        public String Column_Name
        {
            get{return _column_Name;}
            set { 
                _column_Name = value.Replace(" ","_");
            }
        }

        private string _display_Name;
        public String Display_Name
        {
            get
            {
                if (string.IsNullOrEmpty(_display_Name))
                {
                    return _column_Name;
                }
                return _display_Name;
            }
            set { _display_Name = value; }
        }

        private GridDataType _data_type;
        public GridDataType Data_Type
        {
            get
            {
                return _data_type;
            }
            set { _data_type = value; }
        }

        private string _width;
        public String Width
        {
            get
            {
                return _width;
            }
            set { _width = value; }
        }
    }

    public class GridItem
    {
        private String _column_Name;
        public String Column_Name
        {
            get { return _column_Name; }
            set
            {
                _column_Name = value.Replace(" ", "_");
            }
        }
        public object Value { get; set; }

    }

    public class GridRow
    {
        private List<GridItem> _item = new List<GridItem>();
        public List<GridItem> Item
        {
            get { return _item; }
            set
            {
                _item = value;
            }
        }
    }


    public class RenderGridData : AppControl
    {
        public RenderGridData()
            : base("table")
        {
            this.Attributes.Add("class", "table table-datatable table-custom");
            this.Control_Type = AppControlType.GridData;
        }
        

    }
}
