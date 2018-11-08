using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            if (Properties.Settings.Default.effectChecked)
                checkEffect.Checked = true;
            else
                checkEffect.Checked = false;
        }

        string operation = null;
        double firstNumberInput = 0.0;
        double secondNumberInput = 0.0;
        double result = 0.0;
        bool equalWasPressed = false;
        bool operationWasPressed = false;

        #region Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lblResult.Text != "")
            {
                string tempText = lblResult.Text;
                int txtLength = tempText.Length;
                lblResult.Text = lblResult.Text.Remove(txtLength - 1);
            }
            btnEqual.Focus();
        }
        private void btnC_Click(object sender, EventArgs e)
        {
            lblResult.Text = "0";
            lblOld.Text = "";
            btnEqual.Focus();
        }

        #endregion

        #region Number Pad
        private void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (lblResult.Text == "0" || equalWasPressed || operationWasPressed)
            {
                lblResult.Text = "";
                equalWasPressed = false;
                operationWasPressed = false;
            }
            lblResult.Text += b.Text;
            btnEqual.Focus();
        }
        private void btbBracket_Click(object sender, EventArgs e)
        {
            if (lblResult.Text == "0" || equalWasPressed || operationWasPressed)
            {
                lblResult.Text = "";
                equalWasPressed = false;
                operationWasPressed = false;
            }
            if (lblResult.Text.Contains("("))
                lblResult.Text += ")";
            else
                lblResult.Text += "(";
            btnEqual.Focus();
        }
        private void btnNegPos_Click(object sender, EventArgs e)
        {
            double.TryParse(lblResult.Text, out double numberInput);
            if (numberInput > 0)
                lblResult.Text = "-" + lblResult.Text;
            else
                lblResult.Text = Math.Abs(numberInput).ToString();

            btnEqual.Focus();
        }
        private void btnDot_Click(object sender, EventArgs e)
        {
            if (lblResult.Text.Contains("."))
                return;

            if (equalWasPressed || operationWasPressed)
            {
                lblResult.Text = "";
                equalWasPressed = false;
                operationWasPressed = false;
            }
            lblResult.Text += ".";
            btnEqual.Focus();
        }

        #endregion

        #region Calculation Pad
        
        void updateLabel(double number)
        {
            lblResult.Text = number.ToString("#");
        }
        async Task giveRandomNumber()
        {
            Random r = new Random();
            resultFontSizeChange();
            string temp = result.ToString("0.####");
            double tempLength = Math.Pow(10, temp.Length);
            //int length = int.Parse(tempLength.ToString());
            for (int i = 0; i < 10; i++)
            {
                double number = r.NextDouble() * tempLength;
                updateLabel(number);
                await Task.Delay(10);
            }
        }
        private async void btnEqual_Click(object sender, EventArgs e)
        {
            if(lblResult.Text != "" && !equalWasPressed)
            {
                double.TryParse(lblResult.Text, out secondNumberInput);
                switch (operation)
                {
                    case "+":
                        result = firstNumberInput + secondNumberInput;
                        break;
                    case "-":
                        result = firstNumberInput - secondNumberInput;
                        break;
                    case "x":
                        result = firstNumberInput * secondNumberInput;
                        break;
                    case "/":
                        result = firstNumberInput / secondNumberInput;
                        break;
                    case "Mod":
                        result = firstNumberInput % secondNumberInput;
                        break;
                    case "x^y":
                        result = Math.Pow(firstNumberInput, secondNumberInput);
                        break;
                }
            }
            else if(lblResult.Text != "")
            {
                switch (operation)
                {
                    case "+":
                        result += secondNumberInput;
                        break;
                    case "-":
                        result -= secondNumberInput;
                        break;
                    case "x":
                        result *= secondNumberInput;
                        break;
                    case "/":
                        result /= secondNumberInput;
                        break;
                    case "Mod":
                        result %= secondNumberInput;
                        break;
                    case "x^y":
                        result = Math.Pow(result, secondNumberInput);
                        break;
                }
            }

            if(checkEffect.Checked)
                await giveRandomNumber();
            lblResult.Text = result.ToString("0.####");
            lblOld.Text = "";
            equalWasPressed = true;

            btnEqual.Focus();
        }
        private void operator_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (lblResult.Text != "")
            {
                operationWasPressed = true;
                firstNumberInput = double.Parse(lblResult.Text);
                if (b.Text == "x^y")
                    lblOld.Text = firstNumberInput.ToString() + " ^ ";
                else
                    lblOld.Text = firstNumberInput.ToString() + $" {b.Text} ";
                operation = b.Text;
            }
            btnEqual.Focus();
        }
        private async void mathComplex_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            double number = double.Parse(lblResult.Text);
            switch (b.Text)
            {
                case "√":
                    result = Math.Sqrt(number);
                    break;
                case "1/x":
                    result = 1 / number;
                    break;
            }
            lblOld.Text = "";
            if (checkEffect.Checked)
                await giveRandomNumber();
            lblResult.Text = result.ToString("0.########");
            equalWasPressed = true;

            btnEqual.Focus();
        }
        #endregion

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys pressed = e.KeyCode;
            switch (pressed)
            {
                case Keys.Decimal:
                    btnDot.PerformClick();
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                    btnZero.PerformClick();
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    btnOne.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    btnTwo.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    btnThree.PerformClick();
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    btnFour.PerformClick();
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    btnFive.PerformClick();
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    btnSix.PerformClick();
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    btnSeven.PerformClick();
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    btnEight.PerformClick();
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    btnNine.PerformClick();
                    break;
                case Keys.Add:
                    btnPlus.PerformClick();
                    break;
                case Keys.Subtract:
                    btnSubstract.PerformClick();
                    break;
                case Keys.Multiply:
                    btnMultiply.PerformClick();
                    break;
                case Keys.Divide:
                    btnDivide.PerformClick();
                    break;
                case Keys.Enter:
                case Keys.Space:
                    btnEqual.PerformClick();
                    break;
                case Keys.Back:
                case Keys.Delete:
                    btnDelete.PerformClick();
                    break;
                case Keys.Escape:
                    btnC.PerformClick();
                    break;
            }
            this.Focus();
        }

        #region Effect on Buttons
        void resultFontSizeChange()
        {
            string temp = lblResult.Text;
            int textLength = temp.Length;

            if (textLength > 15)
                lblResult.Font = new Font("Fira Code Retina", 18);
            else if (textLength > 9)
                lblResult.Font = new Font("Fira Code Retina", 28);
            else
                lblResult.Font = new Font("Fira Code Retina", 36);

        }

        private void lblResult_TextChanged(object sender, EventArgs e)
        {
            resultFontSizeChange();
        }
        private void controls_OnMouseEnter(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.FlatAppearance.BorderSize = 2;
        }
        private void controls_OnMouseLeave(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.FlatAppearance.BorderSize = 0;
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkEffect.Checked)
                Properties.Settings.Default.effectChecked = true;
            else
                Properties.Settings.Default.effectChecked = false;

            Properties.Settings.Default.Save();
        }

        
    }
}
