#pragma checksum "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f806601850205967131c0f3d899db99f09bf0f5d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Agendamentos__TabelaAgendamentosPartial), @"mvc.1.0.view", @"/Views/Agendamentos/_TabelaAgendamentosPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\_ViewImports.cshtml"
using indra.Web.Admin;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\_ViewImports.cshtml"
using indra.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f806601850205967131c0f3d899db99f09bf0f5d", @"/Views/Agendamentos/_TabelaAgendamentosPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b7f7aecdeaf77e4e686e937e54f46b2c26e52b4a", @"/Views/_ViewImports.cshtml")]
    public class Views_Agendamentos__TabelaAgendamentosPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<indra.Models.Agendamentos>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<table class=""table table-bordered table-hover"" style=""width: 100%"">

    <thead class=""thead-dark col col-xs-12"">
        <tr>
            <th width=""25%"">
                Cliente
            </th>
            <th width=""20%"">
                Profissional
            </th>
            <th width=""15%"">
                Serviço
            </th>
            <th width=""10%"">
                Data
            </th>
            <th width=""10%"">
                Situação
            </th>
            <th width=""15%"">
                Ações
            </th>
        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 28 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 32 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
               Write(Html.DisplayFor(modelItem => item.Cliente.Nome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 35 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
               Write(Html.DisplayFor(modelItem => item.Profissional.Nome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 38 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
               Write(Html.DisplayFor(modelItem => item.Servico.Nome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td style=\"text-align:center\">\r\n                    ");
#nullable restore
#line 41 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
               Write(Html.DisplayFor(modelItem => item.Horario));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td style=\"text-align:center\">\r\n                    ");
#nullable restore
#line 44 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
               Write(Html.DisplayFor(modelItem => item.SituacaoAgendamento.Situacao));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td style=\"text-align:center\">\r\n");
#nullable restore
#line 47 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
                     if (item.SituacaoAgendamentoId == 1)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <button");
            BeginWriteAttribute("onclick", " onclick=\"", 1539, "\"", 1578, 3);
            WriteAttributeValue("", 1549, "concluirAgendamento(", 1549, 20, true);
#nullable restore
#line 49 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
WriteAttributeValue("", 1569, item.Id, 1569, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1577, ")", 1577, 1, true);
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btnVerde btn-sm btn-squared-default\"><i class=\"fa fa-check-square\"></i></button>\r\n                        <button");
            BeginWriteAttribute("onclick", " onclick=\"", 1704, "\"", 1743, 3);
            WriteAttributeValue("", 1714, "cancelarAgendamento(", 1714, 20, true);
#nullable restore
#line 50 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
WriteAttributeValue("", 1734, item.Id, 1734, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1742, ")", 1742, 1, true);
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btnVermelho btn-sm btn-squared-default\"><i class=\"fa fa-ban\"></i></button>\r\n");
#nullable restore
#line 51 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 55 "C:\Users\bruno\Google Drive\PROGRAMAÇÃO\Projeto Interdisciplinar ADS - Bruno Carlos da Silva\Aplicação\indra\indra.Web.Admin\Views\Agendamentos\_TabelaAgendamentosPartial.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n<div class=\"clearfix\"></div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<indra.Models.Agendamentos>> Html { get; private set; }
    }
}
#pragma warning restore 1591
