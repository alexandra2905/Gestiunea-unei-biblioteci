using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfataUtilizator.AccesDate;
using Oracle.DataAccess.Client;

namespace InterfataUtilizator.Prezentare
{
    public partial class FormaClient : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public FormaClient()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = dgvClient.CurrentCell.RowIndex;
            string IDClient = dgvClient[0, currentRowIndex].Value.ToString();
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from  Clientab  where IDClient=" + IDClient;
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    idclient.Text = dr.GetDecimal(0).ToString();
                    nume.Text = dr.GetString(1);
                    prenum.Text = dr.GetString(2);
                    cnp.Text = dr.GetString(3);
                    adresa.Text = dr.GetString(4);
                   


                }
                conn.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nume.Text == "" || adresa.Text == "" || prenum.Text == "" || cnp.Text == "" )

            {
                MessageBox.Show("Eroare la completarea datelor !!!");
                nume.BackColor = Color.OrangeRed;
                adresa.BackColor = Color.OrangeRed;
                prenum.BackColor = Color.OrangeRed;
                cnp.BackColor = Color.OrangeRed;

            }
            else
            {
                try
                {
                    OracleConnection conn = new OracleConnection(CONNECTION_STRING);

                    //deschiderea conexiunii
                    conn.Open();

                    //comanda sql care poate fi interogare sql, procedura stocata etc...
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    String sqlCommand = "INSERT INTO Clientab  VALUES";
                    sqlCommand += "(seq_clientab.nextval, '" + nume.Text + "','" + prenum.Text + "','" + cnp.Text + "','" + adresa.Text + "')";

                    MessageBox.Show(sqlCommand);

                    cmd.CommandText = sqlCommand;

                    int rezult = cmd.ExecuteNonQuery();
                    if (rezult > 0)
                    {
                        MessageBox.Show("Client adaugat");
                        actualizeaza();
                        nume.Clear();
                        prenum.Clear();
                        adresa.Clear();
                        cnp.Clear();
                        nume.BackColor = Color.White;
                        adresa.BackColor = Color.White;
                        prenum.BackColor = Color.White;
                        cnp.BackColor = Color.White;
                    }
                    else
                    {
                        MessageBox.Show("Eroare");
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exceptie" + ex.Message);
                }
            }
        }
        public void actualizeaza()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from Clientab", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dgvClient.DataSource = dt;
            conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OracleConnection conn = new OracleConnection(CONNECTION_STRING);

                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;


                String sqlCommand = "UPDATE Clientab set Nume = '";
                sqlCommand += nume.Text + "'";
                sqlCommand += ", Prenume = '" + prenum.Text + "'";
                sqlCommand += ",CNP= '" + cnp.Text + "'";
                sqlCommand += ",Adresa= '" + adresa.Text + "'";

                sqlCommand += " where IDClient = " + idclient.Text;



                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Updated");
                    nume.Clear();
                    prenum.Clear();
                    cnp.Clear();
                    adresa.Clear();

                    actualizeaza();
                    idclient.Clear();
                }
                else
                {
                    MessageBox.Show("Error");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OracleConnection conn = new OracleConnection(CONNECTION_STRING);
                int Delete = Convert.ToInt32(textBox1.Text);

                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                String sqlCommand = "DELETE FROM Clientab WHERE IDClient = '";
                sqlCommand += Delete + "'";

                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Sters!");
                    actualizeaza();
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("Eroare");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exceptie" + ex.Message);
            }
        }
        private void AfiseazaCarti()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from Clientab";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        dgvClient.DataSource = dt;
                    }

                    dgvClient.Columns[0].Visible = true;
                    dgvClient.Columns[0].HeaderText = "IDClient";
                    dgvClient.Columns[1].Visible = true;
                    dgvClient.Columns[1].HeaderText = "Nume";
                    dgvClient.Columns[2].Visible = true;
                    dgvClient.Columns[2].HeaderText = "Prenume";
                    dgvClient.Columns[3].Visible = true;
                    dgvClient.Columns[3].HeaderText = "CNP";
                    dgvClient.Columns[4].Visible = true;
                    dgvClient.Columns[4].HeaderText = "Adresa";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }

        }

        private void FormaClient_Load(object sender, EventArgs e)
        {
            AfiseazaCarti();
        }
    }
}
    
