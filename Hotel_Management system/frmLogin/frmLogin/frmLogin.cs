using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frmLogin
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(Admin.Text=="" && Password.Text=="")
            {
                MessageBox.Show("Please Enter Username and Password", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(Admin.Text=="admin" && Password.Text=="1234")
            {
                frmHome H = new frmHome();
                H.Show();
                this.Hide();
                //MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Admin.Text = "";
            Password.Text = "";
        }
    }
}
