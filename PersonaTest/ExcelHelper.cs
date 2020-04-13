using Aspose.Cells;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace PersonaTest
{
    public static class ExcelHelper
    {
        public static void OpenFile(string path)
        {
            try
            {
                Questions._alllist = new List<Question>();
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount =3;
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(path);

                // Access first worksheet.
                Aspose.Cells.Worksheet ws = wb.Worksheets[0];

                /*
                ret = new string[colCount][];
                for (int j = 1; j <= colCount; j++)
                    ret[j - 1] = new string[rowCount];
                for (int i = 1; i <= rowCount; i++)
                {

                    for (int j = 1; j <= colCount; j++)
                    {




                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            ret[j - 1][i - 1] = xlRange.Cells[i, j].Value2.ToString();

                    }
                }
                
                int current = 1;
                MessageBox.Show(xlRange.Cells[current, 1].Value2.ToString());
                while (xlRange.Cells[current, 1].Value2.ToString() != null || xlRange.Cells[current, 1].Value2.ToString() != "")
                {
                    int formerge = current + 1;
                    while (xlWorksheet.Range["A" + current.ToString(), "A" + formerge.ToString()].MergeCells)
                    {
                        formerge++;
                    }
                    current = formerge + 1;
                    MessageBox.Show(current.ToString());

                }
                */
                object[] data = null;
                Range objRange = null;
                for (int row = 1; row < xlWorksheet.UsedRange.Cells.Rows.Count; row++)
                {
                    data = new object[colCount - 1];
                    bool flag = false;
                    for (int col = 1; col < colCount; col++)
                    {
                        objRange = xlWorksheet.Cells[row, col];
                        if (objRange.MergeCells)
                        {
                            data[col - 1] = Convert.ToString(((Range)objRange.MergeArea[1, 1]).Text).Trim();
                        }
                        else
                        {
                            data[col - 1] = Convert.ToString(objRange.Text).Trim();
                        }
                        if(((Cell)ws.Cells["B" + row.ToString()]).GetStyle().Font.Color.R>0)
                        {
                            flag = true;
                        }
                       
                    }
                   
                    AddRow(data, flag);
                }
               // MessageBox.Show(current.ToString());
                object mergeCells = xlWorksheet.UsedRange.MergeCells;
                var containsMergedCells = mergeCells == DBNull.Value || (bool)mergeCells;
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }catch
            (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private static void AddRow(object[] data, bool right)
        {
            if (data[0].ToString() == "" || data[1].ToString() == "")
                return;
            Answer answer = new Answer(data[1].ToString(), right);
            if (Questions._alllist.Where(x => x.text == data[0].ToString()).Count() == 0)
            {
                Question question = new Question(data[0].ToString());
                question.AddAnswer(answer);
                Questions._alllist.Add(question);
            }
            else
            {
                Question question = Questions._alllist.Where(x => x.text == data[0].ToString()).First();
                question.AddAnswer(answer);
            }
        }
    }
}
