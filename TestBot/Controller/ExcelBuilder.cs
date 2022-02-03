using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot.Controller
{
    internal static class ExcelBuilder
    {
        internal static double? GetROI(string article, double price, string store)
        {

            try
            {
                IXLWorkbook xLWorkbook = new XLWorkbook($"{GlobalVariables.ProjectDirectory}prices.xlsx");
                if (xLWorkbook.TryGetWorksheet(store, out IXLWorksheet xLWorksheet))
                {
                    int numRowArticle = xLWorksheet
                        .Column(1)?
                        .CellsUsed()?
                        .Where(x => x.Value.ToString() == article)?
                        .FirstOrDefault()?
                        .Address
                        .RowNumber ?? 0;
                    string formulaCustopPercent = xLWorksheet
                        .Cell(numRowArticle, 11)
                        .FormulaA1
                        .ToString() ?? string.Empty;
                    int numRowCustopPercent = int.Parse(formulaCustopPercent.Substring(formulaCustopPercent.LastIndexOf('*') + 2));
                    string firstPriceString = xLWorksheet.Cell(numRowArticle, 6).Value.ToString()?.Replace(',', '.') ?? "0";
                    string warehouseString = xLWorksheet.Cell(numRowArticle, 7).Value.ToString()?.Replace(',', '.') ?? "0";
                    string deliveryString = xLWorksheet.Cell(numRowArticle, 8).Value.ToString()?.Replace(',', '.') ?? "0";
                    string comissionString = xLWorksheet.Cell(numRowArticle, 9).Value.ToString()?.Replace(',', '.') ?? "0";
                    string logisticString = xLWorksheet.Cell(numRowArticle, 10).Value.ToString()?.Replace(',', '.') ?? "0";
                    string custopPercentString = xLWorksheet.Cell(numRowCustopPercent, 16).Value.ToString()?.Replace(',', '.') ?? "0";
                    if (double.TryParse(firstPriceString, NumberStyles.Any, CultureInfo.InvariantCulture, out double firstPrice)
                        && double.TryParse(warehouseString, NumberStyles.Any, CultureInfo.InvariantCulture, out double warehouse)
                        && double.TryParse(deliveryString, NumberStyles.Any, CultureInfo.InvariantCulture, out double delivery)
                        && double.TryParse(comissionString, NumberStyles.Any, CultureInfo.InvariantCulture, out double comission)
                        && double.TryParse(logisticString, NumberStyles.Any, CultureInfo.InvariantCulture, out double logistic)
                        && double.TryParse(custopPercentString, NumberStyles.Any, CultureInfo.InvariantCulture, out double custopPercent))
                    {
                        double profit = price - firstPrice - logistic - price * comission - warehouse - delivery - price * custopPercent;
                        double roi = profit / firstPrice * 100d;
                        return roi;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
