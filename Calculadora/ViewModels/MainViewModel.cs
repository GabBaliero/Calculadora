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
        [ObservableProperty]
        private int _expressionFontSize = 75;

        public string StoreFirstExpression { get; set; }
        public string StoreSecondExpression { get; set; }

        public MainViewModel()
        {
            FontSizeResizer();
        }

        public ICommand NumericButtons => new Command<string>(value =>
        {
            ExpressionDisplay += value;
        });

        public ICommand ACbutton => new Command(_ =>
        {
            ExpressionDisplay = string.Empty;
            ResultDisplay = string.Empty;
            FontSizeResizer();
        });

        public ICommand BackscapeButton => new Command(_ =>
        {
            try
            {
                ExpressionDisplay = ExpressionDisplay.Remove(ExpressionDisplay.Length - 1);
            }catch { }

            FontSizeResizer();
        });

        public ICommand OperatorsButton => new Command<string>(value =>
        {
            if (!string.IsNullOrEmpty(ExpressionDisplay))
            {
                StoreFirstExpression = ExpressionDisplay;
                ExpressionDisplay += value;
                FontSizeResizer();
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
            FontSizeResizer();
        });

        public ICommand PorcentageCommand => new Command(value =>
        {
            if (!string.IsNullOrEmpty(StoreFirstExpression) && !string.IsNullOrEmpty(ExpressionDisplay))
            {
                string baseValue = StoreFirstExpression;
                string full = ExpressionDisplay;
            }
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
        
        static string FormatParenthesis(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return expression;

            var sb = new StringBuilder();

            for (int i = 0; i < expression.Length; i++)
            {
                char current = expression[i];

                if (current == '(')
                    if (i > 0 && (char.IsDigit(expression[i - 1]) || expression[i - 1] == ')'))
                        sb.Append("*(");
                    else
                        sb.Append("1*(");
                else
                    sb.Append(current);
            }

            return sb.ToString();
        }

        public void FontSizeResizer()
        {
            int length = ExpressionDisplay?.Length ?? 0;

            if (length >= 12)
                ExpressionFontSize = 45;
            else if (length >= 6)
                ExpressionFontSize = 60;
            else
                ExpressionFontSize = 75;
            //ExpressionFontSize = length >= 12 ? 45 : length >= 6 ? 60 : 75;
        }
    }
}
