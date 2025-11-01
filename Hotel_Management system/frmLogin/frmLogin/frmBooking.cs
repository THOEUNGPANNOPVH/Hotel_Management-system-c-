using Hotel_Management;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frmLogin
{
    public partial class frmBooking : Form
    {
        HotelCDB db = new HotelCDB();
        SqlCommand com;
        SqlDataAdapter adp;
        DataTable dt;
        public frmBooking()
        {
            InitializeComponent();
            db.Connection();
            LoadData();
            LoadCustomers();
            LoadStaff();
            LoadRoomType();
            LoadBookingType();
            LoadStatus();

        }
        public void LoadData()
        {
            try
            {
                com = new SqlCommand("sp_GetAllBookings", db.con);
                com.CommandType = CommandType.StoredProcedure;

                adp = new SqlDataAdapter(com);
                dt = new DataTable();
                adp.Fill(dt);

                dgvReservation.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }
        private void LoadCustomers()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Customer_ID, Full_Name FROM Customer", db.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboCustomers.DataSource = dt;
                cboCustomers.DisplayMember = "Full_Name";
                cboCustomers.ValueMember = "Customer_ID";
                cboCustomers.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Customer Error: " + ex.Message);
            }
        }
        private void LoadStaff()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Staff_ID, Full_Name FROM Staff", db.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboStaff.DataSource = dt;
                cboStaff.DisplayMember = "Full_Name";
                cboStaff.ValueMember = "Staff_ID";
                cboStaff.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Staff Error: " + ex.Message);
            }
        }
        private void LoadRoomType()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Room_ID, Room_Type FROM Room", db.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboRoomType.DataSource = dt;
                cboRoomType.DisplayMember = "Room_Type";
                cboRoomType.ValueMember = "Room_ID";
                cboRoomType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Room Error: " + ex.Message);
            }
        }
        private void LoadBookingType()
        {
            cboBookingType.Items.Clear();
            cboBookingType.Items.Add("Online");
            cboBookingType.Items.Add("Walk-in");
            cboBookingType.Items.Add("Phone");
            cboBookingType.SelectedIndex = -1;
        }
        private void LoadStatus()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.Add("Active");
            cboStatus.Items.Add("Reserved");
            cboStatus.Items.Add("Checked Out");
            cboStatus.Items.Add("Pending");
            cboStatus.SelectedIndex = -1;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("sp_AddBooking", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Customer_ID", cboCustomers.SelectedValue);
                com.Parameters.AddWithValue("@Room_ID", cboRoomType.SelectedValue);
                com.Parameters.AddWithValue("@Staff_ID", cboStaff.SelectedValue);
                com.Parameters.AddWithValue("@Check_In", dtpCheckIn.Value);
                com.Parameters.AddWithValue("@Check_Out", dtpCheckOut.Value);
                com.Parameters.AddWithValue("@Num_Guests", int.Parse(txtGuests.Text));
                com.Parameters.AddWithValue("@Paid_Amount", decimal.Parse(txtBookingAmount.Text));
                com.Parameters.AddWithValue("@Booking_Type", cboBookingType.Text);
                com.Parameters.AddWithValue("@Status", cboStatus.Text);

                com.ExecuteNonQuery();
                MessageBox.Show("✅ Booking Added Successfully!");
                LoadData();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Error: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReservationID.Text))
                {
                    MessageBox.Show("⚠️ Please select a Booking to update!");
                    return;
                }

                com = new SqlCommand("sp_UpdateBooking", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Booking_ID", int.Parse(txtReservationID.Text));
                com.Parameters.AddWithValue("@Customer_ID", cboCustomers.SelectedValue);
                com.Parameters.AddWithValue("@Room_ID", cboRoomType.SelectedValue);
                com.Parameters.AddWithValue("@Staff_ID", cboStaff.SelectedValue);
                com.Parameters.AddWithValue("@Check_In", dtpCheckIn.Value);
                com.Parameters.AddWithValue("@Check_Out", dtpCheckOut.Value);
                com.Parameters.AddWithValue("@Num_Guests", int.Parse(txtGuests.Text));
                com.Parameters.AddWithValue("@Paid_Amount", decimal.Parse(txtBookingAmount.Text));
                com.Parameters.AddWithValue("@Booking_Type", cboBookingType.Text);
                com.Parameters.AddWithValue("@Status", cboStatus.Text);

                com.ExecuteNonQuery();
                MessageBox.Show("✅ Booking Updated Successfully!");
                LoadData();
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Error: " + ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReservationID.Text))
                {
                    MessageBox.Show("⚠️ Please select a booking to delete!");
                    return;
                }

                DialogResult dr = MessageBox.Show("Are you sure to delete this booking?", "Confirm", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    com = new SqlCommand("sp_DeleteBooking", db.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Booking_ID", int.Parse(txtReservationID.Text));
                    com.ExecuteNonQuery();

                    MessageBox.Show("🗑️ Booking Deleted Successfully!");
                    LoadData();
                    ClearData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Error: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }
        private void ClearData()
        {
            txtReservationID.Clear();
            cboCustomers.SelectedIndex = -1;
            cboRoomType.SelectedIndex = -1;
            cboStaff.SelectedIndex = -1;
            cboBookingType.SelectedIndex = -1;
            cboStatus.SelectedIndex = -1;
            txtGuests.Clear();
            txtBookingAmount.Clear();
            dtpCheckIn.Value = DateTime.Now;
            dtpCheckOut.Value = DateTime.Now;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void dgvReservation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvReservation.Rows[e.RowIndex];
                    txtReservationID.Text = row.Cells["Booking_ID"].Value.ToString();
                    cboCustomers.SelectedValue = row.Cells["Customer_ID"].Value;
                    cboRoomType.SelectedValue = row.Cells["Room_ID"].Value;
                    cboStaff.SelectedValue = row.Cells["Staff_ID"].Value;
                    txtGuests.Text = row.Cells["Num_Guests"].Value.ToString();
                    txtBookingAmount.Text = row.Cells["Paid_Amount"].Value.ToString();  // ✅ fixed
                    cboBookingType.Text = row.Cells["Booking_Type"].Value.ToString();
                    cboStatus.Text = row.Cells["Status"].Value.ToString();
                    dtpCheckIn.Value = Convert.ToDateTime(row.Cells["Check_In"].Value);
                    dtpCheckOut.Value = Convert.ToDateTime(row.Cells["Check_Out"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Error: " + ex.Message);
            }
        }

    }
}






