using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BookLibEFC.Properties.Resources;

namespace BookLibEFC
{
    public partial class formStart : Form
    {
        public const string AuthorsTitle = "Авторы";
        public const string BooksTitle = "Книги";
        public const string CopiesTitle = "Экземпляры";
        public const string ReadersTitle = "Читатели";
        public const string RecordsTitle = "Журнал";

        Menu menu;
        ToolStrip tools;
        List<Form> formsList = new();
        LibContext db;
        ToolStripTextBox textView = new ToolStripTextBox();

        public formStart()
        {
            InitializeComponent();

            Icon = MainIcon;
            Text = "Библиотека";
            IsMdiContainer = true;

            menu = new Menu(this);
            tools = new ToolStrip();

            tools.Items.Add(textView);

            Controls.Add(tools);
            Controls.Add(menu);

            Load += (sender, e) =>
                db = (new LibContextFactory()).CreateDbContext(null);
            FormClosed += (sender, e) => db.Dispose();
        }

        void ShowData(string itemsTitle)
        {
            var form = formsList.FirstOrDefault
                (f => f.Text == itemsTitle);
            if (form == null)
            {
                switch(itemsTitle)
                {
                    case AuthorsTitle:
                        form = new formGrid(db, itemsTitle);
                        break;
                    case BooksTitle:
                        form = new formGridRef(db, itemsTitle);
                        break;
                }
                form.MdiParent = this;
                form.FormClosed += (sender, e) => Save(form);
                form.Show();
                formsList.Add(form);
            }
            else
            {
                if (form.WindowState == FormWindowState.Minimized)
                {
                    form.WindowState = FormWindowState.Normal;
                }
                form.Activate();
            }
        }
        void Save(Form form)
        {
            db.SaveChanges();
            formsList.Remove(form);
        }

        void GetBookCount()
        {
            textView.Text = db.Books.Count().ToString();
        }

        class Menu : MenuStrip
        {
            public Menu(formStart start)
            {
                var fileMenu = new ToolStripMenuItem("&Файл");

                var authorsCommand = new ToolStripMenuItem
                    ("&Авторы", null,
                    (sender, e) => start.ShowData(AuthorsTitle),
                    Keys.Control | Keys.A);
                var booksCommand = new ToolStripMenuItem
                    ("&Книги", null,
                    (sender, e) => start.ShowData(BooksTitle),
                    Keys.Control | Keys.B);
                var copiesCommand = new ToolStripMenuItem
                    ("&Экземпляры", null,
                    (sender, e) => start.ShowData(CopiesTitle),
                    Keys.Control | Keys.C);
                var readersCommand = new ToolStripMenuItem
                    ("&Читатели", null,
                    (sender, e) => start.ShowData(ReadersTitle),
                    Keys.Control | Keys.R);
                var recordsCommand = new ToolStripMenuItem
                    ("&Журнал", null,
                    (sender, e) => start.ShowData(RecordsTitle),
                    Keys.Control | Keys.L);
                var exitCommand = new ToolStripMenuItem
                    ("В&ыход", null,
                    (sender, e) => start.Close());

                fileMenu.DropDownItems.AddRange(new ToolStripItem[]
                    {authorsCommand, booksCommand, copiesCommand,
                    readersCommand, recordsCommand, new ToolStripSeparator(),
                    exitCommand});

                var queryMenu = new ToolStripMenuItem("&Запросы");
                var bookCountCommand = new ToolStripMenuItem
                    ("&Количество книг", null,
                    (sender, e) => start.GetBookCount());
                queryMenu.DropDownItems.AddRange(new ToolStripItem[]
                    {bookCountCommand});

                Items.AddRange(new[] { fileMenu, queryMenu,  });
            }
        }
    }
}
