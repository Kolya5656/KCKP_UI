using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void dataGridViewA_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
                if (e.FormattedValue.ToString() == string.Empty)
                    return;
                else
                    if (!int.TryParse(e.FormattedValue.ToString(), out int res) )
                {
                    dataGridViewA.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                    MessageBox.Show("Введите числовое значение");
                    e.Cancel = true;
                    return;
                }
            
        }

        private void dataGridViewB_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue.ToString() == string.Empty)
                return;
            else
                    if (!int.TryParse(e.FormattedValue.ToString(), out int res))
            {
                dataGridViewB.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                MessageBox.Show("Введите числовое значение");
                e.Cancel = true;
                return;
            }
        }

        private void dataGridViewApx_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue.ToString() == string.Empty)
                return;
            else
                    if (!int.TryParse(e.FormattedValue.ToString(), out int res))
            {
                dataGridViewApx.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                MessageBox.Show("Введите числовое значение");
                e.Cancel = true;
                return;
            }
        }

        private void dataGridViewAns_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (e.FormattedValue.ToString() == string.Empty)
            //    return;
            //else
            //        if (!int.TryParse(e.FormattedValue.ToString(), out int res))
            //{
            //    dataGridViewApx.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
            //    MessageBox.Show("Введите числовое значение");
            //    e.Cancel = true;
            //    return;
            //}
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void solveBtn_Click(object sender, EventArgs e)
        {
            matrixA = new double[size, size];
            matrixB = new double[size];
            matrixAnsw = new double[size];
            matrixApx = new double[size + 1, size];
            SetVariables();
            gaussZeidel = new GaussZeidel(matrixA, matrixB, size);
            if(gaussZeidel.DiagonallyDominant() == true)
            {
                gaussZeidel.Algoritm();
                matrixAnsw = gaussZeidel.Roots;
                SetAnswer();
                SetNameAns();
                Subs();
            }
            else
            {
                MessageBox.Show(
                    "Данная матрица не подходит для решения методом Зейделя","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                SizeChange();
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
                matrixB[i] = Convert.ToDouble(dataGridViewB[0,i].Value);
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
                dataGridViewAns.Columns[i].HeaderCell.Value = $"X{i+1}";
            }
        }
        private void Subs()
        {
            for (int i = 0; i+1 < dataGridViewApx.ColumnCount; i++)
            {
                double summ = 0;
                for (int j = 0; j < dataGridViewApx.RowCount; j++)
                {
                    double x = matrixAnsw[j]*matrixA[i,j];
                    dataGridViewApx[j, i].Value = (x.ToString("+#;-#;0"));
                    summ += x;
                }
                dataGridViewApx[3,i].Value = summ;
            }
        }
    }
}
