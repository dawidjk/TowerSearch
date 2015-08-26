﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Data.SqlClient;

namespace TowerSearch
{
    public partial class Borrow : Form
    {
        public Borrow()
        {
            InitializeComponent();
        }

        [Table(Name = "Log")]
        public class log
        {
            [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false)]
            public int Id;
            [Column(CanBeNull = false)]
            public string FirstName;
            [Column(CanBeNull = false)]
            public string LastName;
            [Column(CanBeNull = false)]
            public string Grade;
            [Column(CanBeNull = false)]
            public string PartName;
            [Column(CanBeNull = false)]
            public int isOut;
            [Column(CanBeNull = false)]
            public string Quantity;
        }

        const string conString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=X:\TowerSearch\TowerSearch\Parts.mdf;Integrated Security=True";
        static DataClasses1DataContext databaseLogging = new DataClasses1DataContext(conString);
        static Table<Log> listOfPeople = databaseLogging.GetTable<Log>();


        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("spCheckIfExists", new SqlConnection(conString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pName", pName.Text);

            cmd.Connection.Open();

            var check = cmd.ExecuteScalar();

            if (check == null)
            {
                 MessageBox.Show("This part does not exist!");
                this.Close();
            }
            else
            {
                double n;
                if (double.TryParse(Quantity.Text, out n))
                {
                    if (double.TryParse(Grade.Text, out n))
                    {
                        int id = listOfPeople.Max(log => log.Id) + 1;

                        Log newPerson = new Log
                        {
                            Id = id,
                            FirstName = fName.Text,
                            LastName = lName.Text,
                            Grade = Grade.Text,
                            Quantity = Quantity.Text,
                            PartName = pName.Text,
                            isOut = 1
                        };
                        listOfPeople.InsertOnSubmit(newPerson);
                        databaseLogging.SubmitChanges();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Grade is not a number!");
                    }
                }
                else
                {
                    MessageBox.Show("Quantity is not a number!");
                }
            }
        }

        private void fName_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void lName_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void Grade_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void pName_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void Quantity_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                button1_Click(sender, e);
            }
        }

        private void Borrow_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("SELECT PartName FROM Parts", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                AutoCompleteStringCollection myCollection = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    myCollection.Add(reader.GetString(0));
                }
                pName.AutoCompleteCustomSource = myCollection;
                con.Close();
            }
        }
    }
}
