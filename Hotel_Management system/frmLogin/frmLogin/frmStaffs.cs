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
    public partial class frmStaffs : Form
    {
        HotelCDB db = new HotelCDB();
        SqlDataAdapter adp;
        SqlCommand com;
        DataTable dt;
        public frmStaffs()
        {
            db.Connection();
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                dgvStaffs.DataSource = null;
                com = new SqlCommand("sp_GetAllStaff", db.con);
                com.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter dap = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                dap.Fill(dt);

                dgvStaffs.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        private void frmStaffs_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("sp_AddStaff", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Full_Name", txtFullname.Text);
                com.Parameters.AddWithValue("@Gender", cboGender.Text);
                com.Parameters.AddWithValue("@Position", txtPosition.Text);
                com.Parameters.AddWithValue("@Phone", txtPhone.Text);
                com.Parameters.AddWithValue("@Email", txtEmail.Text);
                com.Parameters.AddWithValue("@Salary", Convert.ToDecimal(txtSalary.Text));

                com.ExecuteNonQuery();
                MessageBox.Show("Staff added successfully!");
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
                com = new SqlCommand("sp_UpdateStaff", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Staff_ID", txtID.Text);
                com.Parameters.AddWithValue("@Full_Name", txtFullname.Text);
                com.Parameters.AddWithValue("@Gender", cboGender.Text);
                com.Parameters.AddWithValue("@Position", txtPosition.Text);
                com.Parameters.AddWithValue("@Phone", txtPhone.Text);
                com.Parameters.AddWithValue("@Email", txtEmail.Text);
                com.Parameters.AddWithValue("@Salary", Convert.ToDecimal(txtSalary.Text));

                com.ExecuteNonQuery();
                MessageBox.Show("Staff updated successfully!");
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
                if (MessageBox.Show("Are you sure you want to delete this staff?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    com = new SqlCommand("sp_DeleteStaff", db.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Staff_ID", txtID.Text);

                    com.ExecuteNonQuery();
                    MessageBox.Show("Staff deleted successfully!");
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
            txtID.Clear();
            txtFullname.Clear();
            cboGender.SelectedIndex = -1;
            txtPosition.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtSalary.Clear();
            txtFullname.Focus();
        }
        private void dgvStaffs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvStaffs.Rows[e.RowIndex];
                    txtID.Text = row.Cells["Staff_ID"].Value.ToString();
                    txtFullname.Text = row.Cells["Full_Name"].Value.ToString();
                    cboGender.Text = row.Cells["Gender"].Value.ToString();
                    txtPosition.Text = row.Cells["Position"].Value.ToString();
                    txtPhone.Text = row.Cells["Phone"].Value.ToString();
                    txtEmail.Text = row.Cells["Email"].Value.ToString();
                    txtSalary.Text = row.Cells["Salary"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cell Click Error: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dgvStaffs.DataSource as DataTable;

                if (dt == null)
                {
                    MessageBox.Show("No data available for searching.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Escape quotes
                string search = txtSearch.Text.Trim().Replace("'", "''");

                // Apply filter only on string-based columns
                dt.DefaultView.RowFilter = $"Convert(Staff_ID, 'System.String') LIKE '%{search}%' OR Full_Name LIKE '%{search}%'";
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
 
    

    







