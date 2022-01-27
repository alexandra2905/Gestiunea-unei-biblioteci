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
    public partial class FormaImprumut : Form
    {
        private static string CONNECTION_STRING = "Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;";
        public FormaImprumut()
        {
            InitializeComponent();
        }
        private void IncarcaCarte()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                //deschiderea conexiunii
                conn.Open();

                //comanda sql care poate fi interogare sql, procedura stocata etc...
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from Imprumutab";
                cmd.CommandType = CommandType.Text;

                //executia comenzii
                OracleDataReader dr = cmd.ExecuteReader();
                

                //preluarea datelor și plasarea lor într-un combobox

                while (dr.Read())
                {
                    comboNumecarte.Items.Add(new ComboItem(dr.GetString(1), (Int32)dr.GetDecimal(0)));
                }
                
                //inchiderea conexiunii
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

        private void comboNumecarte_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AfiseazaImprumut()
        {
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(CONNECTION_STRING))
                {
                    //deschiderea conexiunii
                    conn.Open();
                    string sqlCommand = "select * from Imprumutab";

                    // creare obiect OracleDataAdapter
                    using (OracleDataAdapter oda = new OracleDataAdapter(sqlCommand, conn))
                    {
                        // Utilizare DataAdapter pentru a seta datele intr-un DataTable
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        dgvImprum.DataSource = dt;
                    }

                    dgvImprum.Columns[0].Visible = true;
                    dgvImprum.Columns[0].HeaderText = "IDCarte";
                    dgvImprum.Columns[1].Visible = true;
                    dgvImprum.Columns[1].HeaderText = "IDClient";
                    dgvImprum.Columns[2].Visible = true;
                    dgvImprum.Columns[2].HeaderText = "Data de_la...";
                    dgvImprum.Columns[3].Visible = true;
                    dgvImprum.Columns[3].HeaderText = "Pana la...";
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

        private void FormaImprumut_Load(object sender, EventArgs e)
        {
            comboNumecarte.Items.Add("Amintiri din Copilarie");
            
            IncarcaCarte();
           
        }
        public void actualizeaza()
        {
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);
            conn.Open();
            OracleDataAdapter oda = new OracleDataAdapter("select * from Imprumutab", conn);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            dgvImprum.DataSource = dt;
            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)//buton adaugare
        {
            if (comboNumecarte.Text == "" || data_dela.Text == "" || pana_la.Text == "")

            {
                MessageBox.Show("Eroare la completarea datelor !!!");
                comboNumecarte.BackColor = Color.OrangeRed;
                data_dela.BackColor = Color.OrangeRed;
                pana_la.BackColor = Color.OrangeRed;

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

                    String sqlCommand = "INSERT INTO Imprumutab VALUES";
                    sqlCommand += "(seq_imprumutab.NEXTVAl ,to_date('" + data_dela.Text + "', 'DD-MM-YYYY','NLS_DATE_LANGUAGE = American')," + pana_la.Text + "', 'DD-MM-YYYY','NLS_DATE_LANGUAGE = American')," + ((ComboItem)comboNumecarte.SelectedItem).Value +")";

                    MessageBox.Show(sqlCommand);

                    cmd.CommandText = sqlCommand;

                    int rezult = cmd.ExecuteNonQuery();
                    if (rezult > 0)
                    {
                        MessageBox.Show("Adaugat");
                       
                        comboNumecarte.Items.Clear();
                        data_dela.Clear();
                        pana_la.Clear();
                        comboNumecarte.BackColor = Color.White;
                        data_dela.BackColor = Color.White;
                        pana_la.BackColor = Color.White;
                        actualizeaza();


                        IncarcaCarte();
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

        private void button2_Click(object sender, EventArgs e)//buton modificare 
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


                String sqlCommand = "UPDATE Imprumutab set Nume = '";
                sqlCommand += comboNumecarte.Text + "'";
                sqlCommand += ", Data de_la = '" + data_dela.Text + "'";
                sqlCommand += ", Pana la = '" + pana_la.Text + "'";

                sqlCommand += " where IDCarte = " + idcarte.Text;
                sqlCommand += " where IDClient = " + idclient.Text;





                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Actualizat");
                    comboNumecarte.Items.Clear();
                    data_dela.Clear();
                    pana_la.Clear();

                    actualizeaza();
                    idcarte.Clear();
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
    

        private void button3_Click(object sender, EventArgs e)//buton stergere
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

                String sqlCommand = "DELETE FROM Imprumutab WHERE IDCarte = '";
                sqlCommand += Delete + "'";

                cmd.CommandText = sqlCommand;

                int rezult = cmd.ExecuteNonQuery();
                if (rezult > 0)
                {
                    MessageBox.Show("Sters!");
                    actualizeaza();
                    textBox1.Clear();
                    Refresh();
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

        private void dgvImprum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex =dgvImprum.CurrentCell.RowIndex;
            string IDAutor = dgvImprum[0, currentRowIndex].Value.ToString();
            OracleConnection conn = new OracleConnection(CONNECTION_STRING);

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from  Imprumutab  where IDClient=" +idclient;
                cmd.CommandText = "select * from  Imprumutab  where IDCarte=" + idcarte;
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    idcarte.Text = dr.GetDecimal(0).ToString();
                    idclient.Text = dr.GetDecimal(0).ToString();
                    comboNumecarte.Text = dr.GetString(1);
                    data_dela.Text = dr.GetString(2);
                    pana_la.Text = dr.GetString(2);

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

