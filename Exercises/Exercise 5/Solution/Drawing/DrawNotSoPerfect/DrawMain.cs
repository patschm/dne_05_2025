using DrawNotSoPerfect.Services;
using Shapes;
using System.Reflection;

namespace DrawNotSoPerfect
{
    public partial class DrawMain : Form
    {
        private readonly SynchronizationContext? _synchronizationContext;
        public class DragInfo
        {
            public Shape? SelectedShape { get; internal set; }
        }

        private List<Shape> _shapes = new List<Shape>();
        private DragInfo? _selected = null;
        private StorageLocator? _storageLocator;
        private IStorage? _storage;

        public DrawMain()
        {
            _synchronizationContext = SynchronizationContext.Current;
            InitializeComponent();
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new CircleDlg();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _shapes.Add(dlg.Shape!);
                Invalidate();
            }
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new RectangleDlg();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _shapes.Add(dlg.Shape!);
                Invalidate();
            }
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new TriangleDlg();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _shapes.Add(dlg.Shape!);
                Invalidate();
            }
        }

        private void DrawMain_Paint(object sender, PaintEventArgs e)
        {
            var g = new GraphicsDevice(e.Graphics);
            foreach (Shape s in _shapes)
            {
                s.Draw(g);
            }
        }

        private void DrawMain_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Shape s in _shapes)
            {
                int dx = s.Location.X - e.X;
                int dy = s.Location.Y - e.Y;
                if (Math.Sqrt(dx * dx + dy * dy) < 50)
                {
                    _selected = new DragInfo { SelectedShape = s };
                }
            }
            Invalidate();
            if (_selected != null)
                _selected.SelectedShape!.IsSelected = true;
        }

        private void DrawMain_DragDrop(object sender, DragEventArgs e)
        {
            DragInfo? s = e.Data?.GetData(typeof(DragInfo)) as DragInfo;
            if (s != null && s.SelectedShape != null)
            {
                var pos = PointToClient(new Point(e.X, e.Y));
                s.SelectedShape.Location = new Position { X = pos.X, Y = pos.Y };
                s.SelectedShape.IsSelected = false;
                Invalidate();
                _selected = null;
            }
        }

        private void DrawMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Invalidate();
        }

        private void DrawMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selected != null)
                DoDragDrop(_selected, DragDropEffects.Move);
        }

        private void DrawMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_selected != null)
            {
                _selected.SelectedShape!.IsSelected = false;
            }
            _selected = null;
            Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _storage?.TypeFilter + "|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _shapes = _storage?.Open(openFileDialog.FileName)!;
                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = _storage?.TypeFilter + "|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _storage?.SaveAs(saveFileDialog.FileName, _shapes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _storage?.Save(_shapes);
            }
            catch (FileNotFoundException)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapes = new List<Shape>();
            _selected = null;
            Invalidate();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DrawMain_Load(object sender, EventArgs e)
        {
            var pluginpath = $@"{Environment.CurrentDirectory}\plugins";
            if (!Directory.Exists(pluginpath))
            {
                try
                {
                    Directory.CreateDirectory(pluginpath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                }
            }
            _storageLocator = new StorageLocator(pluginpath);
            _storageLocator.Changed += StorageLocator_Changed;
            _storageLocator.ScanFolder();

        }

        private void StorageLocator_Changed(object? sender, EventArgs e)
        {
            _synchronizationContext?.Send(obj => ClearFormatMenu(), null);
            foreach (var item in _storageLocator!.StorageOptions)
            {
                _synchronizationContext?.Send(obj=>AddToFormatMenu(item), null);              
            }
        }

        private void AddToFormatMenu(IStorage item)
        {
            var menu = new ToolStripMenuItem();
            menu.Text = item.Name;
            menu.Checked = false;
            menu.Tag = item;
            menu.Click += Menu_Click;
            saveFormatToolStripMenuItem.DropDownItems.Add(menu);
            saveFormatToolStripMenuItem.DropDownItems[0].PerformClick();
        }

        private void ClearFormatMenu()
        {
            foreach (ToolStripMenuItem item in saveFormatToolStripMenuItem.DropDownItems)
            {
                item.Click -= Menu_Click;
            }
            saveFormatToolStripMenuItem.DropDownItems.Clear();
        }

        private void Menu_Click(object? sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in saveFormatToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }
            if (sender is ToolStripMenuItem toolStripMenuItem)
            {
                toolStripMenuItem.Checked = true;
                _storage = toolStripMenuItem.Tag as IStorage;
            }
        }
    }
}