using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;


namespace BookLibEFC
{
    public class formGridRef : formGrid
    {
        public formGridRef(LibContext db, string title) : base(db,title)
        {
        }
        public override void Open()
        {
            base.Open();
            EditButtons editButtons = new EditButtons();
            Controls.Add(editButtons);
            editButtons.Dock = DockStyle.Bottom;

            editButtons.buttonAdd.Click += (sender, e) => AddRecord();
            editButtons.buttonUpdate.Click += (sender, e) => UpdateRecord();
            editButtons.buttonDelete.Click += (sender, e) => DeleteRecord();

            switch (Text)
            {
                case formStart.BooksTitle:
                    ShowBooksGrid();                    break;
            }
        }

        void AddRecord()
        {
            DetailForm detailForm = new DetailForm(db);
            detailForm.ShowDialog();
            String name = detailForm.tbBookName.Text;
            Author author = (Author)detailForm.cbAuthors.SelectedItem;

            db.Books.Add(new Book { Name = name, Author = author });
            db.SaveChanges();
            ShowBooksGrid();
        }

        void DeleteRecord()
        {
            if (grid.SelectedRows.Count > 0)
            {
                Int32 index = grid.SelectedRows[0].Index;
                Int32 id = 0;
                Boolean converted = Int32.TryParse
                    (grid[0, index].Value.ToString(), out id);
                if (converted == false) { return; }
                Book book = db.Books.Find(id);
                db.Books.Remove(book);
                db.SaveChanges();
                ShowBooksGrid();
            }
        }

        void UpdateRecord()
        {
            DetailForm detailForm = new DetailForm(db);
            if (grid.SelectedRows.Count > 0)
            {
                Int32 index = grid.SelectedRows[0].Index;
                Int32 id = 0;
                Boolean converted = Int32.TryParse
                    (grid[0, index].Value.ToString(), out id);
                if (converted == false) { return; }
                Book book = db.Books.Find(id);
                detailForm.tbBookName.Text = book.Name;
                detailForm.cbAuthors.SelectedItem = book.Author;
                detailForm.ShowDialog();
                book.Name = detailForm.tbBookName.Text;
                book.Author = (Author)detailForm.cbAuthors.SelectedItem;
            }
            db.SaveChanges();
            ShowBooksGrid();
        }
        void ShowBooksGrid()
        {
            grid.DataSource = null;
            var queryBooks = db.Books.Include(b => b.Author)
                .Select(x => new {
                    x.Id,
                    x.Name,
                    AuthorName = x.Author.FirstName
                + " " + x.Author.LastName
                });
            grid.DataSource = queryBooks.ToList();
        }
    }

    public class EditButtons : Panel
    {
        public EditButton buttonAdd;
        public EditButton buttonUpdate;
        public EditButton buttonDelete;
        public EditButtons()
        {
            Height = 30;

            buttonAdd = new ("&Добавить");
            buttonUpdate = new ("&Заменить");
            buttonDelete = new ("&Удалить");

            buttonAdd.Dock = DockStyle.Left;
            buttonDelete.Dock = DockStyle.Right;
            buttonUpdate.Dock = DockStyle.Fill;

            Controls.AddRange(new EditButton[] 
            { buttonAdd, buttonUpdate, buttonDelete });
        }
    }
    public class EditButton : Button
    {
        public EditButton(string text)
        {
            Text = text;
            Width = 200;
        }
    }
}
