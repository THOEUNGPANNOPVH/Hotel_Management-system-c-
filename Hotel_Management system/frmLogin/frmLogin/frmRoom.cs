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
    public partial class frmRoom : Form
    {
        HotelCDB db = new HotelCDB();
        SqlCommand com;
        SqlDataAdapter adp;
        DataTable dt;
        public frmRoom()
        {
            InitializeComponent();
            db.Connection();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                com = new SqlCommand("sp_GetAllRooms", db.con);
                com.CommandType = CommandType.StoredProcedure;
                adp = new SqlDataAdapter(com);
                dt = new DataTable();
                adp.Fill(dt);
                dgvRooms.DataSource = dt;
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
                com = new SqlCommand("sp_AddRoom", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Room_Type",cboRoomType.Text);
                com.Parameters.AddWithValue("@Floor", Convert.ToInt32(txtFloor.Text));
                com.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                com.Parameters.AddWithValue("@Status", cboStatus.Text);

                com.ExecuteNonQuery();
                MessageBox.Show("Room added successfully!");
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
                com = new SqlCommand("sp_UpdateRoom", db.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@Room_ID", txtRoomID.Text);
                com.Parameters.AddWithValue("@Room_Type", cboRoomType.Text);
                com.Parameters.AddWithValue("@Floor", Convert.ToInt32(txtFloor.Text));
                com.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                com.Parameters.AddWithValue("@Status", cboStatus.Text);

                com.ExecuteNonQuery();
                MessageBox.Show("Room updated successfully!");
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
                if (MessageBox.Show("Are you sure you want to delete this room?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    com = new SqlCommand("sp_DeleteRoom", db.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Room_ID", txtRoomID.Text);

                    com.ExecuteNonQuery();
                    MessageBox.Show("Room deleted successfully!");
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
            txtRoomID.Clear();
            cboRoomType.SelectedIndex = -1;
            txtFloor.Clear();
            txtPrice.Clear();
            cboStatus.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dgvRooms.DataSource as DataTable;

                if (dt == null)
                {
                    MessageBox.Show("No data available for searching.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Escape quotes
                string search = txtSearch.Text.Trim().Replace("'", "''");

                // Apply filter only on string-based columns
                dt.DefaultView.RowFilter = $"Convert(Room_ID, 'System.String') LIKE '%{search}%' OR Room_Type LIKE '%{search}%'";
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

        private void dgvRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvRooms.Rows[e.RowIndex];
                    txtRoomID.Text = row.Cells["Room_ID"].Value.ToString();
                    cboRoomType.Text = row.Cells["Room_Type"].Value.ToString();
                    txtFloor.Text = row.Cells["Floor"].Value.ToString();
                    txtPrice.Text = row.Cells["Price"].Value.ToString();
                    cboStatus.Text = row.Cells["Status"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cell Click Error: " + ex.Message);
            }
        }
    }
    
}
