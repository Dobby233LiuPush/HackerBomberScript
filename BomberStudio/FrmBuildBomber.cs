﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberStudio
{
    public partial class FrmBuildBomber : Form
    {
        string script = "";
        public FrmBuildBomber(string script)
        {
            InitializeComponent();
            this.script = script;
        }

        private void FrmBuildBomber_Load(object sender, EventArgs e)
        {
            cmbType.SelectedIndex = 0;
        }

        private void ValThreadCount_Scroll(object sender, EventArgs e)
        {

        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            int i = decimal.ToInt32(numThreadCount.Value);
            if (i < 16)
            {
                lblComment.Text = i + "线程，小火慢炖。";
            }
            else if (i < 50)
            {
                lblComment.Text = i + "线程，非常适合轰炸。";
            }
            else if (i < 72)
            {
                lblComment.Text = i + "线程，大力轰炸。";
            }
            else if (i < 128)
            {
                lblComment.Text = i + "线程，电脑可能吃不消。";
            }
            else if (i < 256)
            {
                lblComment.Text = i + "线程，需要非常高配的电脑。";
            }
            else if (i <= 1024)
            {
                lblComment.Text = i + "线程，运营商可能会把电话打到你家里去。";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (chkUseRGB.Checked) {
                script="#开启RGB\r\n" + script;
            }
            script = "#线程数 "+numThreadCount.Value+"\r\n" + script;
			if (cmbType.SelectedIndex == 2)
			{
				script = "#复制这段内容，粘贴到安卓轰炸机里，即可开始轰炸\r\n" + script;
				new FrmDisplayText(script).Show();
				Close();
			
			} else if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
				string root = folderBrowserDialog1.SelectedPath;
				root = Path.Combine(root, "轰炸机");
				if (!Directory.Exists(root)) {
					Directory.CreateDirectory(root);
				} else {
					DialogResult willContinue = MessageBox.Show("文件夹 " + root + "已存在，覆盖吗？", this.Text, MessageBoxButtons.YesNo);
					if (willContinue != DialogResult.Yes){
						return;
					}
				}
				if (cmbType.SelectedIndex == 0) {
					// CopyDirectory("template\\windows", root, true);
					/** files
						HackerBomber.dll
						ScriptInterpreter.dll
						HackerBomberApplication.exe -> 轰炸机.exe
						gonna copy to root.
					**/
					File.Copy("HackerBomber.dll", Path.Combine(root, "HackerBomber.dll"), true);
					File.Copy("ScriptInterpreter.dll", Path.Combine(root, "ScriptInterpreter.dll"), true);
					File.Copy("HackerBomberApplication.exe", Path.Combine(root, "轰炸机.exe"), true);
					File.WriteAllText(Path.Combine(root, "script.hbs"), script);
					Process.Start("explorer", "\""+root+"\"");
				}

				if (cmbType.SelectedIndex == 1)
				{
					// CopyDirectory("template\\linux", root, true);
					/** files
						HackerBomber.dll
						ScriptInterpreter.dll
						HackerBomberCrossPlatform.dll
						gonna copy to root.
					**/
					File.Copy("HackerBomber.dll", Path.Combine(root, "HackerBomber.dll"), true);
					File.Copy("ScriptInterpreter.dll", Path.Combine(root, "ScriptInterpreter.dll"), true);
					File.Copy("HackerBomberCrossPlatform.dll", Path.Combine(root, "HackerBomberCrossPlatform.dll"), true);
					File.WriteAllText(Path.Combine(root, "script.hbs"), script);
					File.WriteAllText(Path.Combine(root, "开启轰炸机.sh"), "mono HackerBomberCrossPlatform.dll");
					Process.Start("explorer", "\"" + root + "\"");
				}
				
				Close();
			}
        }

        private static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }


    }
}
