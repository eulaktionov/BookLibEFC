using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookLibEFC
{
    public partial class DetailForm : Form
    {
        LibContext db;
        public DetailForm(LibContext libContext)
        {
            InitializeComponent();
            db = libContext;
        }

        private void DetailForm_Load(object sender, EventArgs e)
        {
            cbAuthors.DataSource = db.Authors.ToList();
            cbAuthors.DisplayMember = "LastName";
            cbAuthors.ValueMember = "Id";
        }
    }
}
