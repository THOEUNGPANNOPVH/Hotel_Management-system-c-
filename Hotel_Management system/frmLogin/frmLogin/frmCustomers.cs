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
    public partial class frmCustomers : Form
    {
        HotelCDB db = new HotelCDB();
        SqlCommand com;
        SqlDataAdapter adp;
        DataTable dt;
        public frmCustomers()
        {
            InitializeComponent();
            db.Connection();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                com = new SqlCommand("sp_GetAllCustomers", db.con);
                com.CommandType = CommandType.StoredProcedure;

                adp = new SqlDataAdapter(com);
                dt = new DataTable();
                adp.Fill(dt);

                dgvCustomers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("sp_AddCustomer", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Full_Name", txtFullName.Text);
                com.Parameters.AddWithValue("@Gender", cboGender.Text);
                com.Parameters.AddWithValue("@Phone", txtPhone.Text);
                com.Parameters.AddWithValue("@Address", txtAddress.Text);
                com.Parameters.AddWithValue("@National_ID", CboNationalID.Text);

                com.ExecuteNonQuery();
                MessageBox.Show("Customer added successfully!");
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
                com = new SqlCommand("sp_UpdateCustomer", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Customer_ID", txtCustomerID.Text);
                com.Parameters.AddWithValue("@Full_Name", txtFullName.Text);
                com.Parameters.AddWithValue("@Gender", cboGender.Text);
                com.Parameters.AddWithValue("@Phone", txtPhone.Text);
                com.Parameters.AddWithValue("@Address", txtAddress.Text);
                com.Parameters.AddWithValue("@National_ID", CboNationalID.Text);

                com.ExecuteNonQuery();
                MessageBox.Show("Customer updated successfully!");
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
                if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    com = new SqlCommand("sp_DeleteCustomer", db.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Customer_ID", txtCustomerID.Text);

                    com.ExecuteNonQuery();
                    MessageBox.Show("Customer deleted successfully!");
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
        public void ClearData()
        {
            txtCustomerID.Clear();
            txtFullName.Clear();
            cboGender.SelectedIndex = -1;
            txtPhone.Clear();
            txtAddress.Clear();
            CboNationalID.SelectedIndex = -1;
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                    txtCustomerID.Text = row.Cells["Customer_ID"].Value.ToString();
                    txtFullName.Text = row.Cells["Full_Name"].Value.ToString();
                    cboGender.Text = row.Cells["Gender"].Value.ToString();
                    txtPhone.Text = row.Cells["Phone"].Value.ToString();
                    txtAddress.Text = row.Cells["Address"].Value.ToString();
                    CboNationalID.Text = row.Cells["National_ID"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cell Click Error: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dgvCustomers.DataSource as DataTable;

                if (dt == null)
                {
                    MessageBox.Show("No data available for searching.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Escape quotes
                string search = txtSearch.Text.Trim().Replace("'", "''");

                // Apply filter only on string-based columns
                dt.DefaultView.RowFilter = $"Convert(Customer_ID, 'System.String') LIKE '%{search}%' OR Full_Name LIKE '%{search}%'";
            }
            catch (EvaluateException ex)
            {
                MessageBox.Show("Filter error: " + ex.Message, "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
