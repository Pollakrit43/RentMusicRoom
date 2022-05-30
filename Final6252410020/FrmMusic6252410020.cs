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

namespace Final6252410020
{
    public partial class FrmMusic6252410020 : Form
    {
        

        public FrmMusic6252410020()
        {
            InitializeComponent();
        }

        private void FrmMusic6252410020_Load(object sender, EventArgs e)
        {

        }
        private void warningMSG(string msg)
        {
            MessageBox.Show(msg, "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btGenRenId_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ShareDataValue.connStr);
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                string strSql = "SELECT rentId FROM rentroom_tb ORDER BY rentId DESC";
                SqlTransaction sqlTransaction = conn.BeginTransaction();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = conn;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = strSql;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    string rentIdCurrent = sqlDataReader["rentId"].ToString();
                    int numberIdCurrent = Convert.ToInt32(rentIdCurrent.Substring(2, 5));
                    int newId = numberIdCurrent + 1;
                    string rentIdNew = "MS" + newId.ToString("00000");
                    lbRentId.Text = rentIdNew;
                    lbRentId.Enabled = false;
                }
            }
            catch (SqlException ex)
            {
                warningMSG("พบปัญหาในการทำงานกับฐานข้อมูล....!!!!");
            }
            conn.Close();
        }

        private void btCal_Click(object sender, EventArgs e)
        {
            int sum = 0;
            int rent_hour = 0;
            if (tbRentHour.Text.Trim().Length < 1)
            {
                warningMSG("ป้อนชั่วโมงด้วย");
                return;
            }
            else
            {
                rent_hour = Int32.Parse(tbRentHour.Text.Trim());
                string str = "";
                if (chkGuitar.Checked == true)
                {
                    str = tbNumGuitar.Text.Trim();
                    if (str.Length < 1)
                    {
                        warningMSG("ป้อนจำนวนกีต้าด้วย");
                        return;
                    }
                    sum += (20 * Int32.Parse(str));
                }
                if (chkBase.Checked == true)
                {
                    str = tbNumBase.Text.Trim();
                    if (str.Length < 1)
                    {
                        warningMSG("ป้อนจำนวนเบสด้วย");
                        return;
                    }
                    sum += (15 * Int32.Parse(str));
                }
                if (chkKeyboard.Checked == true)
                {
                    str = tbNumKeyboard.Text.Trim();
                    if (str.Length < 1)
                    {
                        warningMSG("ป้อนจำนวนคีย์บอร์ดด้วย");
                        return;
                    }
                    sum += (10 * Int32.Parse(str));
                }
                if (chkDrum.Checked == true)
                {
                    str = tbNumDrum.Text.Trim();
                    if (str.Length < 1)
                    {
                        warningMSG("ป้อนจำนวนกลองด้วย");
                        return;
                    }
                    sum += (30 * Int32.Parse(str));
                }
            }
            sum += (80 * rent_hour);
            lbShowPayTotal.Text = sum.ToString();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            lbRentId.Text = "MSXXXXX";
            tbRentHour.Clear();
            chkGuitar.Checked = false;
            tbNumGuitar.Clear();
            chkBase.Checked = false;
            tbNumBase.Clear();
            chkKeyboard.Checked = false;
            tbNumKeyboard.Clear();
            chkDrum.Checked = false;
            tbNumDrum.Clear();
            lbShowPayTotal.Text = "00.00";
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            string str = lbRentId.Text.Trim();
            if (str.Contains("MSXXXXX"))
            {
                warningMSG("กรุณาสร้างรหัสใหม่");
                return;
            }
            if (lbShowPayTotal.Text.Trim().Contains("00.00"))
            {
                warningMSG("กรุณาคลิกปุ่นคำนวณ เพื่อคำนวณเงินที่ต้องชำระ");
                return;
            }
            SqlConnection conn = new SqlConnection(ShareDataValue.connStr);
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                String query = "INSERT INTO dbo.SMS_PW (rentId,rentHour,rentGuiter,rentGuiterNum,rentBase,rentrBaseNum,rentKeyboard,rentKeyboardNum,rentDrum,rentDrumNum,payTotal) VALUES (@rentId,@rentHour,@rentGuiter,@rentGuiterNum,@rentBase,@rentrBaseNum,@rentKeyboard,@rentKeyboardNum,@rentDrum,@rentDrumNum,@payTotal)";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add("@rentId", lbRentId.Text);
                command.Parameters.Add("@rentHour", tbRentHour.Text);
                command.Parameters.Add("@rentGuiter", chkGuitar.Checked == true ? 1 : 0);
                command.Parameters.Add("@rentGuiterNum", tbNumGuitar.Text);
                command.Parameters.Add("@rentBase", chkBase.Checked == true ? 1 : 0);
                command.Parameters.Add("@rentBaseNum", tbNumBase.Text);
                command.Parameters.Add("@rentKeyboard", chkKeyboard.Checked == true ? 1 : 0);
                command.Parameters.Add("@rentKeyboardNum", tbNumKeyboard.Text);
                command.Parameters.Add("@rentDrum", chkDrum.Checked == true ? 1 : 0);
                command.Parameters.Add("@rentDrumNum", tbNumDrum.Text);
                command.Parameters.Add("@payTotal", lbShowPayTotal.Text);
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                warningMSG("พบปัญหาในการทำงานกับฐานข้อมูล....!!!!");
            }
            conn.Close();
        }
    }
    }



