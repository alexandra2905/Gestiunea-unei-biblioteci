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
using InterfataUtilizator.Biblioteca;

namespace InterfataUtilizator.Prezentare
{
    public partial class FormaAutor : Form

    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public FormaAutor()
        {
            InitializeComponent();
        }
        private void AfiseazaAutor()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from autorab";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        dgvAutor.DataSource = dt;
                    }

                    dgvAutor.Columns[0].Visible = true;
                    dgvAutor.Columns[0].HeaderText = "IDAutor";
                    dgvAutor.Columns[1].Visible = true;
                    dgvAutor.Columns[1].HeaderText = "Nume";
                    dgvAutor.Columns[2].Visible = true;
                    dgvAutor.Columns[2].HeaderText = "Prenume";

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

        private void FormaAutor_Load(object sender, EventArgs e)
        {
            AfiseazaAutor();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();

        }

        private void dgvAutor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = dgvAutor.CurrentCell.RowIndex;
            string IDAutor = dgvAutor[0, currentRowIndex].Value.ToString();
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from  Autorab  where IDAutor=" + IDAutor;
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    idautor.Text = dr.GetDecimal(0).ToString();
                    nume_aut.Text = dr.GetString(1);
                    prenum_aut.Text = dr.GetString(2);
                    
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

    

        private void button1_Click(object sender, EventArgs e)//buton adaugare
        {
            if (nume_aut.Text == "" || prenum_aut.Text == "")

            {
                MessageBox.Show("Eroare la completarea datelor !!!");
                nume_aut.BackColor = Color.OrangeRed;
                prenum_aut.BackColor = Color.OrangeRed;

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

                    String sqlCommand = "INSERT INTO Autorab VALUES";
    
                    sqlCommand += "( seq_autorab.nextval,'" + nume_aut.Text + "','" + prenum_aut.Text + "')";

                    MessageBox.Show(sqlCommand);

                    cmd.CommandText = sqlCommand;

                    int rezult = cmd.ExecuteNonQuery();
                    if (rezult > 0)
                    {
                        MessageBox.Show("Adaugat");
                        actualizeaza();
                        nume_aut.Clear();
                        prenum_aut.Clear();
                        nume_aut.BackColor = Color.White;
                        prenum_aut.BackColor = Color.White;

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
            OracleDataAdapter oda = new OracleDataAdapter("select * from Autorab", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dgvAutor.DataSource = dt;
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


                String sqlCommand = "UPDATE Autorab set Nume = '";
                sqlCommand += nume_aut.Text + "'";
                sqlCommand += ", Prenume = '" + prenum_aut.Text + "'";
                sqlCommand += " where IDAutor = " + idautor.Text;



                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Updated");
                    nume_aut.Clear();
                    prenum_aut.Clear();
                    
                    actualizeaza();
                    idautor.Clear();
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

                String sqlCommand = "DELETE FROM Autorab WHERE IDAutor = '";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }




