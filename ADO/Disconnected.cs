using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class Disconnected
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security =true; Initial Catalog= CinemaDB; Server = WINAPBY2JK8NYD0\SQLEXPRESS";

        public static void DisconnectedMood()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Costruzione addapter
                SqlDataAdapter adapter = new SqlDataAdapter();

                //Creazione comandi da aassociare all'adapter

                SqlCommand selectCommand = new SqlCommand();
                selectCommand.Connection = connection;
                selectCommand.CommandType = System.Data.CommandType.Text;
                selectCommand.CommandText = "SELECT * FROM Movies";


                SqlCommand insertCommand = new SqlCommand();
                insertCommand.Connection = connection;
                insertCommand.CommandType = System.Data.CommandType.Text;
                insertCommand.CommandText = "INSERT INTO Movies VALUES(@Titolo, @Genere, @Durata)";

                insertCommand.Parameters.Add("@Titolo", System.Data.SqlDbType.NVarChar,255,"Titolo");
                insertCommand.Parameters.Add("@Genere", System.Data.SqlDbType.NVarChar, 255, "Genere");
                insertCommand.Parameters.Add("@Durata", System.Data.SqlDbType.Int, 500,"Durata");

                //...

                //Associa i comandi all'addapter
                adapter.SelectCommand = selectCommand;
                adapter.InsertCommand = insertCommand;


                DataSet dataSet = new DataSet();

                try
                {
                    //Scarica la tabella
                    connection.Open();
                    adapter.Fill(dataSet, "Movies");

                    foreach (DataRow row in dataSet.Tables["Movies"].Rows)
                    {
                        Console.WriteLine("Row: {0}", row["Titolo"]);
                    }

                    //Creazione Record
                    DataRow movie = dataSet.Tables["Movies"].NewRow(); //sto definendo una rigga, e questa new row sara movie
                    movie["Titolo"] = "V per Vendetta";
                    movie["Genere"] = "Azione";
                    movie["Durata"] = 125;//ho deffinito come e movie

                    //Agiungo la rigga movies alla dataSet
                    dataSet.Tables["Movies"].Rows.Add(movie);

                    //Update del db
                    adapter.Update(dataSet, "Movies"); // adapter.update utilizza insert, aggiorna la tabella in base alle modifiche che ho fatto nel data set



                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    connection.Close();
                }

            }

        }
    }
}
