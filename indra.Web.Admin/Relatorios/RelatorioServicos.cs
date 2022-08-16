using ImTools;
using indra.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LanguageExt;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Admin.Relatorios
{
    public class RelatorioServicos
    {
        private IWebHostEnvironment _oHostEnvironment;

        public RelatorioServicos(IWebHostEnvironment oHostEnvironment)
        {
            _oHostEnvironment = oHostEnvironment;

        }
        #region Declaração

        int qtdServicos = 0;

        int _totalColumn = 5;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfTable = new PdfPTable(5);
        PdfPCell _pdfPCell;
        PdfPTable _pdfTableSubHeader = new PdfPTable(2);
        PdfPTable _pdfTableHeader = new PdfPTable(2);
        PdfPTable _pdfTitulo = new PdfPTable(3);
        MemoryStream _memoryStream = new MemoryStream();
        List<ProfissionalServico> _servicos = new List<ProfissionalServico>();
        #endregion

        public byte[] PrepareReport(List<ProfissionalServico> servicos)
        {
            _servicos = servicos;
            _document = new Document();
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(5f, 5f, 10f, 5f);
            _pdfTable.WidthPercentage = 90;
            _pdfTableSubHeader.WidthPercentage = 90;
            _pdfTableHeader.WidthPercentage = 90;
            _pdfTitulo.WidthPercentage = 90;
            _pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTableSubHeader.HorizontalAlignment = Element.ALIGN_CENTER;

            _fontStyle = FontFactory.GetFont("Calibri", 10f, 1);
            PdfWriter docWrite = PdfWriter.GetInstance(_document, _memoryStream);

            _document.Open();
            float[] sizes = new float[_totalColumn];
            sizes[0] = 10;
            sizes[1] = 35;
            sizes[2] = 35;
            sizes[3] = 45;
            sizes[4] = 35;

            _pdfTable.SetWidths(sizes);
            this.ReportHeader();
            this.EmpyRow(2, 2, _pdfTableHeader);
            this.SetPageTitle();
            this.EmpyRow(2, 1, _pdfTitulo);
            this.ReportSubHeader();
            this.EmpyRow(2, 4, _pdfTableSubHeader);
            this.ReportBody();

            _pdfTable.HeaderRows = 1;

            _document.Add(_pdfTableHeader);
            _document.Add(_pdfTableSubHeader);
            _document.Add(_pdfTable);
            _document.Close();

            return _memoryStream.ToArray();

        }
        private void ReportSubHeader()
        {
            var fontStyleBold = FontFactory.GetFont("Calibri", 9f, 1);
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 0);

            #region Detalhes sub cabeçalho da tabela principal
            _pdfPCell = new PdfPCell(new Phrase("QTD de Serviços", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Atualizado em", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            _pdfTableSubHeader.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase(_servicos.Count().ToString(), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            string dataHoraLocal = DateTime.Now.Date.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            _pdfPCell = new PdfPCell(new Phrase(dataHoraLocal, _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            _pdfTableSubHeader.CompleteRow();
            #endregion

            #region Detalhes cabeçalho da tabela principal

            _pdfPCell = new PdfPCell(new Phrase("ID", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Nome", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Profissional", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Email", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Valor", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfTable.CompleteRow();
            #endregion
        }
        private void ReportHeader()
        {
            _pdfPCell = new PdfPCell(this.AddLogo());
            _pdfPCell.Colspan = 1;
            _pdfPCell.Border = 0;
            _pdfTableHeader.AddCell(_pdfPCell);


            _fontStyle = FontFactory.GetFont("Times New Roman", 16f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Relatório de Serviços e Atribuição", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.Colspan = 2;
            _pdfPCell.ExtraParagraphSpace = 1;
            _pdfTableHeader.AddCell(_pdfPCell);
            _pdfTableHeader.CompleteRow();

            _fontStyle = FontFactory.GetFont("Times New Roman", 8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("Data de Impressão: " + DateTime.Now, _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.Colspan = 2;
            _pdfPCell.ExtraParagraphSpace = 1;
            _pdfTableHeader.AddCell(_pdfPCell);
            _pdfTableHeader.CompleteRow();

            _pdfTableHeader.CompleteRow();
        }
        private PdfPTable AddLogo()
        {
            int maxColumn = 1;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);

            string imgCombine = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/logo-indra.png");
            Image img = Image.GetInstance(imgCombine);
            img.ScaleAbsolute(50f, 50f);
            _pdfPCell = new PdfPCell(img);
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfPCell);
            pdfPTable.CompleteRow();

            return pdfPTable;
        }
        private void SetPageTitle()
        {
            _pdfPCell = new PdfPCell(new Phrase("Relatório de Serviços e Atribuição", _fontStyle));
            _pdfPCell.Colspan = 3;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTitulo.AddCell(_pdfPCell);
            _pdfTitulo.CompleteRow();
        }
        private void ReportBody()
        {
            var fontStyleBold = FontFactory.GetFont("Times New Roman", 9f, 1);
            _fontStyle = FontFactory.GetFont("Times New Roman", 9f, 0);

            #region Detalhes corpo da tebela

            foreach (var servico in _servicos)
            {

                _pdfPCell = new PdfPCell(new Phrase(servico.Id.ToString(), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(servico.Servico.Nome, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(servico.Profissional.Nome, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(servico.Profissional.Email, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                var valor = servico.Servico.Valor;
                string.Format("{0:N2}", Convert.ToDecimal(valor));
                _pdfPCell = new PdfPCell(new Phrase("R$ " + valor.ToString("#.###,##"), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfTable.CompleteRow();
            }
            #endregion
        }
        private void EmpyRow(int qtdLinhas, int qtdColunas, PdfPTable pdfTable)
        {
            for (int cont = 1; cont <= qtdLinhas; cont++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
                _pdfPCell.Colspan = qtdColunas;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.ExtraParagraphSpace = 2;
                pdfTable.AddCell(_pdfPCell);
                pdfTable.CompleteRow();
            }

        }
    }
}
