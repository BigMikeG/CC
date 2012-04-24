/*
 * Created by SharpDevelop.
 * User: GZNDQ9
 * Date: 4/23/2012
 * Time: 9:46 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;               // Process

namespace MyHelp
{
    /// <summary>
    /// Description of Browser.
    /// </summary>
    public class Browser
    {
        public Browser()
        {
        }
        
        /// <summary>
        /// This function tries to open the User's Guide in a web browser.
        /// Returns true if unsuccesful.
        /// </summary>
        /// <param name="site"></param>
        static public bool Launch(string site)
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

            return error;
        }
    }
}
