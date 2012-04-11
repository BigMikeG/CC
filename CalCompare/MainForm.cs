/*
 * Created by SharpDevelop.
 * User: GZNDQ9
 * Author: Mike Galiati
 * Date: 3/23/2012
 * Time: 12:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 * 
 * Data Grid format:
 * Column: 0       1        2      3              4
 *         Calset, Calname, Units, Diff Detected, Hex Val for calplot file 1, Hex Val for calplot file 2, ...
 *
 * 
 * If you add files (to an existing grid) it seems to load slowly.
 * For instance load file1, file2, file3. They load fast.
 * Add file file4. It takes a lot longer than if you would have added all 4 the first time.
 * 
 * Features To Add:
 * 1) Add AutoFiltering of the calset column from bookmarked pages.
 *    Also hide the part columns that aren't in the calset.
 * 2) Add a scrollbar at bottom for large number of parts (try loading 10 parts).
 * 3) Selection of hex or eng values via radio button.
 * 4) Display values as ASCII characters.
 * 5) String Arrays:
 *    A) Insert zeroes into the calname so that it sorts better.
 *       (1/10 0) becomes (01/10 0).
 *    B) Group character arrays into a single string.
 * 6) Make an option to hide Header and End cals.
 * 7) Export the grid to a csv file.
 */

//using DataGridViewAutoFilter;
using System;
using System.Collections.Generic;
using System.Data;                      // DataTable
using System.Diagnostics;               // Process
using System.Drawing;                   // Color
using System.IO;                        // File
using System.Security;                  // SecurityException
using System.Text.RegularExpressions;   // Regex
using System.Threading;                 // Sleep
using System.Windows.Forms;

