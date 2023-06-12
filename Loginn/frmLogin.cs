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



namespace Loginn
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Server=OWEN_LAPTOP;Database=UsuariosDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string consulta = "select * from UsuariosDB where UserName= '" + txtUsuario.Text + "', and Password= '" + txtPassword + "'";
                SqlCommand comm = new SqlCommand(consulta, con);
                SqlDataReader lector;
                lector = comm.ExecuteReader();

                if (lector.HasRows == true)
                {
                    MessageBox.Show("Inicio de sesión exitoso");

                    frmNoImporta frm2 = new frmNoImporta();
                    this.Hide();
                    frm2.Show();
                }
                else
                {
                    MessageBox.Show("Datos incorrectos, ingreselos de nuevo");
                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
