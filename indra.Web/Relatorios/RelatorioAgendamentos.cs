using indra.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace indra.Web.Relatorios
{
    public class RelatorioAgendamentos
    {
        private IWebHostEnvironment _oHostEnvironment;

        public RelatorioAgendamentos(IWebHostEnvironment oHostEnvironment)
        {
            _oHostEnvironment = oHostEnvironment;

        }
        #region Declaração

        int qtdConcluidos = 0;
        int qtdAguardando = 0;
        int qtdCancelados = 0;
        int tipoSituacao = 0;

        int _totalColumn = 6;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfTable = new PdfPTable(6);
        PdfPCell _pdfPCell;
        PdfPTable _pdfTableSubHeader = null;
        PdfPTable _pdfTableHeader = new PdfPTable(2);
        PdfPTable _pdfTitulo = new PdfPTable(3);
        MemoryStream _memoryStream = new MemoryStream();
        List<Agendamentos> _agendamentos = new List<Agendamentos>();
        #endregion

        public byte[] PrepareReport(List<Agendamentos> agendamentos, int situacao)
        {
            tipoSituacao = situacao;

            var qtdPdfTable = 1;
            if (tipoSituacao == 0) qtdPdfTable += 3;
            else if (tipoSituacao == 1) qtdPdfTable++;
            else if (tipoSituacao == 2) qtdPdfTable++;
            else if (tipoSituacao == 3) qtdPdfTable++;

            _pdfTableSubHeader = new PdfPTable(qtdPdfTable);

            _agendamentos = agendamentos;
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
            sizes[3] = 30;
            sizes[4] = 35;
            sizes[5] = 35;

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

            _pdfPCell = new PdfPCell(new Phrase("QTD de Agendamentos", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            if (tipoSituacao == 0)
            {
                _pdfPCell = new PdfPCell(new Phrase("QTD de Aguardando", fontStyleBold));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.CYAN;
                _pdfTableSubHeader.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("QTD de Concluídos", fontStyleBold));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.GREEN;
                _pdfTableSubHeader.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("QTD de Cancelados", fontStyleBold));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.RED.Brighter().Brighter();
                _pdfTableSubHeader.AddCell(_pdfPCell);

            }
            else if (tipoSituacao == 1)
            {
                _pdfPCell = new PdfPCell(new Phrase("QTD de Aguardando", fontStyleBold));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.CYAN;
                _pdfTableSubHeader.AddCell(_pdfPCell);

            }
            else if (tipoSituacao == 2)
            {
                _pdfPCell = new PdfPCell(new Phrase("QTD de Concluídos", fontStyleBold));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.GREEN;
                _pdfTableSubHeader.AddCell(_pdfPCell);
            }
            else if (tipoSituacao == 3)
            {
                _pdfPCell = new PdfPCell(new Phrase("QTD de Cancelados", fontStyleBold));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.RED;
                _pdfTableSubHeader.AddCell(_pdfPCell);
            }

            foreach (var agendamento in _agendamentos)

            {
                if (agendamento.SituacaoAgendamentoId == 1)
                {
                    qtdAguardando++;
                }
                else if (agendamento.SituacaoAgendamentoId == 2)
                {
                    qtdConcluidos++;
                }
                else if (agendamento.SituacaoAgendamentoId == 3)
                {
                    qtdCancelados++;
                }
            }

            _pdfTableSubHeader.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase(_agendamentos.Count().ToString(), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(qtdAguardando.ToString(), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(qtdConcluidos.ToString(), _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTableSubHeader.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(qtdCancelados.ToString(), _fontStyle));
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

            _pdfPCell = new PdfPCell(new Phrase("Cliente", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Profissional", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Serviço", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Horário", fontStyleBold));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Situação", fontStyleBold));
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
            _pdfPCell = new PdfPCell(new Phrase("Relatório Detalhado de Agendamentos", _fontStyle));
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
            _pdfPCell = new PdfPCell(new Phrase("Relatório Detalhado de Serviços", _fontStyle));
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

            foreach (var agendamento in _agendamentos)
            {

                _pdfPCell = new PdfPCell(new Phrase(agendamento.Id.ToString(), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(agendamento.Cliente.Nome, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(agendamento.Profissional.Nome, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(agendamento.Servico.Nome, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(agendamento.Horario.ToString(), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(agendamento.SituacaoAgendamento.Situacao, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                if (agendamento.SituacaoAgendamentoId == 1)
                {
                    _pdfPCell.BackgroundColor = BaseColor.CYAN;
                }
                else if (agendamento.SituacaoAgendamentoId == 2)
                {
                    _pdfPCell.BackgroundColor = BaseColor.GREEN;
                }
                else if (agendamento.SituacaoAgendamentoId == 3)
                {
                    _pdfPCell.BackgroundColor = BaseColor.RED;
                }
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
