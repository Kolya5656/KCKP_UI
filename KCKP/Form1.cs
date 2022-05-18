using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KCKP_UI
{
    public partial class Form1 : Form
    {
        private double[,] matrixA;

        private double[] matrixB;

        private double[] matrixAnsw;

        private double[,] matrixApx;

        private GaussZeidel gaussZeidel;

        public int size;

        public Form1()
        {
            InitializeComponent();
            matrixSize.SelectedIndex = 0;
            InitMatix();
            SizeChange();
        }

        public void SizeChange()
        {
            dataGridViewA.Rows.Clear();
            dataGridViewB.Rows.Clear();
            dataGridViewApx.Rows.Clear();
            dataGridViewAns.Rows.Clear();
            size = Convert.ToInt32(matrixSize.Text);
            dataGridViewA.ColumnCount = size;
            dataGridViewA.RowCount = size;
            dataGridViewApx.ColumnCount = size + 1;
            dataGridViewApx.RowCount = size;
            dataGridViewB.ColumnCount = 1;
            dataGridViewB.RowCount = size;
            dataGridViewAns.RowCount = 1;
            dataGridViewAns.ColumnCount = size;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void matrixSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SizeChange();
            InitMatix();
        }

        private void InitMatix()
        {
            matrixA = new double[size, size];
            matrixB = new double[size];
            matrixAnsw = new double[size];
            matrixApx = new double[size + 1, size];
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void solveBtn_Click(object sender, EventArgs e)
        {
            InitMatix();
            SetVariables();
            gaussZeidel = new GaussZeidel(matrixA, matrixB, size);
            if (gaussZeidel.DiagonallyDominant() == true)
            {
                gaussZeidel.Algoritm();
                matrixAnsw = gaussZeidel.Roots;
                SetAnswer();
                SetNameAns();
                Subs();
            }
            else
            {
                MessageBox
                    .Show("Данная матрица не подходит для решения методом Зейделя",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                SizeChange();
            }
        }

        private void SetCells()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dataGridViewA[j, i].Value = matrixA[i, j];
                }
            }

            for (int i = 0; i < size; i++)
            {
                dataGridViewB[0, i].Value = matrixB[i];
            }
        }

        private void SetVariables()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrixA[i, j] = Convert.ToDouble(dataGridViewA[j, i].Value);
                }
            }

            for (int i = 0; i < size; i++)
            {
                matrixB[i] = Convert.ToDouble(dataGridViewB[0, i].Value);
            }
        }

        private void SetAnswer()
        {
            for (int i = 0; i < size; i++)
            {
                dataGridViewAns[i, 0].Value = matrixAnsw[i];
            }
        }

        private void SetNameAns()
        {
            for (int i = 0; i <= dataGridViewAns.Rows.Count; i++)
            {
                dataGridViewAns.Columns[i].HeaderCell.Value = $"X{i + 1}";
            }
        }

        private void Subs()
        {
            for (int i = 0; i + 1 < dataGridViewApx.ColumnCount; i++)
            {
                double summ = 0;
                for (int j = 0; j < dataGridViewApx.RowCount; j++)
                {
                    double x = matrixAnsw[j] * matrixA[i, j];
                    dataGridViewApx[j, i].Value = (x.ToString("+#;-#;0"));
                    summ += x;
                }
                dataGridViewApx[dataGridViewApx.ColumnCount - 1, i].Value =
                    summ;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string dir = Directory.GetCurrentDirectory();
            saveFileDialog1.Filter =
                "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = "Выберите текстовый файл";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Результат.txt";
            saveFileDialog1.CheckFileExists = false;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var myStream = saveFileDialog1.OpenFile();
                    if (myStream != null)
                    {
                        using (StreamWriter sw = new StreamWriter(myStream))
                        {
                            sw.WriteLineAsync("Ответ");

                            for (int j = 0; j < size; j++)
                            {
                                sw.WriteLine($"X{j + 1}={dataGridViewAns[j, 0].Value}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK);
                }
            }
        }

        private void dataGridViewA_EditingControlShowing(
            object sender,
            DataGridViewEditingControlShowingEventArgs e
        )
        {
            TextBox tb = (TextBox) e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (
                !(Char.IsDigit(e.KeyChar)) &&
                !((e.KeyChar == '-')) &&
                !((e.KeyChar == ','))
            )
            {
                if (e.KeyChar != (char) Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void dataGridViewB_EditingControlShowing(
            object sender,
            DataGridViewEditingControlShowingEventArgs e
        )
        {
            TextBox tb = (TextBox) e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            string dir = Directory.GetCurrentDirectory();
            openFileDialog1.InitialDirectory = dir;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.FileName = "input data";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter =
                "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileStream = openFileDialog1.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string line;
                        int j = 0;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line == String.Empty)
                            {
                                continue;
                            }
                            string[] test =
                                line.Trim().Split(new char[] { ' ' });
                            for (int i = 0; i < size; i++)
                            {
                                double x = Convert.ToDouble(test[i]);
                                matrixA[j, i] = x;
                            }
                            double y = Convert.ToDouble(test.Last());
                            matrixB[j] = y;
                            j++;
                        }
                        SetCells();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK);
                }
            }
        }
    }
}
