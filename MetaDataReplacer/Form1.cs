using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MetaDataReplacer
{
    public partial class Form1 : Form
    {
        int counter;
        int testcounter;
        string filecnt = "";
        string filename = @"c:\stuff\dump.txt";
        StreamWriter sw;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowDialog();
            txtDir.Text = folderBrowserDialog1.SelectedPath.ToString();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (txtDir.Text == "")
            {
                string message = "Please specify a directory.";
                MessageBox.Show(message, "Specify Directory", MessageBoxButtons.OK);
            }
      

            else
            {

                counter = 0;
                FileInfo[] Files = AllFiles();
                string FileCount = Files.Length.ToString();
                filecnt = FileCount;
                string message = "There are " + FileCount + " Files.  Press OK to start.";
                MessageBox.Show(message, "File Number", MessageBoxButtons.OK);
                string filepath;
               

                foreach (FileInfo testFile in Files)
                {

                    string myAttribute = cbAttribute.SelectedItem.ToString();
                    filepath = testFile.FullName.ToString();
                    bool myTest = TestForAttribute(filepath, myAttribute);

                    if (myTest == false)
                    {
                        AddAttribute(filepath);        

                    }
                    else
                    {
                       UpdateAttribute(filepath);
                    }
                    

                }


            }

            string endmessage = "Files found: " + filecnt + ".  Files Processed: " + counter.ToString() + ".";
            MessageBox.Show(endmessage, "Done", MessageBoxButtons.OK);
        }

        public void UpdateAttribute(string path)
        {

            testcounter = counter;
            var txtLines = File.ReadAllLines(path);
            List<string> TestList = new List<string>(txtLines);
            string attribute = cbAttribute.SelectedItem.ToString();

            //const string quote = @":";
            string replace = "";
            string value = txtValue.Text.ToString();
            //string author = txtAuthor.Text.ToString();

            replace = attribute + " " + value;
            //author = "ms.author: " + author;

         

                for (int i = 0; i < TestList.Count; i++)
                {

                    if (TestList[i].Contains(attribute))
                    {

                        TestList[i] = replace;
                        counter = counter + 1;
                    }


                }

            


            txtLines = TestList.ToArray();
            File.WriteAllLines(path, txtLines);
            if (testcounter == counter)
            {

                string filenameandloc = TestList[2].ToString();

                sw.WriteLine(path);

            }

        }

        public FileInfo[] AllFiles()
        {
            DirectoryInfo dinfo = new DirectoryInfo(txtDir.Text.ToString());
            FileInfo[] Files = dinfo.GetFiles("*.md", SearchOption.AllDirectories);
            return Files;

        }

        bool TestForAttribute(string path, string attrib)
        {

            bool test = false;
            var txtLines = File.ReadAllLines(path);
            List<string> TestList = new List<string>(txtLines);
            foreach (var line in TestList)   //Loop through list to find the title: line

            {
                string tmatch = line.ToString();   //if line contains title: then store this value in a string to use as the insertion point
                tmatch = tmatch.ToLower();
                if (tmatch.Contains(attrib))
                {
                    test = true;
                    return test;
                }

            }

            return test;

        }

        public void AddAttribute(string path)
        {
            string attribute = cbAttribute.SelectedItem.ToString();
            string replace = "";
            string value = txtValue.Text.ToString();
            replace = attribute + " " + value;
            
            
            string insertpoint = "ms.author:";

        
            var endTag = String.Format("[/{0}]", insertpoint);
            var txtLines = File.ReadAllLines(path);

            List<string> TestList = new List<string>(txtLines);

            foreach (var titlefind in TestList)   //Loop through list to find the title: line

            {


                string tmatch = titlefind.ToString();   //if line contains title: then store this value in a string to use as the insertion point
                if (tmatch.Contains("ms.author:"))
                {
                    insertpoint = tmatch.ToString();
                }

            }

            TestList.Insert(TestList.IndexOf(insertpoint), replace);  // insert guid line into list
            txtLines = TestList.ToArray();                               // convert list back to string[]

            //txtLines.Insert(txtLines.IndexOf(insertpoint), linetoadd);
            File.WriteAllLines(path, txtLines);
            counter = counter + 1;
        }

        private void btnAsset_Click(object sender, EventArgs e)
        {

            if (txtDir.Text == "")
            {
                string message = "Please specify a directory.";
                MessageBox.Show(message, "Specify Directory", MessageBoxButtons.OK);
            }


            else
            {

                counter = 0;
                FileInfo[] Files = AllFiles();
                string FileCount = Files.Length.ToString();
                filecnt = FileCount;
                string message = "There are " + FileCount + " Files.  Press OK to start.";
                MessageBox.Show(message, "File Number", MessageBoxButtons.OK);
                string filepath;

                foreach (FileInfo testFile in Files)
                {

                    string myAttribute = cbAttribute.SelectedItem.ToString();
                    filepath = testFile.FullName.ToString();
                    bool myTest = TestForAttribute(filepath, myAttribute);

                    if (myTest == false)
                    {
                        

                    }
                    else
                    {
                        DeleteAsset(filepath);
                    }


                }

                string endmessage = "Files found: " + filecnt + ".  Files Processed: " + counter.ToString() + ".";
                MessageBox.Show(endmessage, "Done", MessageBoxButtons.OK);


            }



            }

        public void DeleteAsset(string path)
        {

            testcounter = counter;
            var txtLines = File.ReadAllLines(path);
            List<string> TestList = new List<string>(txtLines);
            string attribute = cbAttribute.SelectedItem.ToString();

            //const string quote = @":";
            string replace = "";
            string value = txtValue.Text.ToString();
            //string author = txtAuthor.Text.ToString();

            // replace = attribute + " " + value;
            //author = "ms.author: " + author;



            for (int i = 0; i < TestList.Count; i++)
            {

                if (TestList[i].Contains(attribute))
                {

                    TestList[i] = replace;
                    counter = counter + 1;
                }


            }




            txtLines = TestList.ToArray();
            File.WriteAllLines(path, txtLines);
            if (testcounter == counter)
            {

                string filenameandloc = TestList[2].ToString();

                sw.WriteLine(path);

            }

        }
    }
}