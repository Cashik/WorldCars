using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WorldCars
{
    public partial class CommentForm : UserControl
    {
        CommentClass comment;
        MainForm parent;
        public CommentForm(MainForm mainForm, CommentClass _comment,bool deletable)
        {
            InitializeComponent();
            parent = mainForm;
            comment = _comment;

            if (comment.text != "")
            {
                RRTB rrtb = new RRTB(parent, "someName", commentPanel.Width - commentPanel.Padding.Left * 2, true, comment.text);
                rrtb.Dock = DockStyle.Bottom;
                commentPanel.Controls.Add(rrtb);
            }
            if (!deletable) delete.Visible = false;

            UserClass user = Program.app.db.ReturnUserById(comment.user_id);

            if (user.image == null)
            {
                pictureBox1.Image = Image.FromFile("people.png");
            }
            else
            {
                MemoryStream ms = new MemoryStream(user.image);
                pictureBox1.Image = Image.FromStream(ms);
            }

            nameLbl.Text = user.name;
            ratingLbl.Text = Program.app.RatingToString(comment.rating);
            dateLbl.Text = comment.datetime.ToString();

        }

        private void delete_Click(object sender, EventArgs e)
        {
            bool a = Program.app.db.DeleteTableById("Comment", comment.id);
            parent.RefreshCarInfo();
        }

        private void CommentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
