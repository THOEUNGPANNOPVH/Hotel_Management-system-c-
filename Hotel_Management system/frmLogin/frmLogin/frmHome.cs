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
    public partial class frmHome : Form
    {
        frmStaffs staffs = new frmStaffs();
        frmRoom room = new frmRoom();
        frmCustomers customers = new frmCustomers();
        frmBooking reservation = new frmBooking();
        frmPayment payment = new frmPayment();
        public frmHome()
        {
            InitializeComponent();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            this.Hide();
            staffs.ShowDialog();
            this.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            this.Hide();
            room.ShowDialog();
            this.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            customers.ShowDialog();
            this.Show();
        }

        private void btnReservation_Click(object sender, EventArgs e)
        {
            this.Hide();
            reservation.ShowDialog();
            this.Show();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            this.Hide();
            payment.ShowDialog();
            this.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmHome_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