namespace CalCompare
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            InitializeComponent();
        }
        
        private DataTable fullTable; // complete cal data table
        private DataTable diffTable; // only the rows with diffs
        
        void MainFormLoad(object sender, EventArgs e)
        {
            // All of the Calplot data that is read in will be stored in this table.
            // Then we will connect it to the DataGridView to make magic happen!
            fullTable = new DataTable();
            
            // Mandatory columns. A column for each part will be added later for each calplot opened.
            DataColumn Calset  = new DataColumn("Calset", typeof(string));
            DataColumn Calname = new DataColumn("Calname", typeof(string));
            DataColumn Units   = new DataColumn("Units", typeof(string));
            DataColumn Diff    = new DataColumn("Diff", typeof(bool));
            
            fullTable.Columns.Add(Calset);
            fullTable.Columns.Add(Calname);
            fullTable.Columns.Add(Units);
            fullTable.Columns.Add(Diff);
        }
        
        void DataGridView1ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            HighlightDiffs(); // highlight any rows with diffs in the grid
        }

        /// <summary>
        /// This function opens calplot files and adds the cals to the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            string lastFile = "";
            int numFiles = 0; // the number of files successfully opened
            
            UpdateProgressBar(0, 0, 0);
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

    	    if (result == DialogResult.OK) // Test result.
    	    {
    	        UpdateStatusLabel("Reading files and loading to the data table...");

                // Read the files
                foreach (string file in openFileDialog1.FileNames) 
                {
                    try
                    {
                        // Extract the part number from the filename (removing leading cp_ and extension).
                        string part = Regex.Replace(Path.GetFileNameWithoutExtension(file), "(^cp_)", String.Empty);
// I bet this can be made faster
                        // Check if a column header already exists.
                        bool match = false;
                        int col;
                        for (col = (fullTable.Columns.IndexOf("Diff") + 1); col < fullTable.Columns.Count; col++) {
                            if (part == fullTable.Columns[col].ColumnName) {
                                match = true;
                                break;
                            }
                        }

                        // If the column doesn't exist in the table we need to add it.
                        if (match == false) {
                            fullTable.Columns.Add(part, typeof(String));
                        }
// End Faster                        
                	    // Open the file and copy the cal lines to the grid.
                        using (StreamReader r = new StreamReader(file))
                        {
                            // Use while != null pattern for loop
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                ParseLine(line, col); // Add the line to the data grid.
                            }
                        }
                        
                        numFiles++;
                        lastFile = file;
                        UpdateProgressBar(0, openFileDialog1.FileNames.Length, numFiles);
                    }
                    catch (SecurityException ex)
                    {
                        // The user lacks appropriate permissions to read files, discover paths, etc.
                        MessageBox.Show("Security error. Please contact your administrator for details.\n\n" +
                            "Error message: " + ex.Message + "\n\n" +
                            "Details (send to Support):\n\n" + ex.StackTrace
                        );
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot read the file: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                }
                // Endforeach (String file in openFileDialog1.FileNames) 

                // Update the status bar with the open file status.
                switch (numFiles) {
                    case 0:
                        UpdateStatusLabel("No files were added to the table.");
                        break;
                    case 1:
                        UpdateStatusLabel("File '" + Path.GetFileName(lastFile) + "' was added to the table.");
                        break;
                    default:
                        UpdateStatusLabel("Multiple files were added to the table.");
                    	break;
                }
                
                UpdateStatusLabel("Table was updated. Setting diff flags....");
                SetDiffFlags(ref fullTable);     // check for differences
                UpdateStatusLabel("Diff flags set. Creating diff table....");
                CreateDiffTable(ref fullTable);  // create the diff table
                UpdateStatusLabel("Diff table created. Displaying grid....");
                DiffOrFilterChanged(sender, e);  // display grid based on diff and filter settings
                UpdateStatusLabel("Grid has been updated.");
    	    }
    	    else
    	    {
                UpdateStatusLabel("File operation was cancelled.");
    	    }
    	    // End if (result == DialogResult.OK) // Test result.
        }
        
        /// <summary>
        /// This function steps the status bar until it reaches the max value then starts over.
        /// </summary>
        void CylonTheProgressBar()
        {
            if (toolStripProgressBar1.Value < toolStripProgressBar1.Maximum) {
                // bump the progress bar
                toolStripProgressBar1.PerformStep();
            }
            else {
                // clear the progress bar
                toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
            }
        }
        
        /// <summary>
        /// This function reads a line from a calplot file and adds it to the data grid.
        ///   Cal:   hmi_nav_sys_caln.RETRANSMIT_HORIZON_TIME = 10.0,0A,10.0,"ms";
        ///   Words: 0                                              ,1 ,2   ,3
        /// </summary>
        /// <param name="s"></param>
        void ParseLine(string s, int col)
        {
            //int row;
       	    string name   = "";
        	string calset = "";

            // Remove the trailing semicolon.
            s = Regex.Replace(s, ";$", String.Empty);

            // Split string on commas.
            string[] fields = s.Split(',');
            
            // Verify that there are 4 fields (separated by 3 commas).
            if (fields.Length == 4)
            {
                // Remove everything after the equal sign in the calibration name.
                string CalNameWhole = Regex.Replace(fields[0], " =.*", String.Empty);

                // Separate the calset (everything up to the first dot) from the name.
            	Match match = Regex.Match(CalNameWhole, @"(.*?)\.(.*)");
            
            	// Here we check the Match instance.
            	if (match.Success)
            	{
            	    // Finally, we get the Group value and display it.
            	    calset = match.Groups[1].Value;
            	    name   = match.Groups[2].Value.Trim();
            	}
            	else 
            	{
            	    return; // there is a problem with the format of the cal name
            	}
                
                // Removing leading and trailing quotes from the Units.
                string units = Regex.Replace(fields[3], "\"", String.Empty);

                // Set the primary key of our main table.
                fullTable.PrimaryKey = new DataColumn[] {fullTable.Columns["Calname"]};
            	
                if (fullTable.Rows.Find(name) == null)
                {
                    // Cal name is not in the table. Add a new row and populate it.
                    DataRow dr = fullTable.NewRow();
                    dr["Calset"] = calset;
                    dr["Calname"] = name;
                    dr["Units"] = units;
                    dr["Diff"] = false;
                    dr[col] = fields[1]; // set the hex value
                    fullTable.Rows.Add(dr);
                }
                else
                {
                    fullTable.Rows.Find(name)[col] = fields[1];
                }
                
                //HideHeaderCals(name);
            }
            else
            {
                // Not the correct amount of commas. There is an error in the line.
                UpdateStatusLabel("Error, the format of the line is wrong: " + s);
            }
        }

        void HideHeaderCals(string s)
        {
            // If the the cal name starts with "HEADER_" don't print it.
            if (!Regex.Match(s, @"^HEADER_").Success)
            {
                // Set up a pattern to test for a cal that is an array (like "NAME(1/33 0)") 
                string pattern = @"(.+)\((\d+)/(\d+) (\d+)\)";
            
                // Is the cal an array?
                if (Regex.Match(s, pattern).Success)
                {
                    // It is a string array. The index needs to be inserted.
                    //Console.WriteLine("cu\t" + Regex.Replace(s, pattern, "$1\t$4") + "\t" + fields[1]);
                }
                else
                {
                    // Not an array. Just display the script formatted line ("cu CAL 0 FF").
                    //Console.WriteLine("cu\t" + s + "\t0\t" + fields[1]);
                }
            }
        }

        /// <summary>
        /// This function creates a table whose rows have diffs. Any row from the incoming table
        /// that don't have diffs are not added to the table.
        /// </summary>
        /// <param name="dt"></param>
        void CreateDiffTable(ref DataTable dt)
        {
            // Set the primary key of our main table.
            dt.PrimaryKey = new DataColumn[] {dt.Columns["Calname"]};
        	
            // Select the rows that have diffs.
            DataRow[] dr = dt.Select("Diff = 'true'");

            diffTable = new DataTable(); // create the diff table

            // Did we find any rows with diffs?
            if (dr.Length > 0)
            {
                // Copy the matching rows to the diff table.
                diffTable = dr.CopyToDataTable();
            }
        }
        
        void UpdateProgressBar(int min, int max, int value)
        {
            toolStripProgressBar1.Minimum = min;
            toolStripProgressBar1.Maximum = max;
            toolStripProgressBar1.Value = value;
            this.Refresh(); // need to refresh to update the progress bar   
        }
        
        void UpdateStatusLabel(string s)
        {
            toolStripStatusLabel1.Text = DateTime.Now + ": " + s;
            this.Refresh(); // need to refresh to update the status label   
        }
        
        /// <summary>
        /// This function is called when the diff checkbox is clicked or the filter textbox text.
        /// This function is very slow. How to speed up?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DiffOrFilterChanged(object sender, EventArgs e)
        {
            // Set which table to use (full or diff) based on the Diff checkbox.
            DataTable dt = (diffCheckBox.Checked) ? diffTable : fullTable;
            
            // Is there anything in the filter textbox.
            if (filterTextBox.Text == String.Empty) 
            {
                // If the filter box is empty display the whole table.
                dataGridView1.DataSource = dt; // render the DataGridView
                dataGridView1.Columns["Diff"].Visible = false; // hide Diff col
                HighlightDiffs();              // highlight rows with diffs in cal values
            }
            else 
            {
                // There is something in the filter box. Let's filter the rows.
                dt.PrimaryKey = new DataColumn[] {dt.Columns["Calname"]};
            	
                // Search the primary key column of the table for whatever is in entered 
                // in the filter text box.
                DataRow[] dr = dt.Select("Calname like '%" + filterTextBox.Text + "%'");
    
                DataTable filtTable = new DataTable();
                
                if (dr.Length > 0)
                {
                    // Copy the matching rows to a temporary table.
                    filtTable = dr.CopyToDataTable();
                    
                    // Send the temporary table to the grid.
                    dataGridView1.DataSource = filtTable; // render the DataGridView
                    dataGridView1.Columns["Diff"].Visible = false; // hide Diff col
                    HighlightDiffs();                     // highlight rows with diffs in cal values
                }
                else 
                {
                    DisplayHeaderRow();
                }
            }
        }

        /// <summary>
        /// I would like to just display the header row here instead of making the grid invisible.
        /// </summary>
        void DisplayHeaderRow()
        {
            dataGridView1.Visible = false;  // make sure it is visible
        }

        /// <summary>
        /// This function sets the diff flag for any rows whose cals are not all the same.
        /// </summary>
        void SetDiffFlags(ref DataTable dt)
        {
            // Bind the data table to the grid.
            //dataGridView1.DataSource = dt;

            // Loop through the table            
            for (int row = 0; row < dt.Rows.Count - 1; row++)
            {
                // Find first valid value.
                int offset = dt.Columns.IndexOf("Diff") + 1;
                string val = "";
                bool valid = false;
                for (int col = (dt.Columns.IndexOf("Diff") + 1); col < dt.Columns.Count; col++)
                {
                    if ((dt.Rows[row][col] != null) &&
                        (dt.Rows[row][col].ToString() != String.Empty))
                    {
                        offset = col;
                        val = dt.Rows[row][col].ToString();
                        valid = true;
                        break;
                    }
                }
                
                // If we found a valid value and we are not at the last column already, check for diffs.
                if ((valid == true) && (offset < (dt.Columns.Count - 1)))
                {
                    for (int col = (offset + 1); col < dt.Columns.Count; col++)
                    {
                        if ((dt.Rows[row][col] != null) &&
                            (dt.Rows[row][col].ToString() != String.Empty) &&
                            (val != dt.Rows[row][col].ToString()))
                        {
                            // Set Diff flag for highlighting the row.
                            dt.Rows[row]["Diff"] = true;
                            break;
                        }
                    }
                }
            }
            // End for (int row = 0; row < calsDataGridView.Rows.Count; row++) 
        }

        void HighlightDiffs()
        {
            // Grid needs to be visible or else the rows don't highlight.
            dataGridView1.Visible = true;  // make sure it is visible
            
    	    for (int row = 0; row < dataGridView1.Rows.Count; row++) 
    	    {
                if ((bool)dataGridView1.Rows[row].Cells["Diff"].Value == true) 
                {
                    dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else 
                {
                    dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        // Tries to open the User's Guide in a web browser.
        void LaunchBrowser(string site)
        {
            bool error = false;
           	string[] browsers = { "chrome.exe", "firefox.exe", "iexplore.exe" };
            
            Process browser = new Process();
            browser.StartInfo.Arguments = site;
            
            // Look for a web browser.
            foreach (string b in browsers) 
            {
                error = false;
                browser.StartInfo.FileName = b;
                try
                {
                    browser.Start();
                }
                catch
                {
                    error = true;
                }
                
                if (error == false) 
                {
                    // The browser was launched without error, break out of the loop.
                    break;
                }
            }

            // Send a message to the status text box if we couldn't open a browser.
            if (error == true)
            {
                UpdateStatusLabel(site);
            }
        }

        // Open the User's Guide in a web browser.
        void UsersGuideToolStripMenuItemClick(object sender, EventArgs e)
        {
            LaunchBrowser("https://gmweb.gm.com/sites/CalSupport/Cal%20Compare");
        }
        
        
        void CalSupportToolsWebSiteToolStripMenuItemClick(object sender, EventArgs e)
        {
            LaunchBrowser("https://gmweb.gm.com/sites/CalSupport");
        }

        void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        ////////////////////////////////////////////////////////////////////////
        // Start of auto filtering stuff.
        ////////////////////////////////////////////////////////////////////////

        // Configures the autogenerated columns, replacing their header
        // cells with AutoFilter header cells. 
        //private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
//        void DataGridViewBindingContextChanged(object sender, EventArgs e)
//        {
//            // Continue only if the data source has been set.
//            if (dataGridView.DataSource == null)
//            {
//                return;
//            }
//
//            // Add the AutoFilter header cell to each column.
//            foreach (DataGridViewColumn col in dataGridView.Columns)
//            {
//                col.HeaderCell = new
//                    DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
//            }
//
//            // Format the OrderTotal column as currency. 
//            dataGridView.Columns["OrderTotal"].DefaultCellStyle.Format = "c";
//
//            // Resize the columns to fit their contents.
//            dataGridView.AutoResizeColumns();
//        }

//        // Displays the drop-down list when the user presses 
//        // ALT+DOWN ARROW or ALT+UP ARROW.
//        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
//            {
//                DataGridViewAutoFilterColumnHeaderCell filterCell =
//                    dataGridView1.CurrentCell.OwningColumn.HeaderCell as
//                    DataGridViewAutoFilterColumnHeaderCell;
//                if (filterCell != null)
//                {
//                    filterCell.ShowDropDownList();
//                    e.Handled = true;
//                }
//            }
//        }
//
//        // Updates the filter status label. 
//        private void dataGridView1_DataBindingComplete(object sender,
//            DataGridViewBindingCompleteEventArgs e)
//        {
//            String filterStatus = DataGridViewAutoFilterColumnHeaderCell
//                .GetFilterStatus(dataGridView1);
//            if (String.IsNullOrEmpty(filterStatus))
//            {
//                showAllLabel.Visible = false;
//                filterStatusLabel.Visible = false;
//            }
//            else
//            {
//                showAllLabel.Visible = true;
//                filterStatusLabel.Visible = true;
//                filterStatusLabel.Text = filterStatus;
//            }
//        }
//
//        // Clears the filter when the user clicks the "Show All" link
//        // or presses ALT+A. 
//        private void showAllLabel_Click(object sender, EventArgs e)
//        {
//            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
//        }

        ////////////////////////////////////////////////////////////////////////
        // End of auto filtering stuff.
        ////////////////////////////////////////////////////////////////////////
    }
}
