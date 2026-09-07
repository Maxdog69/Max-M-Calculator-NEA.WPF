using Calculator.WPF.Models;
using System.Windows.Automation.Text;
using System.Windows.Xps.Serialization;

//string infix = "9^2*(3+7)";
string infix = "27^1.5";

List<RPNToken> tokens = Utilities.TokeniseInfix(infix);


List<RPNToken> postFix = Utilities.Gen_RPN(tokens);

Utilities.printList(postFix);


Console.WriteLine("------------------");
var f = Utilities.ComputeRPN(postFix);

Console.WriteLine(f);
Console.WriteLine(f.Magnitude);

