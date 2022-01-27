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
    public partial class FormaCarte : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public FormaCarte()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)//buton adaugare
        {
            if (titlu.Text == "" || cantit.Text == "" || isbn.Text == "" || an.Text == ""|| limba.Text == "" || tip.Text == "" )

            {
                MessageBox.Show("Eroare la completarea datelor !!!");
                titlu.BackColor = Color.OrangeRed;
                cantit.BackColor = Color.OrangeRed;
                isbn.BackColor = Color.OrangeRed;
                an.BackColor = Color.OrangeRed;
                limba.BackColor = Color.OrangeRed;
                tip.BackColor = Color.OrangeRed;
               

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

                    String sqlCommand = "INSERT INTO Cartiab  VALUES";
                    sqlCommand += "(seq_cartiab.nextval ," + "idedit.nextval,'" + titlu.Text + "','" + isbn.Text + "'," + int.Parse(an.Text) + ",'" + limba.Text + "'," + cantit.Text + ")";

                    MessageBox.Show(sqlCommand);

                    cmd.CommandText = sqlCommand;

                    int rezult = cmd.ExecuteNonQuery();
                    if (rezult > 0)
                    {
                        MessageBox.Show("Carte adaugata cu succes!");
                        actualizeaza();
                        titlu.Clear();
                        isbn.Clear();
                        cantit.Clear();
                        an.Clear();
                        limba.Clear();
                        tip.Clear();


                        titlu.BackColor = Color.White;
                         isbn.BackColor = Color.White;
                        cantit.BackColor = Color.White;
                        an.BackColor = Color.White;
                        limba.BackColor = Color.White;
                        tip.BackColor = Color.White;

                    }
                    else
                    {
                        MessageBox.Show("Eroare!!!");
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exceptie\n" + ex.Message);
                }

            }
        }


        private void dgvCarte_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = dgvCarte.CurrentCell.RowIndex;
            string IDCarte = dgvCarte[0, currentRowIndex].Value.ToString();
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from Cartiab where IDCarte=" + IDCarte;
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    label1.Text = dr.GetDecimal(0).ToString();
                    titlu.Text = dr.GetString(1);
                    isbn.Text = dr.GetString(2);
                    an.Text = dr.GetString(3);
                    limba.Text = dr.GetString(4);
                    tip.Text = dr.GetString(5);
                    cantit.Text = dr.GetString(6);
                    idedit.Text = dr.GetString(7);

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

        private void button4_Click(object sender, EventArgs e)//revenire la forma initiala
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void FormaCarte_Load(object sender, EventArgs e)
        {
            AfiseazaCarti();
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
                    string sqlCommand = "select * from Cartiab";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        dgvCarte.DataSource = dt;
                    }

                    dgvCarte.Columns[0].Visible = true;
                    dgvCarte.Columns[0].HeaderText = "IDCarte";
                    dgvCarte.Columns[1].Visible = true;
                    dgvCarte.Columns[1].HeaderText = "IDEdit";
                    dgvCarte.Columns[2].Visible = true;
                    dgvCarte.Columns[2].HeaderText = "ISBN";
                    dgvCarte.Columns[3].Visible = true;
                    dgvCarte.Columns[3].HeaderText = "An";
                    dgvCarte.Columns[4].Visible = true;
                    dgvCarte.Columns[4].HeaderText = "Limba";
                    dgvCarte.Columns[5].Visible = true;
                    dgvCarte.Columns[5].HeaderText = "Tip";
                    dgvCarte.Columns[6].Visible = true;
                    dgvCarte.Columns[6].HeaderText = "Cantit";
                    

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

        private void button3_Click(object sender, EventArgs e)//stergere
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            int Delete = Convert.ToInt32(idcarte.Text);

            //deschiderea conexiunii
            conn.Open();

            //comanda sql care poate fi interogare sql, procedura stocata etc...
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            String sqlCommand = "DELETE FROM cartiab WHERE idcarte = '";
            sqlCommand += Delete + "'";

            cmd.CommandText = sqlCommand;

            int rezult = cmd.ExecuteNonQuery();
            if (rezult > 0)
            {
                MessageBox.Show("S-a efectuat stergerea!");
                actualizeaza();
                textBox1.Clear();

            }
            else
            {
                MessageBox.Show("Eroare");
            }

            conn.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tip_TextChanged(object sender, EventArgs e)
        {

        }
        public void actualizeaza()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from Cartiab", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dgvCarte.DataSource = dt;
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


                String sqlCommand = "UPDATE Cartiab set Nume = '";
                sqlCommand += titlu.Text + "'";
                sqlCommand += ", ISBN = '" + isbn.Text + "'";
                sqlCommand += ",An= '" + an.Text + "'";
                sqlCommand += ",Limba= '" + limba.Text + "'";
                sqlCommand += ",Tip= '" + tip.Text + "'";
                sqlCommand += ",Cantit= '" + cantit.Text + "'";
                sqlCommand += " where IDEdit = " + idedit.Text;
                sqlCommand += " where IDCarte = " + idcarte.Text;
                



                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Updated");
                    titlu.Clear();
                   isbn.Clear();
                    an.Clear();
                    limba.Clear();
                    cantit.Clear();

                    actualizeaza();
                    idedit.Clear();
                    idcarte.Clear();
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
    }
    }

    

