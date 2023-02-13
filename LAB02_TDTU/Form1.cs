using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LAB02_TDTU
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Button 
            btnUpdate.Enabled = false;
            btnDel.Enabled = false;            
            //

            if (fullName.Text == "" || (male.Checked == false && female.Checked == false) || email.Text == "")
            {
                MessageBox.Show("Please enter all information!");
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = @"Data Source=(local)\PHAMTHU; Initial Catalog=school;Integrated Security=True";
                conn.Open();
                String sSQL = "INSERT INTO student (hoTen,ngaySinh,gioiTinh,email) VALUES(@FName,@NgaySinh,@GioiTinh,@Email)";
                SqlCommand cmd = new SqlCommand(sSQL, conn);
                cmd.Parameters.Add(new SqlParameter("@FName", fullName.Text));
                cmd.Parameters.Add(new SqlParameter("@NgaySinh", birth.Value));
                var checkedButton = panel1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

                cmd.Parameters.Add(new SqlParameter("@GioiTinh", checkedButton.Checked));
                cmd.Parameters.Add(new SqlParameter("@Email", email.Text));
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error:" + ex.Message);
                }
                MessageBox.Show("Saved Successfully!");
                Form1_Load(sender, e);
            }
            btnDel.Enabled = true;
            btnUpdate.Enabled = true;
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            fullName.Text = string.Empty;
            male.Checked = false;
            female.Checked = false;
            email.Text = string.Empty;
            birth.Value = DateTime.Now;
            dataView.ClearSelection();

            //Button 
            btnAdd.Enabled = true;
            //
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(local)\PHAMTHU; Initial Catalog=school;Integrated Security=True";
            conn.Open();
            String sSQL = "DELETE FROM student WHERE hoTen = @FName";
            SqlCommand cmd = new SqlCommand(sSQL, conn);
            try
            {
                int index = dataView.SelectedRows[0].Index;
                DataGridViewRow row = dataView.Rows[index];
                String hoTen = Convert.ToString(row.Cells[1].Value);
                cmd.Parameters.Add("@FName", SqlDbType.NVarChar).Value = hoTen;
                cmd.ExecuteNonQuery();

                dataView.Rows.RemoveAt(index);
            }
            catch (Exception ex)
            {
                throw new Exception("Error:" + ex.Message);
            }
            MessageBox.Show("Deleted Successfully!");
            Form1_Load(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(local)\PHAMTHU; Initial Catalog=school;Integrated Security=True";
            conn.Open();
            String sSQL = "UPDATE student SET gioiTinh = @GioiTinh, ngaySinh = @NgaySinh, email = @Email WHERE hoTen = @FName";
            SqlCommand cmd = new SqlCommand(sSQL, conn);

            //var checkedButton = panel1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            male.Checked = false;
            female.Checked = false;
            if (male.Checked)
            {
                cmd.Parameters.Add(new SqlParameter("@GioiTinh", male.Checked));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@GioiTinh", female.Checked));
            }
            cmd.Parameters.Add(new SqlParameter("@NgaySinh", birth.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", email.Text));

            int index = dataView.SelectedRows[0].Index;
            DataGridViewRow row = dataView.Rows[index];
            String hoTen = Convert.ToString(row.Cells[1].Value);
            cmd.Parameters.Add("@FName", SqlDbType.NVarChar).Value = hoTen;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error:" + ex.Message);
            }
            MessageBox.Show("Updated Successfully!");
            Form1_Load(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Controls.Add(male);
            panel1.Controls.Add(female);

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = (local)\PHAMTHU; Initial Catalog=school; Integrated Security=True";
            conn.Open();
            String sSQL = "SELECT * FROM student"; 
            SqlCommand cmd = new SqlCommand(sSQL, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable(); 
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                dataView.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No Data!", "Warning!");
            }

            //show First
            DataGridViewRow row = dataView.Rows[0];
            String maso = Convert.ToString(row.Cells[0].Value);
            String hoTen = Convert.ToString(row.Cells[1].Value);
            DateTime ngaySinh = Convert.ToDateTime(row.Cells[2].Value);
            bool gioiTinh = Convert.ToBoolean(row.Cells[3].Value);
            String emailData = Convert.ToString(row.Cells[4].Value);

            //update UI
            fullName.Text = hoTen;
            if (gioiTinh)
            {
                male.Checked = true;
            }
            else
            {
                female.Checked = true;
            }
            email.Text = emailData;
            birth.Value = ngaySinh;
        }

        private void male_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Button 
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDel.Enabled = true;
            btnClr.Enabled = true;
            //

            int index = e.RowIndex;
            if (index < 0 || index >= dataView.RowCount)
                return;
            try
            {
                {
                    DataGridViewRow row = dataView.Rows[index];
                    String maso = Convert.ToString(row.Cells[0].Value);
                    String hoTen = Convert.ToString(row.Cells[1].Value);
                    DateTime ngaySinh = Convert.ToDateTime(row.Cells[2].Value);
                    bool gioiTinh = Convert.ToBoolean(row.Cells[3].Value);
                    String emailData = Convert.ToString(row.Cells[4].Value);

                    //update UI
                    fullName.Text = hoTen;
                    if (gioiTinh)
                    {
                        male.Checked = true;
                    }
                    else
                    {
                        female.Checked = true;
                    }
                    email.Text = emailData;
                    birth.Value = ngaySinh;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error:" + ex.Message);
            }
        }
    }
}