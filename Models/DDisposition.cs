using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace AIBTicketsMVC.Models
{
    public class DDisposition
    {
        string cadenaConexion = "";
        SqlConnection con;

        public DDisposition()
        {
            cadenaConexion = ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString;
            con = new SqlConnection(cadenaConexion);
        }
        // GRABACION
        public DataTable ObtenerArea()
        {
            string Query = "Sp_DispositionGet";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable ObtenerCuenta()
        {
            string Query = "Sp_CuentaGet";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable ObtenerEstado()
        {
            string Query = "Sp_Sel_Estado_Reclamo";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable ObtenerAllEstados()
        {
            string Query = "Sp_Sel_AllEstado_Reclamo";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("Id", estado.Id);
            //if (estado.Id != null) cmd.Parameters.AddWithValue("Id", estado.Id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable ObtenerSubEstado()
        {
            string Query = "Sp_Sel_SubEstado_Reclamo";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        public DataTable gestionarEstado(Disposition estado)
        {
            string Query = "Sp_Sel_AllEstado_Reclamo";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("Id", estado.Id);
            //if (estado.Id != null) cmd.Parameters.AddWithValue("Id", estado.Id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            return dt;
        }

    }
}