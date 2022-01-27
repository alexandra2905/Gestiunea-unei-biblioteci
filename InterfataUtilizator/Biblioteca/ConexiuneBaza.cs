using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace InterfataUtilizator.Biblioteca
{
    class ConexiuneBaza
    {
        private string ConexiuneLant=("Data Source=80.96.123.131/ora09;User Id=hr;Password=oracletest;");
        OracleConnection Conex;
        public OracleConnection StabilireConexiune()
        {
            OracleConnection Conex = new OracleConnection();
            this.Conex = new OracleConnection(this.ConexiuneLant);
            return this.Conex;
        }
        //Metaoda insert, modificare, delete
        public bool ExecutaComandaFaraReturnDate(string strComando)
        {
            try
            {
                //OracleConnection Conex = new OracleConnection();
                OracleCommand Comand = new OracleCommand();

                Comand.CommandText = strComando;//"Select *from Cartiab";
                Comand.Connection = this.StabilireConexiune();
                Conex.Open();
                Comand.ExecuteNonQuery();
                Conex.Close();
                return true;
            }
            catch
            {
                return false;
            }
            }
        }
    //select(returneaza date)

    }

