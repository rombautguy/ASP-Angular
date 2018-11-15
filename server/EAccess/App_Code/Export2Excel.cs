using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace EAccess.App_Code
{
    public class Export2Excel
    {
        private string DBConnectionString = ConfigurationManager.ConnectionStrings["TRIDENTConnectionString"].ConnectionString;
        private string DBProviderName = ConfigurationManager.ConnectionStrings["TRIDENTConnectionString"].ProviderName;
        #region Export2Excel No StorerKey Parameter
        public string Export2ExcelGridView(string strSheetName, string strExp2ExcelSelectCommand)
        {
            SqlDataSource sqlDataSource = new SqlDataSource();
            sqlDataSource.ConnectionString = DBConnectionString;
            sqlDataSource.ProviderName = DBProviderName;
            sqlDataSource.SelectCommand = strExp2ExcelSelectCommand;

            DataView dv = (DataView)sqlDataSource.Select(DataSourceSelectArguments.Empty);
            string strBody = DataView2ExcelString(dv, strSheetName);
            return strBody;
        }

        // DataView2ExcelString
        // 2016-04-01 No StorerKey Parameter Passed
        // Storer.Lottable01Label - Storer.Lottable10Label Not Available For Column Headings
        public string DataView2ExcelString(DataView dv, string strSheetName)
        {
            // XML - HTML  
            // Note: strSheetName is referenced in sbTop.Append below 
            StringBuilder sbTop = new StringBuilder();
            sbTop.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ");
            sbTop.Append("xmlns=\"http://www.w3.org/TR/REC-html40\"><head><meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            sbTop.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content=\"Microsoft Excel 9\"><!--[if gte mso 9]>");
            sbTop.Append("<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + strSheetName + "</x:Name><x:WorksheetOptions>");
            sbTop.Append("<x:Selected/><x:ProtectContents>False</x:ProtectContents><x:ProtectObjects>False</x:ProtectObjects>");
            sbTop.Append("<x:ProtectScenarios>False</x:ProtectScenarios></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>");
            sbTop.Append("<x:ProtectStructure>False</x:ProtectStructure><x:ProtectWindows>False</x:ProtectWindows></x:ExcelWorkbook></xml>");
            sbTop.Append("<![endif]-->");
            // @page definition is used to store document layout settings for the entire document.
            // The line below will add a header & footer to the downloaded Excel sheet.
            sbTop.Append(@"<style>
                        @page
                        {
                        mso-header-data:'&R Date: &D Time: &T';
                        mso-footer-data:'&L Proprietary & Confidential &R Page &P of &N';
                        }
                        </style>"
                          );
            sbTop.Append("</head><body><table>");
            string bottom = "</table></body></html>";

            // Build Body Column Heading
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            for (int i = 0; i < dv.Table.Columns.Count; i++)
            {
                sb.Append("<td nowrap>" + dv.Table.Columns[i].Caption + "</td>");
            }
            sb.Append("</tr>");

            // Build Body Items - Data Rows 
            for (int x = 0; x < dv.Table.Rows.Count; x++)
            {
                sb.Append("<tr>");

                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    // Get Current Table Row Value 
                    string strData = dv.Table.Rows[x][i].ToString();

                    // DataType Column Level
                    if (dv.Table.Columns[i].DataType.ToString() == "System.String")
                    {
                        // Text - Keeps Leading Zeroes 
                        sb.Append(@"<td nowrap style='mso-number-format:\@'>" + strData + "</td>");
                    }
                    else
                    {
                        // Column Not Defined As "System.String" - None Text 
                        sb.Append(@"<td nowrap>" + strData + "</td>");
                    }

                }
                sb.Append("</tr>");
            }

            string SSxml = sbTop.ToString() + sb.ToString() + bottom;
            // Return Final String 
            return SSxml;

        }
        #endregion


        #region Export2Excel with StorerKey Parameter
        // 2016-04-01 StorerKey Parameter Passed
        // Storer.Lottable01Label - Storer.Lottable10Label Available For Column Headings
        public string Export2ExcelGridView(string strSheetName, string strExp2ExcelSelectCommand, string strStorerKey)
        {
            SqlDataSource sqlDataSource = new SqlDataSource();
            sqlDataSource.ConnectionString = DBConnectionString;
            sqlDataSource.ProviderName = DBProviderName;
            sqlDataSource.SelectCommand = strExp2ExcelSelectCommand;

            DataView dv = (DataView)sqlDataSource.Select(DataSourceSelectArguments.Empty);
            string strBody = DataView2ExcelString(dv, strSheetName, strStorerKey);
            return strBody;
        }


        // DataView2ExcelString
        // 2016-04-11 Added Parameter strStorerKey
        public string DataView2ExcelString(System.Data.DataView dv, string strSheetName, string strStorerKey)
        {
            // 2016-04-11 
            // strStorerKey is Passed Parameter
            // Get Override Storer.LottableXXLabel Column Headings for Excel
            // LOTTABLE02LABEL - LOTTABLE10LABEL
            // Create Empty Array - Elements/Index 0 - 8 ==>> 9 
            string[] str_StorerLottables = new string[9];

            // Load String Array 
            SqlConnection connLottables = new SqlConnection(DBConnectionString);
            connLottables.Open();
            // 
            string sql_Lottables = "Select LOTTABLE02LABEL, LOTTABLE03LABEL, LOTTABLE04LABEL, LOTTABLE05LABEL, LOTTABLE06LABEL, LOTTABLE07LABEL, LOTTABLE08LABEL, LOTTABLE09LABEL, LOTTABLE10LABEL FROM STORER (NOLOCK) WHERE STORERKEY = '" + strStorerKey + "'";
            SqlCommand cmdLottables = new SqlCommand(sql_Lottables, connLottables);

            SqlDataReader readerLottables = cmdLottables.ExecuteReader();

            if (readerLottables.Read()) // Found Data
            {
                // Stuff readerLottables in str_StorerLottables Array
                str_StorerLottables[0] = readerLottables["LOTTABLE02LABEL"].ToString();
                str_StorerLottables[1] = readerLottables["LOTTABLE03LABEL"].ToString();
                str_StorerLottables[2] = readerLottables["LOTTABLE04LABEL"].ToString();
                str_StorerLottables[3] = readerLottables["LOTTABLE05LABEL"].ToString();
                str_StorerLottables[4] = readerLottables["LOTTABLE06LABEL"].ToString();
                str_StorerLottables[5] = readerLottables["LOTTABLE07LABEL"].ToString();
                str_StorerLottables[6] = readerLottables["LOTTABLE08LABEL"].ToString();
                str_StorerLottables[7] = readerLottables["LOTTABLE09LABEL"].ToString();
                str_StorerLottables[8] = readerLottables["LOTTABLE10LABEL"].ToString();
                // 
                readerLottables.Close();
            }

            // Close Connection
            connLottables.Close();
            // End - 2016-04-11 

            // XML - HTML  
            // Note: strSheetName is referenced in sbTop.Append below 
            StringBuilder sbTop = new StringBuilder();
            sbTop.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ");
            sbTop.Append("xmlns=\"http://www.w3.org/TR/REC-html40\"><head><meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            sbTop.Append("<meta name=ProgId content=Excel.Sheet><meta name=Generator content=\"Microsoft Excel 9\"><!--[if gte mso 9]>");
            sbTop.Append("<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + strSheetName + "</x:Name><x:WorksheetOptions>");
            sbTop.Append("<x:Selected/><x:ProtectContents>False</x:ProtectContents><x:ProtectObjects>False</x:ProtectObjects>");
            sbTop.Append("<x:ProtectScenarios>False</x:ProtectScenarios></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>");
            sbTop.Append("<x:ProtectStructure>False</x:ProtectStructure><x:ProtectWindows>False</x:ProtectWindows></x:ExcelWorkbook></xml>");
            sbTop.Append("<![endif]-->");
            // @page definition is used to store document layout settings for the entire document.
            // The line below will add a header & footer to the downloaded Excel sheet.
            sbTop.Append(@"<style>
                        @page
                        {
                        mso-header-data:'&R Date: &D Time: &T';
                        mso-footer-data:'&L Proprietary & Confidential &R Page &P of &N';
                        }
                        </style>"
                          );
            sbTop.Append("</head><body><table>");
            string bottom = "</table></body></html>";

            // Build Body Column Heading
            string str_Lottable = string.Empty;
            string str_Column = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            for (int i = 0; i < dv.Table.Columns.Count; i++)
            {
                // 2016-04-11 Override Column Headings 
                str_Column = dv.Table.Columns[i].Caption;
                // 
                switch (str_Column.ToLower())
                {
                    case "lottable02label":
                        str_Column = str_StorerLottables[0];
                        break;
                    case "lottable03label":
                        str_Column = str_StorerLottables[1];
                        break;
                    case "lottable04label":
                        str_Column = str_StorerLottables[2];
                        break;
                    case "lottable05label":
                        str_Column = str_StorerLottables[3];
                        break;
                    case "lottable06label":
                        str_Column = str_StorerLottables[4];
                        break;
                    case "lottable07label":
                        str_Column = str_StorerLottables[5];
                        break;
                    case "lottable08label":
                        str_Column = str_StorerLottables[6];
                        break;
                    case "lottable09label":
                        str_Column = str_StorerLottables[7];
                        break;
                    case "lottable10label":
                        str_Column = str_StorerLottables[8];
                        break;

                    default:
                        // No Change str_Column = str_Column;
                        break;
                }
                // End - 2016-04-11 Override Column Headings 

                // Was: sb.Append( "<td nowrap>" + dv.Table.Columns[ i ].Caption + "</td>" );
                // 2016-04-11 New
                sb.Append("<td nowrap>" + str_Column + "</td>");
            }
            sb.Append("</tr>");


            // Build Body Items - Data Rows 
            for (int x = 0; x < dv.Table.Rows.Count; x++)
            {
                sb.Append("<tr>");

                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    // Get Current Table Row Value 
                    string strData = dv.Table.Rows[x][i].ToString();

                    // DataType Column Level
                    if (dv.Table.Columns[i].DataType.ToString() == "System.String")
                    {
                        // Text - Keeps Leading Zeroes 
                        sb.Append(@"<td nowrap style='mso-number-format:\@'>" + strData + "</td>");
                    }
                    else
                    {
                        // Column Not Defined As "System.String" - None Text 
                        sb.Append(@"<td nowrap>" + strData + "</td>");
                    }

                }
                sb.Append("</tr>");
            }

            string SSxml = sbTop.ToString() + sb.ToString() + bottom;
            // Return Final String 
            return SSxml;

        }
        #endregion

    }

}