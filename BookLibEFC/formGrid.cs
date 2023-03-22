using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using static BookLibEFC.Properties.Resources;

namespace BookLibEFC
{
    public partial class formGrid : Form
    {
        protected DataGridView grid;
        protected LibContext db;
        public formGrid(LibContext db, string title)
        {
            InitializeComponent();

            Icon = MainIcon;
            Text = title;

            this.db = db;
            Load += (sender, e) => Open();
        }

        public virtual void Open()
        {
            grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            Controls.Add(grid);
            
            switch(Text)
            {
                case formStart.AuthorsTitle:
                    db.Authors.Load();
                    grid.DataSource = db.Authors.Local.ToBindingList();
                    break;
            }
        }
    }
}
