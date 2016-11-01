using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldCars
{
    public partial class RRTB : UserControl
    {
        int userId;
        MainForm parent;
        public RRTB(MainForm mainForm,string elemName,int width,bool readOnly,string text="",int creatorId = -1)
        {
            InitializeComponent();
            parent = mainForm;
            Name = elemName;

            richTextBox.Width = width;
            richTextBox.Text = text;
            richTextBox.Height = (richTextBox.GetLineFromCharIndex(richTextBox.Text.Length) + 2) *
                        richTextBox.Font.Height + richTextBox.Margin.Vertical;
            SetReadOnly(readOnly);
            userId = creatorId;
            // подсвчиваем, если это ссылка
            if (userId != -1)
            {
                userId = creatorId;
                richTextBox.ForeColor = Color.Blue;
            }
        }
        public void SetReadOnly(bool readOnly)
        {
            richTextBox.ReadOnly = readOnly;
        }
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            richTextBox.Height = (richTextBox.GetLineFromCharIndex(richTextBox.Text.Length) + 2) *
                        richTextBox.Font.Height + richTextBox.Margin.Vertical;
        }
        private void richTextBox_Click(object sender, EventArgs e)
        {
            if (userId != -1)
            {
                parent.OpenUserInfo(userId);
            } 
        }
        public string getText()
        {
            return richTextBox.Text;
        }
    }
}

