using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DataTable = Gherkin.Ast.DataTable;
using DocString = Gherkin.Ast.DocString;
using TableRow = Gherkin.Ast.TableRow;
using TableCell = Gherkin.Ast.TableCell;


namespace Xunit.Gherkin.Quick
{
    internal static class GherkinScenarioOutlineExtensions
    {
        private static readonly Regex _placeholderRegex = new Regex(@"<([a-zA-Z0-9]([^<>]*[a-zA-Z0-9])?)>");

        private static global::Gherkin.Ast.StepArgument GetStepArgumentWithUpdatedText(global::Gherkin.Ast.Step outlineStep, MatchEvaluator matchEvaluator)
        {
            var stepArgument = outlineStep.Argument;

            if (stepArgument is DataTable table)
            {
                var processedHeaderRow = false;

                var digestedRows = new List<TableRow>();
                
                foreach (var row in table.Rows)
                {
                    if (!processedHeaderRow)
                    {
                        digestedRows.Add(row);
                        processedHeaderRow = true;
                    }
                    else
                    {
                        var digestedCells = row.Cells.Select(r => new TableCell(r.Location, _placeholderRegex.Replace(r.Value, matchEvaluator)));
                        digestedRows.Add(new TableRow(row.Location, digestedCells.ToArray()));
                    }
                }

                stepArgument = new DataTable(digestedRows.ToArray());
            }

            if (stepArgument is DocString stepAsDocString)
            {
                var updatedContent = _placeholderRegex.Replace(stepAsDocString.Content, matchEvaluator);
                stepArgument = new DocString(stepAsDocString.Location, stepAsDocString.ContentType, updatedContent);
            }

            return stepArgument;
        }

        private static Dictionary<string, string> GetExampleRowValues(global::Gherkin.Ast.Examples examples, List<global::Gherkin.Ast.TableCell> exampleRowCells)
        {
            var rowValues = new Dictionary<string, string>();

            var headerCells = examples.TableHeader.Cells.ToList();
            for (int index = 0; index < headerCells.Count; index++)
            {
                rowValues.Add(headerCells[index].Value, exampleRowCells[index].Value);
            }

            return rowValues;
        }
    }
}
