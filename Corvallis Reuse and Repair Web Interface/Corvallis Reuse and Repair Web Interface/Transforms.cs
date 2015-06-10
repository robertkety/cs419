using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRRD_Web_Interface
{
    public class Transforms
    {
        // Thanks! https://social.msdn.microsoft.com/Forums/vstudio/en-US/6ffcb247-77fb-40b4-bcba-08ba377ab9db/converting-a-list-to-datatable
        public static DataTable ConvertToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable FilterByName(DataTable unfiltered, string SearchString)
        {
            DataRow[] FilteredRows = unfiltered.Select("Name like '%" + SearchString + "%'");
            DataTable filtered_dt = new DataTable();
            filtered_dt = unfiltered.Clone();

            if (FilteredRows.Count() == 0)
            {
                return unfiltered;
            }

            foreach (DataRow row in FilteredRows)
            {
                filtered_dt.Rows.Add(row.ItemArray);
            }

            return filtered_dt;
        }
    }
}