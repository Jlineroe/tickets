using AIBTicketsMVC.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AIBTicketsMVC.App_Code
{
    public class DAOConfig
    {
        public async static Task<SqlCommand> SqlCommandELC()
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection conex = new SqlConnection(ConfigurationManager.ConnectionStrings["ATLANTIC"].ToString());
                cmd = conex.CreateCommand();
                cmd.Connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(Tools.MsjError("App_Code.DAOConfig.SqlCommandELC", ex));
            }
            return cmd;
        }
        public async static Task<SqlCommand> SqlCommandGeneralSD()
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                SqlConnection conex = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ToString());
                cmd = conex.CreateCommand();
                cmd.Connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(Tools.MsjError("App_Code.DAOConfig.SqlCommandGeneralSD", ex));
            }
            return cmd;
        }
        public async static Task<DataTable> GetDataTableExecuteCommand(SqlCommand cmd)
        {
            DataTable table = new DataTable();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"App_Code.DAOConfig.GetDataTableExecuteCommand({Tools.GetLineErr(ex)}): {ex.Message}");
            }
            finally
            {
                cmd.Connection.Close();
            }
            return table;
        }
        public async static Task<DataTable> SetDataTableExecuteCommand(SqlCommand cmd)
        {
            DataTable table = new DataTable();
            try
            {
                string NameTransaction = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                cmd.Transaction = cmd.Connection.BeginTransaction(NameTransaction);
                SqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                reader.Close();
                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                string Error = "";
                try { cmd.Transaction.Rollback(); }
                catch (Exception exTrans)
                { Error = $"[Error en la transaccion al hacer Rollback: ({exTrans.Message})]"; }
                throw new Exception(Tools.MsjError($"App_Code.DAOConfig.SetDataTableExecuteCommand{Error}", ex));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return table;
        }
        public async static Task<DataTable> MultiSetDataTableExecuteCommand(SqlCommand cmd, bool? Finalizar = null)
        {
            DataTable table = new DataTable();
            try
            {
                if (cmd.Transaction == null)
                {
                    string NameTransaction = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                    cmd.Transaction = cmd.Connection.BeginTransaction(NameTransaction);
                }
                SqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                reader.Close();
                if (Finalizar == true) cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                string Error = "";
                try { cmd.Transaction.Rollback(); }
                catch (Exception exTrans)
                {
                    Error = $"[Error en la transaccion al hacer Rollback: ({exTrans.Message})]";
                }
                throw new Exception(Tools.MsjError($"App_Code.DAOConfig.MultiSetDataTableExecuteCommand{Error}", ex));
            }
            finally
            {
                if (Finalizar == true) cmd.Connection.Close();
            }
            return table;
        }
        public async static Task<DataTable> MultiGetDataTableExecuteCommand(SqlCommand cmd)
        {
            DataTable table = new DataTable();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"App_Code.DAOConfig.MultiGetDataTableExecuteCommand({Tools.GetLineErr(ex)}): {ex.Message}");
            }
            return table;
        }
        //CONEX EXCEL
        public async static Task<OleDbCommand> OleCommandExcel(string conStringExcel)
        {
            OleDbConnection conex = new OleDbConnection(conStringExcel);
            OleDbCommand cmd = conex.CreateCommand();
            cmd.Connection.Open();
            return cmd;
        }
        public async static Task<DataTable> DataTableOleCommand(OleDbCommand cmd)
        {
            DataTable table = new DataTable();
            try
            {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                cmd.Connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.SelectCommand = cmd;
                da.Fill(table);
            }
            catch (Exception ex)
            {
                throw new Exception($"App_Code.DAOConfig.DataTableOleCommand({Tools.GetLineErr(ex)}): {ex.Message}");
            }
            finally
            {
                cmd.Connection.Close();
            }
            return table;
        }
    }
}
