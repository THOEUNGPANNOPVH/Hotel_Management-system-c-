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
    public partial class frmPayment : Form
    {
        HotelCDB db = new HotelCDB();
        SqlCommand com;
        SqlDataAdapter adp;
        DataTable dt;
        public frmPayment()
        {
            InitializeComponent();
            db.Connection();
            LoadBooking();
            LoadData();

        }
        // =======================
        // Load Payment Records
        // =======================
        private void LoadData()
        {
            try
            {
                com = new SqlCommand("sp_GetAllPayments", db.con);
                com.CommandType = CommandType.StoredProcedure;

                adp = new SqlDataAdapter(com);
                dt = new DataTable();
                adp.Fill(dt);

                dgvPayment.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }
        // Load Booking IDs
        // ================================
        // Load Booking IDs
        // ================================
        private void LoadBooking()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Booking_ID FROM Booking", db.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboBooking.DataSource = dt;
                cboBooking.DisplayMember = "Booking_ID";
                cboBooking.ValueMember = "Booking_ID";
                cboBooking.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Booking Error: " + ex.Message);
            }
        }

        private void cboBooking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBooking.SelectedIndex == -1 || cboBooking.SelectedValue == null)
                return;

            try
            {
                if (db.con.State == ConnectionState.Open)
                    db.con.Close();

                // Make sure SelectedValue is a number
                int bookingId;
                if (cboBooking.SelectedValue is DataRowView drv)
                    bookingId = Convert.ToInt32(drv["Booking_ID"]);
                else
                    bookingId = Convert.ToInt32(cboBooking.SelectedValue);

                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                B.Paid_Amount,
                B.Status,
                C.Full_Name,
                R.Room_Type,
                ISNULL(P.Payment_Method, '') AS Payment_Method,
                ISNULL(P.Paid_Amount, 0) AS Payment_Paid,
                ISNULL(P.Remain_Amount, 0) AS Payment_Remain,
                ISNULL(P.Total_Amount, B.Paid_Amount) AS Payment_Total
            FROM Booking B
            INNER JOIN Customer C ON B.Customer_ID = C.Customer_ID
            INNER JOIN Room R ON B.Room_ID = R.Room_ID
            LEFT JOIN Payment P ON B.Booking_ID = P.Booking_ID
            WHERE B.Booking_ID = @Booking_ID", db.con);

                cmd.Parameters.AddWithValue("@Booking_ID", bookingId);

                db.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtPaidAmount.Text = dr["Payment_Paid"].ToString();
                    txtRemainAmount.Text = dr["Payment_Remain"].ToString();
                    txtTotalAmount.Text = dr["Payment_Total"].ToString();
                    cboMethod.Text = dr["Payment_Method"].ToString();
                }
                else
                {
                    txtPaidAmount.Text = "0.00";
                    txtRemainAmount.Text = "0.00";
                    txtTotalAmount.Text = "0.00";
                    cboMethod.SelectedIndex = -1;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading booking info: " + ex.Message);
            }
            finally
            {
                if (db.con.State == ConnectionState.Open)
                    db.con.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("sp_AddPayment", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Booking_ID", cboBooking.SelectedValue);
                com.Parameters.AddWithValue("@Payment_Date", dtpPaymentDete.Value);
                com.Parameters.AddWithValue("@Payment_Method", cboMethod.Text);
                com.Parameters.AddWithValue("@Paid_Amount", Convert.ToDecimal(txtPaidAmount.Text));
                com.Parameters.AddWithValue("@Remain_Amount", Convert.ToDecimal(txtRemainAmount.Text));
                com.Parameters.AddWithValue("@Total_Amount", Convert.ToDecimal(txtTotalAmount.Text));

                db.con.Open();
                com.ExecuteNonQuery();
                db.con.Close();

                MessageBox.Show("Payment Added Successfully!");
                LoadData();
            }
            catch (Exception ex)
            {
                db.con.Close();
                MessageBox.Show("Add Error: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("sp_UpdatePayment", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Payment_ID", Convert.ToInt32(txtPaymentID.Text));
                com.Parameters.AddWithValue("@Booking_ID", cboBooking.SelectedValue);
                com.Parameters.AddWithValue("@Payment_Date", dtpPaymentDete.Value);
                com.Parameters.AddWithValue("@Payment_Method", cboMethod.Text);
                com.Parameters.AddWithValue("@Paid_Amount", Convert.ToDecimal(txtPaidAmount.Text));
                com.Parameters.AddWithValue("@Remain_Amount", Convert.ToDecimal(txtRemainAmount.Text));
                com.Parameters.AddWithValue("@Total_Amount", Convert.ToDecimal(txtTotalAmount.Text));

                db.con.Open();
                com.ExecuteNonQuery();
                db.con.Close();

                MessageBox.Show("Payment Updated Successfully!");
                LoadData();
            }
            catch (Exception ex)
            {
                db.con.Close();
                MessageBox.Show("Update Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("sp_DeletePayment", db.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Payment_ID", Convert.ToInt32(txtPaymentID.Text));

                db.con.Open();
                com.ExecuteNonQuery();
                db.con.Close();

                MessageBox.Show("Payment Deleted Successfully!");
                LoadData();
            }
            catch (Exception ex)
            {
                db.con.Close();
                MessageBox.Show("Delete Error: " + ex.Message);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }
        private void ClearData()
        {
            txtPaymentID.Clear();
            cboBooking.SelectedIndex = -1;
            cboMethod.SelectedIndex = -1;
            txtPaidAmount.Clear();
            txtRemainAmount.Clear();
            txtTotalAmount.Clear();
            dtpPaymentDete.Value = DateTime.Now;
        }

        private void dgvPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPayment.Rows[e.RowIndex];
                txtPaymentID.Text = row.Cells["Payment_ID"].Value.ToString();
                cboBooking.Text = row.Cells["Booking_ID"].Value.ToString();
                cboMethod.Text = row.Cells["Payment_Method"].Value.ToString();
                txtPaidAmount.Text = row.Cells["Paid_Amount"].Value.ToString();
                txtRemainAmount.Text = row.Cells["Remain_Amount"].Value.ToString();
                txtTotalAmount.Text = row.Cells["Total_Amount"].Value.ToString();
                dtpPaymentDete.Value = Convert.ToDateTime(row.Cells["Payment_Date"].Value);
            }
        }
    }
}
