/*
 * Created by SharpDevelop.
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
 * Verification:
 *  - Verify these 3 things.
 *  1 If you load one calplot and check the diff checkbox, nothing should display.
 *    Type "hy" into the filter box. Should still have nothing displayed.
 *    Backspace to delete the "y" leaving "h". Should still have nothing displayed.
 *  2 Add calplots one at a time. Then click diff. Crash.
 *  3 LOAD ALL CALPLOTS AND MAKE SURE THE FILTER BOX IS WORKING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 *  
 * To Do:
 *  - String Arrays: Group character arrays into a single string.
 *  - This function takes forever "dataGridView1.AutoResizeColumns()" when the grid has lots of data.
 *
 * Done:
 *  - Set the tab order.
 *  - Added speed increase and update of column visibility based on the tool menu setting.
 *  - Padded array indexes with leading zeros.
 *  - Whenever the filter box changes update the grid from the main table.
 */

using System;
using System.Collections.Generic;       // List
using System.Data;                      // DataTable
using System.Drawing;                   // Color
using System.IO;                        // File, StreamReader
using System.Security;                  // SecurityException
using System.Text.RegularExpressions;   // Regex
using System.Windows.Forms;

using MyFileActions;
using MyHelp;

namespace CalCompare
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        // Calplots contain engineering and hex values. The data may be ascii characters as well.
        // A table is created for each type of data. Which data that is displayed is based
        // on the Radio button selected (Eng, Hex, ASCII).
        private DataTable engTable;     // cal data table of engineering values
        private DataTable hexTable;     // cal data table of hex values
        private DataTable charTable;    // cal data table of ascii character data
        
        private DataTable workingTable; // the working table
        private string filterTextBoxTextOld = String.Empty; // previous value of the filter text box

        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            InitializeComponent();
        }
        
        void MainFormLoad(object sender, EventArgs e)
        {
            // Enable virtual mode.
            //dataGridView1.VirtualMode = true;

            //dataGridView1.BindingContextChanged += new EventHandler(dataGridView1_BindingContextChanged);

            // All of the Calplot data that is read in will be stored in this table.
            // Then we will connect it to the DataGridView to make magic happen!
            engTable     = new DataTable("EngTable");
            hexTable     = new DataTable("HexTable");
            charTable    = new DataTable("CharTable");
            workingTable = new DataTable("WorkingTable");
            
            AddStandardColumns(ref engTable); 
            AddStandardColumns(ref hexTable); 
            AddStandardColumns(ref charTable); 
            AddStandardColumns(ref workingTable); 
            
            BirthdayCheck();
        }

    	void AddStandardColumns(ref DataTable table) 
    	{
        	// Mandatory columns. A column for each part will be added later for each calplot opened.
            table.Columns.Add("Calset", typeof(string));
            table.Columns.Add("Calname", typeof(string));
            table.Columns.Add("Units", typeof(string));
            table.Columns.Add("Diff", typeof(bool));
    	}

    	/// <summary>
        /// Display a message if it is my birthday!
        /// </summary>
        void BirthdayCheck()
        {
        	Match match = Regex.Match(DateTime.Now.ToShortDateString(), @"(4)/(22)/(\d+)");
        	if (match.Success)
        	{
                MessageBox.Show("It's April 22. Make sure to call and wish me a happy birthday!");
        	}
        }
        
        void DataGridView1ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            HighlightDiffsInGrid(); // highlight any rows with diffs in the grid
        }

        /// <summary>
        /// This function opens calplot files and adds the cals to the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            string lastFile = String.Empty;
            int numFiles = 0; // the number of files successfully opened
            
            UpdateProgressBar(0, 0, 0);

            // Show the open file dialog. And if we get a list of files continue.
    	    if (openFileDialog1.ShowDialog() == DialogResult.OK)
    	    {
    	        // Make the grid invisible (it updates faster that way). And hide the Diff column if it exists. 
    	        dataGridView1.Visible = false;
    	        if (dataGridView1.Columns["Diff"] != null)
    	        {
    	           dataGridView1.Columns["Diff"].Visible = false;
    	        }
    	        
                // Read the files
                foreach (string file in openFileDialog1.FileNames) 
                {
                    UpdateStatusLabel("Reading file: " + file);
                    // Extract the part number from the filename (removing leading cp_ and extension).
                    string part = Regex.Replace(Path.GetFileNameWithoutExtension(file), "(^cp_)", String.Empty);
                    AddPartToTables(file, part);
                    numFiles++;
                    lastFile = file;
                    UpdateProgressBar(0, openFileDialog1.FileNames.Length, numFiles);
                }

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
                
                UpdateStatusLabel("Setting diff flags....");
                SetDiffFlags(ref engTable);    // check for differences
                SetDiffFlags(ref hexTable);    // check for differences
                SetDiffFlags(ref charTable);   // check for differences
                
                CopySelectedTableToWorking();
                UpdateStatusLabel("Working table has been updated.");
                
                DiffOrFilterChanged();         // display grid based on diff and filter settings
                UpdateStatusLabel("Grid has been updated.");
    	    }
    	    else
    	    {
                UpdateStatusLabel("File operation was cancelled.");
    	    }
    	    // End if (result == DialogResult.OK) // Test result.
        }
        
        void AddPartToTables(string file, string part)
        {
            AddColToTables(part);
            
            // Open the file and copy the cal lines to the grid.
            using (StreamReader r = new StreamReader(file))
            {
                // Use while != null pattern for loop
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    ParseLine(line, part); // Add the line to the data grid.
                }
            }
        }

        //void AddColToTables(string part, ref List<int> cols)
        void AddColToTables(string part)
        {
            // Put the data tables in a list so that we can loop through them.
            List<DataTable> tables = new List<DataTable>();
            tables.Add(engTable);
            tables.Add(hexTable);
            tables.Add(charTable);
            
            // Loop through the tables.
            foreach (DataTable table in tables)
            {
                // Check if the part already exists in a column.
                // If the column doesn't exist in the table we need to add it.
                if (table.Columns.Contains(part) == false)
                {
                    table.Columns.Add(part, typeof(String));
                }
            }
    	}
        
        /// <summary>
        /// This function steps the status bar until it reaches the max value then starts over.
        /// </summary>
        void CylonTheProgressBar()
        {
            if (toolStripProgressBar1.Value < toolStripProgressBar1.Maximum) 
            {
                // bump the progress bar
                toolStripProgressBar1.PerformStep();
            }
            else 
            {
                // clear the progress bar
                toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
            }
        }
        
        /// <summary>
        /// This function reads a line from a calplot file and adds it to the data grid.
        ///   Cal:   hmi_nav_sys_caln.RETRANSMIT_HORIZON_TIME = 10.0,0A,10.0,"ms";
        ///   data:                                             0    ,1 ,2   ,3
        /// </summary>
        /// <param name="s"></param>
        //void ParseLine(string s, List<int> cols)
        void ParseLine(string line, string part)
        {
       	    bool error = true;
            string calname = String.Empty;
        	string calset  = String.Empty;

            // Remove the trailing semicolon.
            line = Regex.Replace(line, ";$", String.Empty);

            // Split string on "=".
            string[] fields = line.Split('=');
            
            // Verify that there are 2 fields.
            if (fields.Length == 2)
            {
                // Separate the calset (everything up to the first dot) from the name.
                Match match = Regex.Match(fields[0], @"(.*?)\.(.*)");
            
            	// Here we check the Match instance.
            	if (match.Success)
            	{
            	    // Finally, we get the Group value and display it.
            	    calset  = match.Groups[1].Value.Trim();
            	    calname = match.Groups[2].Value.Trim();

                    // if the calname is an array, pad some zeros so that it sorts better.
                    calname = CalNameReformat(calname);
                    
            	    // Split string on commas. 
            	    // "fields[1]" contains everything after the equal sign.
            	    // eng value, hex value, eng value, units
                    string[] data = fields[1].Split(',');
                    
                    // Verify that there are 4 fields (separated by 3 commas).
                    if (data.Length == 4)
                    {
                        string hexVal = data[1];
                        string engVal = data[2];
                        string units  = data[3].Trim(' ', '"'); // trim spaces and qoutes;
                        
                        AddRowToTable(ref  engTable,  part, calset, calname, units, engVal);
                        AddRowToTable(ref  hexTable,  part, calset, calname, units, hexVal);
                        
                        if (IsUnitsChar(units))
                        {
                            // Convert the number expressed in base-16 to an integer.
                            int val = Convert.ToInt32(hexVal, 16);
                            string chrVal = Char.ConvertFromUtf32(val); // set the cal value
                            AddRowToTable(ref  charTable, part, calset, calname, units, chrVal);
                        }
                        
                        error = false;
                    }
            	}
            }
            
            if (error)
            {
                UpdateStatusLabel("Error - The format of the line is wrong: " + line);
            }
        }

        void AddRowToTable(ref DataTable table, string col, string set, string name, string units, string val)
        {
            // Set the primary key of our main table.
            table.PrimaryKey = new DataColumn[] {table.Columns["Calname"]};
        	
            // Verify that the row does not exist already.
            if (table.Rows.Find(name) == null)
            {
                // Cal name is not in the table. Add a new row and populate it.
                DataRow dr = table.NewRow();
                dr["Calset"]  = set;
                dr["Calname"] = name;
                dr["Units"]   = units;
                dr["Diff"]    = false;
                
                dr[col] = val;
                table.Rows.Add(dr);
            }
            else
            {
                table.Rows.Find(name)[col] = val;
            }
        }
        
        /// <summary>
        /// This function checks the Units to see if it is a character.
        /// Characters, UTF-16, UTF-8, ASCII.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool IsUnitsChar(string s)
        {
            bool rv = false;

            if (Regex.IsMatch(s, "character", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(s, "utf-",      RegexOptions.IgnoreCase) ||
                Regex.IsMatch(s, "ascii",     RegexOptions.IgnoreCase))
            {
                rv = true;
            }
            
            return rv;
        }
        
        /// <summary>
        /// This function reformats the calname if it is an array so that it will sort better.
        /// </summary>
        /// <param name="name"></param>
        string CalNameReformat(string name)
        {
            string rv = "";
            
    	    // Check for opening and closing parentheses.
            // Tuner_cal.fm_qual_cal(1/4 0,1/4 0,1/15 0)
            string pattern = @"(.+)\((.+)\)";
            Match match = Regex.Match(name, pattern);
            if (match.Success)
            {
                rv = match.Groups[1].Value + "(";
                string array   = match.Groups[2].Value;

                // Is it a multi-dimensional array "1/4 0,1/4 0,1/15 0"?
                // Try splitting string on commas.
                string[] fields = array.Split(',');
                pattern = @"(\d+)/(\d+)(.+)";
                int i = 0;
                foreach (string field in fields)
                {
                    // Insert a comma before each field except the first. 
                    if (i > 0)
                    {
                        rv = rv + ",";
                    }
                    
                    match = Regex.Match(field, pattern);
                    if (match.Success)
                    {
                        rv = rv + PadArrayIndex(field);
                    }
                    else
                    {
                        rv = rv + field;
                    }
                    i++;
                }
                rv = rv + ")";
            }
            else
            {
                rv = name;
            }

            return rv;
        }
        
        // paren num formardSlash num space num paren
        // Got This:  1/33 0
        // Want This: 01/33 0
        string PadArrayIndex(string name)
        {
            string rv;
            
            string pattern = @"(\d+)/(\d+)(.+)";
            Match match = Regex.Match(name, pattern);
            if (match.Success)
            {
                int index = Convert.ToInt32(match.Groups[1].Value);
                int size = Convert.ToInt32(match.Groups[2].Value);
                int numChars = (int)Math.Log10((double)size) + 1;
                string format = "D" + numChars.ToString();
        	    rv = index.ToString(format) + "/" + match.Groups[2].Value + match.Groups[3].Value;
        	}
        	else
        	{
        	    rv = name;
        	}
            
        	return rv;
        }
        
        /// <summary>
        /// This function creates a table based on the selection passed in.
        /// The selection is used to filter the rows. Only the matching rows
        /// are loaded back into the working table.
        /// </summary>
        /// <param name="select">A database formatted selection string.
        /// For example, "Diff = 'true'" or "Calname like '%" + filterTextBox.Text + "%'".</param>
        void CreateTable(ref DataTable table, string select)
        {
            // Set the primary key of our table.
            table.PrimaryKey = new DataColumn[] {table.Columns["Calname"]};
        	
            // Select the rows that have diffs.
            DataRow[] dr = table.Select(select);

            // Did we find any diffs in the table?
            if (dr.Length > 0)
            {
                // Diffs found. Replace the current table with the diff rows just found.
                DataTable dt = new DataTable("WorkingTable");
                dt = workingTable.Clone();
                
                dt = dr.CopyToDataTable();
                workingTable.Rows.Clear();
                workingTable = dt.Copy();
            }
            else
            {
                // No diffs. Just clear the rows from the current table.
                workingTable.Rows.Clear();
            }
        }
        
        // This function is called when the Diff checkbox is clicked (toggled).
        void DiffCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            DiffOrFilterChanged();
        }
        
        // This function is called when the user types a character into the filter textbox.
        // We are giving a slight delay for the user to stop typing before the filtering begins.
        void FilterTextBoxTextChanged(object sender, EventArgs e)
        {
            filterTimer.Stop();  // reset the timer
            filterTimer.Start(); // now start it
        }
        
        // This function is called when the filter timer expires.
        // The timer is restarted when the user types into the filter box.
        void FilterTimerTick(object sender, EventArgs e)
        {
            filterTimer.Stop();
            DiffOrFilterChanged();
        }
        
        /// <summary>
        /// This function is called when the Eng radio buttons is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void engRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (engRadioButton.Checked) 
            {
                workingTable = engTable.Copy();
                workingTable.TableName = engTable.TableName;
                DiffOrFilterChanged();
            }
        }

        void hexRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (hexRadioButton.Checked) 
            {
                workingTable = hexTable.Copy();
                workingTable.TableName = hexTable.TableName;
                DiffOrFilterChanged();
            }
        }

        void charRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (charRadioButton.Checked) 
            {
                workingTable = charTable.Copy();
                workingTable.TableName = charTable.TableName;
                DiffOrFilterChanged();
            }
        }

        /// <summary>
        /// This function is called when the diff checkbox is clicked, the filter textbox text changes,
        /// or one of the radio buttons is clicked.
        /// 
        /// </summary>
        void DiffOrFilterChanged()
        {
            // If filter
            if ((filterTextBox.Text != String.Empty) || (filterTextBoxTextOld != String.Empty))
            {
                UpdateStatusLabel("Filtering Calname on '" + filterTextBox.Text + "', please wait...");
                
                // Filter the selected table on the contents of the filter box.
                // And copy the results to the working table.
                DataTable dt = GetSelectedTable();
                CreateTable(ref dt, "Calname like '%" + filterTextBox.Text + "%'");
                
                filterTextBoxTextOld = filterTextBox.Text;

                UpdateStatusLabel("Displaying rows that contain '" + filterTextBox.Text + "' in the Calname.");
            }

            // If the Diff checkbox is checked, only display the rows with diffs.
            if (diffCheckBox.Checked)
            {
                CreateTable(ref workingTable, "Diff = 'true'");
            }
            
            UpdateGrid();
        }
        
        /// <summary>
        /// This function returns the corresponding table for the radio button selected.
        /// </summary>
        /// <returns></returns>
        DataTable GetSelectedTable()
        {
            if (engRadioButton.Checked) 
            {
                return engTable;    
            }
            else if (hexRadioButton.Checked) 
            {
                return hexTable;
            }
            else if (charRadioButton.Checked) 
            {
                return charTable;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// This function copies the selected (via radio button) table to the 
        /// working table.
        /// </summary>
        void CopySelectedTableToWorking()
        {
            if (engRadioButton.Checked)
            {
                workingTable = engTable.Copy();
            }
            else if (hexRadioButton.Checked)
            {
                workingTable = hexTable.Copy();
            }
            else if (charRadioButton.Checked) 
            {
                workingTable = charTable.Copy();
            }
            else
            {
                // No Action. Shouldn't get here!
            }
        }

        void UpdateGrid()
        {
            BindingSource dataSource = new BindingSource(workingTable, null);
            dataGridView1.DataSource = dataSource;
            UpdateColumnVisibility();
            SetDiffFlags(ref workingTable);
            HighlightDiffsInGrid();              // highlight rows with diffs in cal values
            AutoResizeFirstThreeColumns();
        }
        
        // This function resizes the Calset, Calname, and Units columns.
        // Function dataGridView1.AutoResizeColumns() can take a really long time
        // if you have lots of calplots loaded.
        void AutoResizeFirstThreeColumns()
        {
            if (dataGridView1.Columns["Calset"] != null)
                dataGridView1.Columns["Calset"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dataGridView1.Columns["Calname"] != null)
                dataGridView1.Columns["Calname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dataGridView1.Columns["Units"] != null)
                dataGridView1.Columns["Units"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        
        /// <summary>
        /// This function will hide any columns that have no value in them.
        /// I think the Remove takes a long time.
        /// </summary>
        /// <param name="table"></param>
        void HideEmptyColumns(ref DataTable table)
        {
            // Is there anything in the table?
            if (table.Rows.Count > 0)
            {
                dataGridView1.Visible = false; // hide the grid to speed things up
                UpdateStatusLabel("Hiding empty columns...");
                
                int lastIndex = table.Columns.Count - 1;
                int max = lastIndex - table.Columns.IndexOf("Diff") - 1;
                
                for (int i = lastIndex; i > table.Columns.IndexOf("Diff"); i--)
                {
                    int val = lastIndex - i;
                    string colName = table.Columns[i].ColumnName;
                    string expression = "COUNT([" + colName + "])";
                    string filter     = "[" + colName + "] <> ''";
    
                    UpdateProgressBar(0, max, val);
                    UpdateStatusLabel("Checking column " + colName + "...");
                    
                    // If the column is empty hide it.
                    int count = (int)table.Compute(expression, filter);
                    if (count == 0)
                    {
                        UpdateStatusLabel("Hiding column " + colName + "...");
                        dataGridView1.Columns[colName].Visible = false;
                    }
                    else
                    {
                        dataGridView1.Columns[colName].Visible = true;
                    }
                }
                // End for (int i = lastIndex; i > table.Columns.IndexOf("Diff"); i--)
                
                UpdateStatusLabel("Empty columns have been hidden.");
                dataGridView1.Visible = true;
            }
            // End if (table.Rows.Count > 0) 
        }
        
        /// <summary>
        /// Make all data grid view columns visible.
        /// </summary>
        void UnhideAllColumns()
        {
            for (int i = workingTable.Columns.IndexOf("Diff") + 1; i < workingTable.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Visible = true;
            }
        }
        
        /// <summary>
        /// This function sets the diff flag for any rows whose cals are not all the same.
        /// </summary>
        void SetDiffFlags(ref DataTable dt)
        {
            // Loop through the table            
            for (int row = 0; row < dt.Rows.Count - 1; row++)
            {
                dt.Rows[row]["Diff"] = false; // init the diff flag to false
                
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

        void HighlightDiffsInGrid()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                UpdateStatusLabel("Highlighting diffs...");
    
                // Grid needs to be visible or else the rows don't highlight.
                dataGridView1.Columns["Diff"].Visible = false;
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
                
        	    UpdateStatusLabel("Diffs highlighted.");
            }
        }

        /// <summary>
        /// This function clears all of the rows from the working and master tables. 
        /// </summary>
        void ClearTheGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.RowCount = 0;
            dataGridView1.ColumnCount = 0;
            
            ClearTable(engTable);
            ClearTable(hexTable);
            ClearTable(charTable);
            ClearTable(workingTable);
        }

        private void ClearTable(DataTable table)
        {
            if (table != null) 
            {
                try
                {
                    table.Clear();

                    // Remove all part columns.
                    for (int i = (table.Columns.Count - 1); i > table.Columns.IndexOf("Diff"); i--)
                    {
                        table.Columns.RemoveAt(i);
                    }
                }
                catch (DataException e)
                {
                    // Process exception and return.
                    UpdateStatusLabel("Exception of type " + e.GetType().ToString() + " occured");
                }
            }
        }

        #region status
        
        void UpdateProgressBar(int min, int max, int val)
        {
            toolStripProgressBar1.Minimum = min;
            toolStripProgressBar1.Maximum = max;
            toolStripProgressBar1.Value = val;
            this.Refresh(); // need to refresh to update the progress bar   
        }
        
        void UpdateStatusLabel(string s)
        {
            toolStripStatusLabel1.Text = DateTime.Now + ": " + s;
            this.Refresh(); // need to refresh to update the status label   
        }
        
        #endregion status
        
        #region export - import

        /// <summary>
        /// Export as comma delimited text file (.csv).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExportAsCsvClick(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount < 1) 
            {
                ExportNullGridMessage();
            }
            else
            {
                csvSaveFileDialog.ShowDialog(); // Show the dialog.
            }
        }
        
        void ExportAsCsvFileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Export.AsTextDelimited(ref dataGridView1, GetTableNameFromButton(), csvSaveFileDialog.FileName, ",");
            
        }
        
        /// <summary>
        /// Export as tab delimited text file (.csv).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExportAsTabDelimitedClick(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount < 1) 
            {
                ExportNullGridMessage();
            }
            else
            {
                tabSaveFileDialog.ShowDialog(); // Show the dialog.
            }
        }
        
        void ExportAsTabFileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Export.AsTextDelimited(ref dataGridView1, GetTableNameFromButton(), tabSaveFileDialog.FileName, "\t");
        }
        
        /// <summary>
        /// Export as XML formatted file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExportAsXmlClick(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0) 
            {
                xmlSaveFileDialog.ShowDialog(); // Show the dialog.
            }
            else
            {
                ExportNullGridMessage();
            }
        }
        
        void ExportAsXmlFileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
    	    workingTable.TableName = GetTableNameFromButton();
    	    workingTable.WriteXml(xmlSaveFileDialog.FileName, XmlWriteMode.WriteSchema);
        }
        
        void ExportNullGridMessage()
        {
            MessageBox.Show("What are you trying to Export? Air? Load something to the grid please!");
        }
        
        void ImportClick(object sender, EventArgs e)
        {
            DialogResult result = importOpenFileDialog.ShowDialog(); // Show the dialog.

    	    if (result == DialogResult.OK) // Test result.
    	    {
    	        // Determine the file type based on the files extension.
    	        string ext = Path.GetExtension(importOpenFileDialog.FileName);
    	        if (ext == ".csv")
    	        {
    	            ImportFromTextDelimited(importOpenFileDialog.FileName, ',');
    	        }
    	        else if (ext == ".txt") 
    	        {
                    ImportFromTextDelimited(importOpenFileDialog.FileName, '\t');
    	        }
    	        else if (ext == ".xml") 
    	        {
                    ImportFromXml();
    	        }
    	        else
    	        {
    	            // Invalid extension
    	            UpdateStatusLabel("Invalid extention.");
    	        }
    	    }
    	    else
    	    {
    	        UpdateStatusLabel("Import cancelled.");
    	    }
        }
        
        /// <summary>
        /// This function imports data from a text delimited file.
        /// This could be better. 
        /// Read all the data into a temporary table.
        /// Then merge the tables like the XML import.
        /// </summary>
        /// <param name="filename">The name of the file to import.</param>
        /// <param name="delimiter">The text delimiter (such as a comma "," or 
        /// tab "\t" that the rows in the file will be split on.</param>
        void ImportFromTextDelimited(string filename, char delimiter)
        {
            string tableName = String.Empty;
            DataTable dt;
            
            using (StreamReader sr = new StreamReader(filename))
            {
                // Read table name
                tableName = sr.ReadLine();
                
                // Get the selected table
                dt = GetTableFromName(tableName);
                
                // Does the table exist?
                if (dt == null)
                {
                    UpdateStatusLabel("Import failed. I only import files that I exported.");
                }
                else
                {
                    SetRadioButton(tableName); // set radio button based on the table name
                    
                    // Read column headers
                    char[] delimiters = new char[] { delimiter };
                    string[] headerRow = sr.ReadLine().Split(delimiters);
                    foreach (string col in headerRow) 
                    {
                        // Add the column if it doesn't exist.
                        if (dt.Columns[col] == null)
                        {
                            if (col.Equals("Diff"))
                            {
                                dt.Columns.Add(col, typeof(bool));
                            }
                            else
                            {
                                dt.Columns.Add(col, typeof(string));
                            }
                        }
                    }
    
                    // Reading the content (rows of data).
                    while (sr.Peek() >= 0)
                    {
                        string[] line = sr.ReadLine().Split(delimiters);
                        for (int i = 0; i < line.Length; i++) 
                        {
                            AddRowToTable(ref dt, headerRow[i], line[0], line[1], line[2], line[i]);
                        }
                    }

                    CopySelectedTableToWorking();
                    UpdateGrid();
                    UpdateStatusLabel("Data was imported.");
                }
                // End if (dt == null)
            }
            // End using (StreamReader sr = new StreamReader(filename))
        }
        
        void CopyTableFromName(ref DataTable table, string name)
        {
            if (name.Equals("EngTable"))
            {
                engTable = table.Copy();
            }
            else if (name.Equals("HexTable"))
            {
                hexTable = table.Copy();
            }
            else if (name.Equals("CharTable")) 
            {
                charTable = table.Copy();
            }
            else
            {
                // No Action
            }
        }
        
        DataTable GetTableFromName(string name)
        {
            DataTable dt;
            
            if (name.Equals("EngTable"))
            {
                dt = engTable;    
            }
            else if (name.Equals("HexTable"))
            {
                dt = hexTable;
            }
            else if (name.Equals("CharTable")) 
            {
                dt = charTable;
            }
            else
            {
                dt = null;
            }
            
            return dt;
        }
        
        void SetRadioButton(string name)
        {
            if (name.Equals("EngTable"))
            {
                engRadioButton.Checked = true;
            }
            else if (name.Equals("HexTable"))
            {
                hexRadioButton.Checked = true;
            }
            else if (name.Equals("CharTable")) 
            {
                charRadioButton.Checked = true;
            }
            else
            {
                engRadioButton.Checked = false;
                hexRadioButton.Checked = false;
                charRadioButton.Checked = false;
            }
        }
            
        /// <summary>
        /// This function returns that table name based on the radio button selected.
        /// </summary>
        /// <param name="void"></param>
        /// <returns></returns>
        string GetTableNameFromButton()
        {
            string rv;
            
            if (engRadioButton.Checked)
            {
                rv = "EngTable";
            }
            else if (hexRadioButton.Checked)
            {
                rv = "HexTable";
            }
            else if (charRadioButton.Checked)
            {
                rv = "CharTable";
            }
            else
            {
                rv = null; // No radio button selected.
            }
            
            return rv;
        }
        
        bool IsTableNameValid(string name)
        {
            if (name.Equals("EngTable") || name.Equals("HexTable") || name.Equals("CharTable"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ImportFromXml()
        {
            // Read the XML file into a temporary table.
            DataTable table1 = new DataTable();
            table1.ReadXml(importOpenFileDialog.FileName);
            
            
            // Merge table1 with the table of the same name.
            if (IsTableNameValid(table1.TableName))
            {
                SetRadioButton(table1.TableName);
                DataTable table2 = GetTableFromName(table1.TableName);
                table2.Merge(table1, false); // overwrite existing data
                CopySelectedTableToWorking();
                DiffOrFilterChanged();  // display grid based on diff and filter settings
                UpdateStatusLabel("XML data was imported.");
            }
            else
            {
                UpdateStatusLabel("Bad table name in XML file.");
            }
        }

        #endregion export - import
        
        #region tools: ClearTheGridToolStripMenuItemClick
        void ClearTheGridToolStripMenuItemClick(object sender, EventArgs e)
        {
            ClearTheGrid();
        }
        
        void AutoResizeColumnsToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                UpdateStatusLabel("Resizing the columns. This can take awhile. Please be patient...");
                dataGridView1.AutoResizeColumns(); // Resize the columns to fit their contents.
                UpdateStatusLabel("Columns were auto-resized.");
            }
        }
        
        void HideEmptyColumnsToolStripMenuItemClick(object sender, EventArgs e)
        {
            UpdateColumnVisibility();
        }
        
        void UpdateColumnVisibility()
        {
            if (hideEmptyColumnsToolStripMenuItem.Checked) 
            {
                HideEmptyColumns(ref workingTable);
            }
            else
            {
                UnhideAllColumns();
            }
        }
        
        #endregion tools
        
        #region help: UsersGuideToolStripMenuItemClick, CalSupportToolsWebSiteToolStripMenuItemClick, AboutToolStripMenuItemClick

        // Open the User's Guide in a web browser.
        void UsersGuideToolStripMenuItemClick(object sender, EventArgs e)
        {
            string site = "https://gmweb.gm.com/sites/CalSupport/Cal%20Compare";
            if (Browser.Launch(site))
            {
                UpdateStatusLabel("Unable to launch '" + site + "'.");
            }
        }
        
        void CalSupportToolsWebSiteToolStripMenuItemClick(object sender, EventArgs e)
        {
            string site = "https://gmweb.gm.com/sites/CalSupport";
            if (Browser.Launch(site))
            {
                UpdateStatusLabel("Unable to launch '" + site + "'.");
            }
        }

        void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        #endregion help

        #region auto-filter: dataGridView1_BindingContextChanged, dataGridView1_KeyDown, dataGridView1_DataBindingComplete, showAllLabel_Click

//        // Configures the autogenerated columns, replacing their header
//        // cells with AutoFilter header cells. 
//        private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
//        {
//            // Continue only if the data source has been set.
//            if (dataGridView1.DataSource == null)
//            {
//                return;
//            }
//
//            // Add the AutoFilter header cell to each column.
//            //foreach (DataGridViewColumn col in dataGridView1.Columns)
//            //{
//            //    col.HeaderCell = new
//            //        DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
//            //}
//            DataGridViewColumn col = dataGridView1.Columns["Calset"];
//            col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
//
//            // Format the OrderTotal column as currency. 
//            //dataGridView1.Columns["OrderTotal"].DefaultCellStyle.Format = "c";
//
//            //dataGridView1.AutoResizeColumns(); // Resize the columns to fit their contents.
//            AutoResizeFirstThreeColumns();
//        }
//
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
//        /// <summary>
//        /// Updates the filter status label.
//        /// This function gets call when the autofilter header is clicked.
//        /// This function gets called whenever a row is added to the grid.
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
//        {
//            String filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dataGridView1);
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
//            
//            if (gridUpdateFinished)
//            {
//                // This doesn't hide the empty columns because the working table still has the column
//                // even when the auto-filter hides the rows that contain data.
//                //HideEmptyColumns(ref workingTable);
//                
//                HighlightDiffsInGrid();
//            }
//        }
//
//        // Clears the filter when the user clicks the "Show All" link
//        // or presses ALT+A. 
//        private void showAllLabel_Click(object sender, EventArgs e)
//        {
//            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
//        }

        #endregion auto-filter
    }
}
