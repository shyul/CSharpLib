using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Shyu
{
    public class DBUtil
    {
        public static string GetConnectionString(FileInfo DataFile)
        {
            //string tmp = @"(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + DataFile.FullName + @";Integrated Security=True;Connect Timeout=30";
            string tmp = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + DataFile.FullName + @";Integrated Security=True;Connect Timeout=30";
            return tmp;
        }
        public static DataTable LoadTable(FileInfo DataFile, string TableName, string SqlCmd)
        {
            DataTable t = new DataTable();
            if (CheckExistTable(DataFile, TableName))
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = GetConnectionString(DataFile);
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlCmd, conn);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(t);
                    conn.Close();
                    conn.Dispose();
                }
            }
            t.TableName = TableName;
            return t;
        }
        public static void SaveTable(FileInfo DataFile, DataTable t, string SqlCmd)
        {
            DeleteTable(DataFile, t.TableName);
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnectionString(DataFile);
                conn.Open();
                SqlCommand command = new SqlCommand(SqlCmd, conn);
                command.ExecuteNonQuery();
                using (SqlBulkCopy s = new SqlBulkCopy(conn))
                {
                    s.DestinationTableName = "[" + t.TableName + "]";
                    foreach (var column in t.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    s.WriteToServer(t);
                }
                conn.Close();
                conn.Dispose();
            }
        }
        public static void DeleteTable(FileInfo DataFile, string TableName)
        {
            if (CheckExistTable(DataFile, TableName))
            {
                string cmd = "Drop TABLE [" + TableName + "]";
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = GetConnectionString(DataFile);
                    conn.Open();
                    SqlCommand command = new SqlCommand(cmd, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public static bool CheckExistTable(FileInfo DataFile, string TableName)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("IF OBJECT_ID (N'{0}', N'U') IS NOT NULL\n\tSELECT 1 AS res ELSE SELECT 0 AS res", TableName);
            string ResultStr = string.Empty;
            using (SqlConnection conn = new SqlConnection())
            {
                string ConnString = GetConnectionString(DataFile);
                conn.ConnectionString = ConnString;
                conn.Open();
                SqlCommand command = new SqlCommand(s.ToString(), conn);
                SqlDataReader res = command.ExecuteReader();
                while (res.Read()) ResultStr = res[0].ToString();
                conn.Close();
                conn.Dispose();
            }
            if (ResultStr == "1")
                return true;
            else
                return false;
        }
        public static void ResetDataBaseFile(FileInfo DataFile)
        {
            string FileName = Path.GetFileNameWithoutExtension(DataFile.FullName);
            string FilePath = Path.GetDirectoryName(DataFile.FullName);
            if (File.Exists(DataFile.FullName)) File.Delete(DataFile.FullName);
            if (File.Exists(FilePath + @"\" + FileName + "_log.ldf"))
                File.Delete(FilePath + @"\" + FileName + "_log.ldf");
            InitDataBaseFile(DataFile);
        }
        public static void InitDataBaseFile(FileInfo DataFile)
        {
            string FileName = Path.GetFileNameWithoutExtension(DataFile.FullName);
            string FilePath = Path.GetDirectoryName(DataFile.FullName);
            if (!File.Exists(DataFile.FullName))
            {
                string TemplatePath = AppDomain.CurrentDomain.BaseDirectory + @"\Empty.mdf";
                File.Copy(TemplatePath, DataFile.FullName, true);
            }
            if (File.Exists(FilePath + @"\" + FileName + "_log.ldf"))
                File.Delete(FilePath + @"\" + FileName + "_log.ldf");
        }
    }
}
