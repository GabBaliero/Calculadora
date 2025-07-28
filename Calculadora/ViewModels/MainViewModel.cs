using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace Calculadora.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _expressionDisplay = string.Empty;
        [ObservableProperty]
        private string _resultDisplay = string.Empty;
        [ObservableProperty]
        private bool _openParenthesis = true;

        public int ExpressionFontSize { get; set; }
        public string StoreFirstExpression { get; set; }
        public string StoreSecondExpression { get; set; }

        public MainViewModel()
        {
            ExpressionFontSize = 75;
        }

        public ICommand NumericButtons => new Command<string>(value =>
        {
            ExpressionDisplay += value;
        });

        public ICommand ACbutton => new Command(_ =>
        {
            ExpressionDisplay = string.Empty;
            ResultDisplay = string.Empty;
        });

        public ICommand BackscapeButton => new Command(_ =>
        {
            try
            {
                ExpressionDisplay = ExpressionDisplay.Remove(ExpressionDisplay.Length - 1);
            }catch { }
        });

        public ICommand OperatorsButton => new Command<string>(value =>
        {
            if (!string.IsNullOrEmpty(ExpressionDisplay))
            {
                StoreFirstExpression = ExpressionDisplay;
                ExpressionDisplay += value;
            }
        });

        public ICommand EqualsButton => new Command(async _ =>
        {
            await Calculate();
        });

        public ICommand ParenthesisCommand => new Command(() =>
        {
            if (OpenParenthesis)
            {
                if (!string.IsNullOrEmpty(ExpressionDisplay) && (char.IsDigit(ExpressionDisplay.Last()) || ExpressionDisplay.Last() == ')'))
                    ExpressionDisplay += "(";
                else
                    ExpressionDisplay += "(";
            }
            else
                ExpressionDisplay += ")";

            OpenParenthesis = !OpenParenthesis;
        });

        public ICommand PorcentageCommand => new Command(value =>
        {
            if (!string.IsNullOrEmpty(ExpressionDisplay))
            {
                ExpressionDisplay += value;
                string fOp = StoreFirstExpression;
                string expression = ExpressionDisplay;
                string result = expression.Replace(fOp, "");
            }
            else
                return;
        });

        public async Task<string> Calculate()
        {
            try
            {
                return ResultDisplay = EvaluateExpression(ExpressionDisplay).ToString();
            }
            catch
            {
                return ResultDisplay = "Error";
            }
        }

        static double EvaluateExpression(string expression)
        {
            expression = FormatParenthesis(expression);
            expression = expression.Replace("×", "*").Replace("÷", "/").Replace(",", ".");
            var table = new DataTable();
            object result = table.Compute(expression, "");
            return Convert.ToDouble(result);
        }
        
        static string FormatParenthesis(string param)
        {
            if (char.IsDigit(param.First()))
                param = param.Replace("(", "*(");
            else
                param = param.Replace("(", "1*(");
            return param;
        }
    }
}
