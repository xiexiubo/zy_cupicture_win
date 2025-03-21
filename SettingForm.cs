using System;
using System.Windows.Forms;
namespace zy_cutPicture
{
    public partial class SettingForm : Form
    {
        MainForm mainForm;
        public SettingForm(MainForm main)
        {
            InitializeComponent();
            this.isDebug.Checked = Properties.Settings.Default.isDebug;
            this.cutAlpha.Value = Properties.Settings.Default.cutAlpha;
            this.spacing.Value = Properties.Settings.Default.spacing;
            this.expand.Value = Properties.Settings.Default.expand;
            mainForm = main;
        }        
     
        private void save_Click(object sender, EventArgs e)
        {            
            Properties.Settings.Default.isDebug = this.isDebug.Checked;
            Properties.Settings.Default.spacing = this.spacing.Value;
            Properties.Settings.Default.cutAlpha = this.cutAlpha.Value;
            Properties.Settings.Default.expand = this.expand.Value;
            Properties.Settings.Default.Save(); // 必须调用Save()
            if (mainForm != null)
                mainForm.UpdateProperty();
            this.Close();
        }
    }
}
