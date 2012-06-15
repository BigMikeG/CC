/*
 * Created by SharpDevelop.
 * User: GZNDQ9
 * Date: 4/23/2012
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;                      // DataTable
using System.IO;                        // File, StreamReader
using System.Text;                      // StringBuilder
using System.Windows.Forms;             // DataGridView
    
namespace MyFileActions
{
    /// <summary>
    /// Description of Export.
    /// </summary>
    public class Export
    {
        public Export()
        {
        }
        
        // You can use any delimiter you like (, ; tab etc).
        static public void AsTextDelimited(ref DataGridView grid, string tablename, string filename, string delimiter)
        {
            int i;
            StringBuilder sb = new StringBuilder();
            
            // Write the Table Name to the first row of the file.
            sb.Append(tablename + Environment.NewLine);
            
            // Write the header row to the string builder.
            // All but the last column needs a delimiter after it.
            for (i = 0; i < (grid.Columns.Count - 1); i++)
            {
                sb.Append(grid.Columns[i].HeaderText + delimiter);
            }
            sb.Append(grid.Columns[grid.Columns.Count - 1].HeaderText); // last column header
            sb.Append(Environment.NewLine);

            // Now write each of the data rows to the string builder.
            foreach (DataGridViewRow row in grid.Rows)
            {
                i = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (i > 0) { sb.Append(delimiter); }
                    sb.Append(cell.Value.ToString());
                    i++;
                }
                sb.Append(Environment.NewLine);
            }

            // Write the string to a file.
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(sb.ToString());
            }        
        }
    }

    public class Import
    {
        public Import()
        {
        }
        
        /// <summary>
        /// This function imports data from a text delimited file.
        /// </summary>
        /// <param name="filename">The name of the file to import.</param>
        /// <param name="delimiter">The text delimiter (such as a comma "," or 
        /// tab "\t" that the rows in the file will be split on.</param>
        static public string FromTextDelimited(ref DataTable dt, ref bool colFound, string filename, char delimiter)
        {
            string rv = String.Empty;
            char[] delimiters = new char[] { delimiter };

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    // Read Table Name
                    string name = sr.ReadLine();
                    
                    // Reading header
                    string[] row = sr.ReadLine().Split(delimiters);
                    foreach (string col in row) 
                    {
                        if (col.Equals("Diff"))
                        {
                            dt.Columns.Add(col, typeof(bool));
                            colFound = true;
                        }
                        else
                        {
                            dt.Columns.Add(col, typeof(string));
                        }
                    }
    
                    // Reading the content (rows of data).
                    while (sr.Peek() >= 0)
                    {
                        dt.Rows.Add(sr.ReadLine().Split(delimiters));
                    }
                }
            } 
            catch (Exception e) 
            {
                rv = "Failed to read text file: " + e.ToString();
            }
            
            return rv;
        }

//        /// <summary>
//        /// This function reads the first line of an exported text file.
//        /// </summary>
//        /// <param name="filename">The import file.</param>
//        /// <returns>The data table name.</returns>
//        static public string GetTableName(string filename)
//        {
//            string name = String.Empty;
//
//            try
//            {
//                using (StreamReader sr = new StreamReader(filename))
//                {
//                    // Read Table Name
//                    name = sr.ReadLine();
//                }
//            } 
//            catch (Exception e) 
//            {
//                //rv = "Failed to read text file: " + e.ToString();
//                MessageBox.Show("Failed to read text file: " + e.ToString());
//            }
//            
//            return name;
//        }
    }
}
