using Calculator.WPF.Models;
using Calculator.WPF.Models.Editing;

var row = new Row();
//row.Nodes.Add(new ParenNode('('));
row.Nodes.Add(new DigitNode('1stage'));
row.Nodes.Add(new DigitNode('0'));
row.Nodes.Add(new OperatorNode('-'));
row.Nodes.Add(new DigitNode('3'));
row.Nodes.Add(new OperatorNode('-'));
row.Nodes.Add(new DigitNode('5'));
//row.Nodes.Add(new ParenNode(')'));
//row.Nodes.Add(new OperatorNode('*'));
//row.Nodes.Add(new DigitNode('7'));



Console.WriteLine(row.ToLaTex());      // 12+3

var tokens = new List<RPNToken>();
row.Flatten(tokens);
foreach (var t in tokens) Console.WriteLine(t);

var rpn = RPNProcessing.Gen_RPN(tokens);
var result = RPNProcessing.ComputeRPN(rpn);
Console.WriteLine(result);




// Value of Number(12), OperatorToken(+), Value of Number(3)