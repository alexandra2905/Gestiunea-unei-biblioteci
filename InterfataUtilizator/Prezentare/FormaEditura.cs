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
    public partial class FormaEditura : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public FormaEditura()
        {
            InitializeComponent();
        }
        private void AfiseazaEditura()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from edituraab";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        dgvEdit.DataSource = dt;
                    }

                    dgvEdit.Columns[0].Visible = true;
                    dgvEdit.Columns[0].HeaderText = "IDEdit";
                    dgvEdit.Columns[1].Visible = true;
                    dgvEdit.Columns[1].HeaderText = "Nume";
                    dgvEdit.Columns[2].Visible = true;
                    dgvEdit.Columns[2].HeaderText = "Adresa";
                    dgvEdit.Columns[3].Visible = true;
                    dgvEdit.Columns[3].HeaderText = "Telefon";

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

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void FormaEditura_Load(object sender, EventArgs e)
        {
            AfiseazaEditura();
        }

        private void button1_Click(object sender, EventArgs e)//buton adaugare
        {
            if (nume.Text==""|| adresa.Text==""||  telefon.Text==" " )
            
            {
                MessageBox.Show("Eroare la completarea datelor !!!");
                nume.BackColor = Color.OrangeRed;
                adresa.BackColor = Color.OrangeRed;
                nume.BackColor = Color.OrangeRed;
                telefon.BackColor = Color.OrangeRed;

            }
            else {
                try
                {
                    OracleConnection conn = new OracleConnection(CONNECTION_STRING);

                    //deschiderea conexiunii
                    conn.Open();

                    //comanda sql care poate fi interogare sql, procedura stocata etc...
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    String sqlCommand = "INSERT INTO Edituraab  VALUES";
                    sqlCommand += "(seq_edituraab.nextval, '" + nume.Text + "','" + adresa.Text + "'," + int.Parse(telefon.Text) + ")";
                   
                    MessageBox.Show(sqlCommand);

                    cmd.CommandText = sqlCommand;

                    int rezult = cmd.ExecuteNonQuery();
                    if (rezult > 0)
                    {
                        MessageBox.Show("Adaugat");
                        actualizeaza();
                        nume.Clear();
                        adresa.Clear();
                        telefon.Clear();
                        nume.BackColor = Color.White;
                        adresa.BackColor = Color.White;
                        telefon.BackColor = Color.White;
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
            OracleDataAdapter oda = new OracleDataAdapter("select * from edituraab", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dgvEdit.DataSource = dt;
            conn.Close();

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

                String sqlCommand = "DELETE FROM edituraab WHERE idedit = '";
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


                String sqlCommand = "UPDATE Edituraab set Nume = '";
                sqlCommand += nume.Text + "'";
                sqlCommand += ", Adresa = '" + adresa.Text + "'";
                sqlCommand += ", Telefon  = '" + telefon.Text + "'";   
                sqlCommand += " where IDEdit = " + idedit.Text;



                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Updated");
                    nume.Clear();
                    adresa.Clear();
                    telefon.Clear();
                    actualizeaza();
                    idedit.Clear();
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

        private void dgvEdit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = dgvEdit.CurrentCell.RowIndex;
            string IDEdit = dgvEdit[0, currentRowIndex].Value.ToString();
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from  Edituraab  where IDEdit=" + IDEdit;
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    idedit.Text = dr.GetDecimal(0).ToString();
                    nume.Text = dr.GetString(1);
                    adresa.Text = dr.GetString(2);
                    telefon.Text = dr.GetString(3);
                    
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
    }
}
