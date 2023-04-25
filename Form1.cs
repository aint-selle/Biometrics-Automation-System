using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace Biometrics_Automation_System
{
    public partial class Biometrics : Form
    {
        SqlConnection con;
        SqlCommand command;
        SqlDataReader reader;
        DatabaseConnection db = new DatabaseConnection();

        public Biometrics()
        {
            InitializeComponent();

            con = new SqlConnection(db.GetConnection());
            //LoadRecords();
        }
        /*public void LoadRecords()
        {
            DataGridLogs.Rows.Clear();
            int i = 0;
           con.Open();
            //command = new SqlCommand("Select EmpID, GETDATE() as LogDt, LogTm, LogType from TmpDTRLog", con);
            //command = new SqlCommand("Select * from tTADTR", con);
            command = new SqlCommand("select dbo.fnEmpName(tHREmployee.EmpID) as \"Name\", LogType, LogTm, TmpDTRLog.EmpID, tHREmployee.CompID, LogDt from TmpDTRLog inner join tHREmployee on TmpDTRLog.EmpID = tHREmployee.EmpID ");
            reader = command.ExecuteReader();
            while (reader.Read())  
            {
                i++;
                //DataGridLogs.Rows.Add(i, reader["Name"].ToString(), reader["Status"].ToString(), reader["Date/Time"].ToString(), reader["EmpID"].ToString(), reader["Department"].ToString(), reader["LogDt"].ToString());
                DataGridLogs.Rows.Add(i, reader["dbo.fnEmpName(tHREmployee.EmpID) as \"Name\""].ToString(), reader["LogType"].ToString(), reader["LogTm"].ToString(), reader["TmpDTRLog.EmpID"].ToString(), reader["tHREmployee.CompID"].ToString(), reader["LogDt"].ToString());

            }
            reader.Close();
            con.Close();
        }*/

        private void btn_Select_Click(object sender, EventArgs e)
        {
            DataGridLogs.Rows.Clear();

            Microsoft.Office.Interop.Excel.Application excelapplication;
            Microsoft.Office.Interop.Excel.Workbook excelworkbook;
            Microsoft.Office.Interop.Excel.Worksheet excelworksheet;
            Microsoft.Office.Interop.Excel.Range excelrange;

            int excelrow;
            string stringFileName;

            openFileDialog1.Filter = "Excel Office |*.xls; *xlxs";
            openFileDialog1.ShowDialog();
            stringFileName = openFileDialog1.FileName;

            if(stringFileName != "") 
            {
                excelapplication = new Microsoft.Office.Interop.Excel.Application();
                excelworkbook = excelapplication.Workbooks.Open(stringFileName);
                excelworksheet = excelworkbook.Worksheets["Sheet 1"];
                excelrange = excelworksheet.UsedRange;

                int i = 0;

                for (excelrow = 2; excelrow <= excelrange.Rows.Count; excelrow++)
                {
                    if (excelrange.Cells[excelrow, 1].Text != "")
                    { 
                        i++;
                        //DataGridLogs.Rows.Add(i, excelrange.Cells[excelrow, 1].Text, excelrange.Cells[excelrow, 2].Text, excelrange.Cells[excelrow, 3].Text, excelrange.Cells[excelrow, 4].Text, excelrange.Cells[excelrow, 5].Text, excelrange.Cells[excelrow, 6].Text);
                        DataGridLogs.Rows.Add(i, excelrange.Cells[excelrow, 1].Text, excelrange.Cells[excelrow, 2].Text, excelrange.Cells[excelrow, 3].Text/*, excelrange.Cells[excelrow, 4].Text*/);

                    }
                }
                excelworkbook.Close();
                excelapplication.Quit();
            }
        }

        public void InsertTmp()
        {
            string EmpIDs;
            string LogDt;
            for (int i = 0; i < DataGridLogs.Rows.Count; i++)
            {
                con.Open();
                //command = new SqlCommand("EXEC spProcTempDtrToDtr @EmpID,@LogDt, @LogTm, @stat", con);
               // command = new SqlCommand("insert into TmpDTRLog (EmpID, LogDt, LogTm, LogType) values (@EmpID, @LogDt, @LogTm, @LogType) ", con);
                
                /*command.Parameters.AddWithValue("@EmpID", DataGridLogs.Rows[i].Cells[1].Value.ToString());
                command.Parameters.AddWithValue("@LogDt", DataGridLogs.Rows[i].Cells[2].Value.ToString());
                command.Parameters.AddWithValue("@LogTm", DataGridLogs.Rows[i].Cells[3].Value.ToString());
                command.Parameters.AddWithValue("@LogType", DataGridLogs.Rows[i].Cells[4].Value.ToString());
                command.ExecuteNonQuery();*/
                command = new SqlCommand("spInsertTempDtr", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmpID", SqlDbType.NVarChar).Value = DataGridLogs.Rows[i].Cells[1].Value.ToString();
                command.Parameters.AddWithValue("@LogDt", SqlDbType.DateTime).Value = Convert.ToDateTime(DataGridLogs.Rows[i].Cells[2].Value).ToString();
                command.Parameters.AddWithValue("@LogTm", SqlDbType.DateTime).Value = Convert.ToDateTime(DataGridLogs.Rows[i].Cells[2].Value).ToString();
                command.Parameters.AddWithValue("@LogType", SqlDbType.NVarChar).Value = DataGridLogs.Rows[i].Cells[3].Value.ToString();
                command.ExecuteNonQuery();
                con.Close();
                EmpIDs = DataGridLogs.Rows[i].Cells[1].Value.ToString();
                LogDt = DataGridLogs.Rows[i].Cells[2].Value.ToString();
                Test(EmpIDs, Convert.ToDateTime(LogDt));

            }
        }

        public void Test(string EmpID, DateTime TodayDt)
        {
            
                con.Open();
                SqlCommand command = new SqlCommand("spProcTempDtrToDtr", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmpID", SqlDbType.NVarChar).Value = EmpID;
                command.Parameters.AddWithValue("@TodayDt", SqlDbType.NVarChar).Value = TodayDt;
                command.ExecuteNonQuery();
                con.Close();
            
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            InsertTmp();
            MessageBox.Show("Records Successfully Updated.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //LoadRecords();
        }

        private void Biometrics_Load(object sender, EventArgs e)
        {

        }
    }
}
