using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class ConnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security =true; Initial Catalog= CinemaDB; Server = WINAPBY2JK8NYD0\SQLEXPRESS";
        

        public static void Connected()
        {
            //Creare una connnesione 

            //Metodo 1:
            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = connectionString;


            ////Metodo 2:
            //SqlConnection connection = new SqlConnection(connectionString);

            //Metodo 3:
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connesione

                connection.Open();

                //Creare command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text; //tipo di commando
                command.CommandText = "SELECT * FROM Movies";

                //Eseguire command
                SqlDataReader reader = command.ExecuteReader();

                //Leggere i dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2} {3}",
                        reader["ID"],
                        reader["Titolo"],
                        reader["Genere"],
                        reader["Durata"]);
                }

                //Chiudere la connesione
                reader.Close();
                connection.Close();
            }



        }
        public static void ConnectedWithParameter()
        {
            //Creare connesione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apriam una connesione
                connection.Open();

                //Inserimento parametro da riga di commando
                System.Console.WriteLine("Genere del Film: ");
                string Genere;
                Genere = System.Console.ReadLine();

                //Aprire la connesione
               // connection.Open();

                //Creare il command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Movies WHERE Genere = @Genere";

               // Creare parametro
                SqlParameter generePara = new SqlParameter();
                generePara.ParameterName = "@Genere";
                generePara.Value = Genere;
                command.Parameters.Add(generePara);

                //  command.Parameters.AddWithValue("@Genere", Genere);

                //Eseguire il command
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dei dati 
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2}",
                        reader["Id"],
                        reader["Titolo"],
                        reader["Genere"]);
                }
                //Chiudere la connesione
                reader.Close();
                connection.Close();
            }
        }
        public static void ConnectedStoredProcedure()
        {
            //Crea connesione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apriam una connesione
                connection.Open();

                //Creare command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "stpGetActorByCachetRange";

                //Creare Parametri
                command.Parameters.AddWithValue("@min_cachet", 5000);
                command.Parameters.AddWithValue("@max_cachet", 9000);

                //Creare valore di ritorno
                SqlParameter returnValue = new SqlParameter();
                returnValue.ParameterName = "@returnedCount";
                returnValue.SqlDbType = System.Data.SqlDbType.Int;

                returnValue.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValue);

                //Eseguire il commando
                SqlDataReader reader = command.ExecuteReader();


                //Visualizzazione dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2} {3}",
                        reader["ID"],
                        reader["FirstName"],
                        reader["LastName"],
                        reader["Cachet"]);
                }
                reader.Close();


                //  command.ExecuteNonQuery(); imi arata doar numarul nu si actorii

                Console.WriteLine("#Actors: {0}", command.Parameters["@returnedCount"].Value);
                connection.Close();

            }
        }
        public static void ConnectedScalar()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand scalarCommand = new SqlCommand();
                scalarCommand.Connection = connection;
                scalarCommand.CommandType = System.Data.CommandType.Text;
                scalarCommand.CommandText = "SELECT COUNT(*) FROM Movies";

                int count = (Int32)scalarCommand.ExecuteScalar();

                Console.WriteLine("Contegio dei film: {0}", count);

                connection.Close();
            }
        }

    }

}
