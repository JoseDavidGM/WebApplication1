using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string message;
            using (SqlConnection SqlConn = new SqlConnection(@"Data Source=JOSEDAVID; initial Catalog=App1; Integrated Security=True;"))
            {
                SqlConn.Open();
                /* -- FUNCIONA --
                string query = "select count(1) from [App1].dbo.[users] where [user] = @UserName and [password] = @Password";
                SqlCommand sqlCmd = new SqlCommand(query, SqlConn);
                sqlCmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    Session["username"] = txtUsername.Text.Trim();
                    Response.Redirect("HelloWorld.aspx");
                }
                else {
                    lblErrorMessage.Visible = true;
                }
                */
                SqlCommand cmd = new SqlCommand("[SP_login_logout]", SqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("opt", "1");
                cmd.Parameters.AddWithValue("user", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("pass", txtPassword.Text.Trim());
                cmd.Parameters.Add("@rol", SqlDbType.NVarChar, 400);
                cmd.Parameters["@rol"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@mess", SqlDbType.NVarChar, 400);
                cmd.Parameters["@mess"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                message = (string)cmd.Parameters["@mess"].Value;

                if (message == "Connected")
                {
                    Session["username"] = txtUsername.Text.Trim();
                    Response.Redirect("HelloWorld.aspx");
                }
                else
                {
                    lblErrorMessage.Text = message;
                    lblErrorMessage.Visible = true;
                }
                SqlConn.Close();
            }
        }
    }
}